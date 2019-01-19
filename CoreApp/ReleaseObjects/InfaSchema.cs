using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp.FixpackObjects
{
    public class InfaSchema
    {
        public string schema { get; private set; }

        private static string schemaRegex = @"\\([^\\]*)@";

        public InfaSchema(FileInfo file)
        {
            schema = Regex.Match(file.FullName, schemaRegex).Groups[1].Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(InfaSchema))
            {
                return false;
            }
            return schema.Equals(((InfaSchema)obj).schema, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
