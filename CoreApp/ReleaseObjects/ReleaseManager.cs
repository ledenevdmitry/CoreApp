using CoreApp.OraUtils;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.ReleaseObjects
{
    public class ReleaseManager
    {
        static CVS.CVS cvs;
        public List<Release> releases { get; private set; }
        public Dictionary<int, Release> releasesDict { get; private set; }
        public DirectoryInfo homeDir { get; set; }
        public static Application excelApp;
        
        static ReleaseManager()
        {
            excelApp = new Application();
        }

        public ReleaseManager()
        {
            InitFromDB();
        }

        private void InitFromDB()
        {
            var oraReleases = ReleaseDAL.getReleases();
            releases = new List<Release>();
            releasesDict = new Dictionary<int, Release>();
            foreach (var oraRelease in oraReleases)
            {
                Release release = new Release(oraRelease.releaseId, oraRelease.releaseName);
                release.rm = this;
                releases.Add(release);
                releasesDict.Add(release.releaseId, release);
            }
        }

        public Release AddRelease(string releaseName)
        {
            if(ReleaseDAL.Contains(releaseName))
            {
                return null;
            }
            Release newRelease = new Release(ReleaseDAL.Insert(releaseName), releaseName);
            releases.Add(newRelease);
            releasesDict.Add(newRelease.releaseId, newRelease);
            return newRelease;
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
