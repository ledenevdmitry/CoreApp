using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils.Model
{
    class ReleaseRecord
    {
        public int releaseId { get; private set; }
        public string releaseName { get; private set; }

        public ReleaseRecord(int releaseId, string releaseName)
        {
            this.releaseId = releaseId;
            this.releaseName = releaseName;
        }
    }
}
