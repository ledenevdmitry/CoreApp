﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CoreApp.CVS
{
    public abstract class CVS
    {
        public string location { get; set; }
        public string login { get; set; }

        public CVS() { }

        public CVS(string location, string login)
        {
            this.location = location;
            this.login = login;
        }

        abstract public void Connect();

        abstract public void Move(string source, string destination, IEnumerable<string> items);
        abstract public void Move(string destination, IEnumerable<string> items);
        abstract public void Rename(string oldName, string newName);
        abstract public void Download(string dir, string destination);

        abstract public string FirstInEntireBase(ref string match, Regex pattern);
        abstract public IEnumerable<string> AllInEntireBase(List<string> matches, Regex pattern);

        abstract public void Close();

    }
}