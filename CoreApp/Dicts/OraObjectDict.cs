using CoreApp.Comparers;
using CoreApp.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Dicts
{
    class OraObjectDict : ObjectDict<OraObject>
    {
        public OraObjectDict()
        {
            baseDict = new ObjPatchPairs<OraObject>();
            intersections = new ObjPatchPairs<OraObject>();
        }
    }
}
