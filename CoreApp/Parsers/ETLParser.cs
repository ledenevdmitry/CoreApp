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

        public InfaParser infaParser { get; protected set; }
        public InfaObjectDict infaObjectDict { get; protected set; }

        public SqlParser sqlParser { get; protected set; }
        public OraObjectDict oraObjectDict { get; protected set; }

        public int fileCount()
        {
            int res = 0;
            foreach(Fixpack fp in fixpacks)
            {
                foreach(Patch p in fp.patches.Values)
                {
                    res += p.objs.Count;
                }
            }
            return res;
        }
        
        public ETLParser(DirectoryInfo dir, bool UMEnabled)
        {
            fixpacks = new List<Fixpack>();
            foreach (DirectoryInfo fixpackDir in dir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Fixpack currFixpack = new Fixpack(fixpackDir);
                fixpacks.Add(currFixpack);
            }

            infaObjectDict = new InfaObjectDict();
            infaParser = new InfaParser(fixpacks, infaObjectDict);

            oraObjectDict = new OraObjectDict();
            sqlParser = new SqlParser();
        }

        public void Check(bool UMEnabled)
        {
            foreach (Fixpack fixpack in fixpacks)
            {
                foreach (Patch patch in fixpack.patches.Values)
                {
                    infaParser.RetrieveObjectsFromFiles(patch.objs, infaObjectDict);
                    infaParser.CheckInfaDependencies(patch.objs, infaObjectDict);
                    sqlParser.RetrieveObjectsFromFile(patch.objs, oraObjectDict, UMEnabled);
                }
            }
        }

    }
}
