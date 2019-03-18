using CoreApp.OraUtils;
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
    public class ZPatch
    {
        public string ZPatchName { get; private set; }
        public string ZPatchStatus { get; set; }
        public DirectoryInfo Dir { get; set; }
        public string PathToPatch { get; private set; }
        public HashSet<ZPatch> DependenciesFrom { get; private set; }
        public HashSet<ZPatch> DependenciesTo { get; private set; }
        public List<FileInfo> objs;
        public int rank;
        public int ZPatchId { get; set; }
        public CPatch cpatch;
        public int excelFileRowId;
        public static CVS.CVS cvs;

        public static IEnumerable<string> zpatchStatuses;

        static ZPatch()
        {
            zpatchStatuses = ZPatchStateDAL.GetStatuses();
        }

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

        public ZPatch(string ZPatchName, int CPatch, HashSet<ZPatch> dependenciesFrom, HashSet<ZPatch> dependenciesTo, string ZPatchStatus)
        {
            this.ZPatchName = ZPatchName;
            this.DependenciesFrom = dependenciesFrom;
            this.DependenciesTo = dependenciesTo;
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

        public ZPatch(CPatch cpatch, string ZPatchName, int ZPatchId, string ZPatchStatus)
        {
            this.cpatch = cpatch;
            this.ZPatchId = ZPatchId;
            this.ZPatchName = ZPatchName;
            this.ZPatchStatus = ZPatchStatus;
        }

        public void Delete()
        {
            foreach (ZPatch zpatchFrom in DependenciesFrom)
            {
                zpatchFrom.DependenciesTo.Remove(this);
                ZPatchDAL.DeleteDependency(zpatchFrom.ZPatchId, ZPatchId);
            }

            foreach (ZPatch zpatchTo in DependenciesTo)
            {
                zpatchTo.DependenciesFrom.Remove(this);
                CPatchDAL.DeleteDependency(ZPatchId, zpatchTo.ZPatchId);
            }

            ZPatchDAL.DeleteZPatch(ZPatchId);
            cpatch.ZPatches.Remove(this);
            cpatch.ZPatchesDict.Remove(ZPatchId);
        }

        public void Move(CPatch newCPatch)
        {
            if (cpatch != newCPatch)
            {
                cpatch.ZPatches.Remove(this);
                cpatch.ZPatchesDict.Remove(ZPatchId);

                newCPatch.ZPatches.Add(this);
                newCPatch.ZPatchesDict.Add(ZPatchId, this);

                ZPatchDAL.UpdateCPatch(cpatch.CPatchId, newCPatch.CPatchId);
            }
        }

        public void DeleteDependencyFrom(ZPatch zpatchFrom)
        {
            DependenciesFrom.Remove(zpatchFrom);
            ZPatchDAL.DeleteDependency(zpatchFrom.ZPatchId, ZPatchId);
        }

        public void DeleteDependencyTo(ZPatch zpatchTo)
        {
            DependenciesTo.Remove(zpatchTo);
            ZPatchDAL.DeleteDependency(ZPatchId, zpatchTo.ZPatchId);
        }

        public void AddDependencyFrom(ZPatch zpatchFrom)
        {
            DependenciesFrom.Add(zpatchFrom);
            ZPatchDAL.AddDependency(zpatchFrom.ZPatchId, ZPatchId);
        }

        public void AddDependencyTo(ZPatch zpatchTo)
        {
            DependenciesTo.Add(zpatchTo);
            ZPatchDAL.AddDependency(ZPatchId, zpatchTo.ZPatchId);
        }

        public static bool HaveTransitiveDependency(ZPatch zpatchFrom, ZPatch zpatchTo)
        {
            foreach (ZPatch subPatch in zpatchFrom.DependenciesTo)
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
            string root = ZPatchStateDAL.GetCVSPath(ZPatchStatus);

            string match = null;
            try
            {
                path = cvs.FirstInEntireBase(root, out match, new Regex($"{ZPatchName}$"), 1);
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
            if (GetCVSPath(out string path))
            {
                Dir = new DirectoryInfo($"{cpatch.release.rm.homeDir}/{cpatch.release.ReleaseName}/{cpatch.CPatchName}/{ZPatchName}");
                cvs.Pull(path, Dir);
                return true;
            }
            return false;
        }

        public void SetDependencies()
        {
            DependenciesFrom = new HashSet<ZPatch>();

            var oraZPatchesDependenciesFrom = ZPatchDAL.GetDependenciesFrom(ZPatchId);

            foreach (var oraZPatchRecord in oraZPatchesDependenciesFrom)
            {
                DependenciesFrom.Add(cpatch.GetZPatchById(oraZPatchRecord.ZPatchId));
            }

            DependenciesTo = new HashSet<ZPatch>();

            var oraZPatchesDependenciesTo = ZPatchDAL.GetDependenciesTo(ZPatchId);

            foreach (var oraZPatchRecord in oraZPatchesDependenciesTo)
            {
                DependenciesTo.Add(cpatch.GetZPatchById(oraZPatchRecord.ZPatchId));
            }

        }

        public void UpdateStatus(string newStatus)
        {
            if (newStatus != ZPatchStatus)
            {
                ZPatchStatus = newStatus;
                ZPatchDAL.UpdateStatus(ZPatchId, newStatus);
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
                cpatch.ZPatches.Remove(this);
                cpatch.ZPatchesDict.Remove(ZPatchId);

                newCPatch.ZPatches.Add(this);
                newCPatch.ZPatchesDict.Add(ZPatchId, this);

                ZPatchDAL.UpdateCPatch(ZPatchId, newCPatch.CPatchId);
            }
        }

        public void GetFromCVS()
        {
            if(ZPatchStatus != "OPEN")
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
