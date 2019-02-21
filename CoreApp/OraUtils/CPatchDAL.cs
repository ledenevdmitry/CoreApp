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
            "insert into сpatch_hdim " +
            "( cpatch_id,  parent_id,  release_id,  cpatch_name,  cpatchstatus,  kod_sredy, validfrom, validto, dwsact) " +
            "values " +
           $"(:cpatch_id, :parent_id, :release_id, :cpatch_name, :cpatchstatus, :kod_sredy, {DBManager.MinusInf}, {DBManager.PlusInf}, 'I'); " ;

        private static string insertionsNew(char dmlType, params string[] pars)
        {
            string res =
             "insert into сpatch_hdim " +
             "( cpatch_id,  parent_id,  release_id,  cpatch_name, cpatchstatus, kod_sredy, validfrom, validto, dwsact ) " +
             "select " +
             "cpatch_id, parent_id,  :new_release_id,  :new_cpatch_name, :new_cpatchstatus,  :new_kod_sredy, " +
             "(select max(validto) from cpatch_hdim " +
             "where cpatch_id = :cpatch_id and " +
             "parent_id = :old_parent_id), " +
            $"{DBManager.PlusInf}, '{dmlType}') " +
             "from cpatch " +
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
            "update cpatch_hdim " +
            "set validto = sysdate " +
            "where " +
            $"validto = {DBManager.PlusInf} ";
            foreach(string par in pars)
            {
                res += $"and {par} = :{par} ";
            }
            res += "; ";
            return res;
        }

        private static string updateScript =
            closeOld("cpatch_id") +
            insertionsNew('U', "cpatch_id");

        private static string deleteCPatchScript =
            closeOld("cpatch_id") +
            insertionsNew('D', "cpatch_id") +
            ZPatchDAL.deleteByCPatch;


        public static string deleteByRelease =
            closeOld("release_id") +
            insertionsNew('D', "release_id") + 
            ZPatchDAL.deleteByRelease;

        private static string deleteDependencyScript =
            closeOld("cpatch_id", "parent_id") +
            insertionsNew('D', "cpatch_id", "parent_id");

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

        public static void Update(int cpatch_id, int parent_id, int new_release_id, string new_cpatch_name, string new_cpatchstatus)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("cpatch_id", cpatch_id),
                new OracleParameter("parent_id", parent_id),
                new OracleParameter("new_release_id", new_release_id),
                new OracleParameter("new_cpatch_name", new_cpatch_name),
                new OracleParameter("new_cpatchstatus", new_cpatchstatus));
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
