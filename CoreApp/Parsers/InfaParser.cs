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

        public void RetrieveObjectsFromFiles(List<FileInfo> files, InfaObjectDict dict)
        {
            StartOfCheck();
            foreach (var kvp in dict.baseDict.oneToManyPairs)
            {
                kvp.Key.GenerateParentNames(dict);
                ProgressChanged();
            }
            CheckInfaDependencies(files, dict);
            EndOfCheck();
        }

        public void CheckInfaDependencies(List<FileInfo> files, InfaObjectDict dict)
        {
            foreach (var kvp in dict.baseDict.oneToManyPairs)
            {
                ProgressChanged();
                InfaBaseObject obj = kvp.Key;
                foreach (InfaBaseObject parent in obj.parents)
                {
                    Patch p1 = new Patch(parent.file.FullName);
                    Patch p2 = new Patch(obj.file.FullName);
                    if (!p1.Equals(p2))
                    {
                        InfaSchema s1 = new InfaSchema(parent.file);
                        InfaSchema s2 = new InfaSchema(obj.file);
                        if (s1.Equals(s2))
                        {
                            dict.infaDependencies.Add(parent, obj);
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
