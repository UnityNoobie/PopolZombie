using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour //�ִϸ����͸� �����ϴµ� ����ϴ� �޼ҵ�
{
    Animator m_animator; 
    string m_prevAnimName;
    public void SetFloat(string stateName, float value) // �ִϸ��̼��� �ӵ�, ���� � �ʿ��� Float���� ��������.
    {
        m_animator.SetFloat(stateName, value);
    }
    public void Play(string animName, bool isBlend = true) // �ִϸ��̼��� ���. string���� ���� �ִϸ��̼��� �����ϰ� Blend ȿ���� ������� ������ �޾ƿ�.
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
                Debug.Log(animName + "���� �� �����߻�.");
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
