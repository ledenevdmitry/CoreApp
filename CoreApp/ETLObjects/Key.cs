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
            this.objName = objName;
            this.objType = objType;
        }

        public Key()
        {

        }

        public string objName { get; set; }
        public string objType { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            Key other = (Key)obj;
            return objName.Equals(other.objName, StringComparison.CurrentCultureIgnoreCase) &&
                   objType.Equals(other.objType, StringComparison.CurrentCultureIgnoreCase);

        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + objName.GetHashCode();
                hash = hash * 23 + objType.GetHashCode();
                return hash;
            }
        }
    }
}
