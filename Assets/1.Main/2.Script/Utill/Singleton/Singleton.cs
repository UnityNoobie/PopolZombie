using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    static T instance = null;
    public static T Instance { get { return instance; }private set { instance = value; } }
    static Singleton()
    {
        if(Instance == null)
        {
            Instance = new T();
        }
    }
}
