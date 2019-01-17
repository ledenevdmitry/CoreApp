using CoreApp.Comparers;
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
        private static Application excel;
        Range fixpackExcelColumns;

        string C;
        string FullName;
        string FullPath;

        static string regexC = @"\\(C\d+)";
        static string regexFullName = @"\\(C[^\\]+)";
        static string regexFullPath = @".*C[^\\]+";
        public Dictionary<string, Patch> patches { get; protected set; }

        static Fixpack()
        {
            excel = new Application();
        }

        public Fixpack(DirectoryInfo dir)
        {
            C = Regex.Match(dir.FullName, regexC).Groups[1].Value;
            FullName = Regex.Match(dir.FullName, regexFullName).Groups[1].Value;
            FullPath = Regex.Match(dir.FullName, regexFullPath).Groups[0].Value;

            patches = new Dictionary<string, Patch>();
            //fixpackExcelColumns = OpenExcelColumns();
            foreach(DirectoryInfo patchDir in new DirectoryInfo(FullPath).EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Patch patch = new Patch(patchDir);
                if (!patches.ContainsKey(patch.name))
                {
                    patches.Add(patch.name, patch);
                }
            }

        }

        private Range OpenExcelColumns(string path)
        {
            Workbook wb = excel.Workbooks.Open(path);
            Worksheet ws = wb.Sheets[1];
            Range res = ws.Cells;
            //ExcelCleanup(wb, ws);

            //var testCell = res[1, 1].Value2;

            return res;
        }

        private Range OpenExcelColumns()
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

        private void ParseExcel()
        {
            int patchNameIndex = -1;
            int linkIndex = -1;
            for(int i = 1; i <= fixpackExcelColumns.Columns.Count; ++i)
            {
                if(fixpackExcelColumns.Cells[1, i] == "Тема")
                {
                    patchNameIndex = i;
                }
                else if(fixpackExcelColumns.Cells[1, i] == "Issue Link Type" || 
                        fixpackExcelColumns.Cells[1, i] == "Связанные запросы" ||
                        fixpackExcelColumns.Cells[1, i] == "Связи")
                {
                    linkIndex = i;
                }
            }

            for(int i = 2; i <= fixpackExcelColumns.Rows.Count; ++i)
            {
                string patchName = Regex.Match(fixpackExcelColumns.Cells[i, patchNameIndex], regexPatchName).Value;
                
            }

        }
    }
}
