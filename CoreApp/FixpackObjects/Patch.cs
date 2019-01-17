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
        private static Regex ATCPatchRegex = new Regex(@"\\((\d+\-)?Z(\d+.*?))");
        private static Regex BankPatchRegex = new Regex(@"\\(C(\d+).*?)");
        public bool IsATCPatch { get; private set; }
        public List<Patch> dependendFrom { get; private set; }
        public List<Patch> dependOn { get; private set; }
        public List<FileInfo> objs;

        public Patch(string patchName)
        {
            name = patchName;
        }
      

        public override int GetHashCode()
        {
            return dir.FullName.GetHashCode();
        }

        public Patch(DirectoryInfo dir)
        {
            dependendFrom = new List<Patch>();
            dependOn = new List<Patch>();
            objs = new List<FileInfo>();
            IsATCPatch = true;

            Match match = ATCPatchRegex.Match(dir.FullName);
            if(!match.Success)
            {
                match = BankPatchRegex.Match(dir.FullName);
                IsATCPatch = false;
            }

            //TODO: Проверять зависимости патча в эксельке

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
