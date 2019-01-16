using CoreApp.Comparers;
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
    public class ObjectDict<O> where O : ETLObject
    {
        public ObjPatchPairs<O> baseDict { get; protected set; }
        public ObjPatchPairs<O> intersections { get; protected set; }
        public List<FileInfo> notFoundFiles { get; protected set; }

        public ObjectDict()
        {
            baseDict = new ObjPatchPairs<O>();
            intersections = new ObjPatchPairs<O>();
            notFoundFiles = new List<FileInfo>();
        }   

        public void AddObjectConsiderIntersections(O obj)
        {
            if (baseDict.oneToManyPairs.ContainsKey(obj) && !baseDict.oneToManyPairs[obj].ContainsValue(obj.patch)) //если объект уже был в таблице
            {
                if (!intersections.oneToManyPairs.ContainsKey(obj)) //если его еще нет в пересечениях, добавляем тот, с кем он пересекся
                {
                    intersections.Add(obj, baseDict.SingleValue(obj));
                }
                intersections.Add(obj, obj.patch); //добавляем новый в пересечение
            }
            baseDict.Add(obj, obj.patch);
        }
    }

    public class OneToManyPairs<K, O, M> where O : K
    {
        //внешний O - ключ, внутренний - все значения, удовл. этому ключу
        public Dictionary<K, Dictionary<O, M>> oneToManyPairs { get; protected set; }

        public OneToManyPairs()
        {
            oneToManyPairs = new Dictionary<K, Dictionary<O, M>>();
        }        

        public IEnumerable<KeyValuePair<O, M>> EnumeratePairs()
        {
            foreach (var keyToDictPair in oneToManyPairs.Values)
            {
                foreach (var kvp in keyToDictPair)
                {
                    yield return new KeyValuePair<O, M>(kvp.Key, kvp.Value);
                }
            }
        }

        public IEnumerable<O> EnumerateOnes()
        {
            foreach(var kvp in oneToManyPairs.Values)
            {
                foreach(O o in kvp.Keys)
                {
                    yield return o;
                }
            }
        }
        
        private IEnumerable<KeyValuePair<O, M>> EnumeratePairs(K key)
        {
            foreach(var kvp in oneToManyPairs[key])
            {
                yield return new KeyValuePair<O, M>(kvp.Key, kvp.Value);
            }
        }

        public IEnumerable<IEnumerable<KeyValuePair<O, M>>> EnumerateByDistinctKeys()
        {
            foreach(var key in oneToManyPairs.Keys)
            {
                yield return EnumeratePairs(key);
            }
        }

        public M SingleValue(O obj) 
        {
            if (oneToManyPairs[obj].Values.Count != 1)
            {
                throw new ArgumentException("Хэш-таблица содержит не одно значение");
            }
            else
            {
                return oneToManyPairs[obj].Values.First();
            }
        }

        public void Add(O one, M many)
        {
            if (!oneToManyPairs.ContainsKey(one))
            {
                oneToManyPairs.Add(one, new Dictionary<O, M>());
            }

            //пересечения будем смотреть только по разным патчам
            if (oneToManyPairs[one].ContainsValue(many))
            {
                oneToManyPairs[one].Add(one, many);
            }
        }
    }

    public class ObjObjsPairs<O> : OneToManyPairs<Key, O, O> where O : Key
    { }

    public class ObjPatchPairs<O> : OneToManyPairs<Key, O, Patch> where O : Key
    { }
}
