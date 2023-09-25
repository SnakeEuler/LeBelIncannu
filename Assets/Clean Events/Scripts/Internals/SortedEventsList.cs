using System;
using System.Collections.Generic;

namespace CleanEvents
{
    internal class SortedEventsList<TValue>
    {
        public class DescendingComparer<T> : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y.CompareTo(x);
            }
        }

        SortedDictionary<int, List<TValue>> dictionary;

        public SortedEventsList()
        {
            dictionary = new SortedDictionary<int, List<TValue>>(new DescendingComparer<int>());
        }

        public void Add(int key, TValue value)
        {
            if (dictionary.ContainsKey(key) == false)
            {
                dictionary.Add(key, new List<TValue>());
            }

            dictionary[key].Add(value);
        }

        public bool ContainsKey(int key)
        {
            return dictionary.ContainsKey(key);
        }

        public IEnumerable<TValue> GetValues(int key)
        {
            foreach (var list in dictionary[key])
                yield return list;
        }

        public IEnumerable<int> GetAllKeys()
        {
            foreach (var item in dictionary.Keys)
                yield return item;
        }

        public IEnumerable<TValue> GetAllValues()
        {
            foreach (var list in dictionary.Values)
                foreach (var item in list)
                    yield return item;
        }

        public IEnumerable<KeyValuePair<int, TValue>> GetAll()
        {
            foreach (var list in dictionary)
                foreach (var item in list.Value)
                    yield return new KeyValuePair<int, TValue>(list.Key, item);
        }

        public void Remove(Func<TValue, bool> comparer)
        {
            foreach (var list in dictionary.Values)
                for (int i = 0; i < list.Count; i++)
                {
                    if (comparer(list[i]))
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
        }
    }
}
