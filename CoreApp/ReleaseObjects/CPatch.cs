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
using static CoreApp.Scenario.Scenario;

namespace CoreApp.ReleaseObjects
{
    public class CPatch
    {
        public string C { get; private set; }
        public string FullName { get; private set; }
        public string CPatchName { get; private set; }
        public string CPatchStatus { get; private set; }
        public DirectoryInfo Dir { get; private set; }
        public string cvsPath;
        public static CVS.CVS cvs;

        public List<ZPatch> ZPatches { get; protected set; } //отсортированный на DAL
        public Dictionary<int, ZPatch> ZPatchesDict { get; protected set; } //для поиска

        public HashSet<CPatch> DependenciesFrom { get; protected set; }
        public HashSet<CPatch> DependenciesTo { get; protected set; }
        public Release release;
        public string KodSredy { get; set; }
        public SortedList<int, ZPatch> ZPatchOrder { get; private set; }

        public int CPatchId { get; private set; }

        public static IEnumerable<string> cpatchStatuses;
        public static IEnumerable<string> cpatchEnvCodes;

        static CPatch()
        {
            cpatchStatuses = CPatchStateDAL.GetStatuses();
            cpatchEnvCodes = CPatchStateDAL.GetEnvCodes();
        }


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

        public ZPatch GetZPatchById(int id)
        {
            foreach(CPatch cpatch in release.CPatches)
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

        private void InitFromDB(int CPatchId, string CPatchName, string CPatchStatus, string KodSredy)
        {
            this.CPatchId = CPatchId;
            this.CPatchName = CPatchName;
            this.CPatchStatus = CPatchStatus;
            this.KodSredy = KodSredy;
        }

        public void Delete()
        {
            foreach(CPatch cpatchFrom in DependenciesFrom)
            {
                cpatchFrom.DependenciesTo.Remove(this);
                CPatchDAL.DeleteDependency(cpatchFrom.CPatchId, CPatchId);
            }

            foreach(CPatch cpatchTo in DependenciesTo)
            {
                cpatchTo.DependenciesFrom.Remove(this);
                CPatchDAL.DeleteDependency(CPatchId, cpatchTo.CPatchId);
            }

            CPatchDAL.DeleteCPatch(CPatchId);
            release.CPatches.Remove(this);
            release.CPatchesDict.Remove(CPatchId);
        }

        public static bool CanDeleteCPatchDependency(CPatch cpatchFrom, CPatch cpatchTo, out ZPatch zpatchFromDependency, out ZPatch zpatchToDependency)
        {
            foreach (var zpatchFrom in cpatchFrom.ZPatches)
            {
                foreach (var zpatchTo in cpatchTo.ZPatches)
                {
                    if (zpatchFrom.DependenciesTo.Contains(zpatchTo))
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
            DependenciesFrom.Remove(cpatchFrom);
            cpatchFrom.DependenciesTo.Remove(this);
            CPatchDAL.DeleteDependency(cpatchFrom.CPatchId, CPatchId);
        }

        public void DeleteDependencyTo(CPatch cpatchTo)
        {
            DependenciesTo.Remove(cpatchTo);
            cpatchTo.DependenciesFrom.Remove(this);
            CPatchDAL.DeleteDependency(CPatchId, cpatchTo.CPatchId);
        }

        public void AddDependencyFrom(CPatch cpatchFrom)
        {
            DependenciesFrom.Add(cpatchFrom);
            cpatchFrom.DependenciesTo.Add(this);
            CPatchDAL.AddDependency(cpatchFrom.CPatchId, CPatchId);
        }

        public void AddDependencyTo(CPatch cpatchTo)
        {
            DependenciesTo.Add(cpatchTo);
            cpatchTo.DependenciesFrom.Add(this);
            CPatchDAL.AddDependency(CPatchId, cpatchTo.CPatchId);
        }

        public static bool HaveTransitiveDependency(CPatch cpatchFrom, CPatch cpatchTo)
        {
            foreach(CPatch subPatch in cpatchFrom.DependenciesTo)
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
                release.CPatches.Remove(this);
                release.CPatchesDict.Remove(CPatchId);

                newRelease.CPatches.Add(this);
                newRelease.CPatchesDict.Add(CPatchId, this);

                CPatchDAL.UpdateRelease(CPatchId, newRelease.ReleaseId);
            }
        }

        public void InitZPatches()
        {
            ZPatches = new List<ZPatch>();
            ZPatchesDict = new Dictionary<int, ZPatch>();
            var oraZPatchesRecords = ZPatchDAL.GetZPatchesByCPatch(CPatchId);


            foreach (var oraZPatchRecord in oraZPatchesRecords)
            {
                ZPatch zpatch = new ZPatch(
                    this,
                    oraZPatchRecord.ZPatchName,
                    oraZPatchRecord.ZPatchId,
                    oraZPatchRecord.ZPatchStatus);

                ZPatches.Add(zpatch);
                ZPatchesDict.Add(zpatch.ZPatchId, zpatch);

                zpatch.cpatch = this;
            }
        }

        public void ResetStatusesByLog()
        {
            foreach(ZPatch zpatch in ZPatches)
            {
                if (ZPatchDAL.IsZPatchInstalled(zpatch.ZPatchName, KodSredy))
                {
                    zpatch.ZPatchStatus = "INSTALLED";
                    ZPatchDAL.UpdateStatus(zpatch.ZPatchId, zpatch.ZPatchStatus);
                }
            }
        }
        
        public void SetDependencies()
        {
            DependenciesFrom = new HashSet<CPatch>();

            var oraCPatchesDependenciesFrom = CPatchDAL.GetDependenciesFrom(CPatchId);

            foreach (var oraCPatchRecord in oraCPatchesDependenciesFrom)
            {
                DependenciesFrom.Add(release.GetCPatchById(oraCPatchRecord.CPatchId));
            }

            DependenciesTo = new HashSet<CPatch>();

            var oraCPatchesDependenciesTo = CPatchDAL.GetDependenciesTo(CPatchId);

            foreach (var oraCPatchRecord in oraCPatchesDependenciesTo)
            {
                DependenciesTo.Add(release.GetCPatchById(oraCPatchRecord.CPatchId));
            }
        }

        private string GetCVSPath()
        {
            string cvsRoot = CPatchStateDAL.GetCVSPath(CPatchStatus, KodSredy);
            string shortName = Regex.Match(CPatchName, @"C\d+").Value;

            string cvsForder = cvs.FirstInEntireBase(cvsRoot, out string cpatchmatch, new Regex(shortName), 2);

            foreach (ZPatch zpatch in ZPatches)
            {
                if (zpatch.ZPatchStatus != "OPEN")
                {
                    string patchRoot = $"{cvsRoot}/{cpatchmatch}";
                    try
                    {
                        cvs.FirstInEntireBase(patchRoot, out string zpatchmatch, new Regex(zpatch.ZPatchName), 1);
                        zpatch.Dir = new DirectoryInfo(Path.Combine(Dir.FullName, zpatchmatch));
                    }
                    catch
                    {
                        throw new DirectoryNotFoundException($"Патч {zpatch.ZPatchName} не найден. Добавьте его в папку C-патча или переведите статус в OPEN");
                    }
                }
            }

            return cvsForder;
        }

        public void Download()
        {
            SetAttributesNormal(Dir);
            cvs.Download(GetCVSPath(), Dir);
        }

        private void DeleteLocal()
        {
            if (Dir.Exists)
            {
                SetAttributesNormal(Dir);
                Dir.Delete(true);
            }
        }

        private void SetAttributesNormal(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                foreach (var subDir in dir.GetDirectories())
                    SetAttributesNormal(subDir);
                foreach (var file in dir.GetFiles())
                {
                    file.Attributes = FileAttributes.Normal;
                }
            }
        }

        public CPatch(int CPatchId, string CPatchName, string CPatchStatus, string KodSredy, Release release)
        {
            this.release = release;
            InitFromDB(CPatchId, CPatchName, CPatchStatus, KodSredy);

            Dir = new DirectoryInfo(Path.Combine(release.rm.homeDir.FullName, CPatchName));

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
            foreach (ZPatch patch in ZPatches)
            {
                if (patch.DependenciesFrom.Count == 0)
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

            foreach (ZPatch zpatch in ZPatches)
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
            if(release.ReleaseId != newRelease.ReleaseId)
            {
                release.CPatches.Remove(this);
                release.CPatchesDict.Remove(CPatchId);

                newRelease.CPatches.Add(this);
                newRelease.CPatchesDict.Add(CPatchId, this);

                CPatchDAL.UpdateRelease(CPatchId, newRelease.ReleaseId);
            }
        }

        public IEnumerable<Tuple<LineState, string>> CreateScenario()
        {
            Download();
            Scenario.Scenario scenario = new Scenario.Scenario(this);
            return scenario.CreateScenarioFromZPatches();
        }

        private void SetChildrenRanks(ZPatch currPatch)
        {
            foreach (ZPatch subpatch in currPatch.DependenciesTo)
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


            AddNewZPatches(res, out List<ZPatch> newPatches);
            CreateCPatchDelta(
                res,
                out List<Tuple<ZPatch, ZPatch>> deletedDependenciesFrom,
                out List<Tuple<ZPatch, ZPatch>> addedDependenciesFrom,
                out List<Tuple<ZPatch, ZPatch>> deletedDependenciesTo,
                out List<Tuple<ZPatch, ZPatch>> addedDependenciesTo);

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
                    foreach (ZPatch zPatch in ZPatches)
                    {
                        foreach (ZPatch dependency in zPatch.DependenciesFrom)
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
                        DependenciesFrom.Remove(deletedDependency.Item2.cpatch);
                        deletedDependency.Item2.cpatch.DependenciesTo.Remove(this);

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
                    if (!DependenciesFrom.Contains(addedDependency.Item2.cpatch))
                    {
                        DependenciesFrom.Add(addedDependency.Item2.cpatch);
                        addedDependency.Item2.cpatch.DependenciesTo.Add(this);

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
                    foreach (ZPatch zPatch in ZPatches)
                    {
                        foreach (ZPatch dependency in zPatch.DependenciesTo)
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
                        DependenciesTo.Remove(deletedDependency.Item2.cpatch);
                        deletedDependency.Item2.cpatch.DependenciesFrom.Remove(this);

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
                    if (!DependenciesFrom.Contains(addedDependency.Item2.cpatch))
                    {
                        DependenciesTo.Add(addedDependency.Item2.cpatch);
                        addedDependency.Item2.cpatch.DependenciesFrom.Add(this);

                        CPatchDAL.AddDependency(CPatchId, addedDependency.Item2.cpatch.CPatchId);
                    }
                }
            }

            foreach(ZPatch z1 in ZPatches)
            {
                foreach(ZPatch z2 in ZPatches)
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
            CPatch empty = new CPatch
            {
                ZPatches = new List<ZPatch>(),
                ZPatchesDict = new Dictionary<int, ZPatch>(),

                DependenciesFrom = new HashSet<CPatch>(),
                DependenciesTo = new HashSet<CPatch>(),
                ZPatchOrder = new SortedList<int, ZPatch>(),
                release = release
            };

            empty.release.CPatches.Add(empty);

            empty.ReopenExcelColumns(excelFile);
            empty.release.CPatchesDict.Add(empty.CPatchId, empty);
            return empty;
        }

        private static readonly string regexZPatchName = @"(Z)[0-9]+";
        private static readonly string regexCPatchName = @"(C)[0-9]+";

        private static readonly string regexFrom = "зависит.*?ALFAM.*?([0-9]+)";
        private static readonly string regexTo = "влияет.*?ALFAM.*?([0-9]+)";

        private HashSet<ZPatch> GetDependenciesFrom(string rawString)
        {
            HashSet<ZPatch> res = new HashSet<ZPatch>();
            MatchCollection matchesFrom = Regex.Matches(rawString, regexFrom);
            foreach (Match m in matchesFrom)
            {
                if (FindPatchByShortName(m.Groups[1].Value, out ZPatch zPatch))
                {
                    res.Add(zPatch);
                }
                //TODO мб отлавливать случаи, когда не найден. предупреждение или тип того
            }
            return res;
        }

        private HashSet<ZPatch> GetDependenciesTo(string rawString)
        {
            HashSet<ZPatch> res = new HashSet<ZPatch>();
            MatchCollection matchesTo = Regex.Matches(rawString, regexTo);
            foreach (Match m in matchesTo)
            {
                if (FindPatchByShortName(m.Groups[1].Value, out ZPatch zpatch))
                {
                    res.Add(zpatch);
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

        private bool FindPatchByShortName(string shortName, out ZPatch patch)
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

        public void UpdateStatus(string newStatus)
        {
            if (newStatus != CPatchStatus)
            {
                CPatchStatus = newStatus;
                CPatchDAL.UpdateStatus(CPatchId, newStatus);
            }
        }

        public void UpdateEnvCode(string newEnvCode)
        {
            if(newEnvCode != KodSredy)
            {
                KodSredy = newEnvCode;
                CPatchDAL.UpdateEnvCode(CPatchId, newEnvCode);
            }
        }

        private void AddNewZPatches(Range columns, out List<ZPatch> newPatches)
        {
            newPatches = new List<ZPatch>();

            int patchNameIndex = GetPatchNameIndex(columns);
            int patchStatusIndex = GetPatchStatusIndex(columns);

            CPatchName = "NOT DEFINED";
            CPatchId = CPatchDAL.Insert(release.ReleaseId, null, CPatchName, null, null);

            for (int i = 2; i <= columns.Rows.Count; ++i)
            {
                string patchCell = ((Range)columns.Cells[i, patchNameIndex]).Value2 ?? "";
                string excelStatus = ((Range)columns.Cells[i, patchStatusIndex]).Value2;
                string ZPatchStatus = "UNDEFINED";

                if (excelStatus == "Открытый")
                    ZPatchStatus = "OPEN";
                else if (excelStatus == "Testing" || excelStatus == "Waiting bank confirm")
                    ZPatchStatus = "READY";
                else if (excelStatus == "Installed to STAB" || excelStatus == "Installed to STAB2")
                    ZPatchStatus = "INSTALLED";

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
                    if (!FindPatchByShortName(patchName, out ZPatch currPatch))
                    {
                        if (ZPatchStatus != "OPEN")
                        {
                            ZPatch zpatch = new ZPatch(
                                patchName,
                                CPatchId,
                                new HashSet<ZPatch>(),
                                new HashSet<ZPatch>(),
                                ZPatchStatus)
                            {
                                excelFileRowId = i,
                                cpatch = this
                            };

                            newPatches.Add(zpatch);
                        }
                    }
                }
            }

            foreach(ZPatch zpatch in newPatches)
            {
                zpatch.ZPatchId = ZPatchDAL.Insert(CPatchId, null, zpatch.ZPatchName, null);
                ZPatches.Add(zpatch);
                ZPatchesDict.Add(zpatch.ZPatchId, zpatch);
            }            
        }

        public Graph DrawGraph()
        {
            Graph graph = new Graph();
            foreach (ZPatch zpatch in ZPatches)
            {
                Microsoft.Msagl.Drawing.Node node = new Microsoft.Msagl.Drawing.Node(zpatch.ZPatchId.ToString());
                node.Attr.FillColor = Color.LightGreen;
                node.Label.Text = zpatch.ZPatchName;
                graph.AddNode(node);
            }

            foreach (ZPatch zpatch in ZPatches)
            {
                foreach (ZPatch depFrom in zpatch.DependenciesFrom)
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

                foreach (ZPatch depTo in zpatch.DependenciesTo)
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
                foreach(ZPatch zpatch in ZPatches)
                {
                    string dependenciesCell = ((Range)columns.Cells[zpatch.excelFileRowId, linkIndex]).Value2 ?? "";
                      var dependenciesFrom = GetDependenciesFrom(dependenciesCell);

                    foreach (ZPatch excelFromDependency in dependenciesFrom)
                    {
                        if (!zpatch.DependenciesFrom.Contains(excelFromDependency))
                        {
                            addedDependenciesFrom.Add(new Tuple<ZPatch, ZPatch>(zpatch, excelFromDependency));

                            zpatch.DependenciesFrom.Add(excelFromDependency);
                            excelFromDependency.DependenciesTo.Add(zpatch);
                        }
                    }

                    foreach (ZPatch patchFromDependency in zpatch.DependenciesFrom)
                    {
                        if (!dependenciesFrom.Contains(patchFromDependency))
                        {
                            deletedDependenciesFrom.Add(new Tuple<ZPatch, ZPatch>(zpatch, patchFromDependency));

                            zpatch.DependenciesFrom.Remove(patchFromDependency);
                            patchFromDependency.DependenciesTo.Remove(zpatch);
                        }
                    }

                    var dependenciesTo = GetDependenciesTo(dependenciesCell);

                    foreach (ZPatch excelToDependency in dependenciesTo)
                    {
                        if (!zpatch.DependenciesTo.Contains(excelToDependency))
                        {
                            addedDependenciesTo.Add(new Tuple<ZPatch, ZPatch>(zpatch, excelToDependency));

                            zpatch.DependenciesTo.Add(excelToDependency);
                            excelToDependency.DependenciesFrom.Add(zpatch);
                        }
                    }

                    foreach (ZPatch patchToDependency in zpatch.DependenciesTo)
                    {
                        if (!dependenciesTo.Contains(patchToDependency))
                        {
                            deletedDependenciesFrom.Add(new Tuple<ZPatch, ZPatch>(zpatch, patchToDependency));

                            zpatch.DependenciesTo.Remove(patchToDependency);
                            patchToDependency.DependenciesFrom.Remove(zpatch);
                        }
                    }
                    
                }
            }
        }
    }
}


