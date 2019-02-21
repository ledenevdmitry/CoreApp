using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils
{
    class ReleaseDAL
    {
        private static string insertionsNew(char dmlType, params string[] pars)
        {
            string res =
             "insert into release_hdim " +
             "( release_id,  release_name, validfrom, validto, dwsact ) " +
             "select " +
             "release_id, :new_release_name" +
             "(select max(validto) from release_hdim " +
             "where release_id = :release_id and " +
            $"{DBManager.PlusInf}, '{dmlType}') " +
             "from release_hdim " +
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
            "update release_hdim " +
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

        private static string insertScript =
            "insert into release_hdim " +
            "( release_id,  release_name, validfrom, validto, dwsact) " +
            "values " +
           $"(:release_id, :release_name, {DBManager.MinusInf}, {DBManager.PlusInf}, 'I'); ";

        private static string updateScript =
            closeOld("release_id") +
            insertionsNew('U', "release_id");

        private static string deleteScript =
            closeOld("release_id") +
            insertionsNew('D', "release_id") +
            CPatchDAL.deleteByRelease;

        public static void Insert(int release_id, string release_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("release_id", release_id),
                new OracleParameter("release_name", release_name));
            transaction.Commit();
        }

        public static void Update(int release_id, string new_release_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("release_id", release_id),
                new OracleParameter("new_release_name", new_release_name));
            transaction.Commit();
        }

        public static void Delete(int release_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteScript,
                transaction,
                new OracleParameter("release_id", release_id));
            transaction.Commit();
        }

    }
}
