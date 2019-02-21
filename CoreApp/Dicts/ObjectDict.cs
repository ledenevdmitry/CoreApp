using CoreApp.FixpackObjects;
using CoreApp.Keys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Dicts
{
    public class ObjectDict
    {
        public ETLDict baseDict { get; protected set; }
        public ETLDict intersections { get; protected set; }
        public List<FileInfo> notFoundFiles { get; protected set; }

        public ObjectDict()
        {
            baseDict = new ETLDict();
            intersections = new ETLDict();
            notFoundFiles = new List<FileInfo>();
        }   

        public void AddObjectConsiderIntersections(ETLObject obj)
        {
            Key key = new Key(obj.objName, obj.objType);           

            if (baseDict.oneToManyPairs.ContainsKey(key) && //если объект уже был в таблице
               !baseDict.oneToManyPairs[key].ContainsValue(obj.patch)) //а добавляется из другого патча
            {
                if (!intersections.oneToManyPairs.ContainsKey(key)) //если его еще нет в пересечениях, добавляем тот, с кем он пересекся
                {
                    ETLObject oldObj = baseDict.SingleValue(key);
                    intersections.Add(key, oldObj, oldObj.patch);
                }
                intersections.Add(key, obj, obj.patch); //добавляем новый в пересечение
            }
            baseDict.Add(key, obj, obj.patch);
        }
    }

    public class ObjToParentsDict
    {
        public Dictionary<ETLObject, HashSet<ETLObject>> oneToManyPairs;

        public ObjToParentsDict()
        {
            oneToManyPairs = new Dictionary<ETLObject, HashSet<ETLObject>>();
        }

        public IEnumerable<KeyValuePair<ETLObject, ETLObject>> EnumeratePairs()
        {
            foreach(var kvp in oneToManyPairs)
            {
                foreach(var parent in kvp.Value)
                {
                    yield return new KeyValuePair<ETLObject, ETLObject>(kvp.Key, parent);
                }
            }
        }

        public IEnumerable<ETLObject> EnumerateObjs()
        {
            foreach (var kvp in oneToManyPairs)
            {
                yield return kvp.Key;
            }
        }

        public IEnumerable<KeyValuePair<ETLObject, ETLObject>> EnumeratePairs(ETLObject obj)
        {
            foreach (var parent in oneToManyPairs[obj])
            {
                yield return new KeyValuePair<ETLObject, ETLObject>(obj, parent);
            }
        }

        public IEnumerable<IEnumerable<KeyValuePair<ETLObject, ETLObject>>> EnumerateByDistinctKeys()
        {
            foreach (var obj in oneToManyPairs.Keys)
            {
                yield return EnumeratePairs(obj);
            }
        }
        

        public void Add(ETLObject obj, ETLObject parent)
        {

            if (!oneToManyPairs.ContainsKey(obj))
            {
                oneToManyPairs.Add(obj, new HashSet<ETLObject>());
            }

            oneToManyPairs[obj].Add(parent);
        }

    }

    public class ETLDict
    {
        //внешний O - ключ, внутренний - все значения, удовл. этому ключу
        public Dictionary<Key, Dictionary<ETLObject, ZPatch>> oneToManyPairs { get; protected set; }

        public ETLDict()
        {
            oneToManyPairs = new Dictionary<Key, Dictionary<ETLObject, ZPatch>>();
        }        

        public IEnumerable<KeyValuePair<ETLObject, ZPatch>> EnumerateObjPatchPairs()
        {
            foreach (var keyToDictPair in oneToManyPairs.Values)
            {
                foreach (var kvp in keyToDictPair)
                {
                    yield return new KeyValuePair<ETLObject, ZPatch>(kvp.Key, kvp.Value);
                }
            }
        }

        public IEnumerable<ETLObject> EnumerateObjs()
        {
            foreach(var kvp in oneToManyPairs.Values)
            {
                foreach(ETLObject o in kvp.Keys)
                {
                    yield return o;
                }
            }
        }
        
        public IEnumerable<KeyValuePair<ETLObject, ZPatch>> EnumeratePairs(Key key)
        {
            foreach(var kvp in oneToManyPairs[key])
            {
                yield return new KeyValuePair<ETLObject, ZPatch>(kvp.Key, kvp.Value);
            }
        }

        public IEnumerable<IEnumerable<KeyValuePair<ETLObject, ZPatch>>> EnumerateByDistinctKeys()
        {
            foreach(var key in oneToManyPairs.Keys)
            {
                yield return EnumeratePairs(key);
            }
        }

        public ETLObject SingleValue(Key key) 
        {
            if (oneToManyPairs[key].Keys.Count != 1)
            {
                throw new ArgumentException("Хэш-таблица содержит не одно значение");
            }
            else
            {
                return oneToManyPairs[key].Keys.First();
            }
        }

        public void Add(Key key, ETLObject one, ZPatch many)
        {

            if (!oneToManyPairs.ContainsKey(key))
            {
                oneToManyPairs.Add(key, new Dictionary<ETLObject, ZPatch>());
            }

            //пересечения будем смотреть только по разным патчам
            if (!oneToManyPairs[key].ContainsValue(many))
            {
                oneToManyPairs[key].Add(one, many);
            }
        }
    }
    
}
