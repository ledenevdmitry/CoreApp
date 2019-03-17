using CoreApp.Dicts;
using CoreApp.InfaObjects;
using CoreApp.Keys;
using CoreApp.ReleaseObjects;
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
        public static string Extension { get => ".xml"; }
        public string ObjName { get; private set; }
        XmlDocument xDoc;
        private InfaObjectDict dict;

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

        public static InfaBaseObject CreateInfaObject(string typeStr, string objName, XmlNode node, FileInfo file, ZPatch patch)
        {
            Type type = StrToObjType(typeStr);
            return CreateInfaObject(type, objName, node, file, patch);
        }

        public static InfaBaseObject CreateInfaObject(Type type, string objName, XmlNode node, FileInfo file, ZPatch patch)
        {
            InfaBaseObject obj = (InfaBaseObject)Activator.CreateInstance(type);
            obj.ObjName = objName;
            obj.ObjNode = node;
            obj.File = file;
            obj.Patch = patch;
            return obj;
        }

        public InfaParser(Release release, InfaObjectDict dict)
        {
            this.dict = dict;
            foreach(CPatch cpatch in release.CPatches)
            {
                foreach(ZPatch zpatch in cpatch.ZPatches)
                {
                    foreach(FileInfo file in zpatch.Dir.EnumerateFiles("*.*", SearchOption.AllDirectories))
                    {
                        if (file.Extension.Equals(Extension, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (file.Exists)
                            {
                                xDoc = new XmlDocument();
                                xDoc.Load(file.FullName);
                                XmlNode objNode = null;
                                Type type = null;
                                objNode = GetObjNode(xDoc, ref type);
                                if (objNode == null) throw new ArgumentException("В файле " + file.FullName + " не найден тип объекта");
                                ObjName = objNode.Attributes.GetNamedItem("NAME").Value;
                                InfaBaseObject obj = CreateInfaObject(type, ObjName, xDoc, file, zpatch);
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

        public void Check()
        {
            //StartOfCheck();
            foreach (InfaBaseObject infaObj in dict.baseDict.EnumerateObjs())
            {
                infaObj.GenerateParentNames(dict);
                //ProgressChanged();
            }
            CheckInfaDependencies(dict);
            //EndOfCheck();
        }

        public void CheckInfaDependencies(InfaObjectDict dict)
        {
            foreach (InfaBaseObject infaObj in dict.baseDict.EnumerateObjs())
            {
                //ProgressChanged();
                foreach (InfaBaseObject parent in infaObj.Parents)
                {
                    ZPatch p1 = parent.Patch;
                    ZPatch p2 = infaObj.Patch;
                    if (!p1.Equals(p2))
                    {
                        InfaSchema s1 = new InfaSchema(parent.File);
                        InfaSchema s2 = new InfaSchema(infaObj.File);
                        if (s1.Equals(s2))
                        {
                            dict.infaDependencies.Add(parent, infaObj);
                            if(!parent.Patch.DependenciesTo.Contains(infaObj.Patch) && !infaObj.Patch.DependenciesFrom.Contains(parent.Patch))
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
