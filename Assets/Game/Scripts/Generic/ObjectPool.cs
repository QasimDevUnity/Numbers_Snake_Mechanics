using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Queue<T> pool = new();
    private T prefab;

    public ObjectPool(T prefab, int initialSize)
    {
        this.prefab = prefab;

        for(int i=0;i<initialSize;i++)
            Create();
    }

    private T Create()
    {
        var obj = GameObject.Instantiate(prefab);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public T Get()
    {
        if(pool.Count == 0)
            Create();

        var obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}

