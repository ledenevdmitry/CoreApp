using CoreApp.ReleaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Keys
{
    public class ETLObject : Key
    {
        public ZPatch Patch { get; set; }

        public ETLObject()
        {

        }

        public ETLObject(string objName, string objType, ZPatch patch) : base(objName, objType)
        {
            this.Patch = patch;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return base.GetHashCode() * 23 + Patch.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            ETLObject other = (ETLObject)obj;

            return Patch.ZPatchName.Equals(other.Patch.ZPatchName, StringComparison.CurrentCultureIgnoreCase);
        }

    }
}
