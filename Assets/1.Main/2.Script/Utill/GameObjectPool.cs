using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class GameObjectPool<T> where T : class
{
    public delegate T Func();
    Queue<T> m_objPool = new Queue<T>();
    Func m_createFunc;
    int m_count = 0;
    public int Count { get { return m_objPool.Count; } }
    public GameObjectPool(int count, Func createFunc)
    {
        m_count = count;
        m_createFunc = createFunc;
        Allocation();
    }
    public GameObjectPool() { }
    public void MakePool(int Count, Func createFunc)
    {
        m_count = Count;
        m_createFunc = createFunc;
        Allocation();
    }
    
  
    void Allocation()
    {
        for(int i = 0; i < m_count; i++)
        {
            m_objPool.Enqueue(m_createFunc());
        }
    }
    
    public T New()
    {
        return m_createFunc();
    }
    public T Get()
    {
        if(m_objPool.Count > 0)
        {
            return m_objPool.Dequeue();
        }
        else
        {
            return m_createFunc();
        }
    }
    public void Set(T obj)
    {
        m_objPool.Enqueue(obj);
    }
}
