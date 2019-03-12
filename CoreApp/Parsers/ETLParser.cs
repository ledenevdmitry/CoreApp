using CoreApp.Dicts;
using CoreApp.FixpackObjects;
using CoreApp.ReleaseObjects;
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
        public InfaParser infaParser { get; protected set; }
        public InfaObjectDict infaObjectDict { get; protected set; }

        public SqlParser sqlParser { get; protected set; }
        public OraObjectDict oraObjectDict { get; protected set; }

        private Release release;

        /*
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
        */
        
        public ETLParser(Release release)
        {
            this.release = release;

            infaObjectDict = new InfaObjectDict();
            infaParser = new InfaParser(release, infaObjectDict);

            oraObjectDict = new OraObjectDict();
            sqlParser = new SqlParser(release, oraObjectDict);
        }

        public void Check(bool UMEnabled)
        {
            infaParser.Check();
            sqlParser.Check(UMEnabled);
        }

    }
}
