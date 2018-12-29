using CoreApp.Dicts;
using CoreApp.FixpackObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Parsers
{
    class ETLParser
    {
        List<Fixpack> fixpacks;

        InfaParser infaParser;
        InfaObjectDict infaObjectDict;

        SqlParser sqlParser;
        OraObjectDict oraObjectDict;

        int workAmount;

        public ETLParser(DirectoryInfo dir)
        {
            fixpacks = new List<Fixpack>();
            foreach (DirectoryInfo fixpackDir in dir.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                Fixpack currFixpack = new Fixpack(fixpackDir);
                fixpacks.Add(currFixpack);
            }

            infaObjectDict = new InfaObjectDict();
            InfaParser infaParser = new InfaParser(fixpacks, infaObjectDict);

            oraObjectDict = new OraObjectDict();
            SqlParser sqlParser = new SqlParser();

            foreach(Fixpack fixpack in fixpacks)
            {
                infaParser.RetrieveObjectsFromFiles(fixpack.patches)
            }

        }

    }
}
