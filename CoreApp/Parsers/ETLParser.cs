using CoreApp.Dicts;
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
        public InfaParser InfaParser { get; protected set; }
        public InfaObjectDict InfaObjectDict { get; protected set; }

        public SqlParser SqlParser { get; protected set; }
        public OraObjectDict OraObjectDict { get; protected set; }

        private readonly Release release;
        
        public ETLParser(Release release)
        {
            this.release = release;

            InfaObjectDict = new InfaObjectDict();
            InfaParser = new InfaParser(release, InfaObjectDict);

            OraObjectDict = new OraObjectDict();
            SqlParser = new SqlParser(release, OraObjectDict);
        }

        public void Check(bool UMEnabled)
        {
            InfaParser.Check();
            SqlParser.Check(UMEnabled);
        }

    }
}
