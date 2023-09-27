using System.Collections.Generic;
using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;
using UnityEngine;

namespace CbkSDK.Pooling
{
    public interface IPoolService : IService
    {
        public GameObject Instantiate(string tag, Vector3 position, Quaternion rotation);
        public void Destroy(string tag, GameObject gameObject);
        public void DeActivateAllObjectFromPool(string tag);
    }
    [Service(false)]
    public class PoolService : BaseMonoService ,IPoolService
    {
        [SerializeField] private PoolData _poolData;
        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        public void Initialize()
        {
            _poolData = Resources.LoadAll<PoolData>("")[0];
            InitPools();
        }

        private void InitPools()
        {
            foreach(PoolData.Pool pool in _poolData.pools)
            {
                var objectPool = new Queue<GameObject>();

                for(int i=0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefab);
                    obj.transform.SetParent(transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject Instantiate(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + "does not exist");
                return null;
            }
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public void Destroy(string tag, GameObject gameObject)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + "does not exist");
            }
            if (poolDictionary[tag].Contains(gameObject))
            {
                gameObject.SetActive(false);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("There is no pooled object in '"+tag+"' pool name  like this: '"+gameObject+"'");
#endif
                Destroy(gameObject);
            }
        }

        public void DeActivateAllObjectFromPool(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + "does not exist");
            }
            for (int i = 0; i < poolDictionary[tag].Count; i++)
            {
                var tempObject = poolDictionary[tag].Dequeue();
                if (tempObject)
                {
                    if (tempObject.activeInHierarchy)
                    {
                        tempObject.SetActive(false);
                    }
                    poolDictionary[tag].Enqueue(tempObject);
                }
            }
        }
    }
}