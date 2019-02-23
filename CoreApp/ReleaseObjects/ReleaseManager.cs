using CoreApp.OraUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.ReleaseObjects
{
    class ReleaseManager
    {
        static CVS.CVS cvs;
        List<Release> releases;

        public ReleaseManager()
        {
            InitFromDB();
        }

        private void InitFromDB()
        {
            var oraReleases = ReleaseDAL.getReleases();
            releases = new List<Release>();
            foreach (var oraRelease in oraReleases)
            {
                Release release = new Release(oraRelease.releaseId, oraRelease.releaseName);
                releases.Add(release);
            }
        }

        public bool AddRelease(string releaseName)
        {
            if(ReleaseDAL.Contains(releaseName))
            {
                return false;
            }
            ReleaseDAL.Insert(releaseName);
            InitFromDB();
            return true;
        }

        public void DeleteRelease(int releaseId)
        {
            ReleaseDAL.Delete(releaseId);
            InitFromDB();
        }

        public void UpdateRelease(int releaseId, string newReleaseName)
        {
            ReleaseDAL.Update(releaseId, newReleaseName);
            InitFromDB();
        }
    }
}
