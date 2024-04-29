using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Logic
{
    [Serializable]
    public class SerializableDict<K, V> : ISerializationCallbackReceiver, IEnumerable<KeyValuePair<K, V>>
    {
        public Dictionary<K, V> dict = new Dictionary<K, V>();

        [SerializeField]
        public List<K> keys;
        [SerializeField]
        public List<V> values;

        public Dictionary<K, V>.KeyCollection Keys { get { return dict.Keys; } }
        public Dictionary<K, V>.ValueCollection Values { get { return dict.Values; } }

        public SerializableDict(Dictionary<K, V> dict)
        {
            this.dict = dict;
        }

        public SerializableDict()
        {
            this.dict = new Dictionary<K, V>();
        }

        public Action<K> OnChange;

        public V this[K key]
        {
            get { return dict[key]; }
            set { dict[key] = value; if (OnChange != null) OnChange(key); }
        }


        public bool ContainsKey(K key)
        {
            return dict.ContainsKey(key);
        }

        public void OnBeforeSerialize()
        {
            keys = dict.Keys.ToList();
            values = dict.Values.ToList();
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                dict[keys[i]] = values[i];
            }
        }

        public SerializableDict<K, V> ToDictionary()
        {
            SerializableDict<K, V> clone = new SerializableDict<K, V>();
            clone.dict = dict.ToDictionary(row=>row.Key, row=>row.Value);
            return clone;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dict.GetEnumerator();
        }
    }
}
