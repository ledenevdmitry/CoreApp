using CoreApp.InfaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Comparers
{
    internal class InfaObjectNameComparer : IEqualityComparer<InfaBaseObject>
    {
        public bool Equals(InfaBaseObject o1, InfaBaseObject o2)
        {
            return o1.objName.Equals(o2.objName, StringComparison.CurrentCultureIgnoreCase) && o1.GetType() == o2.GetType();
        }

        public int GetHashCode(InfaBaseObject o)
        {
            return o.objName.ToUpper().GetHashCode();
        }
    }
}
