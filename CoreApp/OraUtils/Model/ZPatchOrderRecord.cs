using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.OraUtils.Model
{
    class ZPatchOrderRecord
    {
        public int zpatchId { get; private set; }
        public int zpatchOrder { get; private set; }

        public ZPatchOrderRecord(int zpatchId, int zpatchOrder)
        {
            this.zpatchId = zpatchId;
            this.zpatchOrder = zpatchOrder;
        }
    }
}
