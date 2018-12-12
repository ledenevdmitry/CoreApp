using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.InfaObjects
{
    class InfaShortcut : InfaBaseObject
    {
        public InfaShortcut(string objName) : base(objName)
        {
            infaParentTypes = infaStaticParentTypes;
        }

        public static HashSet<string> infaStaticParentTypes = new HashSet<string>(new string[] { "CONFIG", "TRANSFORMATION", "MAPPLET", "SOURCE", "TARGET", "MAPPING", "TASK", "SESSION", "WORKLET", "WORKFLOW" });

        public InfaShortcut() : base()
        {
            infaParentTypes = infaStaticParentTypes;
        }
    }
}
