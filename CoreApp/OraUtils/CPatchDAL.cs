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
        private static string MinusInf = "to_date('01.01.1900', 'dd.mm.yyyy')";
        private static string PlusInf  = "to_date('31.12.5999', 'dd.mm.yyyy')";
        private static string insertScript =
            "insert into сpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "values" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, {MinusInf}, {PlusInf}, 'I');" ;

        private static string updateScript =
            "update cpatch_hdim " +
            "set validto = sysdate" +
            "where cpatch_id = :cpatch_id and " +
           $"validto = {PlusInf}; and " +
            "parent_id = :old_parent_id and " +
            "release_id = old_release_id" +

            "insert into сpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "values" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, sysdate, {PlusInf}, 'U');";

        private static string deleteCPatchScript =
            "insert into сpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, sysdate, {PlusInf}, 'D')" +
            "from cpatch" +
            "where cpatch_id = :cpatch_id";

        private static string deleteDependencyScript =
            "insert into сpatch_hdim" +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, sysdate, {PlusInf}, 'D')" +
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

        public static void Update(int cpatch_id, int old_parent_id, int parent_id, int old_release_id, int release_id, string old_cpatch_name, string cpatch_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("old_parent_id", parent_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("old_release_id", release_id),
                new OracleParameter("release_id", release_id),
                new OracleParameter("old_cpatch_name", cpatch_name),
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
