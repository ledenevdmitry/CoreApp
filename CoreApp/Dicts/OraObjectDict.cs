﻿using CoreApp.Comparers;
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
            baseDict = new ObjFilePairs<OraObject>(new OraObjectNameComparer());
            intersections = new ObjFilePairs<OraObject>(new OraObjectNameComparer());
        }
    }
}
