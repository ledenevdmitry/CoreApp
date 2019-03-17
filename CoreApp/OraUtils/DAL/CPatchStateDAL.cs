using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils.DAL
{
    class CPatchStateDAL
    {
        private static readonly string getStatusesScript = "select distinct cpatchstatus from cpatchstate";
        private static readonly string getEnvCodesScript = "select distinct kod_sredy from cpatchstate";
        private static readonly string getCVSPathScript = "select vsspath from cpatchstate where cpatchstatus = :cpatchstatus and kod_sredy = :kod_sredy";

        public static string GetCVSPath(string cpatchstatus, string kod_sredy)
        {
            return GetByScript(
                getCVSPathScript,
                new OracleParameter("cpatchstatus", cpatchstatus),
                new OracleParameter("kod_sredy", kod_sredy)).First();
        }

        public static IEnumerable<string> GetEnvCodes()
        {
            return GetByScript(getEnvCodesScript);
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
