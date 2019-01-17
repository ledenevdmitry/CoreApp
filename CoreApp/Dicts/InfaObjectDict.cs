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
    class InfaObjectDict : ObjectDict
    {
        public ObjToParentsDict infaDependencies;
        public InfaObjectDict()
        {
            baseDict = new ETLDict();
            intersections = new ETLDict();
            infaDependencies = new ObjToParentsDict();
        }

        //!!!!!!!!!!!!!!!!!Обязательная вторая проверка
    }


}
