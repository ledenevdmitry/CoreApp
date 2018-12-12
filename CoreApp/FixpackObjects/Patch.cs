using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp.FixpackObjects
{
    public class Patch
    {
        public string name { get; private set; }
        FileInfo file;
        private static Regex ATCPatchRegex = new Regex(@"\\((\d+\-){0,1}Z\d+.*?)\\");
        private static Regex BankPatchRegex = new Regex(@"\\(C(\d+).*?)\\");

        public Patch(FileInfo file)
        {
            Match match = ATCPatchRegex.Match(file.FullName);
            if(!match.Success)
            {
                match = BankPatchRegex.Match(file.FullName);
            }
            if(match.Success)
            {
                name = match.Groups[1].Value;
            }
            this.file = file;
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(obj.GetType() != GetType())
            {
                return false;
            }
            return ((Patch)obj).name.Equals(name, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
