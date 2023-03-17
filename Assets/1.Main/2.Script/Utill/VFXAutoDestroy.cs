using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAutoDestroy : MonoBehaviour
{
    [SerializeField]
    float m_lifeTime = 0f;
    float m_time;
    ParticleSystem[] m_Particles;
    // Start is called before the first frame update
    void OnEnable()
    {
        m_time= Time.time;
    }
    void Start()
    {
       m_Particles = GetComponentsInChildren<ParticleSystem>();   
    }

    
    // Update is called once per frame
    void Update()
    {
        if(m_lifeTime > 0)
        {
            if(m_time + m_lifeTime < Time.time)
            {
                gameObject.SetActive(false);
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
            }
        }
       

    }
}
