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
    public override void RestoreHP(int heal) //피해회복
    {
        m_hp += (m_maxHP * heal) / 100;
        if(m_hp >= m_maxHP)
        {
            m_hp = m_maxHP;
        }
    }
    public override void IncreaseMaxHp(int hp) //최대체력 증가
    {
        float value = m_hp / m_maxHP;
        m_maxHP += hp;
        m_hp = Mathf.CeilToInt(m_maxHP * value);
    }
    public override void IncreaseDefence(int defence)
    {
        m_defence += defence;
    }
    public override void SetObject(int hp, int def)
    {
        SetTransform();
        base.SetObject(hp, def);
    }
    private void Start()
    {
        SetObject(1000, 30);
    }
}
