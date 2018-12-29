using CoreApp.Comparers;
using CoreApp.InfaObjects;
using CoreApp.Keys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Dicts
{
    class InfaObjectDict : ObjectDict<InfaBaseObject>
    {
        public ObjObjsPairs<InfaBaseObject> infaDependencies;
        public InfaObjectDict()
        {
            baseDict = new ObjPatchPairs<InfaBaseObject>();
            intersections = new ObjPatchPairs<InfaBaseObject>();
            infaDependencies = new ObjObjsPairs<InfaBaseObject>();
        }

            //!!!!!!!!!!!!!!!!!Обязательная вторая проверка
        }
}
