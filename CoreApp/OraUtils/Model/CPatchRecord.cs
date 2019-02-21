using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils.Model
{
    class CPatchRecord
    {
        public int CPatchId { get; private set; }
        public string CPatchName { get; private set; }
        public int parentId { get; private set; }
        public string CPatchStatus { get; private set; }
        public string Kod_Sredy { get; private set; }

        public CPatchRecord(int cPatchId, string cPatchName, int parentId, string cPatchStatus, string kod_Sredy)
        {
            CPatchId = cPatchId;
            CPatchName = cPatchName;
            this.parentId = parentId;
            CPatchStatus = cPatchStatus;
            Kod_Sredy = kod_Sredy;
        }
    }
}
