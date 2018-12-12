using CoreApp.InfaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Comparers
{
    internal class InfaObjectFileNameComparer : IEqualityComparer<InfaBaseObject>
    {
        public bool Equals(InfaBaseObject o1, InfaBaseObject o2)
        {
            return o1.file.FullName.Equals(o2.file.FullName, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(InfaBaseObject o)
        {
            return o.file.FullName.GetHashCode();
        }
    }
}
