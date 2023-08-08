﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : SingletonDontDestroy<T> //싱긅톤 패턴에 MonoBehaviour, DontDestroyOnLoad 적용
{
    public static T Instance { get; private set; }
    protected virtual void OnAwake() { } //Awake에서 로드되는 Awake 대체 사용 메소드
    protected virtual void OnStart() { } //Start에서 로드되는 Start 대체 사용 메소드
    void Awake()
    {
        if (Instance == null) //인스턴스 되어있지 않다면 Awake실행
        {
            Instance = (T)this;
            OnAwake();
            DontDestroyOnLoad(gameObject);
        }
        else //아니면 중복이라 삭제
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == (T)this)
        {
            OnStart();
        }
    }
}
