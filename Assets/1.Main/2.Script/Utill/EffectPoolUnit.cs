using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EffectPoolUnit : MonoBehaviour
{
    #region Constants and Fields
    string m_effectName;
    float m_inactiveTime;
    float m_delay = 1f;
    #endregion
    #region Methods
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
    public void SetObjectPool(string effectName) //����Ʈ�� ��������.
    {
        m_effectName = effectName;
        transform.SetParent(EffectPool.Instance.transform);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    void OnDisable() //����� �� Ǯ�� �־���.
    {
        m_inactiveTime = Time.time;
        EffectPool.Instance.AddPool(m_effectName, this);
    }
    #endregion
}

