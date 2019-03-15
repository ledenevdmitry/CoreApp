using CoreApp.OraUtils.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils
{
    class ZPatchOrderDAL
    {
        private static string insertScript = 
            "insert into zpatchorder_hdim " +
            "( zpatch_id,  zpatch_order , validfrom, validto, dwsact ) " +
            "values " +
           $"(:zpatch_id, :zpatch_order, sysdate, {DBManager.PlusInf}, 'I') ";

        private static string closeOld =
            $"update zpatchorder_hdim set validto = sysdate where validto = {DBManager.PlusInf} and dwsact <> 'D' and zpatch_id = :zpatch_id ";

        private static string insertNew(string dmlType)
        {
            return
            "insert into zpatchorder_hdim " +
            "( zpatch_id,  zpatch_order , validfrom, validto, dwsact ) " +
            "select " +
            "zpatch_id, " +
            (dmlType == "D" ? "" : ":") + 
            "zpatch_order, " +
            "validto," +
           $"{DBManager.PlusInf}, " +
           $"'{dmlType}' " +
            "from zpatchorder_hdim " +
            "where validfrom = (select max(validfrom) from zpatchorder_hdim where zpatch_id = :zpatch_id) and zpatch_id = :zpatch_id";
        }


        public static void Insert(int zpatch_id, int zpatch_order)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("zpatch_order", zpatch_order));

            transaction.Commit();
        }


        public static void Update(int zpatch_id, int zpatch_order)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id));

            DBManager.ExecuteNonQuery(
                insertNew("U"),
                transaction,
                new OracleParameter("zpatch_id", zpatch_id),
                new OracleParameter("zpatch_order", zpatch_order));

            transaction.Commit();
        }

        public static void Delete(int zpatch_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld,
                transaction,
                new OracleParameter("zpatch_id", zpatch_id));

            DBManager.ExecuteNonQuery(
                insertNew("D"),
                transaction,
                new OracleParameter("zpatch_id", zpatch_id));

            transaction.Commit();
        }

        static string allZPatchOrders = $"select zpatch_id, zpatch_order from zpatchorder_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' ";
        static string ZPatchOrdersByCPatch =
            "select o.zpatch_id, o.zpatch_order " +
            "from zpatchorder_hdim o join " +
            "(select distinct zpatch_id from zpatch_hdim z " +
           $"where z.validto = {DBManager.PlusInf} and z.dwsact<> 'D' and z.cpatch_id = :cpatch_id) z " +
            "on o.zpatch_id = z.zpatch_id";

        public static IEnumerable<ZPatchOrderRecord> GetZPatchOrdersByCPatch(int cpatch_id)
        {
            using (var reader = DBManager.ExecuteQuery(ZPatchOrdersByCPatch, new OracleParameter("cpatch_id", cpatch_id)))
            {
                while (reader.Read())
                {
                    yield return new ZPatchOrderRecord(
                        reader.GetInt32(0),
                        reader.GetInt32(1));
                }
            }
        }
    }
}
