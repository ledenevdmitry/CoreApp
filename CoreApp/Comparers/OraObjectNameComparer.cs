using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Comparers
{
    internal class OraObjectNameComparer : IEqualityComparer<OraObject>
    {
        public bool Equals(OraObject o1, OraObject o2)
        {
            return o1.objName.Equals(o2.objName, StringComparison.CurrentCultureIgnoreCase) && o1.type.Equals(o2.type, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(OraObject o)
        {
            return o.objName.ToUpper().GetHashCode();
        }
    }
}
