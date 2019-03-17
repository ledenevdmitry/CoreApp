using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils.DAL
{
    class ZPatchStateDAL
    {
        private static readonly string getStatusesScript = "select distinct zpatchstatus from zpatchstate";
        private static readonly string getCVSPathScript = "select vsspath from zpatchstate where zpatchstatus = :zpatchstatus";

        public static string GetCVSPath(string cpatchstatus)
        {
            return GetByScript(
                getCVSPathScript,
                new OracleParameter("zpatchstatus", cpatchstatus)).First();
        }

        public static IEnumerable<string> GetStatuses()
        {
            return GetByScript(getStatusesScript);
        }

        private static IEnumerable<string> GetByScript(string script, params OracleParameter[] parameters)
        {
            using (var reader = DBManager.ExecuteQuery(script, parameters))
            {
                while (reader.Read())
                {
                    yield return reader.GetString(0);
                }
            }
        }
    }
}

