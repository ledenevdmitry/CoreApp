using CoreApp.OraUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp.FixpackObjects
{
    public class ZPatch
    {
        public string ZPatchName { get; private set; }
        public DirectoryInfo dir { get; private set; }
        public string pathToPatch { get; private set; }
        private static Regex ATCPatchRegex = new Regex(@"\\((\d+\-)?Z(\d+.*?))");
        private static Regex BankPatchRegex = new Regex(@"\\(C(\d+).*?)");
        public bool IsATCPatch { get; private set; }
        public HashSet<ZPatch> dependenciesFrom { get; private set; }
        public HashSet<ZPatch> dependenciesTo { get; private set; }
        public List<FileInfo> objs;
        public int rank;
        public int ZPatchId { get; set; }
        public CPatch cpatch;
        public int excelFileRowId;

        public override int GetHashCode()
        {
            return ZPatchName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(ZPatch)) return false;
            return ((ZPatch)obj).ZPatchName == ZPatchName;
        }

        public ZPatch(string ZPatchName, int CPatch, HashSet<ZPatch> dependenciesFrom, HashSet<ZPatch> dependenciesTo)
        {
            this.ZPatchName = ZPatchName;
            this.dependenciesFrom = dependenciesFrom;
            this.dependenciesTo = dependenciesTo;
        }


        public ZPatch(CPatch cpatch, int oraId)
        {
            this.cpatch = cpatch;
            dependenciesFrom = new HashSet<ZPatch>();

            var oraZPatchesDependenciesFrom = ZPatchDAL.getDependenciesFrom(oraId);

            foreach (var oraZPatchRecord in oraZPatchesDependenciesFrom)
            {
                dependenciesFrom.Add(cpatch.getZPatchById(oraZPatchRecord.ZPatchId));
            }

            dependenciesTo = new HashSet<ZPatch>();

            var oraZPatchesDependenciesTo = ZPatchDAL.getDependenciesTo(oraId);

            foreach (var oraZPatchRecord in oraZPatchesDependenciesTo)
            {
                dependenciesTo.Add(cpatch.getZPatchById(oraZPatchRecord.ZPatchId));
            }
        }

        /*
        public ZPatch(string patchName)
        {
            name = patchName;
        }
      
        */

        public ZPatch(DirectoryInfo dir)
        {
            dependenciesFrom = new HashSet<ZPatch>();
            dependenciesTo = new HashSet<ZPatch>();
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
                ZPatchName = match.Groups[1].Value;
                pathToPatch = dir.FullName.Substring(0, match.Index + ZPatchName.Length + 1);
            }
            this.dir = dir;
        }
    }
}
