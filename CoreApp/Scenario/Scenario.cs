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

        private Dictionary<ZPatch, List<Tuple<LineState, string>>> SplitCPatchScenarioByZPatches(CPatch cpatch, string[] scenarioLines)
        {
            string currfolder = null;
            string prevfolder = null;

            Dictionary<ZPatch, List<Tuple<LineState, string>>> pairs = new Dictionary<ZPatch, List<Tuple<LineState, string>>>();

            //определение, какие патчи вообще есть в сценарии
            foreach(string line in scenarioLines)
            {
                Match folderMatch = Regex.Match(line, regexFolderFromScenario);
                if(folderMatch.Success)
                {
                    currfolder = folderMatch.Groups[1].Value;
                    if(!currfolder.Equals(prevfolder, StringComparison.InvariantCultureIgnoreCase))
                    {
                        string fullCurrFolder = Path.Combine(cpatch.Dir.FullName, currfolder);

                        if (TryGetZPatchByFullFolderName(cpatch, fullCurrFolder, out ZPatch zpatch))
                        {
                            pairs.Add(zpatch, new List<Tuple<LineState, string>>());
                        }

                        prevfolder = currfolder;
                    }
                }
            }

            for(int i = 0; i < scenarioLines.Length; ++i)
            {
                Match folderMatch;
                int j = i;
                if (!(folderMatch = Regex.Match(scenarioLines[i], regexFolderFromScenario)).Success)
                {
                    while (!(folderMatch = Regex.Match(scenarioLines[j], regexFolderFromScenario)).Success)
                    {
                        j++;
                    }

                    if (j < scenarioLines.Length)
                    {
                        AddFolderNotFoundScenarioLinesToSplittedScenario(scenarioLines, i, j, pairs);
                    }
                    else
                    {
                        AddFolderNotFoundScenarioLinesToSplittedScenario(scenarioLines, scenarioLines.Length - 1, scenarioLines.Length, pairs);
                    }

                    i = j;
                }
                else
                {
                    string folderName = folderMatch.Groups[1].Value;
                    string fullFolderName = Path.Combine(cpatch.Dir.FullName, folderName);
                    if(TryGetZPatchByFullFolderName(cpatch, fullFolderName, out ZPatch zpatch))
                    {
                        pairs[zpatch].Add(new Tuple<LineState, string>(LineState.oldScenario, scenarioLines[i]));
                    }

                }
            }

            return pairs;
        }

        private void AddFolderNotFoundScenarioLinesToSplittedScenario(string[] scenarioLines, int i, int j, Dictionary<ZPatch, List<Tuple<LineState, string>>> pairs)
        {
            if (TryGetFoldernameFromScenarioLine(scenarioLines[j], out string folderName))
            {
                string folderFullname = Path.Combine(cpatch.Dir.FullName, folderName);
                if (TryGetZPatchByFullFolderName(cpatch, folderFullname, out ZPatch zpatch))
                {
                    for (int k = i; k < j; ++k)
                    {
                        pairs[zpatch].Add(new Tuple<LineState, string>(LineState.oldScenario, scenarioLines[k]));
                    }
                }
            }
        }

        private bool TryGetZPatchByFullFolderName(CPatch cpatch, string fullFolderName, out ZPatch zpatch)
        {
            foreach(ZPatch currZPatch in cpatch.ZPatches)
            {
                if (currZPatch.ZPatchStatus != "OPEN")
                {
                    if (currZPatch.Dir.FullName.Equals(fullFolderName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        zpatch = currZPatch;
                        return true;
                    }
                }
            }
            zpatch = null;
            return false;
        }

        private bool TryGetFoldernameFromScenarioLine(string scenarioLine, out string folderName)
        {
            Match match = Regex.Match(scenarioLine, regexFolderFromScenario);
            if (match.Success)
            {
                folderName = match.Groups[1].Value;
                return true;
            }
            folderName = null;
            return false;
        }


        private bool TryGetFileNameFromScenarioLine(string pathToFile, string scenarioLine, out string fileName, out int index)
        {
            Match match = Regex.Match(scenarioLine, regexFileFromScLine);
            if(match.Success)
            {
                fileName = Path.Combine(pathToFile, match.Groups[1].Value);
                index = match.Groups[1].Index;
                return true;
            }
            fileName = null;
            index = -1;
            return false;
        }

        private string ZPatchScenarioLineToCPatchScenario(string zpatchScenarioLine, ZPatch zpatch, int index)
        {
            return
               zpatchScenarioLine.Substring(0, index) +  //часть с префиксом
               zpatch.Dir.Name + @"\" + //название папки
               zpatchScenarioLine.Substring(index); //остальная часть

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

                string[] cpatchScenarioLines = cpatchScenarioText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                List<string> fromCPatchScenarioFiles = new List<string>();

                var cpatchSplittedByZPatchScenario = SplitCPatchScenarioByZPatches(cpatch, cpatchScenarioLines);

                //по сценарию c-патча определяю, все ли файлы на месте
                foreach (ZPatch zpatch in cpatch.ZPatchOrder.Values)
                {
                    if (cpatchSplittedByZPatchScenario.ContainsKey(zpatch))
                    {
                        foreach (var scenarioInfo in cpatchSplittedByZPatchScenario[zpatch])
                        {
                            if (TryGetFileNameFromScenarioLine(cpatch.Dir.FullName, scenarioInfo.Item2, out string fileName, out int index))
                            {
                                if (File.Exists(fileName))
                                {
                                    cpatchScenario.Add(new Tuple<LineState, string>(LineState.oldScenario, scenarioInfo.Item2));
                                }
                                else
                                {
                                    cpatchScenario.Add(new Tuple<LineState, string>(LineState.notInFiles, scenarioInfo.Item2));
                                }
                            }
                            else
                            {
                                cpatchScenario.Add(new Tuple<LineState, string>(LineState.oldScenario, scenarioInfo.Item2));
                            }
                        }
                    }
                }

                SortedList<int, string> inZPatchScenarioLinesFound = new SortedList<int, string>();

                foreach (ZPatch zpatch in cpatch.ZPatchOrder.Values)
                {
                    if (cpatchSplittedByZPatchScenario.ContainsKey(zpatch))
                    {
                        foreach (FileInfo file in zpatch.Dir.EnumerateFiles("*.*", SearchOption.AllDirectories))
                        {
                            if (IsCorrectFile(file.Name))
                            {
                                bool foundInCPatchScenario = false;
                                foreach (var scenarioInfo in cpatchSplittedByZPatchScenario[zpatch])
                                {
                                    if (TryGetFileNameFromScenarioLine(cpatch.Dir.FullName, scenarioInfo.Item2, out string fileName, out int index))
                                    {
                                        if (fileName.Equals(file.FullName, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            foundInCPatchScenario = true;
                                            break;
                                        }
                                    }
                                }
                                if (!foundInCPatchScenario)
                                {
                                    FileInfo zpatchScenarioFile = new FileInfo(Path.Combine(zpatch.Dir.FullName, "file_sc.txt"));
                                    string[] zpatchScenarioLines;
                                    string zpatchScenarioText;

                                    if (zpatchScenarioFile.Exists)
                                    {
                                        using (StreamReader sr = new StreamReader(zpatchScenarioFile.FullName))
                                        {
                                            zpatchScenarioText = sr.ReadToEnd();
                                        }
                                        zpatchScenarioLines = zpatchScenarioText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                                        bool foundInZPatchScenario = false;
                                        for (int i = 0; i < zpatchScenarioLines.Length && !foundInZPatchScenario; ++i)
                                        {
                                            if (TryGetFileNameFromScenarioLine(zpatch.Dir.FullName, zpatchScenarioLines[i], out string filenameFromZPatchScenario, out int index))
                                            {
                                                if (file.FullName.Equals(filenameFromZPatchScenario, StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    int j = 0;
                                                    foundInZPatchScenario = true;
                                                    bool foundPrevInZPatch = false;

                                                    for (j = 0; j < cpatchSplittedByZPatchScenario[zpatch].Count && !foundPrevInZPatch; ++j)
                                                    {
                                                        //ищем предыдущую соответствующую строку
                                                        if (TryGetFileNameFromScenarioLine(cpatch.Dir.FullName, cpatchSplittedByZPatchScenario[zpatch][j].Item2, out string cpatchFilename, out int cprevIndex) &&
                                                            TryGetFileNameFromScenarioLine(zpatch.Dir.FullName, zpatchScenarioLines[i - 1], out string zpatchFilename, out int zprevIndex))
                                                        {
                                                            if (cpatchFilename.Equals(zpatchFilename, StringComparison.InvariantCultureIgnoreCase))
                                                            {
                                                                foundPrevInZPatch = true;
                                                            }
                                                        }
                                                    }

                                                    if (TryGetFileNameFromScenarioLine(zpatch.Dir.FullName, zpatchScenarioLines[i], out string filename, out int ZIndex))
                                                    {
                                                        //цикл сделает j++, откатываем назад
                                                        if (foundPrevInZPatch)
                                                            cpatchSplittedByZPatchScenario[zpatch].Insert(j, new Tuple<LineState, string>(LineState.onlyInZPatchScenario, ZPatchScenarioLineToCPatchScenario(zpatchScenarioLines[i], zpatch, ZIndex)));
                                                        else
                                                            cpatchSplittedByZPatchScenario[zpatch].Insert(0, new Tuple<LineState, string>(LineState.onlyInZPatchScenario, ZPatchScenarioLineToCPatchScenario(zpatchScenarioLines[i], zpatch, ZIndex)));
                                                    }
                                                    else
                                                    {
                                                        if (foundPrevInZPatch)
                                                            cpatchSplittedByZPatchScenario[zpatch].Insert(j, new Tuple<LineState, string>(LineState.onlyInZPatchScenario, zpatchScenarioLines[i]));
                                                        else
                                                            cpatchSplittedByZPatchScenario[zpatch].Insert(0, new Tuple<LineState, string>(LineState.onlyInZPatchScenario, zpatchScenarioLines[i]));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach(ZPatch zpatchFromOrder in cpatch.ZPatchOrder.Values)
                {
                    foreach(var zpatchScenarioInfo in cpatchSplittedByZPatchScenario)
                    {
                        if(zpatchScenarioInfo.Key.Equals(zpatchFromOrder))
                        {
                            scenario.AddRange(zpatchScenarioInfo.Value);
                        }
                    }
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
