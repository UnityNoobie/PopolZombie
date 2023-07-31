using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerStriker;

public class BossController : MonsterController
{
    #region Constant and Fields
    public Transform m_skill1Pos;
    Transform m_skill2Pos;
    ProjectileController m_skill;
    ProjectileController m_skill2;
    [SerializeField]
    Transform m_fireAura;
    [SerializeField]
    ProjectileController m_fire;
    bool isRage;
    float RageCool;
    public enum SkillType
    {
        Normal,
        Skill1,
        Skill2,
        Max
    }
    bool isSkill()//스킬 체커
    {
        int skillRate = Random.Range(0, 100);
        if (skillRate <= 70) return false;
        else return true;
    }
    #endregion

    #region Coroutine
    IEnumerator Coroutine_Rage()
    {
        PlayRage();
        isRage = true;
        m_state = MonsterState.Rage;
        var effectName = TableEffect.Instance.m_tableData[8].Prefab[2];
        var effect = EffectPool.Instance.Create(effectName);
        m_fire = effect.gameObject.GetComponent<ProjectileController>();
        m_fire.SetFollowProjectile(gameObject.GetComponent<MonsterController>(), 0.05f);
        m_fire.transform.position = gameObject.transform.position;
        m_skill2Pos.gameObject.SetActive(true);
        float ragespeed = m_status.speed * 1.5f;
        m_navAgent.speed = ragespeed;
        m_animFloat.SetFloat("Speed", ragespeed / 5f);
        yield return new WaitForSeconds(10f);
        m_navAgent.speed = m_status.speed;
        m_animFloat.SetFloat("Speed", m_status.speed / 5f);
        SetIdle(0.5f);
     //   m_fire.gameObject.SetActive(false);
        isRage= false;
    }
    #endregion

    #region AnimEvent
    void AnimEvent_CancleSkill()
    {
        m_skill1Pos.gameObject.SetActive(false);
        m_skill2Pos.gameObject.SetActive(false);
    }
    void AnimEvent_Skill1()    
    {
        PlaySkill1();
        m_skill2Pos.gameObject.SetActive(false);
        m_skill1Pos.gameObject.SetActive(false);
        var effectName = TableEffect.Instance.m_tableData[8].Prefab[0];
        var effect = EffectPool.Instance.Create(effectName);
        m_skill = effect.GetComponent<ProjectileController>();
        m_skill.SetProjectileWithChild(gameObject.GetComponent<BossController>(), 1.5f);
        m_skill.transform.position = gameObject.transform.position;
        m_skill.transform.forward = gameObject.transform.forward;
        m_skill.transform.localScale = new Vector3(2f, 2f, 2f);
    } 
    void AnimEvent_Skill2()
    {
        PlaySkill2();
        m_skill1Pos.gameObject.SetActive(false);
        m_skill2Pos.gameObject.SetActive(false);
        var effectName = TableEffect.Instance.m_tableData[8].Prefab[1];
        var effect = EffectPool.Instance.Create(effectName);
        m_skill2 = effect.GetComponent<ProjectileController>();
        m_skill2.SetProjectile(gameObject.GetComponent<BossController>(), 0.1f);
        m_skill2.transform.position = gameObject.transform.position;
        m_skill2.transform.localScale = new Vector3(5f, 5f, 5f);
    }
    void AnimEvent_Rage()
    {
        RageCool = 0f;
        m_skill1Pos.gameObject.SetActive(false);
        StartCoroutine(Coroutine_Rage());
    }
    protected override void AnimEvent_SetDie()
    {
        isRage = false;
        RageCool = 15f;
        base.AnimEvent_SetDie();
    }
    protected override void SetDie()
    {
        StopSkill();
        base.SetDie();
    }
    void StopSkill()
    {
        m_skill1Pos.gameObject.SetActive(false);
        m_skill2Pos.gameObject.SetActive(false) ;
    }
    #endregion

    #region SFXPlay
    public override void PlayHitSound(string sound) //피격시 소리 재생
    {
        SoundManager.Instance.PlaySFX(sound, m_source);
        SoundManager.Instance.PlaySFX("SFX_BossHit", m_source);
    }
    protected override void PlayAtkSound()
    {
        SoundManager.Instance.PlaySFX("SFX_BossAtk", m_source);
    }
    void PlaySwingSound()
    {
        SoundManager.Instance.PlaySFX("SFX_ZombieAtk", m_source);
    }
    void PlayStepSound()
    {
        SoundManager.Instance.PlaySFX("SFX_BossChase", m_source);
    }
    void PlaySkill1()
    {
        SoundManager.Instance.PlaySFX("SFX_BossSkill", m_source);
    }
    void PlaySkill2()
    {
        SoundManager.Instance.PlaySFX("SFX_BossSkill2", m_source);
    }
    void PlayRage()
    {
        SoundManager.Instance.PlaySFX("SFX_BossRage", m_source);
    }
    #endregion

