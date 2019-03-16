using CoreApp.OraUtils.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils
{
    class CPatchDAL
    {
        private static string insertScript =
            "insert into cpatch_hdim " +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name,  cpatchstatus,  kod_sredy, validfrom, validto, dwsact) " +
            "values " +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, :cpatchstatus, :kod_sredy, sysdate, {DBManager.PlusInf}, 'I') " ;

        public static string insertionsNew(char dmlType, params string[] pars)
        {
            string joinedPars = DBManager.JoinParams(pars);

            string res =
             "insert into cpatch_hdim " +
             "( cpatch_id,  parent_id,  release_id,  cpatch_name, cpatchstatus, kod_sredy, validfrom, validto, dwsact ) " +
             "select " +
             "cpatch_id, parent_id,  release_id,  cpatch_name, cpatchstatus,  kod_sredy, " +
             "validto, " +
            $"{DBManager.PlusInf}, '{dmlType}' " +
             "from cpatch_hdim " +
             "where " +
             "validto = (select max(validto) from cpatch_hdim " +
            $"where {joinedPars}) and {joinedPars}";
            return res;
        }

        private static string Update(string[] filter, HashSet<string> rowsToUpdate)
        {
            string[] semicolons = new string[5];
            if (rowsToUpdate.Contains("parent_id"))    semicolons[0] = ":";
            if (rowsToUpdate.Contains("release_id"))   semicolons[1] = ":";
            if (rowsToUpdate.Contains("cpatch_name"))  semicolons[2] = ":";
            if (rowsToUpdate.Contains("cpatchstatus")) semicolons[3] = ":";
            if (rowsToUpdate.Contains("kod_sredy"))    semicolons[4] = ":";

            string joinedPars = DBManager.JoinParams(filter);

            string res =
             "insert into cpatch_hdim " +
             "( cpatch_id,  parent_id,  release_id,  cpatch_name, cpatchstatus, kod_sredy, validfrom, validto, dwsact ) " +
             "select " +
            $"cpatch_id, {semicolons[0]}parent_id,  {semicolons[1]}release_id, {semicolons[2]}cpatch_name, {semicolons[3]}cpatchstatus,  {semicolons[4]}kod_sredy, " +
             "(select max(validto) from cpatch_hdim " +
            $"where {joinedPars}), " +
            $"{DBManager.PlusInf}, 'U' " +
             "from cpatch_hdim " +
             "where " +
            $"validto = (select max(validto) from cpatch_hdim " +
            $"where {joinedPars}) and {joinedPars}";
            return res;
        }

        static string updateStatus = Update(new string[] { "cpatch_id" }, new HashSet<string>(new string[] { "cpatchstatus" }));
        static string updateName = Update(new string[] { "cpatch_id" }, new HashSet<string>(new string[] { "cpatch_name" }));
        static string updateRelease = Update(new string[] { "cpatch_id" }, new HashSet<string>(new string[] { "release_id" }));

        public static string closeOld(params string[] pars)
        {
            string res = 
            "update cpatch_hdim " +
            "set validto = sysdate " +
            "where " +
            $"validto = {DBManager.PlusInf} and dwsact <> 'D' ";
            foreach(string par in pars)
            {
                res += $"and {par} = :{par} ";
            }
            return res;
        }

        /*
        public static string deleteByRelease =
            closeOld("release_id") +
            insertionsNew('D', "release_id") + 
            ZPatchDAL.deleteByRelease;
        */


        private static string addDependencyScript =
        "insert into cpatch_hdim " +
        "( cpatch_id,  parent_id,  release_id,  cpatch_name,  cpatchstatus, kod_sredy, validfrom, validto, dwsact ) " +
        "select " +
        ":cpatch_id, " +
        ":parent_id, " +
       $"(select max(release_id)   from cpatch_hdim where cpatch_id = :cpatch_id and validto = {DBManager.PlusInf} and dwsact <> 'D')," +
       $"(select max(cpatch_name)  from cpatch_hdim where cpatch_id = :cpatch_id and validto = {DBManager.PlusInf} and dwsact <> 'D')," +
       $"(select max(cpatchstatus) from cpatch_hdim where cpatch_id = :cpatch_id and validto = {DBManager.PlusInf} and dwsact <> 'D')," +
       $"(select max(kod_sredy)    from cpatch_hdim where cpatch_id = :cpatch_id and validto = {DBManager.PlusInf} and dwsact <> 'D')," +
        "sysdate, " +
       $"{DBManager.PlusInf}, " +
        "'I' " +
        "from cpatch_hdim where cpatch_id = :cpatch_id ";

        public static int Insert(int release_id, int? parent_id, string cpatch_name, string cpatchstatus, string kod_sredy)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            var seqReader = DBManager.ExecuteQuery("select cpatch_seq.nextval from dual");
            seqReader.Read();
            int seqValue = seqReader.GetInt32(0);

            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("cpatch_id", seqValue),
                new OracleParameter("parent_id"  , (object)parent_id ?? DBNull.Value),
                new OracleParameter("release_id" , release_id),
                new OracleParameter("cpatch_name", cpatch_name),
                new OracleParameter("cpatchstatus", (object)cpatchstatus ?? DBNull.Value),
                new OracleParameter("kod_sredy", (object)kod_sredy ?? DBNull.Value));

            transaction.Commit();
            return seqValue;
        }

        public static void UpdateStatus(int cpatch_id, string cpatchstatus)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld("cpatch_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id));

            DBManager.ExecuteNonQuery(
                updateStatus,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("cpatchstatus", cpatchstatus));

            transaction.Commit();
        }

        public static void UpdateName(int cpatch_id, string cpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld("cpatch_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id));

            DBManager.ExecuteNonQuery(
                updateName,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("cpatch_name", cpatch_name));

            transaction.Commit();
        }

        public static void UpdateRelease(int cpatch_id, int release_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld("cpatch_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id));

            DBManager.ExecuteNonQuery(
                updateRelease,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("release_id", release_id));

            transaction.Commit();
        }


        public static void DeleteCPatch(int cpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld("cpatch_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id));

            DBManager.ExecuteNonQuery(
                insertionsNew('D', "cpatch_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id));


            DBManager.ExecuteNonQuery(
                ZPatchDAL.closeOld("cpatch_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id));


            DBManager.ExecuteNonQuery(
                ZPatchDAL.insertionsNew('D', "cpatch_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id));

            transaction.Commit();
        }

        public static void DeleteDependency(int parent_id, int cpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(                
                closeOld("cpatch_id", "parent_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("parent_id", parent_id));

            DBManager.ExecuteNonQuery(
                insertionsNew('D', "cpatch_id", "parent_id"),
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("parent_id", parent_id));
            
            transaction.Commit();
        }

        static string allCPatchesScript = 
            $"select " +
            $"cpatch_id, " +
            $"max(cpatch_name), " +
            $"max(cpatchstatus), " +
            $"max(kod_sredy) " +
            $"from " +
            $"cpatch_hdim " +
            $"where " +
            $"validto = {DBManager.PlusInf} and dwsact <> 'D' " +
            $"group by cpatch_id " +
            $"order by max(cpatch_name) ";

        static string CPatchesByRelease = 
            $"select " +
            $"cpatch_id, " +
            $"max(cpatch_name), " +
            $"max(cpatchstatus), " +
            $"max(kod_sredy) " +
            $"from cpatch_hdim " +
            $"where validto = {DBManager.PlusInf} and dwsact <>  'D' and release_id = :release_id " +
            $"group by cpatch_id " +
            $"order by max(cpatch_name) ";

        static string dependenciesTo = 
            $"select " +
            $"cpatch_id, " +
            $"max(cpatch_name), " +
            $"max(cpatchstatus), " +
            $"max(kod_sredy) " +
            $"from cpatch_hdim " +
            $"where validto = {DBManager.PlusInf} and dwsact <>  'D' and parent_id = :cpatch_id " +
            $"group by cpatch_id " +
            $"order by max(cpatch_name) ";

        static string dependenciesFrom = 
            "select " +
            "c2.cpatch_id, " +
            "max(c2.cpatch_name), " +
            "max(c2.cpatchstatus), " +
            "max(c2.kod_sredy)" +
            $"from cpatch_hdim c1 join cpatch_hdim c2 on c1.parent_id = c2.cpatch_id " +
            $"where c1.validto = {DBManager.PlusInf} and c1.dwsact <>  'D' " +
            $"and   c2.validto = {DBManager.PlusInf} and c2.dwsact <>  'D' " +
            $"and c1.cpatch_id = :cpatch_id " +
            $"group by c2.cpatch_id " +
            $"order by max(c2.cpatch_name) ";


        public static IEnumerable<CPatchRecord> getCPatches()
        {
            return getByScript(allCPatchesScript);
        }

        public static IEnumerable<CPatchRecord> getCPatchesByRelease(int release_id)
        {
            return getByScript(CPatchesByRelease, new OracleParameter("release_id", release_id));
        }


        public static IEnumerable<CPatchRecord> getDependenciesFrom(int cpatch_id)
        {
            return getByScript(dependenciesFrom, new OracleParameter("cpatch_id", cpatch_id));
        }

        public static IEnumerable<CPatchRecord> getDependenciesTo(int cpatch_id)
        {
            return getByScript(dependenciesTo, new OracleParameter("cpatch_id", cpatch_id));
        }

        static string containsCPatch = $"select * from dual where (select 1 from cpatch_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' and cpatch_NAME = :cpatch_name)";

        public static bool Contains(string cpatch_name)
        {
            return DBManager.ExecuteQuery(containsCPatch, new OracleParameter(":cpatch_name", cpatch_name)).HasRows;
        }

        public static void AddDependency(int parent_id, int cpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                addDependencyScript,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("parent_id", parent_id)
                );
            transaction.Commit();
        }

        public static IEnumerable<CPatchRecord> getByScript(string script, params OracleParameter [] parameters)
        {
            using (var reader = DBManager.ExecuteQuery(script, parameters))
            {
                while (reader.Read())
                {
                    yield return new CPatchRecord(
                        reader.GetInt32(0), 
                        reader.GetString(1), 
                        reader.IsDBNull(2) ? null : reader.GetString(2), 
                        reader.IsDBNull(3) ? null : reader.GetString(3));
                }
            }
        }

    }
}
