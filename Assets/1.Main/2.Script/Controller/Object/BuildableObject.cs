using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    protected ObjectStat m_baseStat;
    protected TableSkillStat m_skill;
    [SerializeField]
    protected List<MonsterController> m_targetList = new List<MonsterController>(); //공격 가능한 타겟 리스트

    protected bool m_isReflect =false;
    protected int m_killCount = 0;
    protected float m_machineLearningDamage;
    #endregion

    #region Coroutine
    protected IEnumerator Coroutine_SelfRecovery(float regen)
    {
        while (true)
        {
            RestoreHP(regen);
            yield return new WaitForSeconds(2);
        }
    }
    protected Coroutine CoroutineRecovery;
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
    public void KillCount()
    {
        m_killCount++;
        float damage = 0;
        float armorPierce = 0;
        int killdamage = m_killCount;
        if (killdamage <= (m_skill.MaxMachineLearning))
        {
            armorPierce = m_baseStat.ArmorPierce + m_skill.TurretArmorPierce + m_skill.BuffArmorPierce + killdamage;
            damage = m_baseStat.Damage * ((1 + m_skill.TurretDamage + m_skill.publicBuffDamage + (killdamage / 100)) * (1 + m_skill.CyberWear));
            Debug.Log(m_killCount + " 사냥한 적, 추가 데미지 :  " + damage + "추가 방어구관통" + armorPierce + "기존 데미지 : " + m_stat.Damage + " 기존방관 : " + m_stat.ArmorPierce);
            m_stat.Damage = damage;
            m_stat.ArmorPierce = armorPierce;
        }
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
        m_isReflect = false;
        m_baseStat = stat;
        if (m_stat != null) //이미 설치되어있다면 벨류값을 통해 현재 hp 조절
        {
            hpvalue = m_stat.HP / m_stat.MaxHP;
        }
        if(stat.Objecttype == ObjectType.Generator) //오브젝트의 타입에 따라 스탯 적용 방식 다르게
        {
            m_stat = stat;
        }
        else if(stat.Objecttype == ObjectType.Barricade)
        {
            m_stat = new ObjectStat(m_baseStat.ID, m_baseStat.Objecttype, ((m_baseStat.HP + m_skill.BonusHP) * ((1 + m_skill.ObjectHP + m_skill.BarricadeHP) * (1 + m_skill.CyberWear))) * hpvalue, (m_baseStat.HP + m_skill.BonusHP) * ((1 + m_skill.ObjectHP + m_skill.BarricadeHP) * (1 + m_skill.CyberWear)), m_baseStat.Defence + (m_skill.ObjectDefence + m_skill.BarricadeDefence) * (1 + m_skill.CyberWear), m_baseStat.DamageRigist + ((m_skill.ObjectRigist + m_skill.BarricadeRigist) * (1 + m_skill.CyberWear)), m_baseStat.Damage, m_baseStat.FireRate, m_baseStat.Range, m_baseStat.CriRate, m_baseStat.CriDamage, m_baseStat.ArmorPierce, m_baseStat.DamageReflect * (1 + m_skill.CyberWear), m_baseStat.MaxBuild + m_skill.BarricadeMaxBuild, m_baseStat.Info,(m_skill.ObjectRegen + m_skill.BarricadeRegen) * (1+m_skill.CyberWear));
            if(m_stat.DamageReflect > 0)
            {
                m_isReflect = true; //바리케이드 반사데미지 적용용
            }
        }
        else if(stat.Objecttype == ObjectType.Turret) //포탑 특성 중 머신러닝이라는 특성이 있기 때문에 스탯 다르게 적용
        {
            float damage = 0; 
            float armorPierce = 0;
            int killdamage = m_killCount;
            if(m_skill.MaxMachineLearning > 0)
            {
                if(killdamage > skill.MaxMachineLearning) //최대 중첩수일경우 낮춰서~
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
            Debug.Log("킬카운트 : " + m_killCount + " 데미지 보너스 : " + killdamage);
            m_stat = new ObjectStat(m_baseStat.ID, m_baseStat.Objecttype, (m_baseStat.HP * ((1 + m_skill.ObjectHP + m_skill.TurretHP) * (1 + m_skill.CyberWear))) * hpvalue, m_baseStat.HP * ((1 + m_skill.ObjectHP + m_skill.TurretHP) * (1 + m_skill.CyberWear)), m_baseStat.Defence + (m_skill.ObjectDefence * (1 + m_skill.CyberWear)), m_baseStat.DamageRigist + ((m_skill.ObjectRigist + m_skill.TurretRigist) * (1 + m_skill.CyberWear)), damage, m_baseStat.FireRate * ((1+m_skill.TurretAttackSpeed) * (1 + m_skill.CyberWear)), m_baseStat.Range * (1 +  m_skill.TurretRange), m_baseStat.CriRate, m_baseStat.CriDamage,armorPierce, m_baseStat.DamageReflect, m_baseStat.MaxBuild +m_skill.TurretMaxBuild, m_baseStat.Info, m_skill.ObjectRegen * (1 + m_skill.CyberWear));
        } 
        if(m_stat.Regen > 0)
        {
            if(CoroutineRecovery!=null)
               StopCoroutine(CoroutineRecovery);
            CoroutineRecovery = StartCoroutine(Coroutine_SelfRecovery(m_stat.Regen));
        }
    }
    public virtual void SetDamage(float damage, MonsterController mon)
    {
        if (m_isReflect) //반사 효과가 적용되어 있을 때
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
        float healvalue = (m_stat.MaxHP * heal) / 100;
        m_stat.HP += healvalue;
        if (m_stat.HP >= m_stat.MaxHP)
        {
            m_stat.HP = m_stat.MaxHP;
        }
        m_hud.DisplayDamage(healvalue, m_stat.HP, m_stat.MaxHP);
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
