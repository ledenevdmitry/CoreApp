using CoreApp.ReleaseObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp.Scenario
{
    public class Scenario
    {
        readonly CPatch cpatch;

        static string regexFileFromScLine = @"\|\|([^\|]+)";
        static string regexSchema = @"\\([^\\]+)@";
        static string regexFolderFromScenario = @"\|\|([^\\]+)\\";

        public enum LineState { oldScenario, newScenarioNormal, onlyInZPatchScenario, notInScenarios, notInFiles } 

        public Scenario(CPatch cpatch)
        {
            this.cpatch = cpatch;
        }


        public IEnumerable<Tuple<LineState, string>> CreateScenarioFromZPatches()
        {
            List<Tuple<LineState, string>> scenario = new List<Tuple<LineState, string>>();

            //часть сценария по C-патчу
            List<Tuple<LineState, string>> cpatchScenario = new List<Tuple<LineState, string>>();

            FileInfo cpatchFileScInfo = new FileInfo(Path.Combine(cpatch.Dir.FullName, "file_sc.txt"));
            string cpatchScenarioText = "";

            List<ZPatch> notInCPatchScenarioZPatches = new List<ZPatch>(cpatch.ZPatchOrder.Values);

            if (cpatchFileScInfo.Exists)
            {
                using (StreamReader sr = new StreamReader(cpatchFileScInfo.FullName))
                {
                    cpatchScenarioText = sr.ReadToEnd();
                }

                string[] cpatchLines = cpatchScenarioText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                List<string> fromCPatchScenarioFiles = new List<string>();

                List<Tuple<DirectoryInfo, int>> dirsFromScenario = new List<Tuple<DirectoryInfo, int>>();
                List<string> zpatchScenarioLines = new List<string>();

                DirectoryInfo dirFromScenario = null;
                DirectoryInfo prevDirFromScenario = null;
                int addedInNewScenarioLineCount = 0;

                for (int i = 2; i < cpatchLines.Length; ++i)
                {
                    Match match = Regex.Match(cpatchLines[i], regexFolderFromScenario);

                    if (match.Success)
                    {
                        string folderNameFromScenario = match.Groups[1].Value;
                        dirFromScenario = new DirectoryInfo(Path.Combine(cpatch.Dir.FullName, folderNameFromScenario));

                        if (prevDirFromScenario == null || !prevDirFromScenario.FullName.Equals(dirFromScenario.FullName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            dirsFromScenario.Add(new Tuple<DirectoryInfo, int>(dirFromScenario, i));
                            foreach(ZPatch zpatch in cpatch.ZPatches)
                            {
                                if(zpatch.Dir != null && zpatch.Dir.FullName.Equals(dirFromScenario.FullName, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    notInCPatchScenarioZPatches.Remove(zpatch);
                                    break;
                                }
                            }
                            zpatchScenarioLines = new List<string>();
                            prevDirFromScenario = dirFromScenario;
                        }
                    }

                    Match ETLFileFromScMatch = Regex.Match(cpatchLines[i], regexFileFromScLine);

                    if(ETLFileFromScMatch.Success)
                    {
                        string ETLFileFromSc = ETLFileFromScMatch.Groups[1].Value;
                        string ETLFileFromScFullPath = Path.Combine(cpatch.Dir.FullName, ETLFileFromSc);
                        FileInfo fromScenarioFile = new FileInfo(ETLFileFromScFullPath);
                        fromCPatchScenarioFiles.Add(ETLFileFromScFullPath);

                        if (!fromScenarioFile.Exists)
                        {
                            if (IsCorrectFile(ETLFileFromScFullPath))
                            {
                                cpatchScenario.Add(new Tuple<LineState, string>(LineState.notInFiles, cpatchLines[i]));
                            }
                        }
                        else
                        {
                            cpatchScenario.Add(new Tuple<LineState, string>(LineState.oldScenario, cpatchLines[i]));
                        }
                    }

                }                

                foreach (var lineInfo in dirsFromScenario)
                {
                    foreach(var subFile in lineInfo.Item1.GetFiles("*.*", SearchOption.AllDirectories))
                    {
                        if (IsCorrectFile(subFile.Name))
                        {
                            //внутренний инт - где делится префикс и папка, внешний - номер строки в z-патче
                            SortedList<int, Tuple<int, string>> fromZPatchScenarioFiles = new SortedList<int, Tuple<int, string>>(Comparer<int>.Create((x, y) => x < y ? -1 : 1));
                            bool containsInCPatchScenarioFiles = fromCPatchScenarioFiles.Contains(subFile.FullName, StringComparer.InvariantCultureIgnoreCase);
                            if (!containsInCPatchScenarioFiles)
                            {
                                if (lineInfo.Item1.Name != null)
                                {
                                    string zpatchScenarioPath = Path.Combine(lineInfo.Item1.FullName, "file_sc.txt");
                                    if (File.Exists(zpatchScenarioPath))
                                    {
                                        string zpatchScenarioText;
                                        using (StreamReader sr = new StreamReader(zpatchScenarioPath))
                                        {
                                            zpatchScenarioText = sr.ReadToEnd();
                                        }
                                        string[] zpatchLines = zpatchScenarioText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                        for (int j = 2; j < zpatchLines.Length; ++j)
                                        {
                                            Match ETLFileFromZPatchScMatch = Regex.Match(zpatchLines[j], regexFileFromScLine);
                                            if (ETLFileFromZPatchScMatch.Success)
                                            {
                                                string ETLFileFromSc = ETLFileFromZPatchScMatch.Groups[1].Value;
                                                string ETLFileFromScFullPath = Path.Combine(lineInfo.Item1.FullName, ETLFileFromSc);
                                                fromZPatchScenarioFiles.Add(j, new Tuple<int, string>(ETLFileFromZPatchScMatch.Groups[1].Index, ETLFileFromScFullPath));
                                            }
                                        }
                                        foreach (var kvp in fromZPatchScenarioFiles)
                                        {
                                            if (kvp.Value.Item2.Equals(subFile.FullName, StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                string scenarioLineByZPatch =
                                                    zpatchLines[kvp.Key].Substring(0, kvp.Value.Item1) +  //часть с префиксом
                                                    dirFromScenario.Name + @"\" + //название папки
                                                    zpatchLines[kvp.Key].Substring(kvp.Value.Item1); //остальная часть
                                                cpatchScenario.Insert(kvp.Key + lineInfo.Item2 + addedInNewScenarioLineCount, new Tuple<LineState, string>(LineState.onlyInZPatchScenario, scenarioLineByZPatch));
                                                addedInNewScenarioLineCount++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach(var lineInfo in dirsFromScenario)
                {
                    foreach (var subFile in lineInfo.Item1.GetFiles("*.*", SearchOption.AllDirectories))
                    {
                        if (IsCorrectFile(subFile.Name))
                        {
                            if (fromCPatchScenarioFiles.Contains(subFile.FullName, StringComparer.InvariantCultureIgnoreCase))
                            {
                                string scenarioLine = ScenarioLineFromFile(lineInfo.Item1, subFile, out int newLinePriority);

                                bool inserted = false;
                                //старт там, где начинался патч
                                for (int i = lineInfo.Item2; i < cpatchScenario.Count; ++i)
                                {
                                    if (Priority(cpatchScenario[i].Item2, out string prefix) >= newLinePriority)
                                    {
                                        cpatchScenario.Insert(i + addedInNewScenarioLineCount, new Tuple<LineState, string>(LineState.notInScenarios, scenarioLine));
                                        addedInNewScenarioLineCount++;
                                        inserted = true;
                                        break;
                                    }
                                }
                                //если нужно вставлять в конец или еще не добавляли
                                if (!inserted || cpatchScenario.Count == 0)
                                {
                                    cpatchScenario.Add(new Tuple<LineState, string>(LineState.notInScenarios, scenarioLine));
                                    addedInNewScenarioLineCount++;
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                var cpatchTuple = new Tuple<LineState, string>(LineState.newScenarioNormal, cpatch.CPatchName);
                scenario.Add(cpatchTuple);
                scenario.Add(cpatchTuple);
            }

            scenario.AddRange(cpatchScenario);

            List<string> inScenarioFiles = new List<string>();

            foreach (ZPatch zpatch in notInCPatchScenarioZPatches)
            {
                List<Tuple<LineState, string>> zpatchScenario = new List<Tuple<LineState, string>>();
                if (zpatch.ZPatchStatus != "OPEN")
                {
                    FileInfo fileScInfo = new FileInfo(Path.Combine(zpatch.Dir.FullName, "file_sc.txt"));

                    if (fileScInfo.Exists)
                    {
                        string zpatchScenarioText;
                        using (StreamReader sr = new StreamReader(fileScInfo.FullName))
                        {
                            zpatchScenarioText = sr.ReadToEnd();
                        }

                        string[] lines = zpatchScenarioText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        //пропускаю первые 2 строчки
                        for (int i = 2; i < lines.Length; ++i)
                        {
                            try
                            {
                                Match ETLFileFromScMatch = Regex.Match(lines[i], regexFileFromScLine);
                                string ETLFileFromSc = ETLFileFromScMatch.Groups[1].Value;
                                string newScenarioLine =
                                    lines[i].Substring(0, ETLFileFromScMatch.Groups[1].Index) +  //часть с префиксом
                                    zpatch.Dir.Name + @"\" + //название папки
                                    lines[i].Substring(ETLFileFromScMatch.Groups[1].Index); //остальная часть
                                try
                                {
                                    string ETLFileFromScFullPath = Path.Combine(zpatch.Dir.FullName, ETLFileFromSc);

                                    //если файл из сценария не нашелся
                                    if (!(new FileInfo(ETLFileFromScFullPath).Exists))
                                    {
                                        if (IsCorrectFile(ETLFileFromScFullPath))
                                        {
                                            zpatchScenario.Add(new Tuple<LineState, string>(LineState.notInFiles, newScenarioLine));
                                        }
                                    }
                                    else
                                    {
                                        zpatchScenario.Add(new Tuple<LineState, string>(LineState.newScenarioNormal, newScenarioLine));
                                    }

                                    inScenarioFiles.Add(ETLFileFromScFullPath);
                                }
                                catch (ArgumentException) { }
                            }
                            catch (ArgumentOutOfRangeException) { }
                        }
                    }

                    foreach (var file in zpatch.Dir.EnumerateFiles("*.*", SearchOption.AllDirectories))
                    {
                        if (IsCorrectFile(file.Name))
                        {
                            if (!inScenarioFiles.Contains(file.FullName, StringComparer.InvariantCultureIgnoreCase))
                            {
                                string scenarioLine = ScenarioLineFromFile(zpatch.Dir, file, out int newLinePriority);
                                int i = 0;
                                bool inserted = false;
                                foreach (var item in zpatchScenario)
                                {
                                    if (Priority(item.Item2, out string prefix) >= newLinePriority)
                                    {
                                        zpatchScenario.Insert(i, new Tuple<LineState, string>(LineState.notInScenarios, scenarioLine));
                                        inserted = true;
                                        break;
                                    }
                                    ++i;
                                }
                                //если нужно вставлять в конец или еще не добавляли
                                if (!inserted || zpatchScenario.Count == 0)
                                {
                                    zpatchScenario.Add(new Tuple<LineState, string>(LineState.notInScenarios, scenarioLine));
                                }
                            }
                        }
                    }
                    scenario.AddRange(zpatchScenario);
                }
            }

            return scenario;
        }

        private static string ScenarioLineFromFile(DirectoryInfo dir, FileInfo file, out int priority)
        {
            int zpatchIndexInFileFullPath = file.FullName.IndexOf(dir.Name, StringComparison.InvariantCultureIgnoreCase);
            string FileNameFromZPatch = file.FullName.Substring(zpatchIndexInFileFullPath);

            priority = Priority(FileNameFromZPatch, out string prefix);
            string line = prefix + "||" + FileNameFromZPatch;

            if(prefix == "ORA")
            {
                string schema = Regex.Match(FileNameFromZPatch, regexSchema).Groups[1].Value;
                line += "||" + schema;
            }

            return line;
        }

        private static bool IsCorrectFile(string name)
        {
            if (Regex.IsMatch(name, "RELEASE_NOTES.DOCX?|FILE_SC.*.TXT|VSSVER2.SCC|.*.XLS", RegexOptions.IgnoreCase))
                return false;
            return true;
        }

        private static int Priority(string line, out string prefix)
        {
            string upperLine = line.ToUpper();
            int priority = 0;
            prefix = "";

            //скрипты
            if (upperLine.IndexOf("\\DB_SCRIPT") != -1)
            {
                prefix = "ORA";
                priority += 1000;

                //порядок в скриптах
                if (upperLine.IndexOf("\\SEQUENCE") != -1) priority += 5;
                else if (upperLine.IndexOf("\\TABLE") != -1) priority += 10;
                else if (upperLine.IndexOf("\\INDEX") != -1) priority += 15;
                else if (upperLine.IndexOf("\\VIEW") != -1) priority += 20;
                else if (upperLine.IndexOf("\\FUNCTION") != -1) priority += 25;
                else if (upperLine.IndexOf("\\PROCEDURE") != -1) priority += 30;
                else if (upperLine.IndexOf("\\PACKAGE") != -1) priority += 35;
                else if (upperLine.IndexOf("\\SCRIPT") != -1) priority += 40;
            }

            //информатика
            else if (upperLine.IndexOf("\\INFA_XML") != -1)
            {
                prefix = "IPC";
                priority += 2000;

                //сначала shared
                if (upperLine.IndexOf("\\SHARED") != -1) priority += 100;
                else priority += 200;

                //порядок в информатике 
                if (upperLine.IndexOf("\\SOURCE") != -1) priority += 5;
                else if (upperLine.IndexOf("\\TARGET") != -1) priority += 10;
                else if (upperLine.IndexOf("\\USER-DEFINED FUNCTION") != -1)
                    priority += 15;
                else if (upperLine.IndexOf("\\EXP_") != -1) priority += 20;
                else if (upperLine.IndexOf("\\SEQ_") != -1) priority += 25;
                else if (upperLine.IndexOf("\\LKP_") != -1) priority += 30;
                else if (upperLine.IndexOf("\\MPL_") != -1) priority += 35;
                else if (upperLine.IndexOf("\\M_") != -1) priority += 40;
                else if (upperLine.IndexOf("\\CMD_") != -1) priority += 45;
                else if (upperLine.IndexOf("\\S_") != -1) priority += 50;
                else if (upperLine.IndexOf("\\WKLT_") != -1) priority += 55;
                else if (upperLine.IndexOf("\\WF_") != -1) priority += 60;
            }

            //старты потоков
            else if (upperLine.IndexOf("\\START_WF") != -1)
            {
                prefix = "STWF";
                priority += 3000;
            }

            return priority;
        }
    }
}
