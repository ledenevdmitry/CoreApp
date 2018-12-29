using CoreApp.FixpackObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Comparers
{
    public class PatchEqualityComparer : IEqualityComparer<Patch>
    {
        public bool Equals(Patch p1, Patch p2)
        {
            return p1.Equals(p2);
        }

        public int GetHashCode(Patch p)
        {
            return p.name.ToUpper().GetHashCode();
        }
    }
}
