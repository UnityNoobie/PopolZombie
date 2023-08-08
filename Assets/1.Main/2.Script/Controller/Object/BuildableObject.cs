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
    protected List<MonsterController> m_targetList = new List<MonsterController>(); //공격 가능한 타겟 리스트

    protected bool m_isReflect =false;
    protected int m_killCount = 0;
    protected float m_machineLearningDamage;
    protected int m_healCool = 2;
    #endregion

    #region Coroutine
    protected IEnumerator Coroutine_SelfRecovery(float regen) //코루틴을 통해 지속힐 구현.
    {
        while (true)
        {
            RestoreHP(regen);
            yield return new WaitForSeconds(m_healCool);
        }
    }
    protected Coroutine CoroutineRecovery; //대신 실행시킬 코루틴
    #endregion

    #region Methods

    public void KillCount() //공격으로 처치한 적의 수를 카운트.
    {
        m_killCount++;
        float damage = 0;
        float armorPierce = 0;
        int killdamage = m_killCount;
        m_hud.SetKillCount(m_killCount);
        //머신러닝 특성이 활성화 되어있을 경우를 고려해서 아래와 같이 설계하였음.
        if (killdamage <= (m_skill.MaxMachineLearning)) //킬할때마다 스테이터스 전체 수정은 비효율적이라 생각해서 해당 스탯만 변경하는 식으로.
        {
            armorPierce = m_baseStat.ArmorPierce + m_skill.TurretArmorPierce + m_skill.BuffArmorPierce + killdamage;
            damage = m_baseStat.Damage * ((1 + m_skill.TurretDamage + m_skill.publicBuffDamage + (killdamage / 100)) * (1 + m_skill.CyberWear));
            m_stat.Damage = damage;
            m_stat.ArmorPierce = armorPierce;
        }
    }
    protected bool HasTarget()//공격 가능한 타겟 리스트가 있는지 확인해주는 메소드.
    {
        if (m_targetList.Count > 0)
        {
            List<MonsterController> m_activeTargets = new List<MonsterController>();
            foreach (MonsterController go in m_targetList)//리스트 안의 표적이 죽었을 경우를 생각하여 체크해줌.
            {
                if (go.IsAliveObject()) //타겟이 살아 있다면 activeTargetList에 추가해주어 targetList를 변경.
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
    protected void FindNearTarget() // 타겟 리스트에 있는 적들 중 가장 가까운 타겟 탐색하여 전달.
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
    public virtual void InitStatus(TableSkillStat skill, ObjectStat stat) //스테이터스 설정.
    {
        float hpvalue = 1;
        m_skill = skill;
        m_isReflect = false;
        m_baseStat = stat;
        if (m_stat != null) //이미 설치되어있다면 hp값을 통해 벨류값을 지정.
        {
            hpvalue = m_stat.HP / m_stat.MaxHP;
        }
        if(stat.Objecttype == ObjectType.Generator) //오브젝트의 타입에 따라 스탯 적용 방식 다르게
        {
            m_stat = stat; //발전기의 경우 플레이어의 특성이 아닌 상점 업그레이드를 통해 성능 업그레이드를 하기 때문에 기본 성능으로.
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
            m_stat = new ObjectStat(m_baseStat.ID, m_baseStat.Objecttype, (m_baseStat.HP * ((1 + m_skill.ObjectHP + m_skill.TurretHP) * (1 + m_skill.CyberWear))) * hpvalue, m_baseStat.HP * ((1 + m_skill.ObjectHP + m_skill.TurretHP) * (1 + m_skill.CyberWear)), m_baseStat.Defence + (m_skill.ObjectDefence * (1 + m_skill.CyberWear)), m_baseStat.DamageRigist + ((m_skill.ObjectRigist + m_skill.TurretRigist) * (1 + m_skill.CyberWear)), damage, m_baseStat.FireRate * ((1+m_skill.TurretAttackSpeed) * (1 + m_skill.CyberWear)), m_baseStat.Range * (1 +  m_skill.TurretRange), m_baseStat.CriRate, m_baseStat.CriDamage,armorPierce, m_baseStat.DamageReflect, m_baseStat.MaxBuild +m_skill.TurretMaxBuild, m_baseStat.Info, m_skill.ObjectRegen * (1 + m_skill.CyberWear));
        } 
        if(m_stat.Regen > 0) //오브젝트의 리젠값이 높을 경우 자동 수리 코루틴 실행.
        {
            if(CoroutineRecovery!=null)
               StopCoroutine(CoroutineRecovery);
            CoroutineRecovery = StartCoroutine(Coroutine_SelfRecovery(m_stat.Regen));
        }
    }
    public virtual void SetDamage(float damage, MonsterController mon) //피격 담당 메소드.
    {
        if (m_isReflect) //전기담장 특성이 적용되어 있을 때
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
    protected virtual void Destroyed() //오브젝트 파괴.
    {
        m_destroyd.SetActive(true);
        GameManager.Instance.DestroyTarget(gameObject); //공격 가능한 목록에서 게임오브젝트 삭제처리
    }
    public virtual void RestoreHP(float heal) //피해회복 기능.
    {
        float healvalue = (m_stat.MaxHP * heal) / 100;
        m_stat.HP += healvalue;
        if (m_stat.HP >= m_stat.MaxHP)
        {
            m_stat.HP = m_stat.MaxHP;
        }
        if(m_stat.HP < m_stat.MaxHP) //피가 최대체력보다 낮을 때에만 HUD 호출
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
