using CoreApp.OraUtils;
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
        public string LocalPath { get; private set; }
        public static CVS.CVS cvs;

        static string regexC = @"\\(C\d+)";
        static string regexFullName = @"\\(C[^\\]+)";
        static string regexFullPath = @".*C[^\\]+";
        public List<ZPatch> patches { get; protected set; }


        public CPatch(int oraId)
        {
            var oraZPatches = ZPatchDAL.getZPatchesByCPatch(oraId);
            foreach (var oraZPatch in oraZPatches)
            {
                ZPatch zpatch = new ZPatch(oraZPatch.ZPatchId);
            }
        }

        public CPatch(DirectoryInfo dir)
        {
            patches = new Dictionary<string, ZPatch>();
            C = Regex.Match(dir.FullName, regexC).Groups[1].Value;
            FullName = Regex.Match(dir.FullName, regexFullName).Groups[1].Value;
            LocalPath = Regex.Match(dir.FullName, regexFullPath).Groups[0].Value;
            foreach(DirectoryInfo patchDir in new DirectoryInfo(LocalPath).EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                ZPatch patch = new ZPatch(patchDir);
                if (!patches.ContainsKey(patch.name))
                {
                    patches.Add(patch.name, patch);
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
            foreach (ZPatch patch in patches.Values)
            {
                if(patch.dependendFrom.Count == 0)
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

            foreach (ZPatch patch in patches.Values)
            {
                list.Add(patch.rank, patch);
            }

            return list;
        }

        private void SetRanks(ZPatch currPatch)
        {
            foreach(ZPatch subpatch in currPatch.dependOn)
            {
                subpatch.rank = Math.Max(subpatch.rank, currPatch.rank + 1);
            }
        }

    }

}

