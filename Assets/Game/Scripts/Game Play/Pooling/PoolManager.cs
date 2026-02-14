using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolData
    {
        public string key;
        public GameObject prefab;
        public int initialSize;
    }

    [SerializeField] private List<PoolData> pools;

    private Dictionary<string, Queue<PooledObject>> poolDictionary = new();
    private Dictionary<string, GameObject> prefabLookup = new();

    private void Awake()
    {
        foreach (var pool in pools)
        {
            Queue<PooledObject> objectPool = new();

            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);

                PooledObject pooled = obj.GetComponent<PooledObject>();
                pooled.PoolKey = pool.key;

                objectPool.Enqueue(pooled);
            }

            poolDictionary.Add(pool.key, objectPool);
            prefabLookup.Add(pool.key, pool.prefab);
        }
    }

    public PooledObject GetFromPool(string key)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogError($"Pool with key {key} not found!");
            return null;
        }

        if (poolDictionary[key].Count == 0)
        {
            GameObject obj = Instantiate(prefabLookup[key], transform);
            PooledObject pooled = obj.GetComponent<PooledObject>();
            pooled.PoolKey = key;
            obj.SetActive(false);
            poolDictionary[key].Enqueue(pooled);
        }

        PooledObject objectToSpawn = poolDictionary[key].Dequeue();
        objectToSpawn.gameObject.SetActive(true);

        return objectToSpawn;
    }

    public void ReturnToPool(PooledObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.parent = transform;
        poolDictionary[obj.PoolKey].Enqueue(obj);
    }
}