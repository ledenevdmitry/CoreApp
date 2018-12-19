using CoreApp.FixpackObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp
{
    class FileScUtils
    {
        private static Regex OraRegex = new Regex(@"ORA\|\|(.*)\|\|", RegexOptions.IgnoreCase);
        private static Regex InfaRegex = new Regex(@"IPC\|\|(.*)\Z", RegexOptions.IgnoreCase);

        public static List<FileInfo> GetFilesFromMainDir(DirectoryInfo dir, out List<Fixpack> fixpacks)
        {
            return GetListOfFiles(GetFileScsFromDir(dir, out fixpacks));
        }

        public static List<FileInfo> GetFileScsFromDir(DirectoryInfo dir, out List<Fixpack> fixpacks)
        {
            List<FileInfo> res = new List<FileInfo>();
            fixpacks = new List<Fixpack>();
            foreach(DirectoryInfo fixpackDir in dir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    FileInfo fileSc = fixpackDir.GetFiles("file_sc.txt")[0];
                    res.Add(fileSc);
                }
                catch
                {
                    throw new ArgumentException($"В папке {fixpackDir.FullName} отсутствует файл сценария");
                }
                Fixpack fp = null;
                foreach (FileInfo excelFile in fixpackDir.GetFiles("*.xlsx"))
                {
                    try
                    {
                        fp = new Fixpack(excelFile);
                        break;
                    }
                    catch
                    { }
                }
                if (fp == null) throw new Exception($"Не найдена экселька в папке {fixpackDir.FullName}");
                fixpacks.Add(fp);
            }
            return res;
        }


        private static string umRegex = @"\\um@";
        public static bool IsUMFile(FileInfo file)
        {
            return Regex.IsMatch(file.FullName, umRegex, RegexOptions.IgnoreCase);
        }

        public static List<FileInfo> GetListOfFiles(List<FileInfo> fileScs)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (FileInfo fileSc in fileScs)
            {
                StreamReader sr = new StreamReader(fileSc.FullName, Encoding.GetEncoding("Windows-1251"));
                sr.ReadLine();
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    Match match = OraRegex.Match(line);
                    if(!match.Success)
                    {
                        match = InfaRegex.Match(line);
                    }
                    if(!match.Success)
                    {
                        continue;
                    }
                    string localPath = match.Groups[1].Value;
                    string fullPath = String.Join("\\", fileSc.DirectoryName, localPath);
                    files.Add(new FileInfo(fullPath));
                }
            }
            return files;
        }
    }
}
