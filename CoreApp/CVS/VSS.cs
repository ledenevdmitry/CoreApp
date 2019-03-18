using Microsoft.VisualStudio.SourceSafe.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.CVS
{
    public class VSS : CVS
    {
        public VSSDatabase VSSDB { get; private set; }

        public VSS(string location, string login) : base(location, login)
        {
            Connect();
        }

        public VSS() : base() { }

        public override void Connect()
        {
            try
            {
                VSSDB = new VSSDatabase();
                VSSDB.Open(Location, Login, "");
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
                VSSDB.get_VSSItem(source, false);
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

        private string SpecToCorrectPath(string spec)
        {
            return spec.Insert(1, "/");
        }


        public override IEnumerable<string> AllInEntireBase(string root, List<string> matches, Regex pattern, int depth)
        {
            Queue<Tuple<VSSItem, int>> queue = new Queue<Tuple<VSSItem, int>>();
            queue.Enqueue(new Tuple<VSSItem, int>(VSSDB.get_VSSItem(root, false), 0));
            while (queue.Count > 0)
            {
                Tuple<VSSItem, int> currItem = queue.Dequeue();
                if (IsMatch(pattern, currItem.Item1))
                {
                    matches.Add(currItem.Item1.Name);
                    yield return SpecToCorrectPath(currItem.Item1.Spec);
                }

                if (currItem.Item2 < depth)
                {
                    foreach (VSSItem subItem in currItem.Item1.Items)
                    {
                        if ((VSSItemType)subItem.Type == VSSItemType.VSSITEM_PROJECT)
                        {
                            queue.Enqueue(new Tuple<VSSItem, int>(subItem, depth + 1));
                        }
                    }
                }
            }
            throw new ArgumentException("File Not Found");
        }

        public override string FirstInEntireBase(string root, out string match, Regex pattern, int depth)
        {
            Queue<Tuple<VSSItem, int>> queue = new Queue<Tuple<VSSItem, int>>();
            queue.Enqueue(new Tuple<VSSItem, int>(VSSDB.get_VSSItem(root, false), 0));
            while(queue.Count > 0)
            {
                Tuple<VSSItem, int> currItem = queue.Dequeue();
                
                if (IsMatch(pattern, currItem.Item1))
                {
                    match = currItem.Item1.Name;
                    return SpecToCorrectPath(currItem.Item1.Spec);
                }

                if (currItem.Item2 < depth)
                {
                    foreach (VSSItem subItem in currItem.Item1.Items)
                    {
                        if ((VSSItemType)subItem.Type == VSSItemType.VSSITEM_PROJECT)
                        {
                            queue.Enqueue(new Tuple<VSSItem, int>(subItem, depth + 1));
                        }
                    }
                }
            }
            throw new ArgumentException("File Not Found");
        }
        
        private bool IsMatch(Regex pattern, VSSItem item)
        {
            return pattern.IsMatch(item.Name);
        }

        public delegate void MoveDelegate(string movingFolderName, VSSItem movingFolder);
        public event MoveDelegate AfterMove;
        static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

        public override void Move(string destination, IEnumerable<string> items)
        {
            VSSItem destFolder;
            try
            {
                destFolder = VSSDB.get_VSSItem(destination, false);
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
                        VSSItem movingFolder = VSSDB.get_VSSItem(item, false);
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
                VSSItem folder = VSSDB.get_VSSItem(oldName, false);
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

        private void DeleteLocalIfNotExistsInVSS(VSSItem folder, DirectoryInfo destination)
        {
            foreach(var subFile in destination.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
            {
                bool found = false;
                foreach(VSSItem subitem in folder.Items)
                {
                    //1 - файл
                    if(subitem.Type == 1 && subitem.Name.Equals(subFile.Name))
                    {
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                    if (!subFile.Name.Equals("vssver2.scc", StringComparison.InvariantCultureIgnoreCase))
                    {
                        subFile.Delete();
                    }
                }
            }

            foreach(VSSItem subitem in folder.Items)
            {
                //0 - папка
                if (subitem.Type == 0)
                {
                    DirectoryInfo subdestination = new DirectoryInfo(Path.Combine(destination.FullName, subitem.Name));
                    DeleteLocalIfNotExistsInVSS(subitem, subdestination);
                }
            }
        }

        public override void Pull(string dir, DirectoryInfo destination)
        {
            try
            {
                VSSItem folder = VSSDB.get_VSSItem(dir, false);                
                if (!destination.Exists)
                {
                    destination.Create();
                }
                folder.Get(destination.FullName, (int)(VSSFlags.VSSFLAG_RECURSYES | VSSFlags.VSSFLAG_REPREPLACE));
                DeleteLocalIfNotExistsInVSS(folder, destination);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
        }

        private void Unpin(VSSItem objItem)
        {
            int version = 0;
            VSSItem objOldItem = objItem.Version[version];
            VSSItem objProject = objItem.Parent;

            objProject.Share(objOldItem, "", (int)VSSFlags.VSSFLAG_GETNO);
        }

        private void Pin(VSSItem objItem, int version)
        {
            VSSItem objOldItem = objItem.Version[version];
            VSSItem objProject = objItem.Parent;

            objProject.Share(objOldItem, "", 0);
        }

        public override void PrepareToPush(string destination)
        {
            IVSSItem item = VSSDB.get_VSSItem(destination, false);
            if (item.IsPinned)
            {
                Unpin((VSSItem)item);
            }

            //!(file is checked out to the current user)
            if (!(item.IsCheckedOut == 2))
            {
                item.Checkout();
            }
        }

        public override void Push(string source, string destination)
        {
            try
            {
                IVSSItem item = VSSDB.get_VSSItem(destination, false);
                item.Checkin("", source);
                Pin((VSSItem)item, item.VersionNumber);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
        }

        public override void Close()
        {
            VSSDB.Close();
        }
    }
}
