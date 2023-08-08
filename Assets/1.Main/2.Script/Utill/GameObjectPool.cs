using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class GameObjectPool<T> where T : class //오브젝트 풀링할 때 사용하는 메소드
{
    public delegate T Func();
    Queue<T> m_objPool = new Queue<T>();
    Func m_createFunc;
    int m_count = 0;
    public int Count { get { return m_objPool.Count; } }
    public GameObjectPool(int count, Func createFunc)//풀 생성 기능.
    {
        m_count = count;
        m_createFunc = createFunc;
        Allocation();
    }
    public GameObjectPool() { }
    public void MakePool(int Count, Func createFunc) //풀 생성 기능.
    {
        m_count = Count;
        m_createFunc = createFunc;
        Allocation();
    }
    void Allocation() //새로운 오브젝트를 풀에 넣어주기
    {
        for(int i = 0; i < m_count; i++)
        {
            m_objPool.Enqueue(m_createFunc());
        }
    }
    public T New() //새로운 오브젝트 생성
    {
        return m_createFunc();
    }
    public T Get() //들어있는 오브젝트 빼주기 만약 없다면 새로운 값 생성
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
    public void Set(T obj) //오브젝트 풀에 오브젝트 넣어주기
    {
        m_objPool.Enqueue(obj);
    }
}
