using CoreApp.OraUtils;
using CoreApp.OraUtils.Model;
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


        private void InitFromDB(int CPatchId, string CPatchName, string CPatchStatus)
        {
            this.CPatchId = CPatchId;
            this.CPatchName = CPatchName;
            this.CPatchStatus = CPatchStatus;
            InitFromDB();
        }

        private void InitFromDB()
        {
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

            foreach (var oraCPatchRecord in oraCPatchesDependenciesFrom)
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

        public CPatch(int CPatchId, string CPatchName, string CPatchStatus)
        {
            InitFromDB(CPatchId, CPatchName, CPatchStatus);
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


        private bool ReopenExcelColumns(FileInfo excelFile)
        {
            Workbook wb = ReleaseManager.excelApp.Workbooks.Open(excelFile.FullName);
            Worksheet ws = wb.Sheets[1];
            Range res = ws.UsedRange;
            List<ZPatch> newPatches;
            bool areCorrect = AreOldDependenciesCorrect(res, out newPatches);
            if (areCorrect)
            {
                AddNewPatchesInDB();
            }
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

        private static string regexZPatchName = @"(Z)[0-9]+";
        private static string regexCPatchName = @"(C)[0-9]+";

        private static string regexFrom = "зависит.*?ALFAM.*?([0-9]+)";
        private static string regexTo = "влияет.*?ALFAM.*?([0-9]+)";

        private IEnumerable<ZPatch> DependenciesFrom(string rawString)
        {
            MatchCollection matchesFrom = Regex.Matches(rawString, regexFrom);
            ZPatch zPatch;
            foreach (Match m in matchesFrom)
            {
                findPatchByShortName(m.Groups[1].Value, out zPatch);
                yield return zPatch;
            }
        }

        private IEnumerable<ZPatch> DependenciesTo(string rawString)
        {
            MatchCollection matchesTo = Regex.Matches(rawString, regexTo);
            ZPatch zPatch;
            foreach (Match m in matchesTo)
            {
                findPatchByShortName(m.Groups[1].Value, out zPatch);
                yield return zPatch;
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

        private bool findPatchByShortName(string shortName, out ZPatch patch)
        {
            foreach (CPatch cpatch in release.CPatches)
            {
                patch = cpatch.ZPatches.First(x => SameEnding(x.ZPatchName, shortName));
                return true;
            }
            patch = null;
            return false;
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
        
        private bool AreOldDependenciesCorrect(Range columns, out List<ZPatch> newPatches)
        {
            int patchNameIndex, linkIndex;
            newPatches = new List<ZPatch>();
            GetColumnsIndexes(columns, out patchNameIndex, out linkIndex);

            for (int i = 2; i <= columns.Rows.Count; ++i)
            {
                string patchCell = ((Range)columns.Cells[i, patchNameIndex]).Value2 ?? "";
                if (linkIndex != -1)
                {
                    string dependenciesCell = ((Range)columns.Cells[i, linkIndex]).Value2 ?? "";
                    MatchCollection matches = Regex.Matches(patchCell, regexZPatchName);
                    if (matches.Count == 0)
                    {
                        matches = Regex.Matches(patchCell, regexCPatchName);
                        if (matches.Count > 0)
                        {
                            CPatchName = patchCell;
                        }
                    }
                    else
                    {
                        string patchName = Regex.Match(patchCell, regexZPatchName).Value;
                        ZPatch currPatch;
                        if (findPatchByShortName(patchName, out currPatch))
                        {
                            int count = 0;
                            foreach (ZPatch excelFromDependency in DependenciesFrom(dependenciesCell))
                            {
                                count++;
                                if (!currPatch.dependenciesFrom.Contains(excelFromDependency))
                                {
                                    return false;
                                }
                            }
                            if (currPatch.dependenciesFrom.Count != count) return false;

                            count = 0;
                            foreach (ZPatch excelToDependency in DependenciesTo(dependenciesCell))
                            {
                                count++;
                                if (!currPatch.dependenciesTo.Contains(excelToDependency))
                                {
                                    return false;
                                }
                            }
                            if (currPatch.dependenciesTo.Count != count) return false;

                            currPatch.dependenciesFrom.UnionWith(DependenciesFrom(dependenciesCell));
                            currPatch.dependenciesTo.UnionWith(DependenciesTo(dependenciesCell));
                        }
                        else
                        {
                            ZPatch zpatch = new ZPatch(
                                patchName,
                                new HashSet<ZPatch>(DependenciesFrom(dependenciesCell)),
                                new HashSet<ZPatch>(DependenciesTo(dependenciesCell)));
                            newPatches.Add(zpatch);
                        }
                    }
                }
            }

            return true;
        }
    }

}


