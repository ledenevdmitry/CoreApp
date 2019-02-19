using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace CoreApp.OraUtils
{
    class CPatchDAL
    {
        private static string insertScript =
            "insert into сpatch_log" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name,    date,  dwsact)" +
            "values" +
            "(:cpatch_id, :parent_id, :release_id, :cpatch_name, sysdate, 'I');" +
            "insert into сpatch" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name)" +
            "values" +
            "(:cpatch_id, :parent_id, :release_id, :cpatch_name);" ;

        private static string updateScript =
            "insert into сpatch_log" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name,    date,  dwsact)" +
            "values" +
            "(:cpatch_id, :parent_id, :release_id, :cpatch_name, sysdate, 'U');" +
            "update сpatch" +
            "set " +
            "parent_id = :parent_id," +
            "release_id = :release_id" +
            "cpatch_name = :cpatch_name)" +
            "where" +
            "cpatch_id = :cpatch_id;";

        private static string deleteCPatchScript =
            "insert into сpatch_log" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name,    date,  dwsact)" +
            "values" +
            "select cpatch_id, parent_id, release_id, cpatch_name, sysdate, 'D'" +
            "from cpatch " +
            "where cpatch_id = :cpatch_id" +
            "delete from сpatch" +
            "where " +
            "cpatch_id = :cpatch_id;";

        private static string deleteDependencyScript =
            "insert into сpatch_log" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name,    date,  dwsact)" +
            "values" +
            "(:cpatch_id, :parent_id, :release_id, :cpatch_name, sysdate, 'D');" +
            "delete from сpatch" +
            "where " +
            "parent_id = :parent_id," +
            "release_id = :release_id" +
            "cpatch_name = :cpatch_name" +
            "cpatch_id = :cpatch_id);";

        public static void Insert(int cpatch_id, int parent_id, int release_id, int cpatch_name)
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

        public static void Update(int cpatch_id, int parent_id, int release_id, int cpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("parent_id", parent_id),
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

        public static void DeleteDependency(int cpatch_id, int parent_id, int release_id, int cpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteDependencyScript,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("release_id", release_id),
                new OracleParameter("cpatch_name", cpatch_name));
            transaction.Commit();
        }

    }
}
