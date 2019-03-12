using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils.Model
{
    class ZPatchOrderRecord
    {
        int zpatchId;
        int zpatchOrder;

        public ZPatchOrderRecord(int zpatchId, int zpatchOrder)
        {
            this.zpatchId = zpatchId;
            this.zpatchOrder = zpatchOrder;
        }
    }
}
