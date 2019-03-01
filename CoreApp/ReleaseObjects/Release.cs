﻿using CoreApp.FixpackObjects;
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

namespace CoreApp.ReleaseObjects
{

    public class Release
    {
        public int releaseId { get; private set; }

        public override int GetHashCode()
        {
            return releaseId.GetHashCode();
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



        public CPatch getCPatchById(int id)
        {
            return CPatchesDict[id];
        }

        public Release(int releaseId, string releaseName)
        {
            InitFromDB(releaseId, releaseName);
        }        

        public void InitCPatches()
        {
            var oraCPatches = CPatchDAL.getCPatchesByRelease(releaseId);

            CPatchStatuses status;
            CPatches = new List<CPatch>();
            CPatchesDict = new Dictionary<int, CPatch>();

            foreach (var oraCPatch in oraCPatches)
            {

                if(!Enum.TryParse(oraCPatch.CPatchStatus, out status))
                {
                    status = CPatchStatuses.UNDEFINED;
                }

                CPatch cpatch = new CPatch(
                    oraCPatch.CPatchId, 
                    oraCPatch.CPatchName,
                    status, 
                    oraCPatch.Kod_Sredy);
                CPatches.Add(cpatch);
                CPatchesDict.Add(cpatch.CPatchId, cpatch);
            }

            foreach (CPatch cpatch in CPatches)
            {
                cpatch.SetDependencies();
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

        public void AddCPatch(Release release, FileInfo excelFile)
        {
            CPatch newCPatch = CPatch.CreateNewFromExcel(release, excelFile);
            CPatches.Add(newCPatch);
        }

        public void Rename(string newName)
        {
            releaseName = newName;
            ReleaseDAL.Update(releaseId, newName);
        }
    }
}
