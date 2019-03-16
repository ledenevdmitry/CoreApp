﻿using CoreApp.OraUtils;
using CoreApp.OraUtils.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp.ReleaseObjects
{
    public enum ZPatchStatuses { UNDEFINED, OPEN, READY, INSTALLED, ERROR };

    public class ZPatch
    {
        public string ZPatchName { get; private set; }
        public ZPatchStatuses ZPatchStatus { get; set; }
        public DirectoryInfo dir { get; private set; }
        public string pathToPatch { get; private set; }
        private static Regex ATCPatchRegex = new Regex(@"\\((\d+\-)?Z(\d+.*?))");
        private static Regex BankPatchRegex = new Regex(@"\\(C(\d+).*?)");
        public bool IsATCPatch { get; private set; }
        public HashSet<ZPatch> dependenciesFrom { get; private set; }
        public HashSet<ZPatch> dependenciesTo { get; private set; }
        public List<FileInfo> objs;
        public int rank;
        public int ZPatchId { get; set; }
        public CPatch cpatch;
        public int excelFileRowId;
        public static CVS.CVS cvs;

        public override int GetHashCode()
        {
            return ZPatchId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(ZPatch)) return false;
            return ((ZPatch)obj).ZPatchName == ZPatchName;
        }

        public ZPatch(string ZPatchName, int CPatch, HashSet<ZPatch> dependenciesFrom, HashSet<ZPatch> dependenciesTo, ZPatchStatuses ZPatchStatus)
        {
            this.ZPatchName = ZPatchName;
            this.dependenciesFrom = dependenciesFrom;
            this.dependenciesTo = dependenciesTo;
            this.ZPatchStatus = ZPatchStatus;
        }

        public void Rename(string newName)
        {
            if(newName != null && ZPatchName != newName)
            {
                ZPatchName = newName;
                ZPatchDAL.UpdateName(ZPatchId, newName);
            }
        }

        public ZPatch(CPatch cpatch, string ZPatchName, int ZPatchId, ZPatchStatuses ZPatchStatus)
        {
            this.cpatch = cpatch;
            this.ZPatchId = ZPatchId;
            this.ZPatchName = ZPatchName;
            this.ZPatchStatus = ZPatchStatus;
        }

        public void Delete()
        {
            foreach (ZPatch zpatchFrom in dependenciesFrom)
            {
                zpatchFrom.dependenciesTo.Remove(this);
                ZPatchDAL.DeleteDependency(zpatchFrom.ZPatchId, ZPatchId);
            }

            foreach (ZPatch zpatchTo in dependenciesTo)
            {
                zpatchTo.dependenciesFrom.Remove(this);
                CPatchDAL.DeleteDependency(ZPatchId, zpatchTo.ZPatchId);
            }

            ZPatchDAL.DeleteZPatch(ZPatchId);
            cpatch.zpatches.Remove(this);
            cpatch.ZPatchesDict.Remove(ZPatchId);
        }

        public void Move(CPatch newCPatch)
        {
            if (cpatch != newCPatch)
            {
                cpatch.zpatches.Remove(this);
                cpatch.ZPatchesDict.Remove(ZPatchId);

                newCPatch.zpatches.Add(this);
                newCPatch.ZPatchesDict.Add(ZPatchId, this);

                ZPatchDAL.UpdateCPatch(cpatch.CPatchId, newCPatch.CPatchId);
            }
        }

        public void DeleteDependencyFrom(ZPatch zpatchFrom)
        {
            dependenciesFrom.Remove(zpatchFrom);
            ZPatchDAL.DeleteDependency(zpatchFrom.ZPatchId, ZPatchId);
        }

        public void DeleteDependencyTo(ZPatch zpatchTo)
        {
            dependenciesTo.Remove(zpatchTo);
            ZPatchDAL.DeleteDependency(ZPatchId, zpatchTo.ZPatchId);
        }

        public void AddDependencyFrom(ZPatch zpatchFrom)
        {
            dependenciesFrom.Add(zpatchFrom);
            ZPatchDAL.AddDependency(zpatchFrom.ZPatchId, ZPatchId);
        }

        public void AddDependencyTo(ZPatch zpatchTo)
        {
            dependenciesTo.Add(zpatchTo);
            ZPatchDAL.AddDependency(ZPatchId, zpatchTo.ZPatchId);
        }

        public static bool HaveTransitiveDependency(ZPatch zpatchFrom, ZPatch zpatchTo)
        {
            foreach (ZPatch subPatch in zpatchFrom.dependenciesTo)
            {
                if (subPatch.Equals(zpatchTo))
                    return true;
                else
                    return HaveTransitiveDependency(subPatch, zpatchTo);
            }

            return false;
        }


        private bool GetCVSPath(out string path)
        {
            string root;

            if (ZPatchStatus == ZPatchStatuses.OPEN)
            {
                root = CVSProjectsDAL.GetPath(EnvCodes.UNDEFINED.ToString());
            }
            else
            {
                root = CVSProjectsDAL.GetPath(cpatch.KodSredy.ToString());
            }

            string match = null;
            try
            {
                path = cvs.FirstInEntireBase(root, ref match, new Regex($"{ZPatchName}$"), 1);
                return true;
            }
            catch(ArgumentException)
            {
                path = null;
                return false;
            }
        }

        public bool Download()
        {
            string path;
            if(GetCVSPath(out path))
            {
                cvs.Download(path, $"{cpatch.release.rm.homeDir}/{cpatch.release.releaseName}/{cpatch.CPatchName}/{ZPatchName}");
                return true;
            }
            return false;
        }

        public void SetDependencies()
        {
            dependenciesFrom = new HashSet<ZPatch>();

            var oraZPatchesDependenciesFrom = ZPatchDAL.getDependenciesFrom(ZPatchId);

            foreach (var oraZPatchRecord in oraZPatchesDependenciesFrom)
            {
                dependenciesFrom.Add(cpatch.getZPatchById(oraZPatchRecord.ZPatchId));
            }

            dependenciesTo = new HashSet<ZPatch>();

            var oraZPatchesDependenciesTo = ZPatchDAL.getDependenciesTo(ZPatchId);

            foreach (var oraZPatchRecord in oraZPatchesDependenciesTo)
            {
                dependenciesTo.Add(cpatch.getZPatchById(oraZPatchRecord.ZPatchId));
            }

        }

        /*
        public ZPatch(string patchName)
        {
            name = patchName;
        }
      
        */

        public ZPatch(DirectoryInfo dir)
        {
            dependenciesFrom = new HashSet<ZPatch>();
            dependenciesTo = new HashSet<ZPatch>();
            objs = new List<FileInfo>();
            IsATCPatch = true;

            Match match = ATCPatchRegex.Match(dir.FullName);
            if(!match.Success)
            {
                match = BankPatchRegex.Match(dir.FullName);
                IsATCPatch = false;
            }

            if(match.Success)
            {
                ZPatchName = match.Groups[1].Value;
                pathToPatch = dir.FullName.Substring(0, match.Index + ZPatchName.Length + 1);
            }
            this.dir = dir;
        }

        public void UpdateStatus(ZPatchStatuses newStatus)
        {
            if (newStatus != ZPatchStatus)
            {
                ZPatchStatus = newStatus;
                CPatchDAL.UpdateStatus(ZPatchId, newStatus.ToString());
            }
        }

        public override string ToString()
        {
            return ZPatchName;
        }

        public void ChangeCPatch(CPatch newCPatch)
        {
            if (cpatch.CPatchId != newCPatch.CPatchId)
            {
                cpatch.zpatches.Remove(this);
                cpatch.ZPatchesDict.Remove(ZPatchId);

                newCPatch.zpatches.Add(this);
                newCPatch.ZPatchesDict.Add(ZPatchId, this);

                ZPatchDAL.UpdateCPatch(ZPatchId, newCPatch.CPatchId);
            }
        }

        public void GetFromCVS()
        {
            if(ZPatchStatus != ZPatchStatuses.OPEN)
            {
                //string cvsFolder = CVSProjectsDAL.GetPath(cpatch.KodSredy) + ;
            }
            else
            {
                throw new Exception("Патч открыт, переведите статус");
            }
        }
    }
}
