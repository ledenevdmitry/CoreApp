using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Comparers
{
    internal class FileInfoEqualityComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.Equals(f2.FullName, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(FileInfo f)
        {
            return f.FullName.GetHashCode();
        }
    }
    
}
