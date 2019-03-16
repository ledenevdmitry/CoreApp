using CoreApp.OraUtils.Model;
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
        public static string deleteScript = 
             "insert into release_hdim " +
             "( release_id,  release_name, validfrom, validto, dwsact ) " +
             "select " +
             "release_id, release_name,  " +
             "validto, " + 
            $"{DBManager.PlusInf}, 'D' " +
             "from release_hdim " +
             "where " +
             "validto = (select max(validto) from release_hdim " +
            $"where release_id = :release_id) and release_id = :release_id";

        private static string closeOld =
             "update release_hdim " +
             "set validto = sysdate " +
             "where " +
            $"validto = {DBManager.PlusInf} " +
             "and release_id = :release_id ";

        private static string updateScript  =
             "insert into release_hdim " +
             "( release_id,  release_name, validfrom, validto, dwsact ) " +
             "select " +
             "release_id, :release_name,  " +
             "validto, " +
            $"{DBManager.PlusInf}, 'U' " +
             "from release_hdim " +
             "where " +
             "validto = (select max(validto) from release_hdim " +
            $"where release_id = :release_id) and release_id = :release_id";

        private static string insertScript =
            "insert into release_hdim " +
            "( release_id,  release_name, validfrom, validto, dwsact) " +
            "values " +
           $"(:release_id, :release_name, sysdate, {DBManager.PlusInf}, 'I') ";

        public static int Insert(string release_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            var seqReader = DBManager.ExecuteQuery("select release_seq.nextval from dual");
            seqReader.Read();
            int seqValue = seqReader.GetInt32(0);

            DBManager.ExecuteNonQuery(
                insertScript,
                transaction,
                new OracleParameter("release_id", seqValue),
                new OracleParameter("release_name", release_name));
            transaction.Commit();

            return seqValue;
        }

        public static void Update(int release_id, string new_release_name)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld,
                transaction,
                new OracleParameter("release_id", release_id));

            DBManager.ExecuteNonQuery(
                updateScript,
                transaction,
                new OracleParameter("release_id", release_id),
                new OracleParameter("release_name", new_release_name));

            transaction.Commit();
        }

        public static void Delete(int release_id)
        {
            OracleTransaction transaction = DBManager.BeginTransaction();

            DBManager.ExecuteNonQuery(
                closeOld,
                transaction,
                new OracleParameter("release_id", release_id));

            DBManager.ExecuteNonQuery(
                deleteScript,
                transaction,
                new OracleParameter("release_id", release_id));

            DBManager.ExecuteNonQuery(
                CPatchDAL.closeOld("release_id"),
                transaction,
                new OracleParameter("release_id", release_id));

            DBManager.ExecuteNonQuery(
                CPatchDAL.insertionsNew('D', "release_id"),
                transaction,
                new OracleParameter("release_id", release_id));

            DBManager.ExecuteNonQuery(
                ZPatchDAL.deleteByReleaseCloseOld,
                transaction,
                new OracleParameter("release_id", release_id));

            DBManager.ExecuteNonQuery(
                ZPatchDAL.deleteByReleaseInsertionsNew,
                transaction,
                new OracleParameter("release_id", release_id));

            transaction.Commit();
        }

        public static IEnumerable<ReleaseRecord> getReleases()
        {
            return getByScript(allReleasesScript);
        }

        static string allReleasesScript = 
            $"select release_id, " +
            $"max(release_name) " +
            $"from release_hdim " +
            $"where validto = {DBManager.PlusInf} and dwsact <> 'D' " +
            $"group by release_id " +
            $"order by max(release_name) ";

        static string containsRelease = $"select * from dual where exists (select 1 from release_hdim where validto = {DBManager.PlusInf} and dwsact <> 'D' and release_name = :release_name) ";

        public static bool Contains(string release_name)
        {
            return DBManager.ExecuteQuery(containsRelease, new OracleParameter(":release_name", release_name)).HasRows;
        }

        private static IEnumerable<ReleaseRecord> getByScript(string script, params OracleParameter [] parameters)
        {
            using (var reader = DBManager.ExecuteQuery(script, parameters))
            {
                while(reader.Read())
                {
                    yield return new ReleaseRecord(reader.GetInt32(0), reader.GetString(1));
                }
            }
        }

    }
}
