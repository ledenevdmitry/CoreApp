using CoreApp.FixpackObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreApp.CVS;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace CoreApp.ReleaseObjects
{
    public class Release
    {
        public SortedList<string, Fixpack> fixpacks { get; private set; }

        public string name { get; private set; }

        DirectoryInfo localDir;
        CVS.CVS cvs;

        private void setAttributesNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
                setAttributesNormal(subDir);
            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }

        public void DeleteLocal()
        {
            if (localDir.Exists)
            {
                setAttributesNormal(localDir);
                localDir.Delete(true);
            }
        }

        //из системы контроля версий
        public Release(string name, DirectoryInfo dir, CVS.CVS cvs, Regex pattern) : this(name)
        {
            SetLocalDir(dir);
            DeleteLocal();
            dir.Create();

            this.cvs = cvs;

            List<string> fpNames = new List<string>();

            var cvsPaths = cvs.AllInEntireBase(fpNames, pattern);

            int i = 0;
            foreach(var cvsPath in cvsPaths)
            {
                string localPath = string.Join("\\", localDir.FullName, fpNames[i++]);
                cvs.Download(cvsPath, localPath);

                Fixpack fp = new Fixpack(new DirectoryInfo(localPath));
                fixpacks.Add(fp.FullName, fp);
            }
        }

        //загрузиться локально
        public Release(DirectoryInfo dir) : this(dir.Name)
        {
            SetLocalDir(dir);
            foreach(var subdir in dir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Fixpack currFixpack = new Fixpack(subdir);
                fixpacks.Add(currFixpack.FullName, currFixpack);
            }
        }

        public Release(string name)
        {
            fixpacks = new SortedList<string, Fixpack>();

            this.name = name;
        }


        public void SetLocalDir(DirectoryInfo localDir)
        {
            this.localDir = localDir;
        }

        public void SetCVS(CVS.CVS cvs)
        {
            this.cvs = cvs;
        }

        public void LoadFixpackFromCVS(string code)
        {
            string shortName = "";
            string fixpackCVSPath = cvs.FirstInEntireBase(ref shortName, new Regex($".*{code}.*"));
            string fpPath = string.Join("\\", localDir.FullName, shortName);
            cvs.Download(fixpackCVSPath, fpPath);

            Fixpack fp = new Fixpack(new DirectoryInfo(fpPath));
            fixpacks.Add(fp.FullName, fp);
        }

        public void SetAllDependencies()
        {
            foreach(Fixpack fp in fixpacks.Values)
            {
                try
                {
                    fp.ReadMetaFromExcelFile(FindLocalExcel(fp));
                }
                catch { }
            }
        }


    }
}
