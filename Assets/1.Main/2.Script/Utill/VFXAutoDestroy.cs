using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAutoDestroy : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    float m_lifeTime = 0f;
    float m_time;
    ParticleSystem[] m_Particles;
    // Start is called before the first frame update
    #endregion

    #region Methods
    void OnEnable() //������ �� �ð� �������ְ� �ڷ�ƾ ����
    {
        m_time= Time.time;
        StartCoroutine(Coroutine_AutoDestroy());
    }
    void Start()
    {
       m_Particles = GetComponentsInChildren<ParticleSystem>();   
    }   
    // Update is called once per frame

    IEnumerator Coroutine_AutoDestroy() //�ڵ����� ����Ʈ�� �������ִ� �ڷ�ƾ
    {
        while (true)
        {
            yield return null;

            if (m_lifeTime > 0)
            {
                if (m_time + m_lifeTime < Time.time)
                {
                    gameObject.SetActive(false);
                    StopCoroutine(Coroutine_AutoDestroy());
                }
            }
            else
            {
                bool isPlaying = false;
                for (int i = 0; i < m_Particles.Length; i++)
                {
                    if (m_Particles[i].isPlaying)
                    {
                        isPlaying = true;
                        break;
                    }
                }
                if (!isPlaying)
                {
                    gameObject.SetActive(false);
                    StopCoroutine(Coroutine_AutoDestroy());
                }
            }
        }
        
    }
    #endregion
}
