using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableObject : MonoBehaviour, IDamageAbleObject
{
    #region Constants and Fields
    [SerializeField]
    protected int m_maxHP;
    [SerializeField]
    protected int m_hp;
    [SerializeField]
    protected int m_defence;
    [SerializeField]
    protected float m_damage;
    [SerializeField]
    protected float m_criRate;
    [SerializeField]
    protected float m_criDamage;
    [SerializeField]
    protected float m_fireRate;
    [SerializeField]
    protected float m_barrelSpeed;
    [SerializeField]
    protected float m_rotationSpeed;
    [SerializeField]
    protected float m_fireRange;
    protected float lastFireTime;
    protected float m_armorPierce = 0;

    protected GameObject m_target;
    protected GameObject m_destroyd;
    protected AudioSource m_audio;
    protected DamageAbleObjectHUD m_hud;
    protected Transform m_hudPos;
    protected AreaChecker m_attackArea;

    [SerializeField]
    protected List<MonsterController> m_targetList = new List<MonsterController>(); //���� ������ Ÿ�� ����Ʈ
    #endregion

    #region Methods

    public virtual int GetMaxHP()
    {
        return m_maxHP;
    }
    public virtual int GetDefence()
    {
        return m_defence;
    }
    protected bool HasTarget()
    {
        if (m_targetList.Count > 0)
        {
            List<MonsterController> m_activeTargets = new List<MonsterController>();
            foreach (MonsterController go in m_targetList)//����Ʈ ���� ǥ���� �׾��� ��츦 �����Ͽ� üũ
            {
                if (go.IsAliveObject())
                {
                    m_activeTargets.Add(go);
                }
            }
            m_targetList = m_activeTargets;
        }
        if (m_targetList.Count > 0)//����Ʈ�� ���� ���� ������ Ʈ��
        {
            return true;
        }
        return false; //�ƴϸ� false
    }
    protected void FindNearTarget() //���� ����� Ÿ�� Ž��
    {
        MonsterController closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (MonsterController target in m_targetList)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }
        m_target = closestTarget.gameObject;
    }
    public virtual void SetTower(int hp,float damage,float defence, float fireRate, float range,float crirate, float cridam,float armorpierce)
    {
        m_maxHP = m_hp = hp;
        m_damage = damage;
        m_fireRate = fireRate;
        m_fireRange = range;
        m_criDamage = cridam;
        m_criRate = crirate;
        m_armorPierce = armorpierce;
    }
    public virtual void SetDamage(float damage)
    {
        float attack = CalculationDamage.NormalDamage(damage, m_defence, 0f);
        m_hp -= Mathf.CeilToInt(attack);
        m_hud.DisplayDamage(-damage, m_hp, m_maxHP);
        if (m_hp <= 0)
        {
            Destroyed();
        }
    }
    protected virtual void DestroyGameObject() //Ȱ��ȭ ���� �� Ǯ�� �ٽ� �־��ֱ�
    {
        m_destroyd.SetActive(false);
        gameObject.SetActive(false);
    }
    protected virtual void Destroyed()
    {
        m_destroyd.SetActive(true);
        GameManager.Instance.DestroyTarget(gameObject); //���� ������ ��Ͽ��� ���ӿ�����Ʈ ����ó��

    }
    public virtual void RestoreHP(int heal) //����ȸ��
    {
        m_hp += (m_maxHP * heal) / 100;
        if (m_hp >= m_maxHP)
        {
            m_hp = m_maxHP;
        }
    }
    public virtual void IncreaseMaxHp(int hp) //�ִ�ü�� ����
    {
        float value = m_hp / m_maxHP;
        m_maxHP = hp;
        m_hp = Mathf.CeilToInt(m_maxHP * value);
    }
    public virtual void IncreaseDefence(int defence)
    {
        m_defence += defence;
    }
    public virtual void SetTransform()
    {
        m_destroyd = Utill.GetChildObject(gameObject, "DestroyEffect").gameObject;
        m_hudPos = Utill.GetChildObject(gameObject, "HudPos");
        m_audio = GetComponent<AudioSource>();
        m_hud = ObjectManager.Instance.GetHud();
        m_hud.SetTransform(m_hudPos);
    }
    public virtual void SetObject(int hp, int def)
    {
        m_maxHP = m_hp = hp;
        m_defence = def;
        GameManager.Instance.SetGameObject(gameObject);
    }
    #endregion
}
