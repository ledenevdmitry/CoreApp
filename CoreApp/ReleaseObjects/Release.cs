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

namespace CoreApp.ReleaseObjects
{
    class Release
    {
        public static Application excel;

        public SortedList<string, Fixpack> fixpacks { get; private set; }

        public string name { get; private set; }

        DirectoryInfo localDir;
        CVS.CVS cvs;

        private void setAttributesNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
                setAttributesNormal(subDir);
            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }

        //из системы контроля версий
        public Release(string name, DirectoryInfo dir, CVS.CVS cvs, Regex pattern) : this(name)
        {
            if(dir.Exists)
            {
                setAttributesNormal(dir);
                dir.Delete(true);
            }
            dir.Create();

            SetLocalDir(dir);

            List<string> fpNames = new List<string>();

            var cvsPaths = cvs.AllInEntireBase(fpNames, pattern);

            int i = 0;
            foreach(var cvsPath in cvsPaths)
            {
                string localPath = string.Join("\\", localDir.FullName, fpNames[i++]);
                cvs.Download(cvsPath, localPath);

                Fixpack fp = new Fixpack(new DirectoryInfo(localPath));
                fixpacks.Add(fp.FullName, fp);
            }
        }

        //загрузиться локально
        public Release(DirectoryInfo dir) : this(dir.Name)
        {
            SetLocalDir(dir);
            foreach(var subdir in dir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Fixpack currFixpack = new Fixpack(subdir);
                fixpacks.Add(currFixpack.FullName, currFixpack);
            }
        }

        public Release(string name)
        {
            fixpacks = new SortedList<string, Fixpack>();

            this.name = name;
        }

        public void SetLocalDir(DirectoryInfo localDir)
        {
            this.localDir = localDir;
        }

        public void SetCVS(CVS.CVS cvs)
        {
            this.cvs = cvs;
        }

        public void LoadFixpackFromCVS(string code)
        {
            string shortName = "";
            string fixpackCVSPath = cvs.FirstInEntireBase(ref shortName, new Regex($".*{code}.*"));
            string fpPath = string.Join("\\", localDir.FullName, shortName);
            cvs.Download(fixpackCVSPath, fpPath);

            Fixpack fp = new Fixpack(new DirectoryInfo(fpPath));
            fixpacks.Add(fp.FullName, fp);
        }

        public void SetAllDependencies()
        {
            foreach(Fixpack fp in fixpacks.Values)
            {
                ReadMetaFromExcel(fp);
            }
        }


        private Range ReadMetaFromExcel(Fixpack fp)
        {
            string path = fp.LocalPath + $"\\{fp.C}.xlsx";
            if (File.Exists(path))
            {
                return OpenExcelColumns(path);
            }
            else
            {
                path = fp.LocalPath + $"\\{fp.C}.xls";
                if (File.Exists(path))
                {
                    return OpenExcelColumns(path);
                }
                else
                {
                    throw new Exception("Экселька не найдена");
                }
            }
        }

        private Range OpenExcelColumns(string path)
        {
            Workbook wb = excel.Workbooks.Open(path);
            Worksheet ws = wb.Sheets[1];
            Range res = ws.UsedRange;
            ParseExcel(res);
            ExcelCleanup(wb, ws);

            return res;
        }


        private void ExcelCleanup(Workbook wb, Worksheet ws)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.FinalReleaseComObject(ws);

            wb.Close(Type.Missing, Type.Missing, Type.Missing);
            Marshal.FinalReleaseComObject(wb);
        }

        private static string regexPatchName = @"(C|Z)[0-9]+";

        private static string regexFrom = "зависит.*ALFAM.*?([0-9]+)";
        private static string regexTo = "влияет.*ALFAM.*?([0-9]+)";

        private IEnumerable<Patch> DependedFrom(string rawString)
        {
            MatchCollection matchesFrom = Regex.Matches(rawString, regexFrom);
            foreach (Match m in matchesFrom)
            {
                yield return findPatchByShortName(m.Groups[1].Value);
            }
        }

        private IEnumerable<Patch> DependOn(string rawString)
        {
            MatchCollection matchesTo = Regex.Matches(rawString, regexTo);
            foreach (Match m in matchesTo)
            {
                yield return findPatchByShortName(m.Groups[1].Value);
            }
        }

        private bool SameEnding(string s1, string s2)
        {
            for (int i = 1; i <= Math.Min(s1.Length, s2.Length); i++)
            {
                if (s1[s1.Length - i] != s2[s2.Length - i]) return false;
            }

            return true;
        }

        private Patch findPatchByShortName(string shortName)
        {
            foreach (Fixpack fp in fixpacks.Values)
            {
                return fp.patches.First(x => SameEnding(x.Value.name, shortName)).Value;
            }
            throw new KeyNotFoundException("Патч не найден");
        }

        private void ParseExcel(Range columns)
        {
            int patchNameIndex = -1;
            int linkIndex = -1;
            for (int i = 1; i <= columns.Columns.Count; ++i)
            {
                string currCell = ((Range)columns.Cells[1, i]).Value2;
                if (currCell == "Тема")
                {
                    patchNameIndex = i;
                }
                else if (currCell == "Issue Link Type" ||
                        currCell == "Связанные запросы" ||
                        currCell == "Связи")
                {
                    linkIndex = i;
                }
            }

            for (int i = 2; i <= columns.Rows.Count; ++i)
            {
                string patchCell = ((Range)columns.Cells[i, patchNameIndex]).Value2 ?? "";
                string dependenciesCell = ((Range)columns.Cells[i, linkIndex]).Value2 ?? "";

                string patchName = Regex.Match(patchCell, regexPatchName).Value;

                try
                {
                    Patch currPatch = findPatchByShortName(patchName);

                    currPatch.dependendFrom.UnionWith(DependedFrom(dependenciesCell));
                    currPatch.dependOn.UnionWith(DependOn(dependenciesCell));
                }
                catch (System.InvalidOperationException) { }
            }
        }
    }
}
