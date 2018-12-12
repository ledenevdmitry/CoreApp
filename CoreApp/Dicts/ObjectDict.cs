using CoreApp.Comparers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Dicts
{
    public class ObjectDict<T>
    {
        public ObjFilePairs<T> baseDict { get; protected set; }
        public ObjFilePairs<T> intersections { get; protected set; }
        public List<FileInfo> notFoundFiles { get; protected set; }

        public ObjectDict()
        {
            baseDict = new ObjFilePairs<T>();
            intersections = new ObjFilePairs<T>();
            notFoundFiles = new List<FileInfo>();
        }   

        public void AddObjectConsiderIntersections(T objName, FileInfo file)
        {
            if (baseDict.objFilesPairs.ContainsKey(objName) && !baseDict.objFilesPairs[objName].Contains(file)) //если объект уже был в таблице
            {
                if (!intersections.objFilesPairs.ContainsKey(objName)) //если его еще нет в пересечениях, добавляем тот, с кем он пересекся
                {
                    intersections.Add(objName, baseDict.Value(objName));
                }
                intersections.Add(objName, file);//добавляем новый в пересечение
            }
            baseDict.Add(objName, file);
        }
    }


    public class ObjFilePairs<T>
    {
        public Dictionary<T, HashSet<FileInfo>> objFilesPairs { get; protected set; }

        public ObjFilePairs()
        {
            objFilesPairs = new Dictionary<T, HashSet<FileInfo>>();
        }

        public ObjFilePairs(IEqualityComparer<T> comparer)
        {
            objFilesPairs = new Dictionary<T, HashSet<FileInfo>>(comparer);
        }

        public FileInfo Value(T key)
        {
            if (objFilesPairs[key].Count != 1)
            {
                throw new ArgumentException("Хэш-таблица содержит не одно значение");
            }
            else
            {
                return objFilesPairs[key].First();
            }
        }

        public void Add(T obj, FileInfo file)
        {
            if (!objFilesPairs.ContainsKey(obj))
            {
                objFilesPairs.Add(obj, new HashSet<FileInfo>(new FileInfoEqualityComparer()));
            }

            objFilesPairs[obj].Add(file);
        }
        
    }
}
