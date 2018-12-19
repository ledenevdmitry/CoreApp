using CoreApp.Comparers;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp.FixpackObjects
{
    class Fixpack
    {
        private static Application excel;
        Worksheet fixpackSheet;

        string C;
        string FullName;
        string FullPath;

        static string regexC = @"\\(C\d+)";
        static string regexFullName = @"\\(C[^\\]+)\\";
        static string regexFullPath = @".*C[^\\]+\\";
        Dictionary<string, Patch> patches;

        static Fixpack()
        {
            excel = new Application();
        }

        public Fixpack(FileInfo file)
        {
            C = Regex.Match(file.FullName, regexC).Groups[0].Value;
            FullName = Regex.Match(file.FullName, regexFullName).Groups[0].Value;
            FullPath = Regex.Match(file.FullName, regexFullPath).Groups[0].Value;

            patches = new Dictionary<string, Patch>();
            fixpackSheet = OpenExcelSheet();
            foreach(DirectoryInfo patchDir in new DirectoryInfo(FullPath).EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Patch patch = new Patch(patchDir);
                patches.Add(patch.name, patch);
            }

        }

        private Worksheet OpenExcelSheet()
        {
            string path = string.Join("\\", new DirectoryInfo(FullPath).Parent.FullName, $"{C}.xlsx");
            if (File.Exists(path))
            {
                return excel.Workbooks.Open(path).Sheets[0];
            }
            path = string.Join("\\", new DirectoryInfo(FullPath).Parent.FullName, $"{C}.xls");
            if(File.Exists(path))
            {
                return excel.Workbooks.Open(path).Sheets[0];
            }
            throw new Exception("Экселька не найдена");
        }

        private static string regexPatchName = @"(C|Z)[0-9]+";

        private void ParseExcel()
        {
            int patchNameIndex = -1;
            int linkIndex = -1;
            for(int i = 1; i <= fixpackSheet.Columns.Count; ++i)
            {
                if(fixpackSheet.Cells[1, i] == "Тема")
                {
                    patchNameIndex = i;
                }
                else if(fixpackSheet.Cells[1, i] == "Issue Link Type" || 
                        fixpackSheet.Cells[1, i] == "Связанные запросы" ||
                        fixpackSheet.Cells[1, i] == "Связи")
                {
                    linkIndex = i;
                }
            }

            for(int i = 2; i <= fixpackSheet.Rows.Count; ++i)
            {
                string patchName = Regex.Match(fixpackSheet.Cells[i, patchNameIndex], regexPatchName).Value;
                
            }

        }
    }
}
