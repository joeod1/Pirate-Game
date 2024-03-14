using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Assets
{
    [Serializable]
    public class NameMap
    {
        public List<string> prefixes;
        public List<string> suffixes;
        public List<string> names;

        public HashSet<int3> existingNames = new HashSet<int3>();
    }
}