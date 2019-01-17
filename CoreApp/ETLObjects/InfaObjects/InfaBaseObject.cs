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
            parents = new HashSet<ETLObject>(/*new InfaObjectFileNameComparer()*/); //при добавлении родителей объекты уникальны по названию файла (тк может быть 2 одинаковых сущности, которые выкатываются два раза)
            objType = GetType().ToString();
        }

        public InfaBaseObject(string objName) : this()
        {
            this.objName = objName;
        }        

        public HashSet<ETLObject> parents { get; set; }
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
                            ETLObject parentPattern = InfaParser.CreateInfaObject(subNode.Attributes.GetNamedItem("TYPE").Value, subNode.Attributes.GetNamedItem("NAME").Value, subNode, null, null);
                            Key key = new Key(parentPattern.objName, parentPattern.objType);
                            if (dict.baseDict.oneToManyPairs.ContainsKey(key))
                            {
                                HashSet<ETLObject> newParents = new HashSet<ETLObject>(dict.baseDict.oneToManyPairs[key].Keys);
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
