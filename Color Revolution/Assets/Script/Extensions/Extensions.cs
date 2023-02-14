using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Kinopi.Enums;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kinopi.Extensions
{
    public static class Extensions
    {        
       public static void SetActive(this Component obj, bool isActive)
        {
            obj.gameObject.SetActive(isActive);
        }

        public static bool ActiveSelf(this Component obj)
        {
            return obj.gameObject.activeSelf;
        }
        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            return list[Random.Range(0, list.Count)];
        }


        public static T GetRandomElement<T>(this ReadOnlyCollection<T> list)
        {
            if (list.Count == 0) return default(T);
            return list[Random.Range(0, list.Count)];
        }

        public static List<T> GetRandomElements<T>(this List<T> list, int count, List<T> exceptList = null)
        {
            List<T> copyList = new List<T>(list);
            
            if (exceptList != null) copyList.RemoveAll(x => exceptList.Contains(x));            
            if (count > copyList.Count) return copyList;
            
            List<T> randomList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, copyList.Count);
                randomList.Add(copyList[index]);
                copyList.RemoveAt(index);
            }
            return randomList;
        }

        public static T GetRandomElements<T>(this List<T> list)
        {                        
            return list[Random.Range(0, list.Count)];
        }


        public static List<T> GetRandomElementsAndRemoveFromList<T>(this List<T> list, int count)
        {

            if (count > list.Count)
            {
                List<T> copyList = new List<T>(list);
                list.Clear();
                return copyList;
            }

            List<T> randomList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                randomList.Add(list[index]);
                list.RemoveAt(index);
            }
            return randomList;
        }

        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count; i > 0; i--)
            {
                int k = UnityEngine.Random.Range(0, i);
                T value = list[k];
                list[k] = list[i - 1];
                list[i - 1] = value;
            }
        }
        

        public static T RandomEnumValue<T>() where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            var Arr = Enum.GetValues(typeof(T));
            return (T)Arr.GetValue(UnityEngine.Random.Range(0, Arr.Length));
        }

        public static T RandomEnumValue<T>(List<T> exludeList) where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            var Arr = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            Arr = Arr.Except(exludeList).ToList();
            return Arr[Random.Range(0, Arr.Count)];
        }

        public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key)
        {
            TV value;
            return dict.TryGetValue(key, out value) ? value : default(TV);
        }
        

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static T Next<T>(this T src) where T : Enum
        {
            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (j == Arr.Length) ? Arr[0] : Arr[j];
        }
        
        public static T Prev<T>(this T src) where T : Enum
        {
            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) - 1;
            return (j < 0) ? Arr[^1] : Arr[j];
        }
        
        
    }
}

