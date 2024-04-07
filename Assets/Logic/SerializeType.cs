using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Logic
{
    [Serializable]
    public class SerializeType<T> : ISerializationCallbackReceiver where T : UnityEngine.Object
    {
        T[] array;

        [SerializeField]
        public List<string> objects;

        public void GetElements()
        {
            array = UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None);
        }

        public void OnAfterDeserialize()
        {
            
        }

        public void OnBeforeSerialize()
        {
            if (array == null) return;
            for (int i = 0; i < array.Length; i++)
            {
                objects.Add(JsonUtility.ToJson(array[i]));
            }
        }
    }
}
