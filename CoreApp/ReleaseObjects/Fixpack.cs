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
        public static Application excel;
        public string C { get; private set; }
        public string FullName { get; private set; }
        public string LocalPath { get; private set; }

        public bool dependenciesSet;

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

            dependenciesSet = false;

        }


        private string FindLocalExcel(Fixpack fp)
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

        public bool ReadMetaFromExcelFile(Fixpack fp, string newExcelFilePath)
        {
            if (!dependenciesSet)
            {
                OpenExcelColumns(newExcelFilePath);
                dependenciesSet = true;
                return true;
            }
            else
            {
                return ReopenExcelColumns(newExcelFilePath);
            }
        }

        private void OpenExcelColumns(string path)
        {
            Workbook wb = excel.Workbooks.Open(path);
            Worksheet ws = wb.Sheets[1];
            Range res = ws.UsedRange;
            SetDependencies(res);
            ExcelCleanup(wb, ws);
        }

        private bool ReopenExcelColumns(string path)
        {
            Workbook wb = excel.Workbooks.Open(path);
            Worksheet ws = wb.Sheets[1];
            Range res = ws.UsedRange;
            bool areCorrect = AreOldDependenciesCorrect(res);
            ExcelCleanup(wb, ws);
            return areCorrect;
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

        private static string regexFrom = "зависит.*?ALFAM.*?([0-9]+)";
        private static string regexTo = "влияет.*?ALFAM.*?([0-9]+)";

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

        private void GetColumnsIndexes(Range columns, out int patchNameIndex, out int linkIndex)
        {
            patchNameIndex = -1;
            linkIndex = -1;
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
        }

        private void SetDependencies(Range columns)
        {
            int patchNameIndex, linkIndex;
            GetColumnsIndexes(columns, out patchNameIndex, out linkIndex);

            for (int i = 2; i <= columns.Rows.Count; ++i)
            {
                string patchCell = ((Range)columns.Cells[i, patchNameIndex]).Value2 ?? "";
                if (linkIndex != -1)
                {
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

        private bool AreOldDependenciesCorrect(Range columns)
        {
            int patchNameIndex, linkIndex;
            GetColumnsIndexes(columns, out patchNameIndex, out linkIndex);

            for (int i = 2; i <= columns.Rows.Count; ++i)
            {
                string patchCell = ((Range)columns.Cells[i, patchNameIndex]).Value2 ?? "";
                if (linkIndex != -1)
                {
                    string dependenciesCell = ((Range)columns.Cells[i, linkIndex]).Value2 ?? "";
                    string patchName = Regex.Match(patchCell, regexPatchName).Value;

                    try
                    {
                        Patch currPatch = findPatchByShortName(patchName);

                        foreach (Patch excelFromDependency in DependedFrom(dependenciesCell))
                        {
                            if (!currPatch.dependendFrom.Contains(excelFromDependency))
                            {
                                return false;
                            }
                        }

                        foreach (Patch excelToDependency in DependOn(dependenciesCell))
                        {
                            if (!currPatch.dependOn.Contains(excelToDependency))
                            {
                                return false;
                            }
                        }

                        currPatch.dependendFrom.UnionWith(DependedFrom(dependenciesCell));
                        currPatch.dependOn.UnionWith(DependOn(dependenciesCell));
                    }
                    catch (System.InvalidOperationException) { }
                }
            }

            return true;
        }

    }

}

