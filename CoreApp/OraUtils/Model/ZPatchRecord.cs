using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils.Model
{
    class ZPatchRecord
    {
        public int ZPatchId { get; private set; }
        public int ParentId { get; private set; }
        public string ZPatchName { get; private set; }
        public string ZPatchStatus { get; private set; }

        public ZPatchRecord(int zPatchId, int parentId, string zPatchName, string zPatchStatus)
        {
            ZPatchId = zPatchId;
            ParentId = parentId;
            ZPatchName = zPatchName;
            ZPatchStatus = zPatchStatus;
        }
    }
}
