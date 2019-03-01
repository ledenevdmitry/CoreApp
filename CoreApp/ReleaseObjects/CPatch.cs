﻿using CoreApp.OraUtils;
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
    public enum CPatchStatuses  { UNDEFINED, NOTREADY, READY }

    public class CPatch
    {
        public string C { get; private set; }
        public string FullName { get; private set; }
        public string CPatchName { get; private set; }
        public CPatchStatuses CPatchStatus { get; private set; }
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
        public string KodSredy { get; set; }

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


        private void InitFromDB(int CPatchId, string CPatchName, CPatchStatuses CPatchStatus, string KodSredy)
        {
            this.CPatchId = CPatchId;
            this.CPatchName = CPatchName;
            this.CPatchStatus = CPatchStatus;
            this.KodSredy = KodSredy;
        }

        public void InitZPatches()
        {
            ZPatches = new List<ZPatch>();
            ZPatchesDict = new Dictionary<int, ZPatch>();
            var oraZPatchesRecords = ZPatchDAL.getZPatchesByCPatch(CPatchId);

            ZPatchStatuses status;

            foreach (var oraZPatchRecord in oraZPatchesRecords)
            {
                if (!Enum.TryParse(oraZPatchRecord.ZPatchStatus, out status))
                {
                    status = ZPatchStatuses.UNDEFINED;
                }

                ZPatch zpatch = new ZPatch(
                    this, 
                    oraZPatchRecord.ZPatchName, 
                    oraZPatchRecord.ZPatchId, 
                    status);

                ZPatches.Add(zpatch);
                ZPatchesDict.Add(zpatch.ZPatchId, zpatch);

                zpatch.cpatch = this;
            }
            foreach (ZPatch zpatch in ZPatches)
            {
                zpatch.SetDependencies();
            }
        }

        public void ResetStatusesByLog()
        {
            foreach(ZPatch zpatch in ZPatches)
            {
                if (zpatch.ZPatchStatus != ZPatchStatuses.INSTALLED && ZPatchDAL.IsZPatchInstalled(zpatch.ZPatchName, KodSredy))
                {
                    zpatch.ZPatchStatus = ZPatchStatuses.INSTALLED;
                }
            }
        }
        
        public void SetDependencies()
        {
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

        public CPatch(int CPatchId, string CPatchName, CPatchStatuses CPatchStatus, string KodSredy)
        {
            InitFromDB(CPatchId, CPatchName, CPatchStatus, KodSredy);
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
                if (patch.dependenciesFrom.Count == 0)
                {
                    roots.Add(patch);
                }
            }

            foreach (ZPatch root in roots)
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
            foreach (ZPatch subpatch in currPatch.dependenciesTo)
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
                out addedDependenciesTo);

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

            //deletedfrom
            foreach (var deletedDependency in deletedDependenciesFrom)
            {
                ZPatchDAL.DeleteDependency(deletedDependency.Item2.ZPatchId, deletedDependency.Item1);

                if (deletedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    bool canDeleteCDependency = true;
                    foreach (ZPatch zPatch in ZPatches)
                    {
                        foreach (ZPatch dependency in zPatch.dependenciesFrom)
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
                        CPatchDAL.DeleteDependency(CPatchId, deletedDependency.Item2.cpatch.CPatchId);
                    }
                }

            }

            //addedfrom
            foreach (var addedDependency in addedDependenciesFrom)
            {
                ZPatchDAL.AddDependency(addedDependency.Item2.ZPatchId, addedDependency.Item1);
                if (addedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    if (!dependenciesFrom.Contains(addedDependency.Item2.cpatch))
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

        public static CPatch CreateNewFromExcel(Release release, FileInfo excelFile)
        {
            CPatch empty = new CPatch();
            empty.ZPatches = new List<ZPatch>();
            empty.ZPatchesDict = new Dictionary<int, ZPatch>();

            empty.dependenciesFrom = new HashSet<CPatch>();
            empty.dependenciesTo = new HashSet<CPatch>();
            empty.release = release;
            //release.CPatchesDict.Add(-1, empty);

            empty.ReopenExcelColumns(excelFile);
            return empty;
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
                if (findPatchByShortName(m.Groups[1].Value, out zPatch))
                {
                    res.Add(zPatch);
                }
                //TODO мб отлавливать случаи, когда не найден. предупреждение или тип того
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
                if (findPatchByShortName(m.Groups[1].Value, out zPatch))
                {
                    res.Add(zPatch);
                }
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
                try
                {
                    patch = cpatch.ZPatches.First(x => SameEnding(x.ZPatchName, shortName));
                    return true;
                }
                catch { }
            }
            patch = null;
            return false;
        }

        private int GetPatchNameIndex(Range columns)
        {
            for (int i = 1; i <= columns.Columns.Count; ++i)
            {
                string currCell = ((Range)columns.Cells[1, i]).Value2;
                if (currCell == "Тема")
                {
                    return i;
                }
            }
            throw new KeyNotFoundException("Колонка с именем патча не найдена");
        }

        private int GetPatchStatusIndex(Range columns)
        {
            for (int i = 1; i <= columns.Columns.Count; ++i)
            {
                string currCell = ((Range)columns.Cells[1, i]).Value2;
                if (currCell == "Статус")
                {
                    return i;
                }
            }
            throw new KeyNotFoundException("Колонка со статусом не найдена");
        }

        private int GetLinkIndex(Range columns)
        {
            for (int i = 1; i <= columns.Columns.Count; ++i)
            {
                string currCell = ((Range)columns.Cells[1, i]).Value2;
                if (currCell == "Issue Link Type")
                {
                    return i;
                }
            }
            throw new KeyNotFoundException("Колонка с зависимостями не найдена");
        }

        public void UpdateStatus(CPatchStatuses newStatus)
        {
            CPatchStatus = newStatus;
            //TODO update
            CPatchDAL.UpdateStatus(CPatchId, newStatus.ToString());
        }

        private void AddNewZPatches(Range columns, out List<ZPatch> newPatches)
        {
            newPatches = new List<ZPatch>();

            int patchNameIndex = GetPatchNameIndex(columns);
            int patchStatusIndex = GetPatchStatusIndex(columns);

            for (int i = 2; i <= columns.Rows.Count; ++i)
            {
                string patchCell = ((Range)columns.Cells[i, patchNameIndex]).Value2 ?? "";
                string excelStatus = ((Range)columns.Cells[i, patchStatusIndex]).Value2;
                ZPatchStatuses ZPatchStatus = ZPatchStatuses.UNDEFINED;

                if (excelStatus == "Open")
                    ZPatchStatus = ZPatchStatuses.OPEN;
                else if (excelStatus == "Testing" || excelStatus == "Waiting Bank Confirm")
                    ZPatchStatus = ZPatchStatuses.READY;
                else if (excelStatus == "Installed To STAB")
                    ZPatchStatus = ZPatchStatuses.INSTALLED;

                MatchCollection matches = Regex.Matches(patchCell, regexZPatchName);
                if (matches.Count == 0)
                {
                    matches = Regex.Matches(patchCell, regexCPatchName);
                    if (matches.Count > 0)
                    {
                        //ФП еще не добавлялся
                        if (CPatchId == 0)
                        {
                            CPatchName = patchCell;
                            CPatchId = CPatchDAL.Insert(release.releaseId, null, patchCell, null, null);
                        }
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
                            new HashSet<ZPatch>(),
                            new HashSet<ZPatch>(),
                            ZPatchStatus);

                        zpatch.excelFileRowId = i;
                        zpatch.cpatch = this;

                        newPatches.Add(zpatch);
                        ZPatches.Add(zpatch);                        

                        //TODO проверить последствия
                        //идентификатор назначится только на dal, печаль
                        //ZPatchesDict.Add(zpatch.ZPatchId, zpatch);
                    }
                }

            }

            foreach(ZPatch zpatch in newPatches)
            {
                zpatch.ZPatchId = ZPatchDAL.Insert(CPatchId, null, zpatch.ZPatchName, null);
            }
        }


        private void CreateCPatchDelta(
            Range columns,
            out List<Tuple<int, ZPatch>> deletedDependenciesFrom,
            out List<Tuple<int, ZPatch>> addedDependenciesFrom,
            out List<Tuple<int, ZPatch>> deletedDependenciesTo,
            out List<Tuple<int, ZPatch>> addedDependenciesTo)
        {
            deletedDependenciesFrom = new List<Tuple<int, ZPatch>>();
            addedDependenciesFrom = new List<Tuple<int, ZPatch>>();
            deletedDependenciesTo = new List<Tuple<int, ZPatch>>();
            addedDependenciesTo = new List<Tuple<int, ZPatch>>();


            int linkIndex = GetLinkIndex(columns);
            if (linkIndex != -1)
            {
                foreach(ZPatch zpatch in ZPatches)
                {
                    string dependenciesCell = ((Range)columns.Cells[zpatch.excelFileRowId, linkIndex]).Value2 ?? "";
                      var dependenciesFrom = DependenciesFrom(dependenciesCell);

                    foreach (ZPatch excelFromDependency in dependenciesFrom)
                    {
                        if (!zpatch.dependenciesFrom.Contains(excelFromDependency))
                        {
                            addedDependenciesFrom.Add(new Tuple<int, ZPatch>(zpatch.ZPatchId, excelFromDependency));
                            zpatch.dependenciesFrom.Add(excelFromDependency);
                        }
                    }

                    foreach (ZPatch patchFromDependency in zpatch.dependenciesFrom)
                    {
                        if (!dependenciesFrom.Contains(patchFromDependency))
                        {
                            deletedDependenciesFrom.Add(new Tuple<int, ZPatch>(zpatch.ZPatchId, patchFromDependency));
                            zpatch.dependenciesFrom.Remove(patchFromDependency);
                        }
                    }

                    var dependenciesTo = DependenciesTo(dependenciesCell);

                    foreach (ZPatch excelToDependency in dependenciesTo)
                    {
                        if (!zpatch.dependenciesTo.Contains(excelToDependency))
                        {
                            addedDependenciesTo.Add(new Tuple<int, ZPatch>(zpatch.ZPatchId, excelToDependency));
                            zpatch.dependenciesTo.Add(excelToDependency);
                        }
                    }

                    foreach (ZPatch patchToDependency in zpatch.dependenciesTo)
                    {
                        if (!dependenciesTo.Contains(patchToDependency))
                        {
                            deletedDependenciesFrom.Add(new Tuple<int, ZPatch>(zpatch.ZPatchId, patchToDependency));
                            zpatch.dependenciesTo.Remove(patchToDependency);
                        }
                    }
                    
                }
            }
        }
    }
}


