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
