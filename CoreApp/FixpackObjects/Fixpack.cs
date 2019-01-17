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
    class Fixpack
    {
        public static Application excel;

        string C;
        string FullName;
        string FullPath;

        static string regexC = @"\\(C\d+)";
        static string regexFullName = @"\\(C[^\\]+)";
        static string regexFullPath = @".*C[^\\]+";
        public static Dictionary<string, Patch> allPatches { get; protected set; }
        public Dictionary<string, Patch> patches { get; protected set; }

        static Fixpack()
        {
            allPatches = new Dictionary<string, Patch>();
        }

        public Fixpack(DirectoryInfo dir)
        {
            patches = new Dictionary<string, Patch>();
            C = Regex.Match(dir.FullName, regexC).Groups[1].Value;
            FullName = Regex.Match(dir.FullName, regexFullName).Groups[1].Value;
            FullPath = Regex.Match(dir.FullName, regexFullPath).Groups[0].Value;
            foreach(DirectoryInfo patchDir in new DirectoryInfo(FullPath).EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Patch patch = new Patch(patchDir);
                if (!allPatches.ContainsKey(patch.name))
                {
                    patches.Add(patch.name, patch);
                    allPatches.Add(patch.name, patch);
                }
            }
            ReadMetaFromExcel();

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

        private Range ReadMetaFromExcel()
        {
            string path = FullPath + $"\\{C}.xlsx";
            if (File.Exists(path))
            {
                return OpenExcelColumns(path);
            }
            else
            {
                path = FullPath + $"\\{C}.xls";
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
            foreach(Match m in matchesFrom)
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
            for(int i = 1; i <= Math.Min(s1.Length, s2.Length); i++)
            {
                if (s1[s1.Length - i] != s2[s2.Length - i]) return false;
            }

            return true;
        }

        private Patch findPatchByShortName(string shortName)
        {
            return allPatches.First(x => SameEnding(x.Value.name, shortName)).Value;
        }

        private void ParseExcel(Range columns)
        {
            int patchNameIndex = -1;
            int linkIndex = -1;
            for(int i = 1; i <= columns.Columns.Count; ++i)
            {
                string currCell = ((Range)columns.Cells[1, i]).Value2;
                if (currCell == "Тема")
                {
                    patchNameIndex = i;
                }
                else if(currCell == "Issue Link Type" ||
                        currCell == "Связанные запросы" ||
                        currCell == "Связи")
                {
                    linkIndex = i;
                }
            }

            for(int i = 2; i <= columns.Rows.Count; ++i)
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

