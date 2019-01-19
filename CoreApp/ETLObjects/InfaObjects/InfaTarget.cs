using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.InfaObjects
{
    class InfaTarget : InfaBaseObject
    {
        public InfaTarget(string objName) : base(objName)
        {
            infaParentTypes = infaStaticParentTypes;
        }

        public static HashSet<string> infaStaticParentTypes = new HashSet<string>(new string[] { "CONFIG" });

        public InfaTarget() : base()
        {
            infaParentTypes = infaStaticParentTypes;
        }
    }
}
