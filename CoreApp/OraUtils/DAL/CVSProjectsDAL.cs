using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils.DAL
{
    class CVSProjectsDAL
    {
        public static string GetPath(string kod_sredy)
        {
            return 
                DBManager.ExecuteQuery(
                $"select max(vsspath) from vssfolder where kod_sredy = :kod_sredy", 
                new Oracle.ManagedDataAccess.Client.OracleParameter("kod_sredy", kod_sredy))
                .GetString(0);
        }
    }
}
