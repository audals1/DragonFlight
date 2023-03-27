using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class 
{
    public delegate T CreatFunc();
    Queue<T> m_objQueue = new Queue<T>();
    CreatFunc m_creatFunc;
    int m_count;
    public GameObjectPool(int count, CreatFunc func)
    {
        m_count = count;
        m_creatFunc = func;
        Allocation();
    }
    void Allocation()
    {
        for (int i = 0; i < m_count; i++)
        {
            m_objQueue.Enqueue(m_creatFunc());
        }
    }
    public T Get()
    {
        if(m_objQueue.Count > 0)
        {
            return m_objQueue.Dequeue();
        }
        else
        {
            return m_creatFunc();
        }
    }
    public void Set(T obj)
    {
        m_objQueue.Enqueue(obj);
    }
}