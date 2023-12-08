using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly GameObject _prefab;
    private readonly Queue<GameObject> _pool = new();

    public ObjectPool(GameObject prefab, int count, Transform parent = null)
    {
        _prefab = prefab;
        
        Fill(count, parent);
    }
    
    private void Fill(int count, Transform t)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = Object.Instantiate(_prefab, t);
            obj.SetActive(false);
            
            Push(obj);
        }
    }

    public GameObject Pop()
    {
        return _pool.Count > 0 ? _pool.Dequeue() : Object.Instantiate(_prefab);
    }

    public void Push(GameObject obj)
    {
        if (obj != null)
            _pool.Enqueue(obj);
    }
}

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T _prefab;
    private readonly Queue<T> _pool = new();

    public ObjectPool(T prefab, int count, Transform parent = null)
    {
        _prefab = prefab;
        
        Fill(count, parent);
    }
    
    private void Fill(int count, Transform t)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = Object.Instantiate(_prefab, t);
            obj.gameObject.SetActive(false);
            
            Push(obj);
        }
    }

    public T Pop()
    {
        return _pool.Count > 0 ? _pool.Dequeue() : Object.Instantiate(_prefab);
    }

    public void Push(T obj)
    {
        if (obj != null)
            _pool.Enqueue(obj);
    }
}