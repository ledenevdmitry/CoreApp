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
        public List<CPatch> cpatches { get; private set; } //отсортированный на DAL
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
            cpatches = new List<CPatch>();
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

                cpatches.Add(cpatch);
                CPatchesDict.Add(cpatch.CPatchId, cpatch);
            }

            foreach (CPatch cpatch in cpatches)
            {
                cpatch.SetDependencies();

                foreach (ZPatch zpatch in cpatch.zpatches)
                {
                    zpatch.SetDependencies();
                }
            }
        }

        private void InitFromDB(int releaseId, string releaseName)
        {
            cpatches = new List<CPatch>();
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

        public void Delete()
        {
            ReleaseDAL.Delete(releaseId);
            rm.releases.Remove(this);
            rm.releasesDict.Remove(releaseId);
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
            foreach(CPatch cpatch in cpatches)
            {
                Node node = new Node(cpatch.CPatchId.ToString());
                node.Label.Text = cpatch.CPatchName;
                node.Attr.FillColor = Color.LightGreen;
                graph.AddNode(node);
            }

            foreach (CPatch cpatch in cpatches)
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
