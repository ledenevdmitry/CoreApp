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
        public ZPatch patch { get; set; }

        public ETLObject()
        {

        }

        public ETLObject(string objName, string objType, ZPatch patch) : base(objName, objType)
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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            ETLObject other = (ETLObject)obj;

            return patch.ZPatchName.Equals(other.patch.ZPatchName, StringComparison.CurrentCultureIgnoreCase);
        }

    }
}
