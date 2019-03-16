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
           $"(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, :zpatchstatus, sysdate, {DBManager.PlusInf}, 'I') ";

        public static string insertionsNew(char dmlType, params string[] pars)
        {
            string joinedPars = DBManager.JoinParams(pars);
            string res =
             "insert into zpatch_hdim " +
             "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, zpatchstatus, validfrom, validto, dwsact ) " +
             "select " +
             "zpatch_id, parent_id,  cpatch_id,  zpatch_name, zpatchstatus, " +
             "validto, " +
            $"{DBManager.PlusInf}, '{dmlType}' " +
             "from zpatch_hdim " +
             "where " +
            $"validto = (select max(validto) from zpatch_hdim " +
            $"where {joinedPars}) and {joinedPars}";
            return res;
        }

        public static string closeOld(params string[] pars)
        {
            string res =
            "update zpatch_hdim " +
            "set validto = sysdate " +
            "where " +
            $"validto = {DBManager.PlusInf} and dwsact <> 'D' ";
            foreach (string par in pars)
            {
                res += $"and {par} = :{par} ";
            }
            return res;
        }

        private static string Update(string[] filter, HashSet<string> rowsToUpdate)
        {
            string[] semicolons = new string[5];
            if (rowsToUpdate.Contains("parent_id")) semicolons[0] = ":";
            if (rowsToUpdate.Contains("cpatch_id")) semicolons[1] = ":";
            if (rowsToUpdate.Contains("zpatch_name")) semicolons[2] = ":";
            if (rowsToUpdate.Contains("zpatchstatus")) semicolons[3] = ":";

            string joinedPars = DBManager.JoinParams(filter);

            string res =
             "insert into zpatch_hdim " +
             "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, zpatchstatus, validfrom, validto, dwsact ) " +
             "select " +
            $"zpatch_id, {semicolons[0]}parent_id,  {semicolons[1]}cpatch_id, {semicolons[2]}zpatch_name, {semicolons[3]}zpatchstatus, " +
             "(select max(validto) from zpatch_hdim " +
            $"where {joinedPars}), " +
            $"{DBManager.PlusInf}, 'U' " +
             "from zpatch_hdim " +
             "where " +
            $"validto = (select max(validto) from zpatch_hdim " +
            $"where {joinedPars}) and {joinedPars}";
            return res;
        }
        
        static string updateStatus = Update(new string[] { "zpatch_id" }, new HashSet<string>(new string[] { "zpatchstatus" }));
        static string updateName = Update(new string[] { "zpatch_id" }, new HashSet<string>(new string[] { "zpatch_name" }));
        static string updateCPatch = Update(new string[] { "zpatch_id" }, new HashSet<string>(new string[] { "cpatch_id" }));

        public static string deleteByReleaseCloseOld =
            "update zpatch_hdim z " +
            "set validto = sysdate " +
            "where " +
            $"validto = {DBManager.PlusInf} and dwsact <> 'D' " +
            "and exists (select 1 from " +
                       $"(select zpatch_id, cpatch_id  from zpatch_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D') z1 join " +
                       $"cpatch_hdim c1 on " +
                        "z1.cpatch_id = c1.cpatch_id " +
                        "where z1.zpatch_id = z.zpatch_id and c1.release_id = :release_id)";

        public static string deleteByReleaseInsertionsNew =
             "insert into zpatch_hdim " +
             "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, zpatchstatus, validfrom, validto, dwsact ) " +
             "select " +
             "zpatch_id, parent_id,  cpatch_id,  zpatch_name, zpatchstatus, " +
             "validto, " +
            $"{DBManager.PlusInf}, 'D' " +
             "from zpatch_hdim z " +
             "where " +
            $"validto = (select max(validto) from zpatch_hdim z2 " +
            $"where z2.zpatch_id = z.zpatch_id and " +
             "exists " +  
                        "(select 1 from zpatch_hdim z1 join cpatch_hdim c1 on " +
                        "z1.cpatch_id = c1.cpatch_id " +
                        "where z1.zpatch_id = z2.zpatch_id and c1.release_id = :release_id)) " +
            "and exists (select 1 from zpatch_hdim z1 join cpatch_hdim c1 on " +
                        "z1.cpatch_id = c1.cpatch_id " +
                        "where z1.zpatch_id = z.zpatch_id and c1.release_id = :release_id)";


        private static string addDependencyScript =
            "insert into zpatch_hdim " +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name,  zpatchstatus, validfrom, validto, dwsact ) " +
            "select " +
            ":zpatch_id, " +
            ":parent_id, " +
           $"(select max(cpatch_id)   from zpatch_hdim where zpatch_id = :zpatch_id and validto = {DBManager.PlusInf} and dwsact <> 'D')," +
           $"(select max(zpatch_name) from zpatch_hdim where zpatch_id = :zpatch_id and validto = {DBManager.PlusInf} and dwsact <> 'D')," +
           $"(select max(zpatchstatus) from zpatch_hdim where zpatch_id = :zpatch_id and validto = {DBManager.PlusInf} and dwsact <> 'D')," +
            "sysdate, " +
           $"{DBManager.PlusInf}, " +
            "'I' " +
            "from zpatch_hdim where zpatch_id = :zpatch_id ";

        private static string ZPatchInstalledScript =
            "select 1 from dual " +
            "where exists (select 1 from patch_stat where command_text like '%:zpatch_name%' and status = 0) " +
            "and not exists(select 1 from patch_stat where command_text like '%:zpatch_name%' and status <> 0)";

        public static int Insert(int cpatch_id, int? parent_id, string zpatch_name, string zpatchstatus)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            var seqReader = DBManager.ExecuteQuery("select zpatch_seq.nextval from dual");
            seqReader.Read();
            int seqValue = seqReader.GetInt32(0);

            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("zpatch_id", seqValue),
                new OracleParameter("parent_id", (object)parent_id ?? DBNull.Value),
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("zpatch_name", zpatch_name),
                new OracleParameter("zpatchstatus", (object)zpatchstatus ?? DBNull.Value));

            transaction.Commit();
            return seqValue;
        }

        public static bool IsZPatchInstalled(string zpatch_name, string KodSredy)
        {
            if (KodSredy == "STAB")
            {
                return DBManager.ExecuteQuery(
                    ZPatchInstalledScript,
                    new OracleParameter("zpatch_name", zpatch_name)).HasRows;
            }
            return DBManagerForTest.ExecuteQuery(
                ZPatchInstalledScript,
                new OracleParameter("zpatch_name", zpatch_name)).HasRows;
        }

        public static void UpdateName(int zpatch_id, string zpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                closeOld("zpatch_id"),
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("zpatch_name", zpatch_name));

            DBManager.ExecuteNonQuery(
                updateName,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("zpatch_name", zpatch_name));    

            transaction.Commit();
        }

        public static void UpdateStatus(int zpatch_id, string zpatchstatus)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                closeOld("zpatch_id"),
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("zpatchstatus", zpatchstatus));

            DBManager.ExecuteNonQuery(
                updateStatus,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("zpatchstatus", zpatchstatus));

            transaction.Commit();
        }

        public static void DeleteZPatch(int zpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld("zpatch_id"),
                transaction,
                new OracleParameter("zpatch_id", zpatch_id));

            DBManager.ExecuteNonQuery(
                insertionsNew('D', "zpatch_id"),
                transaction,
                new OracleParameter("zpatch_id", zpatch_id));

            transaction.Commit();
        }

        public static void DeleteDependency(int parent_id, int zpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
            closeOld("zpatch_id", "parent_id"),
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id));

            DBManager.ExecuteNonQuery(
                insertionsNew('D', "zpatch_id", "parent_id"),
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id));

            transaction.Commit();
        }

        public static void AddDependency(int parent_id, int zpatch_id)
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



        static string allZPatchesScript = 
            $"select " +
            $"zpatch_id, " +
            $"max(zpatch_name), " +
            $"max(zpatchstatus) " +
            $"from zpatch_hdim " +
            $"where validto = {DBManager.PlusInf} and dwsact <> 'D' " +
            $"group by zpatch_id " +
            $"order by max(zpatch_name) ";

        static string ZPatchesByCPatch = 
            $"select " +
            $"zpatch_id, " +
            $"max(zpatch_name), " +
            $"max(zpatchstatus) " +
            $"from zpatch_hdim " +
            $"where validto = {DBManager.PlusInf} and dwsact <> 'D' and cpatch_id = :cpatch_id " +
            $"group by zpatch_id " +
            $"order by max(zpatch_name) ";

        static string dependenciesTo = 
            $"select " +
            $"zpatch_id, " +
            $"max(zpatch_name), " +
            $"max(zpatchstatus) " +
            $"from zpatch_hdim " +
            $"where validto = {DBManager.PlusInf} and dwsact <>  'D' and parent_id = :zpatch_id " +
            $"group by zpatch_id " +
            $"order by max(zpatch_name) ";

        static string dependenciesFrom =
             "select " +
            "z2.zpatch_id, " +
            "max(z2.zpatch_name), " +
            "max(z2.zpatchstatus) " +
            $"from zpatch_hdim z1 join zpatch_hdim z2 on z1.parent_id = z2.zpatch_id " +
            $"where z1.validto = {DBManager.PlusInf} and z1.dwsact <>  'D' " +
            $"and   z2.validto = {DBManager.PlusInf} and z2.dwsact <>  'D' " +
            $"and z1.zpatch_id = :zpatch_id " +
            $"group by z2.zpatch_id " +
            $"order by max(z2.zpatch_name) ";

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
            using (var reader = DBManager.ExecuteQuery(script, parameters))
            {
                while (reader.Read())
                {
                    yield return new ZPatchRecord(
                        reader.GetInt32(0), 
                        reader.GetString(1), 
                        reader.IsDBNull(2) ? null : reader.GetString(2));
                }
            }
        }

        static string containsZPatch = $"select * from dual where exists (select 1 from zpatch_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' and zpatch_NAME = :zpatch_name)";

        private static bool Contains(string zpatch_name)
        {
            return DBManager.ExecuteQuery(containsZPatch, new OracleParameter(":zpatch_name", zpatch_name)).HasRows;
        }

        public static void UpdateCPatch(int zpatch_id, int cpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld("zpatch_id"),
                transaction,
                new OracleParameter("zpatch_id", cpatch_id));

            DBManager.ExecuteNonQuery(
                updateCPatch,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("cpatch_id", cpatch_id));

            transaction.Commit();
        }
    }
}