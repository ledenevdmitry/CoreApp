using CoreApp.Dicts;
using CoreApp.FixpackObjects;
using CoreApp.Keys;
using CoreApp.ReleaseObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp
{
    class SqlParser
    {
        public static string extension { get => ".sql"; }
        //public static readonly string[] DDLStatements = Properties.Settings.Default.DDLSTM.Split('|');
        public static readonly string RegexComment;
        public static readonly string RegexWhiteSpaces;
        public static readonly string RegexWhiteSpacesStart;
        public static readonly string RegexWhiteSpacesEnd;
        public static readonly string RegexIdentifier;
        public static readonly string RegexIndexingTable;
        public static readonly string[] OraObjectTypes;
        public static readonly string[] DMLStatements;
        public static readonly HashSet<string> ReservedWords;
        private static Regex IdentifierRegex;

        private Release release;
        private OraObjectDict dict;

        public SqlParser(Release release, OraObjectDict dict)
        {
            this.release = release;
            this.dict = dict;
        }

        static SqlParser()
        {
            RegexComment = @"((--.*?\Z)|(/\*[\s\S]*?\*/))";
            RegexWhiteSpaces = @"(\s|(\r\n))+";
            RegexWhiteSpacesStart = @"(\A|(\s+))";
            RegexWhiteSpacesEnd = @"(\Z|(\s+))";
            RegexIdentifier = @"(([^\s',;\(\)])+)";
            IdentifierRegex = new Regex(RegexIdentifier);
            RegexIndexingTable = "INDEX" +
                RegexWhiteSpaces +
                RegexIdentifier +
                RegexWhiteSpaces +
                "ON";
           OraObjectTypes = Regex.Replace(Properties.Settings.Default.ORAOBJ, RegexWhiteSpaces, RegexWhiteSpaces).Split('~');
           DMLStatements = Regex.Replace(Properties.Settings.Default.DMLSTM, RegexWhiteSpaces, RegexWhiteSpaces).Split('~');
           ReservedWords = new HashSet<string>(Properties.Settings.Default.RSVDWRDS.Split('~'));
        }


        private static void RemoveComments(ref string script)
        {
            Regex regexComment = new Regex(RegexComment);
            //MatchCollection commMatches = regexComment.Matches(script);

            script = regexComment.Replace(script, "");
        }

        //список должен быть создан до запуска
        public static void RetrieveObjectsFromSQL(string script, FileInfo file, OraObjectDict dict, ZPatch patch)
        {
            RemoveComments(ref script);
            //проверка всех DDL 
            foreach (string objType in OraObjectTypes)
            {
                Regex regex = new Regex(RegexWhiteSpacesStart + objType + RegexWhiteSpacesEnd, RegexOptions.IgnoreCase);
                InsertionsByRegex(script, objType, file, dict, regex, patch);
            }

            foreach(string sttm in DMLStatements)
            {
                Regex regex = new Regex(RegexWhiteSpacesStart + sttm + RegexWhiteSpacesEnd, RegexOptions.IgnoreCase);
                InsertionsByRegex(script, sttm, file, dict, regex, patch);
            }

            InsertionsByRegex(script, "INDEX", file, dict, new Regex(RegexWhiteSpacesStart + RegexIndexingTable + RegexWhiteSpacesEnd), patch);
        }

        private static void InsertionsByRegex(string script, string type, FileInfo file, OraObjectDict dict, Regex regex, ZPatch patch)
        {
            MatchCollection matchCollection = regex.Matches(script);
            foreach (Match match in matchCollection)
            {
                int start = match.Index + match.Length;
                Match obj = IdentifierRegex.Match(script, start);
                if (!ReservedWords.Contains(obj.Value.ToUpper()))
                {
                    InsertIntoDict(obj, type, file, dict, patch);
                }
            }
        }

        private static void InsertIntoDict(Match obj, string type, FileInfo file, OraObjectDict dict, ZPatch patch)
        {
            string objName = obj.Value.Trim().ToUpper();
            dict.AddObjectConsiderIntersections(new OraObject(objName, type, patch));
        }

        public void RetrieveObjectsFromFile(FileInfo file, OraObjectDict dict, ZPatch patch)
        {
            StreamReader streamReader = new StreamReader(file.FullName, Encoding.GetEncoding("Windows-1251"));
            string script = streamReader.ReadToEnd();
            RetrieveObjectsFromSQL(script, file, dict as OraObjectDict, patch);
        }

        //public delegate void ResetProgress();
        //public event ResetProgress StartOfCheck, ProgressChanged, EndOfCheck;     
        /*
        public int WorkAmoumt(List<FileInfo> files)
        {
            return files.Count(x => x.Extension.Equals(extension, StringComparison.CurrentCultureIgnoreCase));
        }
        */


        //TODO: проверка фигня, сейчас проверяет пересечения только внутри патча Оо
        public void Check(bool UMEnabled)
        {
            //StartOfCheck();
            foreach(var fp in release.fixpacks.Values)
            {
                foreach(var patch in fp.patches.Values)
                {
                    foreach (FileInfo file in patch.dir.EnumerateFiles("*.*", SearchOption.AllDirectories))
                    {
                        if (file.Extension.Equals(extension, StringComparison.CurrentCultureIgnoreCase) &&
                            (UMEnabled || !FileScUtils.IsUMFile(file)))
                        {
                            if (file.Exists)
                            {
                                RetrieveObjectsFromFile(file, dict, patch);
                                //ProgressChanged();
                            }
                            else
                            {
                                dict.notFoundFiles.Add(file);
                            }
                        }
                    }
                }
            }
            //EndOfCheck();
        }
    }
}
