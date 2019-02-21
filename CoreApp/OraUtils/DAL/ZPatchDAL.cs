using CoreApp.OraUtils.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils
{
    class ZPatchDAL
    {
        private static string insertScript =
            "insert into zpatch_hdim " +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, zpatchstatus, validfrom, validto, dwsact ) " +
            "values " +
           $"(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, {DBManager.MinusInf}, {DBManager.PlusInf}, 'I'); ";

        private static string insertionsNew(char dmlType, params string[] pars)
        {
            string res =
             "insert into zpatch_hdim " +
             "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, zpatchstatus, validfrom, validto, dwsact ) " +
             "select " +
             "zpatch_id, parent_id,  :new_cpatch_id,  :new_zpatch_name, :new_zpatchstatus, " +
             "(select max(validto) from zpatch_hdim " +
             "where zpatch_id = :zpatch_id and " +
             "parent_id = :parent_id), " +
            $"{DBManager.PlusInf}, '{dmlType}') " +
             "from zpatch " +
             "where " +
            $"validto = {DBManager.PlusInf} ";
            foreach (string par in pars)
            {
                res += $"and {par} = :{par} ";
            }
            res += "; ";
            return res;
        }

        private static string closeOld(params string[] pars)
        {
            string res =
            "update zpatch_hdim " +
            "set validto = sysdate " +
            "where " +
            $"validto = {DBManager.PlusInf} ";
            foreach (string par in pars)
            {
                res += $"and {par} = :{par} ";
            }
            res += "; ";
            return res;
        }

        private static string updateScript =
            closeOld("zpatch_id") +
            insertionsNew('U', "zpatch_id");

        private static string deleteZPatchScript =
            closeOld("zpatch_id") +
            insertionsNew('D', "zpatch_id");


        public static string deleteByCPatch =
            closeOld("cpatch") +
            insertionsNew('D', "cpatch");

        public static string deleteByRelease =
            "update zpatch_hdim z" +
            "set validto = sysdate " +
            "where " +
            $"validto = {DBManager.PlusInf} " +
            "and exists (select 1 from zpatch_hdim z1 join cpatch_hdim c1 or z1.cpatch_id = c1.cpatch_id " +
                        "where z1.zpatch_id = z.zpatch_id and c1.release_id = :release_id)" +

             "insert into zpatch_hdim " +
             "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, zpatchstatus, validfrom, validto, dwsact ) " +
             "select " +
             "zpatch_id, parent_id,  :new_cpatch_id,  :new_zpatch_name, :new_zpatchstatus, " +
             "(select max(validto) from zpatch_hdim " +
             "where zpatch_id = :zpatch_id and " +
             "parent_id = :parent_id), " +
            $"{DBManager.PlusInf}, 'D') " +
             "from zpatch z" +
             "where " +
            $"validto = {DBManager.PlusInf} " +
             "and exists (select 1 from zpatch_hdim z1 join cpatch_hdim c1 or z1.cpatch_id = c1.cpatch_id " +
                        "where z1.zpatch_id = z.zpatch_id and c1.release_id = :release_id)";


        private static string deleteDependencyScript =
            closeOld("zpatch_id", "parent_id") +
            insertionsNew('D', "zpatch_id", "parent_id");

        public static void Insert(int zpatch_id, int parent_id, int cpatch_id, int zpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("zpatch_name", zpatch_name));
            transaction.Commit();
        }

        public static void Update(int zpatch_id, int parent_id, int new_cpatch_id, int new_zpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("new_cpatch_id", new_cpatch_id),
                new OracleParameter("new_zpatch_name", new_zpatch_name));
            transaction.Commit();
        }

        public static void DeleteZPatch(int zpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteZPatchScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id));
            transaction.Commit();
        }

        public static void DeleteDependency(int zpatch_id, int parent_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteDependencyScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id));
            transaction.Commit();
        }



        static string allZPatchesScript = $"select zpatch_id, parent_id, zpatch_name, zpatchstatus from cpatch_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' order by release_name ";
        static string ZPatchesByCPatch = $"select zpatch_id, parent_id, zpatch_name, zpatchstatus from cpatch_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' and cpatch_id = :cpatch_id order by release_name ";


        public static IEnumerable<ZPatchRecord> getCPatches()
        {
            return getByScript(allZPatchesScript);
        }

        public static IEnumerable<ZPatchRecord> getZPatchesByCPatch(int cpatch_id)
        {
            return getByScript(ZPatchesByCPatch, new OracleParameter("cpatch_id", cpatch_id));
        }


        public static IEnumerable<ZPatchRecord> getByScript(string script, params OracleParameter[] parameters)
        {
            using (var reader = DBManager.ExecuteQuery(script))
            {
                while (reader.Read())
                {
                    yield return new ZPatchRecord(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3));
                }
            }
        }

    }
}