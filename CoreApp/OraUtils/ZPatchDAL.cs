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
            "insert into zpatch_log" +
            "( zpatch_id,  parent_id,  release_id,  zpatch_name,    date,  dwsact)" +
            "values" +
            "(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, sysdate, 'I');" +
            "insert into сpatch" +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name)" +
            "values" +
            "(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name);";

        private static string updateScript =
            "insert into сpatch_log" +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name,    date,  dwsact)" +
            "values" +
            "(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, sysdate, 'U');" +
            "update zpatch" +
            "set " +
            "parent_id = :parent_id," +
            "cpatch_id = :cpatch_id" +
            "zpatch_name = :zpatch_name)" +
            "where" +
            "zpatch_id = :zpatch_id;";

        private static string deleteCPatchScript =
            "insert into сpatch_log" +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name,    date,  dwsact)" +
            "values" +
            "select zpatch_id, parent_id, cpatch_id, zpatch_name, sysdate, 'D'" +
            "from zpatch " +
            "where zpatch_id = :zpatch_id;" +
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

        public static void Insert(int zpatch_id, int parent_id, int cpatch_id, int cpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("cpatch_name", cpatch_name));
            transaction.Commit();
        }

        public static void Update(int zpatch_id, int parent_id, int cpatch_id, int cpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("cpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("release_id", cpatch_id),
                new OracleParameter("cpatch_name", cpatch_name));
            transaction.Commit();
        }

        public static void DeleteZPatch(int zpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteCPatchScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id));
            transaction.Commit();
        }

        public static void DeleteDependency(int zpatch_id, int parent_id, int cpatch_id, int zpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteDependencyScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("zpatch_name", zpatch_name));
            transaction.Commit();
        }

    }
}