    #region Methods
    void SetAttack()
    {
        SetState(MonsterState.Attack); //공격 상태로 변경
        if(isSkill())  //30퍼센트 확률로 스킬 실행하도록.
        {
            if(Random.Range(0,5) <= 1)
            {
                
                m_skill1Pos.gameObject.SetActive(true);
               // m_navAgent.ResetPath();
                m_animctr.Play(MonsterAnimController.Motion.Skill1);
            }
            else
            {
                m_skill2Pos.gameObject.SetActive(true);
              //  m_navAgent.ResetPath();
                m_animctr.Play(MonsterAnimController.Motion.Skill2);
            }
            
        }
        else {
          //  m_navAgent.ResetPath();
             PlaySwingSound();
             m_animctr.Play(MonsterAnimController.Motion.Attack); }
    }
    public override void BurnDamage() //처음엔 중첩 안되게 하려 했으나 밸런스상 중첩 가능하게 수정(수치조정)
    {
        StartCoroutine(Couroutine_BurnDamage(m_burnDamage / 10));
    }
    public override void SetDamage(AttackType type, float damage, PlayerController player, bool isburn, IDamageAbleObject obj)
    {
   
        if (m_status.hp <= 0) //hp가 0이하일때는 적용 x // 바로 리턴을 하였는데 죽어도 안죽는 현상 발생 다시 수정함.
        {
            SetDie();
            return;
        }
        int dmg = Mathf.CeilToInt(damage); // 데미지를 인트로 바꾸어 받기
        DamageProcess(dmg, type);
        if (isburn)
        {
            BurnDamage();
        }
        if(player != null)
        {
            if (player.GetStatus.KnockBackPer > Random.Range(0, 100 + m_status.KnockbackRigist)) //넉백이 될때만 아래 적용 넉백없는 공격은 무시하고 돌진하도록!
            {
                m_navAgent.ResetPath();
                SetState(MonsterState.Damaged);
                if (m_motionDelaycoroutine != null)
                {
                    StopCoroutine(m_motionDelaycoroutine);
                    m_motionDelaycoroutine = null;
                }
                if (m_damagedCoroutine != null)
                {
                    StopCoroutine(m_damagedCoroutine);
                    m_damagedCoroutine = null;
                }
                m_animctr.Play(MonsterAnimController.Motion.KnockBack, false);
                StopSkill();
                var dir = (transform.position - player.transform.position);
                dir.y = 0f;
                m_tweenmove.m_from = transform.position;
                m_tweenmove.m_to = m_tweenmove.m_from + dir.normalized * player.GetStatus.KnockBackDist;
                m_tweenmove.m_duration = player.GetStatus.KnockBackDist;
                m_tweenmove.Play();
            }
        }
        if (m_status.hp <= 0) //피해를 받은 후 피가 0 이하일때 사망처리
        {
            obj.KillCount();
            m_hudPanel.Died();
            SetDie();
        }
    }

    public override void BehaviourProcess()
    {
        if (!gameObject.activeSelf || m_state.Equals(MonsterState.Die)) return;
        RageCool += Time.deltaTime;
        timeafterAttack += Time.deltaTime;
        if(!isRage && m_status.hp / m_status.hpMax < 0.5 && RageCool > 20 && m_state != MonsterState.Attack)
        {
            isRage= true;
            RageCool= 0;
            m_state= MonsterState.Rage;
            m_animctr.Play(MonsterAnimController.Motion.Rage);
        }
        switch (m_state)
        {
            case MonsterState.Idle:   
                m_navAgent.ResetPath();
                m_navAgent.isStopped = true;
                m_idleTime += Time.deltaTime;

                if (m_idleTime >= m_idleDuration) //대기시간을 초과한다면
                {
                    m_idleTime = 0f; //대기시간 초기화
                    if (FindTarget(30)) //관측 가능한 거리에 플레이어가 있다면.
                    {
                        if (CheckArea(m_status.attackDist)) // 사정거리 내에 위치해 있으면
                        {
                            if (m_status.atkSpeed <= timeafterAttack)
                            {
                                transform.forward = GetTargetDir();//플레이어 방향 봐주기
                                timeafterAttack = 0f;
                                m_navAgent.ResetPath();
                                SetAttack();
                            }
                            return;
                        }
                        SetState(MonsterState.Chase); //상태를 추적으로 변경
                        m_animctr.Play(MonsterAnimController.Motion.Chase);
                        m_navAgent.isStopped = false; //다시 움직이기 시작s
                        m_navAgent.stoppingDistance = m_status.attackDist - 1f; //사정거리 안에 들어오면 멈추도록
                        StartCoroutine(Coroutine_SerchTargetPath(10));
                        return;
                    }
                    else
                    {
                        m_navAgent.isStopped = false; //다시 움직이기 시작
                        SetState(MonsterState.Chase); //상태를 추적으로 변경
                        m_animctr.Play(MonsterAnimController.Motion.Chase);
                        m_navAgent.stoppingDistance = m_status.attackDist - 1f;
                        StartCoroutine(Coroutine_SerchTargetPath(60)); //거리안에 적이 없다면 느린 간격으로 추적 실행
                    }
                }
                break;
                
            case MonsterState.Chase:
                if (CheckArea(m_navAgent.stoppingDistance))
                {
                    SetIdle(0.1f);
                }
                break;
        }
    }
    protected override void Awake()
    {
        base.Awake();

        m_skill1Pos = Utill.GetChildObject(gameObject, "AttackArea");
        m_skill = GetComponent<ProjectileController>();
        m_skill1Pos.gameObject.SetActive(false);
        m_skill2Pos = Utill.GetChildObject(gameObject, "Circle");
        m_skill2Pos.gameObject.SetActive(false);
        RageCool = 15;
    }
    #endregion
}
