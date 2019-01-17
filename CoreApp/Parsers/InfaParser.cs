using CoreApp.Dicts;
using CoreApp.FixpackObjects;
using CoreApp.InfaObjects;
using CoreApp.Keys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoreApp
{
    enum ObjType { Source, Target, Mapplet, Mapping, Task, Session, Worklet, Workflow, Configuration, NotAnObject };
    class InfaParser
    {
        public static string extension { get => ".sql"; }
        public string objName { get; private set; }
        XmlDocument xDoc;

        private XmlNode GetObjNode(XmlNode xRoot, ref Type type)
        {
            foreach(XmlNode node in xRoot.ChildNodes)
            {
                type = StrToObjType(node.Name);
                if (type != null)
                {
                    return node;
                }
                else
                {
                    XmlNode node2 = GetObjNode(node, ref type);
                    if(node2 != null)
                    {
                        return node2;
                    }
                }
            }
            return null;
        }

        public static InfaBaseObject CreateInfaObject(string typeStr, string objName, XmlNode node, FileInfo file, Patch patch)
        {
            Type type = StrToObjType(typeStr);
            return CreateInfaObject(type, objName, node, file, patch);
        }

        public static InfaBaseObject CreateInfaObject(Type type, string objName, XmlNode node, FileInfo file, Patch patch)
        {
            InfaBaseObject obj = (InfaBaseObject)Activator.CreateInstance(type);
            obj.objName = objName;
            obj.objNode = node;
            obj.file = file;
            obj.patch = patch;
            return obj;
        }

        public int WorkAmount(InfaObjectDict dict)
        {
            return 2 * dict.baseDict.oneToManyPairs.Count;
        }
        
        public InfaParser(List<Fixpack> fixpacks, InfaObjectDict dict)
        {
            /*
            foreach (FileInfo file in files)
            {
                if (file.Extension.Equals(".xml", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (file.Exists)
                    {
                        xDoc = new XmlDocument();
                        xDoc.Load(file.FullName);
                        XmlNode objNode = null;
                        Type type = null;
                        objNode = GetObjNode(xDoc, ref type);
                        if (objNode == null) throw new ArgumentException("В файле " + file.FullName + " не найден тип объекта");
                        objName = objNode.Attributes.GetNamedItem("NAME").Value;
                        InfaBaseObject obj = CreateInfaObject(type, objName, xDoc, file);
                        dict.AddObjectConsiderIntersections(obj);
                    }
                    else
                    {
                        dict.notFoundFiles.Add(file);
                    }
                }
            }
            */

            foreach(Fixpack fixpack in fixpacks)
            {
                foreach(Patch patch in fixpack.patches.Values)
                {
                    foreach(FileInfo file in patch.dir.EnumerateFiles("*.*", SearchOption.AllDirectories))
                    {
                        if (file.Extension.Equals(".xml", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (file.Exists)
                            {
                                xDoc = new XmlDocument();
                                xDoc.Load(file.FullName);
                                XmlNode objNode = null;
                                Type type = null;
                                objNode = GetObjNode(xDoc, ref type);
                                if (objNode == null) throw new ArgumentException("В файле " + file.FullName + " не найден тип объекта");
                                objName = objNode.Attributes.GetNamedItem("NAME").Value;
                                InfaBaseObject obj = CreateInfaObject(type, objName, xDoc, file, patch);
                                dict.AddObjectConsiderIntersections(obj);
                            }
                            else
                            {
                                dict.notFoundFiles.Add(file);
                            }
                        }
                    }
                }
            }
        }

        public delegate void ResetProgress();
        public event ResetProgress StartOfCheck, ProgressChanged, EndOfCheck;

        public void RetrieveObjectsFromFiles(IEnumerable<FileInfo> files, InfaObjectDict dict)
        {
            //StartOfCheck();
            foreach (InfaBaseObject infaObj in dict.baseDict.EnumerateObjs())
            {
                infaObj.GenerateParentNames(dict);
                //ProgressChanged();
            }
            CheckInfaDependencies(files, dict);
            //EndOfCheck();
        }

        public void CheckInfaDependencies(IEnumerable<FileInfo> files, InfaObjectDict dict)
        {
            foreach (InfaBaseObject infaObj in dict.baseDict.EnumerateObjs())
            {
                //ProgressChanged();
                foreach (InfaBaseObject parent in infaObj.parents)
                {
                    Patch p1 = parent.patch;
                    Patch p2 = infaObj.patch;
                    if (!p1.Equals(p2))
                    {
                        InfaSchema s1 = new InfaSchema(parent.file);
                        InfaSchema s2 = new InfaSchema(infaObj.file);
                        if (s1.Equals(s2))
                        {
                            dict.infaDependencies.Add(parent, infaObj);
                            if(!parent.patch.dependOn.Contains(infaObj.patch) && !infaObj.patch.dependendFrom.Contains(parent.patch))
                            {
                                dict.infaLostDependencies.Add(parent, infaObj);
                            }
                        }
                    }
                }
            }
        }

        public static Type StrToObjType(string str)
        {
            switch (str)
            {
                case "SOURCE":
                    return typeof(InfaSource);
                case "TARGET":
                    return typeof(InfaTarget);
                case "MAPPLET":
                    return typeof(InfaMapplet);
                case "MAPPING":
                    return typeof(InfaMapping);
                case "TASK":
                    return typeof(InfaTask);
                case "SESSION":
                    return typeof(InfaSession);
                case "WORKLET":
                    return typeof(InfaWorklet);
                case "WORKFLOW":
                    return typeof(InfaWorkflow);
                case "CONFIG":
                    return typeof(InfaConfiguration);
                case "SHORTCUT":
                    return typeof(InfaShortcut);
                case "TRANSFORMATION":
                    return typeof(InfaTransformation);
            }
            return null;
        }
        

    }

    
}
