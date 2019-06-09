using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.InfaObjects
{
    class InfaConfiguration : InfaBaseObject
    {
        public InfaConfiguration(string objName) : base(objName)
        {
            infaPossibleParentTypes = infaStaticParentTypes;
        }

        public static HashSet<string> infaStaticParentTypes = new HashSet<string>(new string[] { });

        public InfaConfiguration() : base()
        {
            infaPossibleParentTypes = infaStaticParentTypes;
        }
    }
}
