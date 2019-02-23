using CoreApp.OraUtils;
using CoreApp.ReleaseObjects;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp.FixpackObjects
{
    public class CPatch
    {
        public string C { get; private set; }
        public string FullName { get; private set; }
        public string CPatchName { get; private set; }
        public string CPatchStatus { get; private set; }
        public string LocalPath { get; private set; }
        public static CVS.CVS cvs;

        static string regexC = @"\\(C\d+)";
        static string regexFullName = @"\\(C[^\\]+)";
        static string regexFullPath = @".*C[^\\]+";
        public List<ZPatch> ZPatches { get; protected set; } //отсортированный на DAL
        public Dictionary<int, ZPatch> ZPatchesDict { get; protected set; } //для поиска

        public HashSet<CPatch> dependenciesFrom { get; protected set; }
        public HashSet<CPatch> dependenciesTo { get; protected set; }
        public Release release;

        public int CPatchId { get; private set; }

        public override int GetHashCode()
        {
            return CPatchId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CPatch)) return false;
            return ((CPatch)obj).CPatchId == CPatchId;
        }

        public ZPatch getZPatchById(int id)
        {
            return ZPatchesDict[id];
        }


        private void InitFromDB()
        {

        }

        public CPatch(int CPatchId, string CPatchName, string CPatchStatus)
        {
            this.CPatchId = CPatchId;
            this.CPatchName = CPatchName;

            var oraZPatchesRecords = ZPatchDAL.getZPatchesByCPatch(CPatchId);
            foreach (var oraZPatchRecord in oraZPatchesRecords)
            {
                ZPatch zpatch = new ZPatch(oraZPatchRecord.ZPatchId);
                ZPatches.Add(zpatch);
                ZPatchesDict.Add(zpatch.ZPatchId, zpatch);

                zpatch.cpatch = this;
            }

            dependenciesFrom = new HashSet<CPatch>();

            var oraCPatchesDependenciesFrom = CPatchDAL.getDependenciesFrom(CPatchId);

            foreach(var oraCPatchRecord in oraCPatchesDependenciesFrom)
            {
                dependenciesFrom.Add(release.getCPatchById(oraCPatchRecord.CPatchId));
            }

            dependenciesTo = new HashSet<CPatch>();

            var oraCPatchesDependenciesTo = CPatchDAL.getDependenciesTo(CPatchId);

            foreach (var oraCPatchRecord in oraCPatchesDependenciesTo)
            {
                dependenciesTo.Add(release.getCPatchById(oraCPatchRecord.CPatchId));
            } 
        }

        public CPatch(FileInfo excelFile)
        {
            
        }

        public CPatch(DirectoryInfo dir)
        {
            ZPatches = new Dictionary<string, ZPatch>();
            C = Regex.Match(dir.FullName, regexC).Groups[1].Value;
            FullName = Regex.Match(dir.FullName, regexFullName).Groups[1].Value;
            LocalPath = Regex.Match(dir.FullName, regexFullPath).Groups[0].Value;
            foreach(DirectoryInfo patchDir in new DirectoryInfo(LocalPath).EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                ZPatch patch = new ZPatch(patchDir);
                if (!ZPatches.ContainsKey(patch.name))
                {
                    ZPatches.Add(patch.name, patch);
                }
            }

        }


        public void LoadFixpackFromCVS(string code, string cvsRootName, DirectoryInfo localDir)
        {
            string shortName = "";
            //TODO здесь нужно вытащить корень по названию

            string fixpackCVSPath = cvs.FirstInEntireBase(ref shortName, new Regex($".*{code}.*"));
            string fpPath = string.Join("\\", localDir.FullName, shortName);
            cvs.Download(fixpackCVSPath, fpPath);

            CPatch fp = new CPatch(new DirectoryInfo(fpPath));
        }

        private string FindLocalExcel(CPatch fp)
        {
            string path = fp.LocalPath + $"\\{fp.C}.xlsx";
            if (File.Exists(path))
            {
                return path;
            }
            else
            {
                path = fp.LocalPath + $"\\{fp.C}.xls";
                if (File.Exists(path))
                {
                    return path;
                }
                else
                {
                    throw new Exception("Экселька не найдена");
                }
            }
        }


        public SortedList<int, ZPatch> DependenciesToList()
        {
            List<ZPatch> roots = new List<ZPatch>();
            foreach (ZPatch patch in ZPatches)
            {
                if(patch.dependenciesFrom.Count == 0)
                {
                    roots.Add(patch);
                }
            }

            foreach(ZPatch root in roots)
            {
                root.rank = 0;
                SetRanks(root);
            }

            SortedList<int, ZPatch> list = new SortedList<int, ZPatch>();

            foreach (ZPatch patch in ZPatches)
            {
                list.Add(patch.rank, patch);
            }

            return list;
        }

        private void SetRanks(ZPatch currPatch)
        {
            foreach(ZPatch subpatch in currPatch.dependenciesTo)
            {
                subpatch.rank = Math.Max(subpatch.rank, currPatch.rank + 1);
            }
        }

    }

}

