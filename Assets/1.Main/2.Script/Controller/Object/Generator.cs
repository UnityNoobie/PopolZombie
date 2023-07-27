using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : BuildableObject
{
    #region Coroutine

    #endregion
    public override void SetDamage(float damage)
    {
        UGUIManager.Instance.SystemMessageSendMessage("발전기가 공격받고 있습니다!!");
        SoundManager.Instance.PlaySFX("SFX_Generator", m_audio);
        base.SetDamage(damage);
    }
    protected override void Destroyed()
    {
        base.Destroyed();
        StopAllCoroutines();   
        GameManager.Instance.GameOver();
    }
    void SetObject()
    {
        SetTransform();
        m_stat = ObjectManager.Instance.GetObjectStat(ObjectType.Generator);
        InitStatus(null, m_stat);
        GameManager.Instance.SetGameObject(gameObject);
        
    }
    private void Start()
    {
        SetObject();
    }
}
