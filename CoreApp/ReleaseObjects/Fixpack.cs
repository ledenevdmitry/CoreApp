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
    public class Fixpack
    {
        public string C { get; private set; }
        public string FullName { get; private set; }
        public string LocalPath { get; private set; }

        static string regexC = @"\\(C\d+)";
        static string regexFullName = @"\\(C[^\\]+)";
        static string regexFullPath = @".*C[^\\]+";
        public Dictionary<string, Patch> patches { get; protected set; }        

        public Fixpack(DirectoryInfo dir)
        {
            patches = new Dictionary<string, Patch>();
            C = Regex.Match(dir.FullName, regexC).Groups[1].Value;
            FullName = Regex.Match(dir.FullName, regexFullName).Groups[1].Value;
            LocalPath = Regex.Match(dir.FullName, regexFullPath).Groups[0].Value;
            foreach(DirectoryInfo patchDir in new DirectoryInfo(LocalPath).EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Patch patch = new Patch(patchDir);
                if (!patches.ContainsKey(patch.name))
                {
                    patches.Add(patch.name, patch);
                }
            }

        }


        public SortedList<int, Patch> DependenciesToList()
        {
            List<Patch> roots = new List<Patch>();
            foreach (Patch patch in patches.Values)
            {
                if(patch.dependendFrom.Count == 0)
                {
                    roots.Add(patch);
                }
            }

            foreach(Patch root in roots)
            {
                root.rank = 0;
                SetRanks(root);
            }

            SortedList<int, Patch> list = new SortedList<int, Patch>();

            foreach (Patch patch in patches.Values)
            {
                list.Add(patch.rank, patch);
            }

            return list;
        }

        private void SetRanks(Patch currPatch)
        {
            foreach(Patch subpatch in currPatch.dependOn)
            {
                subpatch.rank = Math.Max(subpatch.rank, currPatch.rank + 1);
            }
        }

    }

}

