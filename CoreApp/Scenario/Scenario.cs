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

        public enum LineState { normal, notInScenario, notInFiles } 

        public Scenario(CPatch cpatch)
        {
            this.cpatch = cpatch;
        }

        public IEnumerable<Tuple<LineState, string>> CreateScenarioFromZPatches()
        {
            List<Tuple<LineState, string>> scenario = new List<Tuple<LineState, string>>();

            List<string> inScenarioFiles = new List<string>();

            foreach (ZPatch zpatch in cpatch.ZPatchOrder.Values)
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

                        string [] lines = zpatchScenarioText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        
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
                                        zpatchScenario.Add(new Tuple<LineState, string>(LineState.normal, newScenarioLine));
                                    }

                                    inScenarioFiles.Add(ETLFileFromScFullPath);
                                }
                                catch (ArgumentException) { }
                            }
                            catch (ArgumentOutOfRangeException) { }
                        }

                        foreach (var file in zpatch.Dir.EnumerateFiles("*.*", SearchOption.AllDirectories))
                        {
                            if (IsCorrectFile(file.Name))
                            {
                                if (!inScenarioFiles.Contains(file.FullName, StringComparer.InvariantCultureIgnoreCase))
                                {
                                    string scenarioLine = ScenarioLineFromFile(zpatch, file, out int newLinePriority);
                                    int i = 0;
                                    foreach (var item in zpatchScenario)
                                    {
                                        if (Priority(item.Item2, out string prefix) >= newLinePriority)
                                        {
                                            zpatchScenario.Insert(i, new Tuple<LineState, string>(LineState.notInScenario, scenarioLine));
                                            break;
                                        }
                                        ++i;
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

        private static string ScenarioLineFromFile(ZPatch zpatch, FileInfo file, out int priority)
        {
            int zpatchIndexInFileFullPath = file.FullName.IndexOf(zpatch.Dir.Name, StringComparison.InvariantCultureIgnoreCase);
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
