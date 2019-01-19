using Microsoft.VisualStudio.SourceSafe.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.CVS
{
    public class VSS : CVS
    {
        public VSSDatabase vssDatabase { get; private set; }

        public VSS(string location, string login) : base(location, login) { }

        public VSS() : base() { }

        public override void Connect()
        {
            try
            {
                vssDatabase = new VSSDatabase();
                vssDatabase.Open(location, login, "");
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                if (exc.ErrorCode == -2147167977)
                {
                    throw new ArgumentException("Wrong location or login");
                }
                else
                    throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
            catch
            {
                throw new Exception("Неопознанная ошибка");
            }
        }

        public override void Move(string source, string destination, IEnumerable<string> items)
        {
            try
            {
                vssDatabase.get_VSSItem(source, false);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
            catch
            {
                throw new Exception("Неопознанная ошибка");
            }
            items = items.Select(x => string.Join("/", source, x));
            Move(destination, items);
        }

        public override string FindInEntireBase(string partOfName, ref string shortName)
        {
            string res = FindInEntireBase(partOfName, vssDatabase.get_VSSItem("$", false), ref shortName);
            if(res == null)
            {
                throw new ArgumentException("File Not Found");
            }
            else
            {
                return res;
            }
        }

        private string SpecToCorrectPath(string spec)
        {
            return spec.Insert(1, "/");
        }

        private string FindInEntireBase(string partOfName, VSSItem currItem, ref string shortName)
        {
            if(MatchPart(partOfName, currItem))
            {
                shortName = currItem.Name;
                return SpecToCorrectPath(currItem.Spec);
            }
            else
            {
                foreach(VSSItem subItem in currItem.Items)
                {
                    if ((VSSItemType)subItem.Type == VSSItemType.VSSITEM_PROJECT)
                    {
                        string res = FindInEntireBase(partOfName, subItem, ref shortName);
                        if(res != null)
                        {
                            return res;
                        }
                    }
                }
            }
            return null;
        }

        private bool MatchPart(string partOfName, VSSItem item)
        {
            return item.Name.IndexOf(partOfName) != -1;
        }

        public delegate void MoveDelegate(string movingFolderName, VSSItem movingFolder);
        public event MoveDelegate AfterMove;
        static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

        public override void Move(string destination, IEnumerable<string> items)
        {
            VSSItem destFolder;
            try
            {
                destFolder = vssDatabase.get_VSSItem(destination, false);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
            catch
            {
                throw new Exception("Неопознанная ошибка");
            }

            foreach (string item in items)
            {
                Thread th = new Thread(() =>
                {
                    try
                    {
                        rwl.EnterReadLock();
                        VSSItem movingFolder = vssDatabase.get_VSSItem(item, false);
                        rwl.ExitReadLock();
                        movingFolder.Move(destFolder);
                        AfterMove(item, movingFolder);
                    }

                    catch (System.Runtime.InteropServices.COMException exc)
                    {
                        throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
                    }
                    catch
                    {
                        throw new Exception("Неопознанная ошибка");
                    }
                });
                th.Start();
            }
        }

        public override void Rename(string oldName, string newName)
        {
            try
            {
                VSSItem folder = vssDatabase.get_VSSItem(oldName, false);
                folder.Name = newName;
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
            catch
            {
                throw new Exception("Неопознанная ошибка");
            }
        }

        public override void Download(string dir, string destination)
        {
            try
            {
                VSSItem folder = vssDatabase.get_VSSItem(dir, false);
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }
                folder.Get(destination, (int)VSSFlags.VSSFLAG_RECURSYES);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
            catch
            {
                throw new Exception("Неопознанная ошибка");
            }
        }

        public override void Close()
        {
            vssDatabase.Close();
        }
    }
}
