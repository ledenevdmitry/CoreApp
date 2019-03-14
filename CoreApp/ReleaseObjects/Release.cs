using CoreApp.FixpackObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreApp.CVS;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Security.Principal;
using CoreApp.OraUtils;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;

namespace CoreApp.ReleaseObjects
{

    public class Release
    {
        public int releaseId { get; private set; }

        public override int GetHashCode()
        {
            return releaseId.GetHashCode();
        }

        public override string ToString()
        {
            return releaseName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Release)) return false;
            return ((Release)obj).releaseId == releaseId;
        }

        public string releaseName { get; private set; }
        public string releaseStatus { get; private set; }
        public List<CPatch> CPatches { get; private set; } //отсортированный на DAL
        public Dictionary<int, CPatch> CPatchesDict { get; private set; } //для поиска
        public ReleaseManager rm;



        public CPatch getCPatchById(int id)
        {
            return CPatchesDict[id];
        }

        public Release(int releaseId, string releaseName, ReleaseManager rm)
        {
            InitFromDB(releaseId, releaseName);
            this.rm = rm;
        }        

        public void InitCPatches()
        {
            var oraCPatches = CPatchDAL.getCPatchesByRelease(releaseId);

            CPatchStatuses status;
            EnvCodes kod_sredy;
            CPatches = new List<CPatch>();
            CPatchesDict = new Dictionary<int, CPatch>();

            foreach (var oraCPatch in oraCPatches)
            {

                if(!Enum.TryParse(oraCPatch.CPatchStatus, out status))
                {
                    status = CPatchStatuses.UNDEFINED;
                }

                if(!Enum.TryParse(oraCPatch.Kod_Sredy, out kod_sredy))
                {
                    kod_sredy = EnvCodes.UNDEFINED;
                }

                CPatch cpatch = new CPatch(
                    oraCPatch.CPatchId, 
                    oraCPatch.CPatchName,
                    status, 
                    kod_sredy, 
                    this);                

                CPatches.Add(cpatch);
                CPatchesDict.Add(cpatch.CPatchId, cpatch);
            }

            foreach (CPatch cpatch in CPatches)
            {
                cpatch.SetDependencies();

                foreach (ZPatch zpatch in cpatch.ZPatches)
                {
                    zpatch.SetDependencies();
                }
            }
        }

        private void InitFromDB(int releaseId, string releaseName)
        {
            CPatches = new List<CPatch>();
            CPatchesDict = new Dictionary<int, CPatch>();
            this.releaseId = releaseId;
            this.releaseName = releaseName;
        }

        DirectoryInfo localDir;
        public static CVS.CVS cvs;

        private void setAttributesNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
                setAttributesNormal(subDir);
            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }

        public void DeleteLocal()
        {
            if (localDir.Exists)
            {
                setAttributesNormal(localDir);
                localDir.Delete(true);
            }
        }

        public void SetLocalDir(DirectoryInfo localDir)
        {
            this.localDir = localDir;
        }

        public CPatch AddCPatch(Release release, FileInfo excelFile)
        {
            CPatch newCPatch = CPatch.CreateNewFromExcel(release, excelFile);
            CPatches.Add(newCPatch);
            CPatchesDict.Add(newCPatch.CPatchId, newCPatch);
            newCPatch.release = this;

            return newCPatch;
        }

        public void Rename(string newName)
        {
            if (newName != null && newName != releaseName)
            {
                releaseName = newName;
                ReleaseDAL.Update(releaseId, newName);
            }
        }

        public Graph DrawGraph()
        {
            Graph graph = new Graph(); 
            foreach(CPatch cpatch in CPatches)
            {
                Node node = new Node(cpatch.CPatchId.ToString());
                node.Label.Text = cpatch.CPatchName;
                node.Attr.FillColor = Color.LightGreen;
                graph.AddNode(node);
            }

            foreach (CPatch cpatch in CPatches)
            {
                foreach(CPatch depFrom in cpatch.dependenciesFrom)
                {
                    graph.AddEdge(depFrom.CPatchId.ToString(), cpatch.CPatchId.ToString());
                }
            }

            return graph;
        }
    }
}
