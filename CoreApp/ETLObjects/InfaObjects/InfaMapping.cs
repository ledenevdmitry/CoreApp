using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoreApp.InfaObjects
{
    class InfaMapping : InfaBaseObject
    {
        //должны найтись:
        //тэг INSTANCE атрибут TYPE="TRANSFORMATION"
        //тэг INSTANCE атрибут TYPE="MAPPLET"
        //тэг INSTANCE атрибут TYPE="SOURCE"
        //TARGET
        public InfaMapping(string objName) : base(objName)
        {
            infaParentTypes = infaStaticParentTypes;
        }

        public static HashSet<string> infaStaticParentTypes = new HashSet<string>(new string[] { "CONFIG", "TRANSFORMATION", "MAPPLET", "SOURCE" , "TARGET"});

        public InfaMapping() : base()
        {
            infaParentTypes = infaStaticParentTypes;
        }
    }
}
