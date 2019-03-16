using CoreApp.OraUtils;
using CoreApp.OraUtils.DAL;
using CoreApp.OraUtils.Model;
using CoreApp.ReleaseObjects;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp.ReleaseObjects
{
    public enum CPatchStatuses  { UNDEFINED, NOTREADY, READY, INSTALLED, REVISION }
    public enum EnvCodes { UNDEFINED, STAB, TEST}

    public class CPatch
    {
        public string C { get; private set; }
        public string FullName { get; private set; }
        public string CPatchName { get; private set; }
        public CPatchStatuses CPatchStatus { get; private set; }
        public string LocalPath { get; private set; }
        public string cvsPath;
        public static CVS.CVS cvs;

        static string regexC = @"\\(C\d+)";
        static string regexFullName = @"\\(C[^\\]+)";
        static string regexFullPath = @".*C[^\\]+";
        public List<ZPatch> zpatches { get; protected set; } //отсортированный на DAL
        public Dictionary<int, ZPatch> ZPatchesDict { get; protected set; } //для поиска

        public HashSet<CPatch> dependenciesFrom { get; protected set; }
        public HashSet<CPatch> dependenciesTo { get; protected set; }
        public Release release;
        public EnvCodes KodSredy { get; set; }
        public SortedList<int, ZPatch> ZPatchOrder { get; private set; }

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
            foreach(CPatch cpatch in release.cpatches)
            {
                if (cpatch.ZPatchesDict.ContainsKey(id))
                {
                    return cpatch.ZPatchesDict[id];
                }
            }
            return null;
        }

        public override string ToString()
        {
            return CPatchName;
        }

        private void InitFromDB(int CPatchId, string CPatchName, CPatchStatuses CPatchStatus, EnvCodes KodSredy)
        {
            this.CPatchId = CPatchId;
            this.CPatchName = CPatchName;
            this.CPatchStatus = CPatchStatus;
            this.KodSredy = KodSredy;
        }

        public void Delete()
        {
            foreach(CPatch cpatchFrom in dependenciesFrom)
            {
                cpatchFrom.dependenciesTo.Remove(this);
                CPatchDAL.DeleteDependency(cpatchFrom.CPatchId, CPatchId);
            }

            foreach(CPatch cpatchTo in dependenciesTo)
            {
                cpatchTo.dependenciesFrom.Remove(this);
                CPatchDAL.DeleteDependency(CPatchId, cpatchTo.CPatchId);
            }

            CPatchDAL.DeleteCPatch(CPatchId);
            release.cpatches.Remove(this);
            release.CPatchesDict.Remove(CPatchId);
        }

        public static bool CanDeleteCPatchDependency(CPatch cpatchFrom, CPatch cpatchTo, out ZPatch zpatchFromDependency, out ZPatch zpatchToDependency)
        {
            foreach (var zpatchFrom in cpatchFrom.zpatches)
            {
                foreach (var zpatchTo in cpatchTo.zpatches)
                {
                    if (zpatchFrom.dependenciesTo.Contains(zpatchTo))
                    {
                        zpatchFromDependency = zpatchFrom;
                        zpatchToDependency = zpatchTo;

                        return false;
                    }
                }
            }

            zpatchFromDependency = zpatchToDependency = null;
            return true;
        }

        public void DeleteDependencyFrom(CPatch cpatchFrom)
        {
            dependenciesFrom.Remove(cpatchFrom);
            cpatchFrom.dependenciesTo.Remove(this);
            CPatchDAL.DeleteDependency(cpatchFrom.CPatchId, CPatchId);
        }

        public void DeleteDependencyTo(CPatch cpatchTo)
        {
            dependenciesTo.Remove(cpatchTo);
            cpatchTo.dependenciesFrom.Remove(this);
            CPatchDAL.DeleteDependency(CPatchId, cpatchTo.CPatchId);
        }

        public void AddDependencyFrom(CPatch cpatchFrom)
        {
            dependenciesFrom.Add(cpatchFrom);
            cpatchFrom.dependenciesTo.Add(this);
            CPatchDAL.AddDependency(cpatchFrom.CPatchId, CPatchId);
        }

        public void AddDependencyTo(CPatch cpatchTo)
        {
            dependenciesTo.Add(cpatchTo);
            cpatchTo.dependenciesFrom.Add(this);
            CPatchDAL.AddDependency(CPatchId, cpatchTo.CPatchId);
        }

        public static bool HaveTransitiveDependency(CPatch cpatchFrom, CPatch cpatchTo)
        {
            foreach(CPatch subPatch in cpatchFrom.dependenciesTo)
            {
                if (subPatch.Equals(cpatchTo))
                    return true;
                else
                    return HaveTransitiveDependency(subPatch, cpatchTo);
            }

            return false;
        }

        public void Move(Release newRelease)
        {
            if (release != newRelease)
            {
                release.cpatches.Remove(this);
                release.CPatchesDict.Remove(CPatchId);

                newRelease.cpatches.Add(this);
                newRelease.CPatchesDict.Add(CPatchId, this);

                CPatchDAL.UpdateRelease(CPatchId, newRelease.releaseId);
            }
        }

        public void InitZPatches()
        {
            zpatches = new List<ZPatch>();
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

                zpatches.Add(zpatch);
                ZPatchesDict.Add(zpatch.ZPatchId, zpatch);

                zpatch.cpatch = this;
            }
        }

        public void ResetStatusesByLog()
        {
            foreach(ZPatch zpatch in zpatches)
            {
                if (zpatch.ZPatchStatus != ZPatchStatuses.INSTALLED && ZPatchDAL.IsZPatchInstalled(zpatch.ZPatchName, KodSredy.ToString()))
                {
                    zpatch.ZPatchStatus = ZPatchStatuses.INSTALLED;
                    ZPatchDAL.UpdateStatus(zpatch.ZPatchId, ZPatchStatuses.INSTALLED.ToString());
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

        private string GetCVSPath()
        {
            string cvsRoot = CVSProjectsDAL.GetPath(KodSredy.ToString());
            string match = null;
            string shortName = Regex.Match(CPatchName, @"C\d+").Value;
            return cvs.FirstInEntireBase(cvsRoot, ref match, new Regex(shortName), 2);
        }

        public void Download()
        {
            cvs.Download(GetCVSPath(), LocalPath);
        }

        public CPatch(int CPatchId, string CPatchName, CPatchStatuses CPatchStatus, EnvCodes KodSredy, Release release)
        {
            this.release = release;
            InitFromDB(CPatchId, CPatchName, CPatchStatus, KodSredy);

            LocalPath = $"{release.rm.homeDir.FullName}/{CPatchName}";

            //лучше их инициализировать сразу, чтобы проще было с зависимостями
            InitZPatches();
            InitZPatchOrder();
        }

        private void InitZPatchOrder()
        {
            ZPatchOrder = new SortedList<int, ZPatch>();
            var zpatchOrderRecords = ZPatchOrderDAL.GetZPatchOrdersByCPatch(CPatchId);
            foreach(ZPatchOrderRecord patchOrderRecord in zpatchOrderRecords)
            {
                ZPatchOrder.Add(patchOrderRecord.zpatchOrder, ZPatchesDict[patchOrderRecord.zpatchId]);
            }
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

        private void AddNewDependenciesToList(List<ZPatch> newPatches)
        {
            var fullList = AllDependenciesToList();
            int i = fullList.Count - newPatches.Count;

            //иду по порядку, добавляю в конец новые патчи
            foreach(var patch in fullList.Values)
            {
                if(newPatches.Contains(patch))
                {
                    ZPatchOrder.Add(i, patch);
                    ZPatchOrderDAL.Insert(patch.ZPatchId, i);

                    i++;
                }
            }
        }

        private void ResetAllDependenciesToList()
        {
            var sourceList = AllDependenciesToList();

            SortedList<int, ZPatch> list = new SortedList<int, ZPatch>();

            int i = 0;
            foreach (var item in sourceList)
            {
                list.Add(i++, item.Value);
            }

            foreach (var item in list)
            {
                ZPatchOrderDAL.Insert(item.Value.ZPatchId, item.Key);
            }
        }

        public void UpdateZPatchOrder(ZPatch zpatch, int order)
        {
            if(ZPatchOrder[order] != zpatch)
            {
                ZPatchOrder[order] = zpatch;
                ZPatchOrderDAL.Update(zpatch.ZPatchId, order);
            }
        }

        private SortedList<int, ZPatch> AllDependenciesToList()
        {
            List<ZPatch> roots = new List<ZPatch>();
            foreach (ZPatch patch in zpatches)
            {
                if (patch.dependenciesFrom.Count == 0)
                {
                    roots.Add(patch);
                }
            }

            foreach (ZPatch root in roots)
            {
                root.rank = 0;
                SetChildrenRanks(root);
            }

            SortedList<int, ZPatch> sourceList = new SortedList<int, ZPatch>(Comparer<int>.Create((x, y) => x < y ? -1 : 1));

            foreach (ZPatch zpatch in zpatches)
            {
                sourceList.Add(zpatch.rank, zpatch);
            }

            return sourceList;
        }

        public void Rename(string newName)
        {
            if(newName != null && newName != CPatchName)
            {
                CPatchName = newName;
                CPatchDAL.UpdateName(CPatchId, CPatchName);
            }
        }

        public void ChangeRelease(Release newRelease)
        {
            if(release.releaseId != newRelease.releaseId)
            {
                release.cpatches.Remove(this);
                release.CPatchesDict.Remove(CPatchId);

                newRelease.cpatches.Add(this);
                newRelease.CPatchesDict.Add(CPatchId, this);

                CPatchDAL.UpdateRelease(CPatchId, newRelease.releaseId);
            }
        }

        private void SetChildrenRanks(ZPatch currPatch)
        {
            foreach (ZPatch subpatch in currPatch.dependenciesTo)
            {
                subpatch.rank = Math.Max(subpatch.rank, currPatch.rank + 1);
                SetChildrenRanks(subpatch);
            }
        }


        private void ReopenExcelColumns(FileInfo excelFile)
        {
            Workbook wb = ReleaseManager.excelApp.Workbooks.Open(excelFile.FullName);
            Worksheet ws = wb.Sheets[1];
            Range res = ws.UsedRange;
            List<ZPatch> newPatches;

            List<Tuple<ZPatch, ZPatch>> deletedDependenciesFrom;
            List<Tuple<ZPatch, ZPatch>> addedDependenciesFrom;
            List<Tuple<ZPatch, ZPatch>> deletedDependenciesTo;
            List<Tuple<ZPatch, ZPatch>> addedDependenciesTo;

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
            List<Tuple<ZPatch, ZPatch>> deletedDependenciesFrom,
            List<Tuple<ZPatch, ZPatch>> addedDependenciesFrom,
            List<Tuple<ZPatch, ZPatch>> deletedDependenciesTo,
            List<Tuple<ZPatch, ZPatch>> addedDependenciesTo,
            List<ZPatch> newPatches)
        {
            //deletedfrom
            foreach (var deletedDependency in deletedDependenciesFrom)
            {
                ZPatchDAL.DeleteDependency(deletedDependency.Item2.ZPatchId, deletedDependency.Item1.ZPatchId);

                if (deletedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    bool canDeleteCDependency = true;
                    foreach (ZPatch zPatch in zpatches)
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
                        dependenciesFrom.Remove(deletedDependency.Item2.cpatch);
                        deletedDependency.Item2.cpatch.dependenciesTo.Remove(this);

                        CPatchDAL.DeleteDependency(CPatchId, deletedDependency.Item2.cpatch.CPatchId);
                    }
                }

            }

            //addedfrom
            foreach (var addedDependency in addedDependenciesFrom)
            {
                ZPatchDAL.AddDependency(addedDependency.Item2.ZPatchId, addedDependency.Item1.ZPatchId);
                if (addedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    if (!dependenciesFrom.Contains(addedDependency.Item2.cpatch))
                    {
                        dependenciesFrom.Add(addedDependency.Item2.cpatch);
                        addedDependency.Item2.cpatch.dependenciesTo.Add(this);

                        CPatchDAL.AddDependency(addedDependency.Item2.cpatch.CPatchId, CPatchId);
                    }
                }
            }

            //deletedto
            foreach (var deletedDependency in deletedDependenciesTo)
            {
                ZPatchDAL.DeleteDependency(deletedDependency.Item1.ZPatchId, deletedDependency.Item2.ZPatchId);

                if (deletedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    bool canDeleteCDependency = true;
                    foreach (ZPatch zPatch in zpatches)
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
                        dependenciesTo.Remove(deletedDependency.Item2.cpatch);
                        deletedDependency.Item2.cpatch.dependenciesFrom.Remove(this);

                        CPatchDAL.DeleteDependency(deletedDependency.Item2.cpatch.CPatchId, CPatchId);
                    }
                }

            }

            //addedto
            foreach (var addedDependency in addedDependenciesTo)
            {
                ZPatchDAL.AddDependency(addedDependency.Item1.ZPatchId, addedDependency.Item2.ZPatchId);
                if (addedDependency.Item2.cpatch.CPatchId != CPatchId)
                {
                    if (!dependenciesFrom.Contains(addedDependency.Item2.cpatch))
                    {
                        dependenciesTo.Add(addedDependency.Item2.cpatch);
                        addedDependency.Item2.cpatch.dependenciesFrom.Add(this);

                        CPatchDAL.AddDependency(CPatchId, addedDependency.Item2.cpatch.CPatchId);
                    }
                }
            }

            foreach(ZPatch z1 in zpatches)
            {
                foreach(ZPatch z2 in zpatches)
                {
                    if(ZPatch.HaveTransitiveDependency(z1, z2) &&
                       ZPatch.HaveTransitiveDependency(z2, z1))
                    {
                        throw new Exception($"Одновременные зависимости {z1} -> {z2} и {z2} -> {z1}");
                    }
                }
            }

            bool oldDependenciesAreCorrect = true;

            //определяем, должен ли порядок быть переписан, или достаточно только добавить новые патчи в конец
            foreach (var newDependency in addedDependenciesFrom)
            {
                //если появилась новая зависимость, такая, что появился новый патч, влияющий на старый
                if (newPatches.Contains(newDependency.Item1) && !newPatches.Contains(newDependency.Item2))
                {
                    oldDependenciesAreCorrect = false;
                    break;
                }

                //если появилась новая зависимость, такая, что старый порядок для старых патчей стал некорректным
                if (ZPatchOrder.IndexOfValue(newDependency.Item1) > ZPatchOrder.IndexOfValue(newDependency.Item2))
                {
                    oldDependenciesAreCorrect = false;
                    break;
                }
            }

            if (oldDependenciesAreCorrect)
                foreach (var newDependency in addedDependenciesTo)
                {
                    //если появилась новая зависимость, такая, что появился новый патч, влияющий на старый
                    if (!newPatches.Contains(newDependency.Item1) && newPatches.Contains(newDependency.Item2))
                    {
                        oldDependenciesAreCorrect = false;
                        break;
                    }

                    //если появилась новая зависимость, такая, что старый порядок для старых патчей стал некорректным
                    if (ZPatchOrder.IndexOfValue(newDependency.Item1) < ZPatchOrder.IndexOfValue(newDependency.Item2))
                    {
                        oldDependenciesAreCorrect = false;
                        break;
                    }
                }

            if (oldDependenciesAreCorrect)
            {
                AddNewDependenciesToList(newPatches);
            }
            else
            {
                ResetAllDependenciesToList();
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
            empty.zpatches = new List<ZPatch>();
            empty.ZPatchesDict = new Dictionary<int, ZPatch>();

            empty.dependenciesFrom = new HashSet<CPatch>();
            empty.dependenciesTo = new HashSet<CPatch>();
            empty.ZPatchOrder = new SortedList<int, ZPatch>();
            empty.release = release;

            empty.release.cpatches.Add(empty);
            //release.CPatchesDict.Add(-1, empty);

            empty.ReopenExcelColumns(excelFile);
            empty.release.CPatchesDict.Add(empty.CPatchId, empty);
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
            foreach (CPatch cpatch in release.cpatches)
            {
                try
                {
                    patch = cpatch.zpatches.First(x => SameEnding(x.ZPatchName, shortName));
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
            if (newStatus != CPatchStatus)
            {
                CPatchStatus = newStatus;
                CPatchDAL.UpdateStatus(CPatchId, newStatus.ToString());
            }
        }

        private void AddNewZPatches(Range columns, out List<ZPatch> newPatches)
        {
            newPatches = new List<ZPatch>();

            int patchNameIndex = GetPatchNameIndex(columns);
            int patchStatusIndex = GetPatchStatusIndex(columns);

            CPatchName = "NOT DEFINED";
            CPatchId = CPatchDAL.Insert(release.releaseId, null, CPatchName, null, null);

            for (int i = 2; i <= columns.Rows.Count; ++i)
            {
                string patchCell = ((Range)columns.Cells[i, patchNameIndex]).Value2 ?? "";
                string excelStatus = ((Range)columns.Cells[i, patchStatusIndex]).Value2;
                ZPatchStatuses ZPatchStatus = ZPatchStatuses.UNDEFINED;

                if (excelStatus == "Открытый")
                    ZPatchStatus = ZPatchStatuses.OPEN;
                else if (excelStatus == "Testing" || excelStatus == "Waiting bank confirm")
                    ZPatchStatus = ZPatchStatuses.READY;
                else if (excelStatus == "Installed to STAB" || excelStatus == "Installed to STAB2")
                    ZPatchStatus = ZPatchStatuses.INSTALLED;

                MatchCollection matches = Regex.Matches(patchCell, regexZPatchName);
                if (matches.Count == 0)
                {
                    matches = Regex.Matches(patchCell, regexCPatchName);
                    if (matches.Count > 0)
                    {
                        //ФП еще не добавлялся
                        if (CPatchName == "NOT DEFINED")
                        {
                            CPatchName = patchCell;
                            CPatchDAL.UpdateName(CPatchId, patchCell);
                        }
                    }
                }
                else
                {
                    string patchName = Regex.Match(patchCell, regexZPatchName).Value;
                    ZPatch currPatch;
                    if (!findPatchByShortName(patchName, out currPatch))
                    {
                        if (ZPatchStatus != ZPatchStatuses.OPEN)
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
                        }
                    }
                }
            }

            foreach(ZPatch zpatch in newPatches)
            {
                zpatch.ZPatchId = ZPatchDAL.Insert(CPatchId, null, zpatch.ZPatchName, null);
                zpatches.Add(zpatch);
                ZPatchesDict.Add(zpatch.ZPatchId, zpatch);
            }            
        }

        public Graph DrawGraph()
        {
            Graph graph = new Graph();
            foreach (ZPatch zpatch in zpatches)
            {
                Microsoft.Msagl.Drawing.Node node = new Microsoft.Msagl.Drawing.Node(zpatch.ZPatchId.ToString());
                node.Attr.FillColor = Color.LightGreen;
                node.Label.Text = zpatch.ZPatchName;
                graph.AddNode(node);
            }

            foreach (ZPatch zpatch in zpatches)
            {
                foreach (ZPatch depFrom in zpatch.dependenciesFrom)
                {
                    if(depFrom.cpatch != this)
                    {
                        Microsoft.Msagl.Drawing.Node node = new Microsoft.Msagl.Drawing.Node(depFrom.ZPatchId.ToString());
                        node.Attr.FillColor = Color.DarkRed;
                        node.Label.FontColor = Color.White;
                        node.LabelText = $"{depFrom.ZPatchName} ({depFrom.cpatch.CPatchName})";
                        graph.AddNode(node);
                    }
                    graph.AddEdge(depFrom.ZPatchId.ToString(), zpatch.ZPatchId.ToString());
                }

                foreach (ZPatch depTo in zpatch.dependenciesTo)
                {
                    if (depTo.cpatch != this)
                    {
                        Microsoft.Msagl.Drawing.Node node = new Microsoft.Msagl.Drawing.Node(depTo.ZPatchId.ToString());
                        node.Attr.FillColor = Color.DarkRed;
                        node.Label.FontColor = Color.White;
                        node.LabelText = $"{depTo.ZPatchName} ({depTo.cpatch.CPatchName})";
                        graph.AddNode(node);
                        //должна быть именно здесь, чтобы не было двух дуг
                        graph.AddEdge(zpatch.ZPatchId.ToString(), depTo.ZPatchId.ToString());
                    }
                }
            }

            return graph;
        }        

        private void CreateCPatchDelta(
            Range columns,
            out List<Tuple<ZPatch, ZPatch>> deletedDependenciesFrom,
            out List<Tuple<ZPatch, ZPatch>> addedDependenciesFrom,
            out List<Tuple<ZPatch, ZPatch>> deletedDependenciesTo,
            out List<Tuple<ZPatch, ZPatch>> addedDependenciesTo)
        {
            deletedDependenciesFrom = new List<Tuple<ZPatch, ZPatch>>();
            addedDependenciesFrom = new List<Tuple<ZPatch, ZPatch>>();
            deletedDependenciesTo = new List<Tuple<ZPatch, ZPatch>>();
            addedDependenciesTo = new List<Tuple<ZPatch, ZPatch>>();


            int linkIndex = GetLinkIndex(columns);
            if (linkIndex != -1)
            {
                foreach(ZPatch zpatch in zpatches)
                {
                    string dependenciesCell = ((Range)columns.Cells[zpatch.excelFileRowId, linkIndex]).Value2 ?? "";
                      var dependenciesFrom = DependenciesFrom(dependenciesCell);

                    foreach (ZPatch excelFromDependency in dependenciesFrom)
                    {
                        if (!zpatch.dependenciesFrom.Contains(excelFromDependency))
                        {
                            addedDependenciesFrom.Add(new Tuple<ZPatch, ZPatch>(zpatch, excelFromDependency));

                            zpatch.dependenciesFrom.Add(excelFromDependency);
                            excelFromDependency.dependenciesTo.Add(zpatch);
                        }
                    }

                    foreach (ZPatch patchFromDependency in zpatch.dependenciesFrom)
                    {
                        if (!dependenciesFrom.Contains(patchFromDependency))
                        {
                            deletedDependenciesFrom.Add(new Tuple<ZPatch, ZPatch>(zpatch, patchFromDependency));

                            zpatch.dependenciesFrom.Remove(patchFromDependency);
                            patchFromDependency.dependenciesTo.Remove(zpatch);
                        }
                    }

                    var dependenciesTo = DependenciesTo(dependenciesCell);

                    foreach (ZPatch excelToDependency in dependenciesTo)
                    {
                        if (!zpatch.dependenciesTo.Contains(excelToDependency))
                        {
                            addedDependenciesTo.Add(new Tuple<ZPatch, ZPatch>(zpatch, excelToDependency));

                            zpatch.dependenciesTo.Add(excelToDependency);
                            excelToDependency.dependenciesFrom.Add(zpatch);
                        }
                    }

                    foreach (ZPatch patchToDependency in zpatch.dependenciesTo)
                    {
                        if (!dependenciesTo.Contains(patchToDependency))
                        {
                            deletedDependenciesFrom.Add(new Tuple<ZPatch, ZPatch>(zpatch, patchToDependency));

                            zpatch.dependenciesTo.Remove(patchToDependency);
                            patchToDependency.dependenciesFrom.Remove(zpatch);
                        }
                    }
                    
                }
            }
        }
    }
}


