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
        public int ReleaseId { get; private set; }

        public override int GetHashCode()
        {
            return ReleaseId.GetHashCode();
        }

        public override string ToString()
        {
            return ReleaseName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Release)) return false;
            return ((Release)obj).ReleaseId == ReleaseId;
        }

        public string ReleaseName { get; private set; }
        public string ReleaseStatus { get; private set; }
        public List<CPatch> CPatches { get; private set; } //отсортированный на DAL
        public Dictionary<int, CPatch> CPatchesDict { get; private set; } //для поиска
        public ReleaseManager rm;



        public CPatch GetCPatchById(int id)
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
            var oraCPatches = CPatchDAL.GetCPatchesByRelease(ReleaseId);

            CPatches = new List<CPatch>();
            CPatchesDict = new Dictionary<int, CPatch>();

            foreach (var oraCPatch in oraCPatches)
            {
                CPatch cpatch = new CPatch(
                    oraCPatch.CPatchId, 
                    oraCPatch.CPatchName,
                    oraCPatch.CPatchStatus, 
                    oraCPatch.Kod_Sredy, 
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
            this.ReleaseId = releaseId;
            this.ReleaseName = releaseName;
        }

        DirectoryInfo localDir;
        public static CVS.CVS cvs;

        public void Delete()
        {
            ReleaseDAL.Delete(ReleaseId);
            rm.releases.Remove(this);
            rm.releasesDict.Remove(ReleaseId);
        }

        public void SetLocalDir(DirectoryInfo localDir)
        {
            this.localDir = localDir;
        }

        public CPatch AddCPatch(Release release, FileInfo excelFile)
        {
            CPatch newCPatch = CPatch.CreateNewFromExcel(release, excelFile);

            return newCPatch;
        }

        public void Rename(string newName)
        {
            if (newName != null && newName != ReleaseName)
            {
                ReleaseName = newName;
                ReleaseDAL.Update(ReleaseId, newName);
            }
        }

        public void Download()
        {
            foreach(CPatch cpatch in CPatches)
            {
                cpatch.Download();
            }
        }

        public Graph DrawGraph()
        {
            Graph graph = new Graph(); 
            foreach(CPatch cpatch in CPatches)
            {
                Node node = new Node(cpatch.CPatchId.ToString());
                node.Label.Text = cpatch.CPatchName.Replace(' ', '\n');

                node.Attr.FillColor = Color.LightGreen;
                graph.AddNode(node);
            }

            foreach (CPatch cpatch in CPatches)
            {
                foreach(CPatch depFrom in cpatch.DependenciesFrom)
                {
                    graph.AddEdge(depFrom.CPatchId.ToString(), cpatch.CPatchId.ToString());
                }
            }

            return graph;
        }
    }
}
