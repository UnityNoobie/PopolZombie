using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BuildableObject : MonoBehaviour, IDamageAbleObject
{
    #region Constants and Fields

    protected GameObject m_target;
    protected GameObject m_destroyd;
    protected AudioSource m_audio;
    protected DamageAbleObjectHUD m_hud;
    protected Transform m_hudPos;
    protected AreaChecker m_attackArea;
    protected ObjectStat m_stat;
    protected ObjectStat m_baseStat;
    protected TableSkillStat m_skill;
    [SerializeField]
    protected List<MonsterController> m_targetList = new List<MonsterController>(); //���� ������ Ÿ�� ����Ʈ

    protected bool m_isReflect =false;
    protected int m_killCount = 0;
    protected float m_machineLearningDamage;
    protected int m_healCool = 2;
    #endregion

    #region Coroutine
    protected IEnumerator Coroutine_SelfRecovery(float regen) //�ڷ�ƾ�� ���� ������ ����.
    {
        while (true)
        {
            RestoreHP(regen);
            yield return new WaitForSeconds(m_healCool);
        }
    }
    protected Coroutine CoroutineRecovery; //��� �����ų �ڷ�ƾ
    #endregion

    #region Methods

    public void KillCount() //�������� óġ�� ���� ���� ī��Ʈ.
    {
        m_killCount++;
        float damage = 0;
        float armorPierce = 0;
        int killdamage = m_killCount;
        m_hud.SetKillCount(m_killCount);
        //�ӽŷ��� Ư���� Ȱ��ȭ �Ǿ����� ��츦 ����ؼ� �Ʒ��� ���� �����Ͽ���.
        if (killdamage <= (m_skill.MaxMachineLearning)) //ų�Ҷ����� �������ͽ� ��ü ������ ��ȿ�����̶� �����ؼ� �ش� ���ȸ� �����ϴ� ������.
        {
            armorPierce = m_baseStat.ArmorPierce + m_skill.TurretArmorPierce + m_skill.BuffArmorPierce + killdamage;
            damage = m_baseStat.Damage * ((1 + m_skill.TurretDamage + m_skill.publicBuffDamage + (killdamage / 100)) * (1 + m_skill.CyberWear));
            m_stat.Damage = damage;
            m_stat.ArmorPierce = armorPierce;
        }
    }
    protected bool HasTarget()//���� ������ Ÿ�� ����Ʈ�� �ִ��� Ȯ�����ִ� �޼ҵ�.
    {
        if (m_targetList.Count > 0)
        {
            List<MonsterController> m_activeTargets = new List<MonsterController>();
            foreach (MonsterController go in m_targetList)//����Ʈ ���� ǥ���� �׾��� ��츦 �����Ͽ� üũ����.
            {
                if (go.IsAliveObject()) //Ÿ���� ��� �ִٸ� activeTargetList�� �߰����־� targetList�� ����.
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
    protected void FindNearTarget() // Ÿ�� ����Ʈ�� �ִ� ���� �� ���� ����� Ÿ�� Ž���Ͽ� ����.
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
    public virtual void InitStatus(TableSkillStat skill, ObjectStat stat) //�������ͽ� ����.
    {
        float hpvalue = 1;
        m_skill = skill;
        m_isReflect = false;
        m_baseStat = stat;
        if (m_stat != null) //�̹� ��ġ�Ǿ��ִٸ� hp���� ���� �������� ����.
        {
            hpvalue = m_stat.HP / m_stat.MaxHP;
        }
        if(stat.Objecttype == ObjectType.Generator) //������Ʈ�� Ÿ�Կ� ���� ���� ���� ��� �ٸ���
        {
            m_stat = stat; //�������� ��� �÷��̾��� Ư���� �ƴ� ���� ���׷��̵带 ���� ���� ���׷��̵带 �ϱ� ������ �⺻ ��������.
        }
        else if(stat.Objecttype == ObjectType.Barricade)
        {
            m_stat = new ObjectStat(m_baseStat.ID, m_baseStat.Objecttype, ((m_baseStat.HP + m_skill.BonusHP) * ((1 + m_skill.ObjectHP + m_skill.BarricadeHP) * (1 + m_skill.CyberWear))) * hpvalue, (m_baseStat.HP + m_skill.BonusHP) * ((1 + m_skill.ObjectHP + m_skill.BarricadeHP) * (1 + m_skill.CyberWear)), m_baseStat.Defence + (m_skill.ObjectDefence + m_skill.BarricadeDefence) * (1 + m_skill.CyberWear), m_baseStat.DamageRigist + ((m_skill.ObjectRigist + m_skill.BarricadeRigist) * (1 + m_skill.CyberWear)), m_baseStat.Damage, m_baseStat.FireRate, m_baseStat.Range, m_baseStat.CriRate, m_baseStat.CriDamage, m_baseStat.ArmorPierce, m_baseStat.DamageReflect * (1 + m_skill.CyberWear), m_baseStat.MaxBuild + m_skill.BarricadeMaxBuild, m_baseStat.Info,(m_skill.ObjectRegen + m_skill.BarricadeRegen) * (1+m_skill.CyberWear));
            if(m_stat.DamageReflect > 0)
            {
                m_isReflect = true; //�ٸ����̵� �ݻ絥���� �����
            }
        }
        else if(stat.Objecttype == ObjectType.Turret) //��ž Ư�� �� �ӽŷ����̶�� Ư���� �ֱ� ������ ���� �ٸ��� ����
        {
            float damage = 0; 
            float armorPierce = 0;
            int killdamage = m_killCount;
            if(m_skill.MaxMachineLearning > 0)
            {
                if(killdamage > skill.MaxMachineLearning) //�ִ� ��ø���ϰ�� ���缭~
                {
                    killdamage = skill.MaxMachineLearning;
                }
                armorPierce = m_baseStat.ArmorPierce + m_skill.TurretArmorPierce + m_skill.BuffArmorPierce + killdamage;
                damage = m_baseStat.Damage * ((1 + m_skill.TurretDamage + m_skill.publicBuffDamage + (killdamage /100)) * (1 + m_skill.CyberWear));
            }
            else
            {
                armorPierce = m_baseStat.ArmorPierce + m_skill.TurretArmorPierce + m_skill.BuffArmorPierce;
                damage = m_baseStat.Damage * ((1 + m_skill.TurretDamage + m_skill.publicBuffDamage) * (1 + m_skill.CyberWear));
            }
            m_stat = new ObjectStat(m_baseStat.ID, m_baseStat.Objecttype, (m_baseStat.HP * ((1 + m_skill.ObjectHP + m_skill.TurretHP) * (1 + m_skill.CyberWear))) * hpvalue, m_baseStat.HP * ((1 + m_skill.ObjectHP + m_skill.TurretHP) * (1 + m_skill.CyberWear)), m_baseStat.Defence + (m_skill.ObjectDefence * (1 + m_skill.CyberWear)), m_baseStat.DamageRigist + ((m_skill.ObjectRigist + m_skill.TurretRigist) * (1 + m_skill.CyberWear)), damage, m_baseStat.FireRate * ((1+m_skill.TurretAttackSpeed) * (1 + m_skill.CyberWear)), m_baseStat.Range * (1 +  m_skill.TurretRange), m_baseStat.CriRate, m_baseStat.CriDamage,armorPierce, m_baseStat.DamageReflect, m_baseStat.MaxBuild +m_skill.TurretMaxBuild, m_baseStat.Info, m_skill.ObjectRegen * (1 + m_skill.CyberWear));
        } 
        if(m_stat.Regen > 0) //������Ʈ�� �������� ���� ��� �ڵ� ���� �ڷ�ƾ ����.
        {
            if(CoroutineRecovery!=null)
               StopCoroutine(CoroutineRecovery);
            CoroutineRecovery = StartCoroutine(Coroutine_SelfRecovery(m_stat.Regen));
        }
    }
    public virtual void SetDamage(float damage, MonsterController mon) //�ǰ� ��� �޼ҵ�.
    {
        if (m_isReflect) //������� Ư���� ����Ǿ� ���� ��
        {
            float reflectDamage = (damage * m_stat.DamageReflect) / 100;
            mon.SetDamage(AttackType.Normal, reflectDamage,null,false,this);
        }
        float attack = CalculationDamage.NormalDamage(damage, m_stat.Defence, 0f, m_stat.DamageRigist);
        m_stat.HP -= attack;
        m_hud.DisplayDamage(-attack, m_stat.HP, m_stat.MaxHP);
        if (m_stat.HP <= 0)
        {
            Destroyed();
        }
    }
    protected virtual void DestroyGameObject() //Ȱ��ȭ ���� �� Ǯ�� �ٽ� �־��ֱ�
    {
        m_destroyd.SetActive(false);
        gameObject.SetActive(false);
    }
    protected virtual void Destroyed() //������Ʈ �ı�.
    {
        m_destroyd.SetActive(true);
        GameManager.Instance.DestroyTarget(gameObject); //���� ������ ��Ͽ��� ���ӿ�����Ʈ ����ó��
    }
    public virtual void RestoreHP(float heal) //����ȸ�� ���.
    {
        float healvalue = (m_stat.MaxHP * heal) / 100;
        m_stat.HP += healvalue;
        if (m_stat.HP >= m_stat.MaxHP)
        {
            m_stat.HP = m_stat.MaxHP;
        }
        if(m_stat.HP < m_stat.MaxHP) //�ǰ� �ִ�ü�º��� ���� ������ HUD ȣ��
        {
            m_hud.DisplayDamage(healvalue, m_stat.HP, m_stat.MaxHP);
        }
        
    }
   
    public virtual void SetTransform()
    {
        m_destroyd = Utill.GetChildObject(gameObject, "DestroyEffect").gameObject;
        m_hudPos = Utill.GetChildObject(gameObject, "HudPos");
        m_audio = GetComponent<AudioSource>();
        m_hud = ObjectManager.Instance.GetHudObject();
        m_hud.SetTransform(m_hudPos);
    }
    #endregion
}
