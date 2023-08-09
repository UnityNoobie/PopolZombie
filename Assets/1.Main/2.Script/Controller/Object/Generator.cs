using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : BuildableObject
{
    #region Constants and Fields
    int m_upgreadeHP = 0;
    int m_upgradeDefence = 0;
    int m_upgradeRegen = 0;
    const int m_maxUpgrade = 5;
    float timeAfterWarning = 0f;
    float cooltime = 3f;
    const string m_underAttack = "발전기가 공격받고 있습니다!!";
    const string m_generatorDamaged = "SFX_Generator";
    #endregion

    #region Methods
    public override void SetDamage(float damage, MonsterController mon) //피해 입을 때 호출.
    {
        if(Time.time > cooltime + timeAfterWarning) //너무 중복되어 호출되니 정신이 없어서 쿨타임 있도록 수정.
        {
            timeAfterWarning = Time.time;
            UGUIManager.Instance.SystemMessageSendMessage(m_underAttack);
        }
        SoundManager.Instance.PlaySFX(m_generatorDamaged, m_audio);
        base.SetDamage(damage, mon);
    }
    protected override void Destroyed() //파괴되었을 때.
    {
        base.Destroyed(); // base 메소드 실행
        StopAllCoroutines();   //회복 등 진행중인 코루틴 모두 중지
        GameManager.Instance.GameOver(); //발전기가 파괴된 경우니 게임오버상태로 변경
    }
    public bool IsCanUpgrade(int id) //현재 최대 업그레이드 수치를 각각 5로 잡았기 때문에  maxUpgrade보다 현재 업그레이드가 적을 시 true반환
    {
        if(id == 38)
        {
            if(m_upgreadeHP < m_maxUpgrade)
            {
                return true;
            }
        }
        else if (id== 39)
        {
            if(m_upgradeDefence < m_maxUpgrade)
            {
                return true;
            }
        }
        else if(id == 40)
        {
            if(m_upgradeRegen < m_maxUpgrade)
            {
                return true;
            }
        }
        return false;
    }
    public void IncreaseMaxHp(int hp) //최대체력 증가
    {
        m_upgreadeHP++;
        if (m_upgreadeHP > m_maxUpgrade) return;
        float value = m_stat.HP / m_stat.MaxHP;
        m_stat.MaxHP += hp;
        m_stat.HP = Mathf.CeilToInt(m_stat.MaxHP * value);
    }
    public void IncreaseDefence(int defence) //방어력 증가
    {
        m_upgradeDefence++;
        if (m_upgradeDefence > m_maxUpgrade) return;
        m_stat.Defence += defence;
    }
    public void IncreaseHPRegen(float regen) //체력 리젠 증가
    {
        m_upgradeRegen++;
        if (m_upgradeRegen > m_maxUpgrade) return;
        m_stat.Regen += regen;
        if(CoroutineRecovery != null)
        {
            StopCoroutine(CoroutineRecovery);
        }
        CoroutineRecovery =  StartCoroutine(Coroutine_SelfRecovery(m_stat.Regen));
    }
    void SetObject() //오브젝트 세팅
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
    #endregion
}
