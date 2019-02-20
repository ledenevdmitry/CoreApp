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
            "insert into zpatch_hdim" +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, validfrom, validto, dwsact )" +
            "values" +
           $"(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, {DBManager.MinusInf}, {DBManager.PlusInf}, 'I');";

        private static string updateScript =
            "update cpatch_hdim " +
            "set validto = sysdate" +
            "where zpatch_id = :zpatch_id and " +
           $"validto = {DBManager.PlusInf} and " +
            "parent_id = :old_parent_id and " +
            "cpatch_id = old_cpatch_id" +

            "insert into сpatch_hdim" +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, validfrom, validto, dwsact )" +
            "values" +
           $"(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, sysdate, {DBManager.PlusInf}, 'U');";

        public static string deleteByCPatch = 
            "update zpatch_hdim " +
            "set validto = sysdate" +
            "where cpatch_id = :cpatch_id and " +
           $"validto = {DBManager.PlusInf};" +

            "insert into zpatch_hdim" +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, sysdate, {DBManager.PlusInf}, 'D')" +
            "from cpatch" +
            "where cpatch_id = :cpatch_id";

        private static string deleteZPatchScript =
            "update cpatch_hdim " +
            "set validto = sysdate" +
            "where zpatch_id = :zpatch_id and " +
           $"validto = {DBManager.PlusInf};" +

            "insert into сpatch_hdim" +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, sysdate, {DBManager.PlusInf}, 'D')" +
            "from cpatch" +
            "where zpatch_id = :zpatch_id";

        private static string deleteDependencyScript =
            "update cpatch_hdim " +
            "set validto = sysdate" +
            "where zpatch_id = :zpatch_id and " +
           $"validto = {DBManager.PlusInf}" +
            "and parent_id = :parent_id;" +

            "insert into сpatch_hdim" +
            "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:zpatch_id, :parent_id, :cpatch_id, :zpatch_name, sysdate, {DBManager.PlusInf}, 'D')" +
            "from cpatch" +
            "where zpatch_id = :zpatch_id and parent_id = :parent_id";

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

        public static void Update(int zpatch_id, int parent_id, int cpatch_id, int zpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("zpatch_name", zpatch_name));
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
