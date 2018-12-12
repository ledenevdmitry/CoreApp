using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    class OraObject
    {
        public string objName { get; private set; }
        public string type { get; private set; }
        public FileInfo file { get; private set; }

        public OraObject(string objName, string type, FileInfo file)
        {
            this.objName = objName;
            this.type = type;
            this.file = file;
        }
    }
}
