using CoreApp.Comparers;
using CoreApp.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Dicts
{
    class OraObjectDict : ObjectDict
    {
        public OraObjectDict()
        {
            baseDict = new ETLDict();
            intersections = new ETLDict();
        }

    }
}
