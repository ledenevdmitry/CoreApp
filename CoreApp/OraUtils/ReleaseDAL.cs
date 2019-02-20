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
        private static string insertScript = 
            "insert into release_hdim" +
            "( release_id,  release_name, validfrom, validto, dwsact )" +
            "values" +
           $"(:release_id, :release_name, {DBManager.MinusInf}, {DBManager.PlusInf}, 'I');";

        private static string updateScript =
            "update release_hdim " +
            "set validto = sysdate" +
            "where release_id = :release_id and " +
           $"validto = {DBManager.PlusInf};" +

            "insert into release_hdim" +
            "( release_id,  release_name, validfrom, validto, dwsact )" +
            "values" +
           $"(:release_id, :release_name, (select max(validfrom) from release_hdim where release_id = :release_id) , {DBManager.PlusInf}, 'I');";

        private static string deleteReleaseScript =
            "update release_hdim " +
            "set validto = sysdate " +
            "where release_id = :release_id and " +
           $"validto = {DBManager.PlusInf};" +

            "insert into release_hdim" +
            "( release_id, release_name, validfrom, validto, dwsact )" +
            "select" +
           $"(:release_id, :release_name, (select max(validfrom) from release_hdim where release_id = :release_id), {DBManager.PlusInf}, 'D')" +
            "from release_hdim" +
            "where release_id = :release_id"

            + CPatchDAL.deleteByRelease + 
            
            "update zpatch_hdim z " +
            "set validto = sysdate " +
            "where exists " +
            "(select 1 from cpatch c1 join zpatch z1 on c1.cpatch = z1.cpatch " +
            " where c.release_id = :release_id); " +
            
            "insert into z_patch z" +
             "( zpatch_id,  parent_id,  cpatch_id,  zpatch_name, validfrom, validto, dwsact )" +
            $"select zpatch_id, parent_id, cpatch_id, zpatch_name, " +
             "(select max(validfrom) from release_hdim where exists " +
                "(select 1 from cpatch c1 join zpatch z1 on c1.cpatch = z1.cpatch " +
                " where c.release_id = :release_id)), " +
            $"{DBManager.PlusInf}, 'D' " +
                 "from zpatch_id " +
             "where exists " +
             "(select 1 from cpatch c1 join zpatch z1 on c1.cpatch = z1.cpatch " +
             " where c.release_id = :release_id); ";

       
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

        public static void Update(int release_id, string release_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("release_id", release_id),
                new OracleParameter("release_name", release_name));
            transaction.Commit();
        }

        public static void Delete(int release_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();
            DBManager.ExecuteNonQuery(
                deleteReleaseScript,
                transaction,
                new OracleParameter("release_id", release_id));
            transaction.Commit();
        }

    }
}
