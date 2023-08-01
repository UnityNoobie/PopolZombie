using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour //애니메이터를 동작하는데 사용하는 메소드
{
    Animator m_animator; 
    string m_prevAnimName;
    public void SetFloat(string stateName, float value) // 애니메이션의 속도, 동작 등에 필요한 Float값을 설정해줌.
    {
        m_animator.SetFloat(stateName, value);
    }
    public void Play(string animName, bool isBlend = true) // 애니메이션의 재생. string값을 통해 애니메이션을 실행하고 Blend 효과를 사용할지 말지도 받아옴.
    {
        if(!string.IsNullOrEmpty(m_prevAnimName))
        {
            m_animator.ResetTrigger(m_prevAnimName);
            m_prevAnimName = null;
        }
        if(isBlend)
        {
            try
            {
                m_animator.SetTrigger(animName);
                m_prevAnimName = animName;
            }
            catch (NullReferenceException)
            {
                Debug.Log(animName + "실행 중 에러발생.");
            }
        }
        else
        {
            m_animator.Play(animName, 0, 0f);
        }
    }
    // Start is called before the first frame update
    protected virtual void Start() 
    {
        m_animator = GetComponent<Animator>();
    }    
}
