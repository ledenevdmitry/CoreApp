﻿using CoreApp.Comparers;
using CoreApp.Dicts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoreApp.InfaObjects
{
    class InfaBaseObject
    {
        public InfaBaseObject()
        {
            parents = new HashSet<InfaBaseObject>(new InfaObjectFileNameComparer()); //при добавлении родителей объекты уникальны по названию файла (тк может быть 2 одинаковых сущности, которые выкатываются два раза)

        }

        public InfaBaseObject(string objName) : this()
        {
            this.objName = objName;
        }

        public string objName;
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
                            InfaBaseObject parentPattern = InfaParser.CreateInfaObject(subNode.Attributes.GetNamedItem("TYPE").Value, subNode.Attributes.GetNamedItem("NAME").Value, subNode, null);
                            if (dict.baseDict.objFilesPairs.ContainsKey(parentPattern))
                            {
                                var currParents = dict.baseDict.objFilesPairs.Where(kvp => new InfaObjectNameComparer().Equals(kvp.Key, parentPattern)).Select(kvp => kvp.Key);
                                parents.UnionWith(currParents);
                            }
                            else
                            {
                                if(!dict.notFoundObject.ContainsKey(this))
                                {
                                    dict.notFoundObject[this] = new HashSet<InfaBaseObject>();
                                }
                                dict.notFoundObject[this].Add(parentPattern);
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
