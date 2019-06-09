using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.InfaObjects
{
    class InfaMapplet : InfaBaseObject
    {
        public InfaMapplet(string objName) : base(objName)
        {
            infaPossibleParentTypes = infaStaticParentTypes;
        }

        public static HashSet<string> infaStaticParentTypes = new HashSet<string>(new string[] { "CONFIG", "TRANSFORMATION", "SOURCE", "TARGET"} );

        public InfaMapplet() : base()
        {
            infaPossibleParentTypes = infaStaticParentTypes;
        }
    }
}
