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
            var oraReleases = ReleaseDAL.getReleases();
            foreach(var oraRelease in oraReleases)
            {
                Release release = new Release(oraRelease.releaseId, oraRelease.releaseName);
            }
        }

    }
}
