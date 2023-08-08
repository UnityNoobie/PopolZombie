using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new() //싱글턴 디자인패턴 간편화해서 사용하기 위함.
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
