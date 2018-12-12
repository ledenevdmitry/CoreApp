using CoreApp.Dicts;
using CoreApp.FixpackObjects;
using CoreApp.InfaObjects;
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

        public static InfaBaseObject CreateInfaObject(string typeStr, string objName, XmlNode node, FileInfo file)
        {
            Type type = StrToObjType(typeStr);
            return CreateInfaObject(type, objName, node, file);
        }

        public static InfaBaseObject CreateInfaObject(Type type, string objName, XmlNode node, FileInfo file)
        {
            InfaBaseObject obj = (InfaBaseObject)Activator.CreateInstance(type);
            obj.objName = objName;
            obj.objNode = node;
            obj.file = file;
            return obj;
        }

        public int WorkAmount(InfaObjectDict dict)
        {
            return 2 * dict.baseDict.objFilesPairs.Count;
        }

        //dict должен быть с компаратором на название объекта
        public InfaParser(List<FileInfo> files, InfaObjectDict dict)
        {
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
                        dict.AddObjectConsiderIntersections(obj, new Patch(file));
                    }
                    else
                    {
                        dict.notFoundFiles.Add(file);
                    }
                }
            }
        }

        public delegate void ResetProgress();
        public event ResetProgress StartOfCheck, ProgressChanged, EndOfCheck;

        public void RetrieveObjectsFromFiles(List<FileInfo> files, InfaObjectDict dict)
        {
            StartOfCheck();
            foreach (KeyValuePair<InfaBaseObject, HashSet<Patch>> kvp in dict.baseDict.objFilesPairs)
            {
                kvp.Key.GenerateParentNames(dict);
                ProgressChanged();
            }
            CheckInfaDependencies(files, dict);
            EndOfCheck();
        }

        public void CheckInfaDependencies(List<FileInfo> files, InfaObjectDict dict)
        {
            foreach (KeyValuePair<InfaBaseObject, HashSet<Patch>> kvp in dict.baseDict.objFilesPairs)
            {
                ProgressChanged();
                InfaBaseObject obj = kvp.Key;
                foreach (InfaBaseObject parent in obj.parents)
                {
                    Patch p1 = new Patch(parent.file);
                    Patch p2 = new Patch(obj.file);
                    if (!p1.Equals(p2))
                    {
                        dict.infaDependencies.Add(new KeyValuePair<InfaBaseObject, InfaBaseObject>(parent, obj));
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
