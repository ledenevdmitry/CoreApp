using CoreApp.Comparers;
using CoreApp.Dicts;
using CoreApp.Keys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoreApp.InfaObjects
{
    class InfaBaseObject : ETLObject
    {    
        

        public InfaBaseObject()
        {
            parents = new HashSet<InfaBaseObject>(new InfaObjectFileNameComparer()); //при добавлении родителей объекты уникальны по названию файла (тк может быть 2 одинаковых сущности, которые выкатываются два раза)
            objType = GetType().ToString();
        }

        public InfaBaseObject(string objName) : this()
        {
            this.objName = objName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            InfaBaseObject other = (InfaBaseObject)obj;

            return file.FullName.Equals(other.file.FullName, StringComparison.CurrentCultureIgnoreCase);
        }

        public HashSet<InfaBaseObject> parents { get; set; }
        public HashSet<string> infaParentTypes;
        public FileInfo file { get; set; }
        public XmlNode objNode { get; set; }

        public void GenerateParentNames(InfaObjectDict dict)
        {
            GenerateParentNames(objNode, dict);
        }


        public void GenerateParentNames(XmlNode currNode, InfaObjectDict dict)
        {
            foreach (XmlNode subNode in currNode)
            {
                if (subNode.Name == "INSTANCE")
                {
                    foreach(XmlAttribute attr in subNode.Attributes)
                    {
                        if(attr.Name == "TYPE" && infaParentTypes.Contains(attr.Value))
                        {
                            InfaBaseObject parentPattern = InfaParser.CreateInfaObject(subNode.Attributes.GetNamedItem("TYPE").Value, subNode.Attributes.GetNamedItem("NAME").Value, subNode, null, null);
                            if (dict.baseDict.oneToManyPairs.ContainsKey(parentPattern))
                            {
                                HashSet<InfaBaseObject> newParents = new HashSet<InfaBaseObject>(dict.baseDict.oneToManyPairs[parentPattern].Keys);
                                parents.UnionWith(newParents);
                            }
                        }
                    }
                }
                else
                {
                    GenerateParentNames(subNode, dict);
                }
            }
        }
    }
}
