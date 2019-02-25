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
            "( zpatch_id,          parent_id,  cpatch_id,  zpatch_name, zpatchstatus, validfrom, validto, dwsact ) " +
            "values " +
           $"(zpatch_seq.nextval, :parent_id, :cpatch_id, :zpatch_name, :zpatchstatus, sysdate, {DBManager.PlusInf}, 'I'); ";

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

        private static string addDependencyScript =
            "insert into zpatch_hdim " +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name,  zpatchstatus, validfrom, validto, dwsact ) " +
            "select" +
            ":zpatch_id, " +
            ":parent_id, " +
            "max(cpatch_id)," +
            "max(zpatch_name)" +
            "max(zpatchstatus)" +
            "sysdate, " +
           $"{DBManager.PlusInf}, " +
            "'I'" +
            "from zpatch_hdim where zpatch_id = :zpatch_id; ";

        public static void Insert(int cpatch_id, int? parent_id, string zpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("parent_id", (object)parent_id ?? DBNull.Value),
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("zpatch_name", zpatch_name));
            transaction.Commit();
        }

        public static void Update(int zpatch_id, int? parent_id, int new_cpatch_id, int new_zpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", (object)parent_id ?? DBNull.Value),
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

        public static void AddDependency(int zpatch_id, int parent_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                addDependencyScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id)
                );
            transaction.Commit();
        }



        static string allZPatchesScript = $"select distinct zpatch_id, zpatch_name, zpatchstatus from zpatch_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' order by zpatch_name ";
        static string ZPatchesByCPatch = $"select distinct zpatch_id, zpatch_name, zpatchstatus from zpatch_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' and cpatch_id = :cpatch_id order by release_name ";
        static string dependenciesTo = $"select distinct zpatch_id, zpatch_name, zpatchstatus from zpatch_hdim where validto = {DBManager.PlusInf} and dwsact <>  'D' and parent_id = :zpatch_id order by zpatch_name ";
        static string dependenciesFrom =
             "select distinct z2.zpatch_id, z2.zpatch_name, z2.zpatchstatus" +
            $"from zpatch_hdim z1 join zpatch_hdim z2 on z1.parent_id = z2.zpatch_id " +
            $"where z1.validto = {DBManager.PlusInf} and z1.dwsact <>  'D' " +
            $"and   z2.validto = {DBManager.PlusInf} and z2.dwsact <>  'D' " +
            $"and z1.zpatch_id = :zpatch_id order by c2.cpatch_name ";

        public static IEnumerable<ZPatchRecord> getCPatches()
        {
            return getByScript(allZPatchesScript);
        }

        public static IEnumerable<ZPatchRecord> getZPatchesByCPatch(int cpatch_id)
        {
            return getByScript(ZPatchesByCPatch, new OracleParameter("cpatch_id", cpatch_id));
        }

        public static IEnumerable<ZPatchRecord> getDependenciesFrom(int zpatch_id)
        {
            return getByScript(dependenciesFrom, new OracleParameter("zpatch_id", zpatch_id));
        }

        public static IEnumerable<ZPatchRecord> getDependenciesTo(int zpatch_id)
        {
            return getByScript(dependenciesTo, new OracleParameter("zpatch_id", zpatch_id));
        }

        private static IEnumerable<ZPatchRecord> getByScript(string script, params OracleParameter[] parameters)
        {
            using (var reader = DBManager.ExecuteQuery(script))
            {
                while (reader.Read())
                {
                    yield return new ZPatchRecord(reader.GetInt32(0), reader.GetString(2), reader.GetString(3));
                }
            }
        }

        static string containsZPatch = $"select * from dual when exists (select 1 from zpatch_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' and zpatch_NAME = :zpatch_name)";

        private static bool Contains(string zpatch_name)
        {
            return DBManager.ExecuteQuery(containsZPatch, new OracleParameter(":zpatch_name", zpatch_name)).HasRows;
        }
    }
}