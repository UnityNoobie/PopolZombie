using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : BuildableObject
{
    int maxUpgreadeHP = 0;
    int maxUpgradeDefence = 0;
    int maxUpgradeRegen = 0;

    #region Coroutine

    #endregion
    public override void SetDamage(float damage, MonsterController mon)
    {
        UGUIManager.Instance.SystemMessageSendMessage("발전기가 공격받고 있습니다!!");
        SoundManager.Instance.PlaySFX("SFX_Generator", m_audio);
        base.SetDamage(damage, mon);
    }
    protected override void Destroyed()
    {
        base.Destroyed();
        StopAllCoroutines();   
        GameManager.Instance.GameOver();
    }
    public bool IsCanUpgrade(int id)
    {
        if(id == 38)
        {
            if(maxUpgreadeHP < 5)
            {
                return true;
            }
        }
        else if (id== 39)
        {
            if(maxUpgradeDefence < 5)
            {
                return true;
            }
        }
        else if(id == 40)
        {
            if(maxUpgradeRegen < 5)
            {
                return true;
            }
        }
        return false;
    }
    public void IncreaseMaxHp(int hp) //최대체력 증가
    {
        maxUpgreadeHP++;
        if (maxUpgreadeHP > 5) return;
        float value = m_stat.HP / m_stat.MaxHP;
        m_stat.MaxHP += hp;
        m_stat.HP = Mathf.CeilToInt(m_stat.MaxHP * value);
    }
    public void IncreaseDefence(int defence)
    {
        maxUpgradeDefence++;
        if (maxUpgradeDefence > 5) return;
        m_stat.Defence += defence;
    }
    public void IncreaseHPRegen(float regen)
    {
        maxUpgradeRegen++;
        if (maxUpgradeRegen > 5) return;
        m_stat.Regen += regen;
        if(CoroutineRecovery != null)
        {
            StopCoroutine(CoroutineRecovery);
        }
        CoroutineRecovery =  StartCoroutine(Coroutine_SelfRecovery(m_stat.Regen));
    }
    void SetObject()
    {
        SetTransform();
        m_stat = ObjectManager.Instance.GetObjectStat(ObjectType.Generator);
        InitStatus(null, m_stat);
        ObjectManager.Instance.SetGenerator(this);
        GameManager.Instance.SetGameObject(gameObject);
    }
    private void Start()
    {
        SetObject();
    }
}
