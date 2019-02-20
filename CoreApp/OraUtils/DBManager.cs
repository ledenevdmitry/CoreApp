using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils
{
    class DBManager : IDisposable
    {
        private static OracleConnection conn;

        public const string MinusInf = "to_date('01.01.1900', 'dd.mm.yyyy')";
        public const string PlusInf = "to_date('31.12.5999', 'dd.mm.yyyy')";

        static DBManager()
        {
            CreateConn(IniUtils.IniUtils.GetConfig("Oracle", "TNS"));
        }

        //создание менеджера с переданным логином и паролем
        private static void CreateConn(string TNS)
        {
            conn = new OracleConnection(TNS);
            try
            {
                conn.Open();
            }
            catch (System.InvalidOperationException exc)
            {
                throw new ArgumentException("Задайте правильный TNS", exc);
            }
        }

        //открыть соединение
        public static void Test()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                return;
            conn.Open();
        }

        //закрыть соединение
        public void Close()
        {
            if (conn.State == System.Data.ConnectionState.Closed ||
                conn.State == System.Data.ConnectionState.Broken)
                return;
            conn.Close();
        }

        public static OracleTransaction BeginTransaction()
        {
            return conn.BeginTransaction();
        }

        //static object locker = new object();

        //выполнить выражение, возвращающее результат
        public static OracleDataReader ExecuteQuery(string sql, params OracleParameter[] parameters)
        {
            Test();
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.BindByName = true;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                OracleDataReader res = cmd.ExecuteReader();
                return res;
            }
        }

        //выполнить выражение, не возвращающее результат
        public static void ExecuteNonQuery(string sql, OracleTransaction transaction, params OracleParameter[] parameters)
        {
            Test();
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.BindByName = true;
                cmd.Transaction = transaction;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            Close();
            conn.Dispose();
        }

    }
}
