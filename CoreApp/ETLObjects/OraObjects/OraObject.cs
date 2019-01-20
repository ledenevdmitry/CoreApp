using CoreApp.FixpackObjects;
using CoreApp.Keys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class OraObject : ETLObject
    {
        public OraObject(string objName, string objType, Patch patch) : base(objName, objType, patch)
        {

        }
    }
}
