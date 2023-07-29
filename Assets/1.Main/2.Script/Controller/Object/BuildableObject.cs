using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableObject : MonoBehaviour, IDamageAbleObject
{
    #region Constants and Fields

    [SerializeField]
    protected float m_barrelSpeed;
    [SerializeField]
    protected float m_rotationSpeed;

    protected float lastFireTime;
    protected float m_armorPierce = 0;


    protected PlayerSkillController m_player;
    protected GameObject m_target;
    protected GameObject m_destroyd;
    protected AudioSource m_audio;
    protected DamageAbleObjectHUD m_hud;
    protected Transform m_hudPos;
    protected AreaChecker m_attackArea;
    protected ObjectStat m_stat;
    protected TableSkillStat m_skill;
    [SerializeField]
    protected List<MonsterController> m_targetList = new List<MonsterController>(); //공격 가능한 타겟 리스트
    #endregion

    #region Coroutine
    IEnumerator Coroutine_SelfRecovery()
    {
        while (true)
        {
            RestoreHP(m_stat.Regen);
            yield return new WaitForSeconds(2);
        }
    }
    #endregion
    #region Methods

    public virtual float GetMaxHP()
    {
        return m_stat.MaxHP;
    }
    public virtual float GetDefence()
    {
        return m_stat.Defence;
    }
    public void SetPlayer(PlayerSkillController player)
    {
        m_player = player;
    }
    protected bool HasTarget()
    {
        if (m_targetList.Count > 0)
        {
            List<MonsterController> m_activeTargets = new List<MonsterController>();
            foreach (MonsterController go in m_targetList)//리스트 안의 표적이 죽었을 경우를 생각하여 체크
            {
                if (go.IsAliveObject())
                {
                    m_activeTargets.Add(go);
                }
            }
            m_targetList = m_activeTargets;
        }
        if (m_targetList.Count > 0)//리스트에 적이 남아 있으면 트루
        {
            return true;
        }
        return false; //아니면 false
    }
    protected void FindNearTarget() //가장 가까운 타겟 탐색
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
    public virtual void InitStatus(TableSkillStat skill, ObjectStat stat)
    {
        float hpvalue = 1;
        m_skill = skill;
        if(m_stat != null) //이미 설치되어있다면 벨류값을 통해 현재 hp 조절
        {
            hpvalue = m_stat.HP / m_stat.MaxHP;
        }
        if(stat.Objecttype == ObjectType.Generator) //오브젝트의 타입에 따라 스탯 적용 방식 다르게
        {
            m_stat = stat;
        }
        else if(stat.Objecttype == ObjectType.Barricade)
        {
            m_stat = new ObjectStat(stat.ID, stat.Objecttype, ((stat.HP + skill.BonusHP) * ((1 + skill.ObjectHP + skill.BarricadeHP) * (1 + skill.CyberWear))) * hpvalue, (stat.HP + skill.BonusHP) * ((1 + skill.ObjectHP + skill.BarricadeHP) * (1 + skill.CyberWear)), stat.Defence + (skill.ObjectDefence + skill.BarricadeDefence) * (1 + skill.CyberWear), stat.DamageRigist + ((skill.ObjectRigist + skill.BarricadeRigist) * (1 + skill.CyberWear)), stat.Damage, stat.FireRate, stat.Range, stat.CriRate, stat.CriDamage, stat.ArmorPierce, stat.DamageReflect * (1 + skill.CyberWear), stat.MaxBuild + skill.BarricadeMaxBuild, stat.Info,(m_skill.ObjectRegen + m_skill.BarricadeRegen) * (1+m_skill.CyberWear));

        }
        else if(stat.Objecttype == ObjectType.Turret)
        {
            m_stat = new ObjectStat(stat.ID, stat.Objecttype, (stat.HP * ((1 + skill.ObjectHP + skill.TurretHP) * (1 + skill.CyberWear))) * hpvalue, stat.HP * ((1 + skill.ObjectHP + skill.TurretHP) * (1 + skill.CyberWear)), stat.Defence + (skill.ObjectDefence * (1 + skill.CyberWear)), stat.DamageRigist + ((skill.ObjectRigist + skill.TurretRigist) * (1 + skill.CyberWear)), stat.Damage * ((1+skill.TurretDamage + skill.publicBuffDamage) * (1 + skill.CyberWear)), stat.FireRate * ((1+skill.TurretAttackSpeed) * (1 + skill.CyberWear)), stat.Range * (1 +  skill.TurretRange), stat.CriRate, stat.CriDamage, stat.ArmorPierce + skill.TurretArmorPierce + skill.BuffArmorPierce, stat.DamageReflect, stat.MaxBuild + skill.TurretMaxBuild, stat.Info, m_skill.ObjectRegen * (1 + m_skill.CyberWear));
        } 
        if(m_stat.Regen > 0)
        {
            StopCoroutine("Coroutine_SelfRecovery");
            StartCoroutine("Coroutine_SelfRecovery");
        }
    }
    public virtual void SetDamage(float damage)
    {
        float attack = CalculationDamage.NormalDamage(damage, m_stat.Defence, 0f, m_stat.DamageRigist);
        m_stat.HP -= attack;
        m_hud.DisplayDamage(-attack, m_stat.HP, m_stat.MaxHP);
        if (m_stat.HP <= 0)
        {
            Destroyed();
        }
    }
    protected virtual void DestroyGameObject() //활성화 종료 후 풀에 다시 넣어주기
    {
        m_destroyd.SetActive(false);
        gameObject.SetActive(false);
    }
    protected virtual void Destroyed()
    {
        m_destroyd.SetActive(true);
        GameManager.Instance.DestroyTarget(gameObject); //공격 가능한 목록에서 게임오브젝트 삭제처리

    }
    public virtual void RestoreHP(float heal) //피해회복
    {
        m_stat.HP += (m_stat.MaxHP * heal) / 100;
        if (m_stat.HP >= m_stat.MaxHP)
        {
            m_stat.HP = m_stat.MaxHP;
        }
    }
    public virtual void IncreaseMaxHp(int hp) //최대체력 증가
    {
        float value = m_stat.HP / m_stat.MaxHP;
        m_stat.MaxHP += hp;
        m_stat.HP = Mathf.CeilToInt(m_stat.MaxHP * value);
    }
    public virtual void IncreaseDefence(int defence)
    {
        m_stat.Defence += defence;
    }
    public virtual void SetTransform()
    {
        m_destroyd = Utill.GetChildObject(gameObject, "DestroyEffect").gameObject;
        m_hudPos = Utill.GetChildObject(gameObject, "HudPos");
        m_audio = GetComponent<AudioSource>();
        m_hud = ObjectManager.Instance.GetHud();
        m_hud.SetTransform(m_hudPos);
    }
    #endregion
}
