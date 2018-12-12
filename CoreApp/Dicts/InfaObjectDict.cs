using CoreApp.Comparers;
using CoreApp.InfaObjects;
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
        public Dictionary<InfaBaseObject, HashSet<InfaBaseObject>> notFoundObject { get; private set; }
        public List<KeyValuePair<InfaBaseObject, InfaBaseObject>> orderMistakes;
        public InfaObjectDict()
        {
            baseDict = new ObjFilePairs<InfaBaseObject>(new InfaObjectNameComparer());
            intersections = new ObjFilePairs<InfaBaseObject>(new InfaObjectNameComparer());
            orderMistakes  = new List<KeyValuePair<InfaBaseObject, InfaBaseObject>>();
            notFoundObject = new Dictionary<InfaBaseObject, HashSet<InfaBaseObject>>(new InfaObjectNameComparer());
        }

        //!!!!!!!!!!!!!!!!!Обязательная вторая проверка
    }
}
