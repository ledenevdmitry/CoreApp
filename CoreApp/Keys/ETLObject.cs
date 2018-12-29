using CoreApp.FixpackObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Keys
{
    public class ETLObject : Key
    {
        public Patch patch { get; set; }

        public ETLObject()
        {

        }

        public ETLObject(string objName, string objType, Patch patch) : base(objName, objType)
        {
            this.patch = patch;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return base.GetHashCode() * 23 + patch.GetHashCode();
            }
        }
    }
}
