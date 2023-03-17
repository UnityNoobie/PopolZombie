using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EffectPoolUnit : MonoBehaviour
{
    string m_effectName;
    float m_inactiveTime;
    float m_delay = 1f;
    public bool IsReady
    {
        get
        {
            if (!gameObject.activeSelf)
            {
                if (Time.time > m_inactiveTime + m_delay)
                    return true;
            }
            return false;
        }
    }
    public void SetObjectPool(string effectName)
    {
        m_effectName = effectName;
        transform.SetParent(EffectPool.Instance.transform);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    void OnDisable()
    {
        m_inactiveTime = Time.time;
        EffectPool.Instance.AddPool(m_effectName, this);
    }
    private void Start()
    {

    }

}

