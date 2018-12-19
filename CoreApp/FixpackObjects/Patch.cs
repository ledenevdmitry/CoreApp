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
        public DirectoryInfo dir { get; private set; }
        public string pathToPatch { get; private set; }
        private static Regex ATCPatchRegex = new Regex(@"\\((\d+\-){0,1}Z\d+.*?)\\");
        private static Regex BankPatchRegex = new Regex(@"\\(C(\d+).*?)\\");
        public List<Patch> dependendFrom { get; private set; }
        public List<Patch> dependOn { get; private set; }

        public Patch(string patchName)
        {
            name = patchName;
        }

        public Patch(DirectoryInfo dir)
        {
            dependendFrom = new List<Patch>();
            dependOn = new List<Patch>();

            Match match = ATCPatchRegex.Match(dir.FullName);
            if(!match.Success)
            {
                match = BankPatchRegex.Match(dir.FullName);
            }
            if(match.Success)
            {
                name = match.Groups[1].Value;
                pathToPatch = dir.FullName.Substring(0, match.Index + name.Length + 1);
            }
            this.dir = dir;
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
