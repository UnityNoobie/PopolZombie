using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator m_animator;
    string m_prevAnimName;
    public void SetFloat(string stateName, float value)
    {
        m_animator.SetFloat(stateName, value);
    }
    public void Play(string animName, bool isBlend = true)
    {
        if(!string.IsNullOrEmpty(m_prevAnimName))
        {
            m_animator.ResetTrigger(m_prevAnimName);
            m_prevAnimName = null;
        }
        if(isBlend)
        {
            //Debug.Log(animName);
            try
            {
                m_animator.SetTrigger(animName);
                m_prevAnimName = animName;
            }
            catch (NullReferenceException)
            {
                Debug.Log(animName + "실행 중 에러발생.");
                //이유를 알 수없고 에러가 났지만 적들은 매우 정상적으로 작동함. 애니메이션, 네비메쉬, 피격, 공격 모든거 정상 이유가모지...ㅠㅠㅠㅠ 
                // 코드한줄 안바꿨는데 갑자기 에러가 났음. Start Awake 순서문제인거로 추정되나 현 기능상 문제는 없으니 일단은 진행. 추후 픽스
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
