using System.Collections.Generic;
using UnityEngine;

namespace CbkSDK.Pooling
{
    [CreateAssetMenu(fileName = "PoolData", menuName = "_GAME/PoolData")]
    public class PoolData:ScriptableObject
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        public List<Pool> pools = new List<Pool>();
    }
}