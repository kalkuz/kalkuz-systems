using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Generics
{
    [System.Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        [SerializeField] private TKey key;
        [SerializeField] private TValue value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey Key => this.key;
        public TValue Value => this.value;

        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> kvp)
        {
            return new KeyValuePair<TKey, TValue>(kvp.key, kvp.value);
        }
    }
}