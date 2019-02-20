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
            "insert into сpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "values" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, {DBManager.MinusInf}, {DBManager.PlusInf}, 'I');" ;

        private static string updateScript =
            "update cpatch_hdim " +
            "set validto = sysdate" +
            "where cpatch_id = :cpatch_id and " +
           $"validto = {DBManager.PlusInf} and " +
            "parent_id = :old_parent_id and " +
            "release_id = old_release_id" +

            "insert into сpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "values" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, " +
            "(select max(validfrom from cpatch_hdim " +
            "where cpatch_id = :cpatch_id and " +
            "parent_id = :old_parent_id and " +
            "release_id = old_release_id), " +
           $"{DBManager.PlusInf}, 'U');";

        private static string deleteCPatchScript =
            "update cpatch_hdim " +
            "set validto = sysdate" +
            "where cpatch_id = :cpatch_id and " +
           $"validto = {DBManager.PlusInf};" +

            "insert into сpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, (select(max(valid_to) from cpatch_hdim where cpatch_id = :cpatch_id), {DBManager.PlusInf}, 'D')" +
            "from cpatch" +
            "where cpatch_id = :cpatch_id" 
            + ZPatchDAL.deleteByCPatch;


        public static string deleteByRelease =
            "update cpatch_hdim " +
            "set validto = sysdate" +
            "where release_id = :release_id and " +
           $"validto = {DBManager.PlusInf};" +

            "insert into zpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, sysdate, {DBManager.PlusInf}, 'D')" +
            "from cpatch" +
            "where cpatch_id = :cpatch_id";

        private static string deleteDependencyScript =
            "update cpatch_hdim " +
            "set validto = sysdate" +
            "where cpatch_id = :cpatch_id and " +
           $"validto = {DBManager.PlusInf}" +
            "and parent_id = :parent_id;" +

            "insert into сpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, sysdate, {DBManager.PlusInf}, 'D')" +
            "from cpatch" +
            "where cpatch_id = :cpatch_id and parent_id = :parent_id";

        public static void Insert(int cpatch_id, int parent_id, int release_id, string cpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("cpatch_id"  , cpatch_id),
                new OracleParameter("parent_id"  , parent_id),
                new OracleParameter("release_id" , release_id),
                new OracleParameter("cpatch_name", cpatch_name));
            transaction.Commit();
        }

        public static void Update(int cpatch_id, int old_parent_id, int parent_id, int old_release_id, int release_id, string cpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("old_parent_id", old_parent_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("old_release_id", old_release_id),
                new OracleParameter("release_id", release_id),
                new OracleParameter("cpatch_name", cpatch_name));
            transaction.Commit();
        }

        public static void DeleteCPatch(int cpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteCPatchScript,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id));
            transaction.Commit();
        }

        public static void DeleteDependency(int cpatch_id, int parent_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteDependencyScript,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("parent_id", parent_id));
            transaction.Commit();
        }

    }
}
