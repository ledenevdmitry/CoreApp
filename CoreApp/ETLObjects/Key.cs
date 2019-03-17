using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Keys
{
    public class Key
    {
        public Key(string objName, string objType)
        {
            this.ObjName = objName;
            this.ObjType = objType;
        }

        public Key()
        {

        }

        public string ObjName { get; set; }
        public string ObjType { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            Key other = (Key)obj;
            return ObjName.Equals(other.ObjName, StringComparison.CurrentCultureIgnoreCase) &&
                   ObjType.Equals(other.ObjType, StringComparison.CurrentCultureIgnoreCase);

        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ObjName.GetHashCode();
                hash = hash * 23 + ObjType.GetHashCode();
                return hash;
            }
        }
    }
}
