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
    const string m_underAttack = "�����Ⱑ ���ݹް� �ֽ��ϴ�!!";
    const string m_generatorDamaged = "SFX_Generator";
    #endregion

    #region Methods
    public override void SetDamage(float damage, MonsterController mon) //���� ���� �� ȣ��.
    {
        if(Time.time > cooltime + timeAfterWarning) //�ʹ� �ߺ��Ǿ� ȣ��Ǵ� ������ ��� ��Ÿ�� �ֵ��� ����.
        {
            timeAfterWarning = Time.time;
            UGUIManager.Instance.SystemMessageSendMessage(m_underAttack);
        }
        SoundManager.Instance.PlaySFX(m_generatorDamaged, m_audio);
        base.SetDamage(damage, mon);
    }
    protected override void Destroyed() //�ı��Ǿ��� ��.
    {
        base.Destroyed(); // base �޼ҵ� ����
        StopAllCoroutines();   //ȸ�� �� �������� �ڷ�ƾ ��� ����
        GameManager.Instance.GameOver(); //�����Ⱑ �ı��� ���� ���ӿ������·� ����
    }
    public bool IsCanUpgrade(int id) //���� �ִ� ���׷��̵� ��ġ�� ���� 5�� ��ұ� ������  maxUpgrade���� ���� ���׷��̵尡 ���� �� true��ȯ
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
    public void IncreaseMaxHp(int hp) //�ִ�ü�� ����
    {
        m_upgreadeHP++;
        if (m_upgreadeHP > m_maxUpgrade) return;
        float value = m_stat.HP / m_stat.MaxHP;
        m_stat.MaxHP += hp;
        m_stat.HP = Mathf.CeilToInt(m_stat.MaxHP * value);
    }
    public void IncreaseDefence(int defence) //���� ����
    {
        m_upgradeDefence++;
        if (m_upgradeDefence > m_maxUpgrade) return;
        m_stat.Defence += defence;
    }
    public void IncreaseHPRegen(float regen) //ü�� ���� ����
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
    void SetObject() //������Ʈ ����
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
