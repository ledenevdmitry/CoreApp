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
            ZPatches = new List<ZPatch>();
            ZPatchesDict = new Dictionary<int, ZPatch>();
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


        private void ReopenExcelColumns(FileInfo excelFile)
        {
            Workbook wb = ReleaseManager.excelApp.Workbooks.Open(excelFile.FullName);
            Worksheet ws = wb.Sheets[1];
            Range res = ws.UsedRange;
            List<ZPatch> newPatches;

            List<Tuple<int, ZPatch>> deletedDependenciesFrom;
            List<Tuple<int, ZPatch>> addedDependenciesFrom;
            List<Tuple<int, ZPatch>> deletedDependenciesTo;
            List<Tuple<int, ZPatch>> addedDependenciesTo;

            AddNewZPatches(res, out newPatches);
            CreateCPatchDelta(
                res,
                out deletedDependenciesFrom,
                out addedDependenciesFrom,
                out deletedDependenciesTo,
                out addedDependenciesTo,
                out newPatches);

            InsertCPatchDelta(
                deletedDependenciesFrom,
                addedDependenciesFrom,
                deletedDependenciesTo,
                addedDependenciesTo,
                newPatches);

            ExcelCleanup(wb, ws);
        }

        private void InsertCPatchDelta(
            List<Tuple<int, ZPatch>> deletedDependenciesFrom,
            List<Tuple<int, ZPatch>> addedDependenciesFrom,
            List<Tuple<int, ZPatch>> deletedDependenciesTo,
            List<Tuple<int, ZPatch>> addedDependenciesTo,
            List<ZPatch> newPatches)
        {
            foreach (var newPatch in newPatches)
            {
                ZPatchDAL.Insert(CPatchId, null, newPatch.ZPatchName);
            }

            //deletedfrom
            foreach (var deletedDependency in deletedDependenciesFrom)
            {
                ZPatchDAL.DeleteDependency(deletedDependency.Item2.ZPatchId, deletedDependency.Item1);

                if (deletedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    bool canDeleteCDependency = true;
                    foreach (ZPatch zPatch in ZPatches)
                    {
                        foreach(ZPatch dependency in zPatch.dependenciesFrom)
                        {
                            //если есть другая связь по этому ФП
                            if(dependency.cpatch.CPatchId == deletedDependency.Item2.cpatch.CPatchId)
                            {
                                canDeleteCDependency = false;
                                break;
                            }
                        }
                        if (!canDeleteCDependency) break;
                    }

                    if (canDeleteCDependency)
                    {
                        CPatchDAL.DeleteDependency(CPatchId, deletedDependency.Item2.cpatch.CPatchId);
                    }
                }
                
            }

            //addedfrom
            foreach(var addedDependency in addedDependenciesFrom)
            {
                ZPatchDAL.AddDependency(addedDependency.Item2.ZPatchId, addedDependency.Item1);
                if(addedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    if(!dependenciesFrom.Contains(addedDependency.Item2.cpatch))
                    {
                        dependenciesFrom.Add(addedDependency.Item2.cpatch);
                        CPatchDAL.AddDependency(addedDependency.Item2.cpatch.CPatchId, CPatchId);
                    }
                }
            }

            //deletedto
            foreach (var deletedDependency in deletedDependenciesTo)
            {
                ZPatchDAL.DeleteDependency(deletedDependency.Item1, deletedDependency.Item2.ZPatchId);

                if (deletedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    bool canDeleteCDependency = true;
                    foreach (ZPatch zPatch in ZPatches)
                    {
                        foreach (ZPatch dependency in zPatch.dependenciesTo)
                        {
                            //если есть другая связь по этому ФП
                            if (dependency.cpatch.CPatchId == deletedDependency.Item2.cpatch.CPatchId)
                            {
                                canDeleteCDependency = false;
                                break;
                            }
                        }
                        if (!canDeleteCDependency) break;
                    }

                    if (canDeleteCDependency)
                    {
                        CPatchDAL.DeleteDependency(deletedDependency.Item2.cpatch.CPatchId, CPatchId);
                    }
                }

            }

            //addedto
            foreach (var addedDependency in addedDependenciesTo)
            {
                ZPatchDAL.AddDependency(addedDependency.Item1, addedDependency.Item2.ZPatchId);
                if (addedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    if (!dependenciesFrom.Contains(addedDependency.Item2.cpatch))
                    {
                        dependenciesTo.Add(addedDependency.Item2.cpatch);
                        CPatchDAL.AddDependency(CPatchId, addedDependency.Item2.cpatch.CPatchId);
                    }
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

        private CPatch()
        { }

        public static CPatch CreateNewFromExcel(int releaseId, FileInfo excelFile)
        {
            CPatch emptyCPatch = new CPatch();
            emptyCPatch.ReopenExcelColumns(excelFile);
            CPatchDAL.Insert(releaseId, null, emptyCPatch.CPatchName);
            return emptyCPatch;
        }

        private static string regexZPatchName = @"(Z)[0-9]+";
        private static string regexCPatchName = @"(C)[0-9]+";

        private static string regexFrom = "зависит.*?ALFAM.*?([0-9]+)";
        private static string regexTo = "влияет.*?ALFAM.*?([0-9]+)";

        private HashSet<ZPatch> DependenciesFrom(string rawString)
        {
            HashSet<ZPatch> res = new HashSet<ZPatch>();
            MatchCollection matchesFrom = Regex.Matches(rawString, regexFrom);
            ZPatch zPatch;
            foreach (Match m in matchesFrom)
            {
                findPatchByShortName(m.Groups[1].Value, out zPatch);
                res.Add(zPatch);
            }
            return res;
        }

        private HashSet<ZPatch> DependenciesTo(string rawString)
        {
            HashSet<ZPatch> res = new HashSet<ZPatch>();
            MatchCollection matchesTo = Regex.Matches(rawString, regexTo);
            ZPatch zPatch;
            foreach (Match m in matchesTo)
            {
                findPatchByShortName(m.Groups[1].Value, out zPatch);
                res.Add(zPatch);
            }
            return res;
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
        
        private void AddNewZPatches(Range columns, out List<ZPatch> newPatches)
        {
            newPatches = new List<ZPatch>();

            int patchNameIndex, linkIndex;

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
                        if (!findPatchByShortName(patchName, out currPatch))
                        {
                            ZPatch zpatch = new ZPatch(
                                patchName,
                                CPatchId,
                                null,
                                null);

                            newPatches.Add(zpatch);
                            ZPatches.Add(zpatch);
                            ZPatchesDict.Add(zpatch.ZPatchId, zpatch);
                        }
                    }
                }
            }
        }


        private void CreateCPatchDelta(
            Range columns, 
            out List<Tuple<int, ZPatch>> deletedDependenciesFrom,  
            out List<Tuple<int, ZPatch>> addedDependenciesFrom,
            out List<Tuple<int, ZPatch>> deletedDependenciesTo,
            out List<Tuple<int, ZPatch>> addedDependenciesTo,
            out List<ZPatch> newPatches)
        {
            int patchNameIndex, linkIndex;

            newPatches = new List<ZPatch>();
            deletedDependenciesFrom = new List<Tuple<int, ZPatch>>();
            addedDependenciesFrom = new List<Tuple<int, ZPatch>>();
            deletedDependenciesTo = new List<Tuple<int, ZPatch>>();
            addedDependenciesTo = new List<Tuple<int, ZPatch>>();

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
                        findPatchByShortName(patchName, out currPatch);

                        var dependenciesFrom = DependenciesFrom(dependenciesCell);

                        foreach (ZPatch excelFromDependency in dependenciesFrom)
                        {
                            if (!currPatch.dependenciesFrom.Contains(excelFromDependency))
                            {
                                addedDependenciesFrom.Add(new Tuple<int, ZPatch>(currPatch.ZPatchId, excelFromDependency));
                                currPatch.dependenciesFrom.Add(excelFromDependency);
                            }
                        }

                        foreach(ZPatch patchFromDependency in currPatch.dependenciesFrom)
                        {
                            if(!dependenciesFrom.Contains(patchFromDependency))
                            {
                                deletedDependenciesFrom.Add(new Tuple<int, ZPatch>(currPatch.ZPatchId, patchFromDependency));
                                currPatch.dependenciesFrom.Remove(patchFromDependency);
                            }
                        }

                        var dependenciesTo = DependenciesTo(dependenciesCell);

                        foreach (ZPatch excelToDependency in dependenciesTo)
                        {
                            if (!currPatch.dependenciesTo.Contains(excelToDependency))
                            {
                                addedDependenciesTo.Add(new Tuple<int, ZPatch>(currPatch.ZPatchId, excelToDependency));
                                currPatch.dependenciesTo.Add(excelToDependency);
                            }
                        }

                        foreach (ZPatch patchToDependency in currPatch.dependenciesTo)
                        {
                            if (!dependenciesTo.Contains(patchToDependency))
                            {
                                deletedDependenciesFrom.Add(new Tuple<int, ZPatch>(currPatch.ZPatchId, patchToDependency));
                                currPatch.dependenciesTo.Remove(patchToDependency);
                            }
                        }                    
                    }
                }
            }
        }
    }

}


