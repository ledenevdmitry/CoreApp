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
using CoreApp.OraUtils;

namespace CoreApp.ReleaseObjects
{

    public class Release
    {
        public int releaseId { get; private set; }

        public override int GetHashCode()
        {
            return releaseId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Release)) return false;
            return ((Release)obj).releaseId == releaseId;
        }

        public string releaseName { get; private set; }
        public string releaseStatus { get; private set; }
        public List<CPatch> CPatches { get; private set; } //отсортированный на DAL
        public Dictionary<int, CPatch> CPatchesDict { get; private set; } //для поиска
        


        public CPatch getCPatchById(int id)
        {
            return CPatchesDict[id];
        }

        public Release(int releaseId, string release)
        {
            InitFromDB(releaseId, releaseName);
        }

        private void InitFromDB()
        {
            var oraCPatches = CPatchDAL.getCPatchesByRelease(releaseId);
            foreach (var oraCPatch in oraCPatches)
            {
                CPatch cpatch = new CPatch(oraCPatch.CPatchId, oraCPatch.CPatchName, oraCPatch.CPatchStatus);
                CPatches.Add(cpatch);
                CPatchesDict.Add(cpatch.CPatchId, cpatch);
            }
        }

        private void InitFromDB(int releaseId, string releaseName)
        {
            this.releaseId = releaseId;
            this.releaseName = releaseName;
            InitFromDB();
        }
        
        DirectoryInfo localDir;
        public static CVS.CVS cvs;

        private void setAttributesNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
                setAttributesNormal(subDir);
            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }

        public void DeleteLocal()
        {
            if (localDir.Exists)
            {
                setAttributesNormal(localDir);
                localDir.Delete(true);
            }
        }

        public void SetLocalDir(DirectoryInfo localDir)
        {
            this.localDir = localDir;
        }

        public void ReinitByExcelFile()
        {
            foreach(CPatch fp in CPatches)
            {
                try
                {
                    ReadMetaFromExcelFile(fp, FindLocalExcel(fp));
                }
                catch { }
            }
        }


        public bool ReadMetaFromExcelFile(CPatch fp, string newExcelFilePath)
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

        private static string regexZPatchName = @"(Z)[0-9]+";
        private static string regexCPatchName = @"(C)[0-9]+";

        private static string regexFrom = "зависит.*?ALFAM.*?([0-9]+)";
        private static string regexTo = "влияет.*?ALFAM.*?([0-9]+)";

        private IEnumerable<ZPatch> DependedFrom(string rawString)
        {
            MatchCollection matchesFrom = Regex.Matches(rawString, regexFrom);
            foreach (Match m in matchesFrom)
            {
                yield return findPatchByShortName(m.Groups[1].Value);
            }
        }

        private IEnumerable<ZPatch> DependOn(string rawString)
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

        private ZPatch findPatchByShortName(string shortName)
        {
            foreach (CPatch fp in CPatches)
            {
                return fp.ZPatches.First(x => SameEnding(x.name, shortName));
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
                    string patchName = Regex.Match(patchCell, regexZPatchName).Value;

                    try
                    {
                        ZPatch currPatch = findPatchByShortName(patchName);

                        currPatch.dependenciesFrom.UnionWith(DependedFrom(dependenciesCell));
                        currPatch.dependenciesTo.UnionWith(DependOn(dependenciesCell));
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
                    string patchName = Regex.Match(patchCell, regexZPatchName).Value;

                    try
                    {
                        ZPatch currPatch = findPatchByShortName(patchName);

                        foreach(ZPatch excelFromDependency in DependedFrom(dependenciesCell))
                        {
                            if(!currPatch.dependenciesFrom.Contains(excelFromDependency))
                            {
                                return false;
                            }
                        }

                        foreach (ZPatch excelToDependency in DependOn(dependenciesCell))
                        {
                            if (!currPatch.dependenciesTo.Contains(excelToDependency))
                            {
                                return false;
                            }
                        }

                        currPatch.dependenciesFrom.UnionWith(DependedFrom(dependenciesCell));
                        currPatch.dependenciesTo.UnionWith(DependOn(dependenciesCell));
                    }
                    catch (System.InvalidOperationException) { }
                }
            }

            return true;
        }
    }
}
