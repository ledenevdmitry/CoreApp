using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CoreApp.CVS
{
    public abstract class CVS
    {
        public string Location { get; set; }
        public string Login { get; set; }

        public CVS() { }

        public CVS(string location, string login)
        {
            this.Location = location;
            this.Login = login;
        }

        abstract public void Connect();

        abstract public void Move(string source, string destination, IEnumerable<string> items);
        abstract public void Move(string destination, IEnumerable<string> items);
        abstract public void Rename(string oldName, string newName);
        abstract public void Pull(string dir, DirectoryInfo destination);
        abstract public void Push(string source, string destination);
        abstract public void PrepareToPush(string destination);

        abstract public string FirstInEntireBase(string root, out string match, Regex pattern, int depth);
        abstract public IEnumerable<string> AllInEntireBase(string root, List<string> matches, Regex pattern, int depth);

        abstract public void Close();

    }
}
