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
        FileInfo file;

        public OraObject(string objName, string objType, FileInfo file) : base(objName, objType, new Patch(file.FullName))
        {
            this.file = file;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            OraObject other = (OraObject)obj;

            return base.Equals(obj) && file.FullName.Equals(other.file.FullName, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
