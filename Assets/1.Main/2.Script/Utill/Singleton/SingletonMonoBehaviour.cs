using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> //MonoBehaviur 상속받는 싱긅톤 클래스
{
    public static T Instance { get; private set; }
    protected virtual void OnAwake() { } //Awake에서 실행될 메소드
    protected virtual void OnStart() { } //Start에서 실행될 메소드
    void Awake() 
    {
        if(Instance == null) //인스턴스 되어있지 않다면 인스턴스 후 실행
        {
            Instance = (T)this;
            OnAwake();
        }
        else //이미 있으면 삭제
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(Instance == (T)this)
        {
            OnStart();
        }
    }   
}
