using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Generics
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ICollection<SerializableKeyValuePair<TKey, TValue>>
    {
        [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> dictionary;

        public SerializableDictionary()
        {
            dictionary = new List<SerializableKeyValuePair<TKey, TValue>>();
        }
        
        public TValue this[TKey key]
        {
            get
            {
                TryGetValue(key, out TValue value);
                return value;
            }
            set
            {
                if(!ContainsKey(key)) Add(key, value);
                else
                {
                    Remove(key);
                    Add(key, value);
                }
            }
        }
        
        public int Count => dictionary.Count;
        public bool IsReadOnly => false;
        
        public ICollection<TKey> Keys => dictionary.Select((kvp) => kvp.Key).ToList();

        public ICollection<TValue> Values => dictionary.Select((kvp) => kvp.Value).ToList();
        
        IEnumerator<SerializableKeyValuePair<TKey, TValue>> IEnumerable<SerializableKeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        #region Methods

        public void Add(SerializableKeyValuePair<TKey, TValue> item)
        {
            if (ContainsKey(item.Key)) throw new Exception($"Key '{item.Key}' already exists.");
            
            dictionary.Add(item);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(SerializableKeyValuePair<TKey, TValue> item)
        {
            return dictionary.Contains(item);
        }

        public void CopyTo(SerializableKeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(SerializableKeyValuePair<TKey, TValue> item)
        {
            return dictionary.Remove(item);
        }

        public void Add(TKey key, TValue value)
        {
            Add(new SerializableKeyValuePair<TKey, TValue>(key, value));
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.Exists((kvp) => kvp.Key.Equals(key));
        }

        public bool Remove(TKey key)
        {
            int removed = dictionary.RemoveAll((kvp) => kvp.Key.Equals(key));
            return removed > 0;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var result = dictionary.Find((kvp) => kvp.Key.Equals(key));
            if (result.Value != null)
            {
                value = result.Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        #endregion
    }
}