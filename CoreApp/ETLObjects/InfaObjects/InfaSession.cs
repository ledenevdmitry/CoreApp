﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.InfaObjects
{
    class InfaSession : InfaBaseObject
    {
        public InfaSession(string objName) : base(objName)
        {
            infaParentTypes = infaStaticParentTypes;
        }

        public static HashSet<string> infaStaticParentTypes = new HashSet<string>(new string[] { "CONFIG", "TRANSFORMATION", "MAPPLET", "SOURCE", "TARGET", "MAPPING" });

        public InfaSession() : base()
        {
            infaParentTypes = infaStaticParentTypes;
        }
    }
}
