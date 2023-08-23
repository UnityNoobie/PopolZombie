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
    void OnEnable() //켜졌을 때 시간 설정해주고 코루틴 실행
    {
        m_time= Time.time;
        StartCoroutine(Coroutine_AutoDestroy());
    }
    void Start()
    {
       m_Particles = GetComponentsInChildren<ParticleSystem>();   
    }   
    // Update is called once per frame

    IEnumerator Coroutine_AutoDestroy() //자동으로 이펙트를 삭제해주는 코루틴
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
