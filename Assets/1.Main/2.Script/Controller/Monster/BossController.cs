using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerStriker;

public class BossController : MonsterController
{
    
    public Transform m_skill1Pos;
    Transform m_skill2Pos;
    ProjectileController m_skill;
    ProjectileController m_skill2;
    [SerializeField]
    Transform m_fireAura;
    [SerializeField]
    ProjectileController m_fire;
    bool isRage;
    //   MonsterAnimController m_animCtr;

    float RageCool;
    int detectDist = 20;
    public enum SkillType
    {
        Normal,
        Skill1,
        Skill2,
        Max
    }
    bool isSkill()
    {
        int skillRate = Random.Range(0, 100);
        if (skillRate <= 10) return false;
        else return true;
    }
    IEnumerator Coroutine_Rage()
    {
        isRage = true;
        m_state = MonsterState.Rage;
        var effectName = TableEffect.Instance.m_tableData[7].Prefab[2];
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
        SetIdle(1f);
     //   m_fire.gameObject.SetActive(false);
        isRage= false;
    }
    #region AnimEvent
    void AnimEvent_CancleSkill()
    {
        m_skill1Pos.gameObject.SetActive(false);
        m_skill2Pos.gameObject.SetActive(false);
    }
    void AnimEvent_Skill1()    
    {
        m_skill2Pos.gameObject.SetActive(false);
        m_skill1Pos.gameObject.SetActive(false);
        var effectName = TableEffect.Instance.m_tableData[7].Prefab[0];
        var effect = EffectPool.Instance.Create(effectName);
        m_skill = effect.GetComponent<ProjectileController>();
        m_skill.SetProjectileWithChild(gameObject.GetComponent<BossController>(), 1.5f);
        m_skill.transform.position = gameObject.transform.position;
        m_skill.transform.forward = gameObject.transform.forward;
        m_skill.transform.localScale = new Vector3(2f, 2f, 2f);
    } 
    void AnimEvent_Skill2()
    {
        m_skill1Pos.gameObject.SetActive(false);
        m_skill2Pos.gameObject.SetActive(false);
        var effectName = TableEffect.Instance.m_tableData[7].Prefab[1];
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

    #endregion
    void SetAttack()
    {
        SetState(MonsterState.Attack); //공격 상태로 변경
        if(isSkill())  //30퍼센트 확률로 스킬 실행하도록.
        {
            if(Random.Range(0,5) <= 2)
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
            m_animctr.Play(MonsterAnimController.Motion.Attack); }
       
    }
 
    public override void BehaviourProcess()
    {
        if (!gameObject.activeSelf) return;
        RageCool += Time.deltaTime;
        timeafterAttack += Time.deltaTime;
        if(!isRage && (float)m_status.hp / m_status.hpMax < 0.5 && RageCool > 20 && m_state != MonsterState.Attack)
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
                    if (m_state != MonsterState.Die && gameObject.tag == "Die") //간혹 살아있는놈의 태그가 Die일경우가 있어서 수정해봄
                    {
                        gameObject.tag = "Zombie";
                    }
                    m_idleTime = 0f; //대기시간 초기화
                    if (FindTarget(m_player.transform, 50)) //관측 가능한 거리에 플레이어가 있다면.
                    {
                        
                        if (CheckArea(m_player.transform.position, m_status.attackDist)) // 사정거리 내에 위치해 있으면
                        {

                            if (m_status.atkSpeed <= timeafterAttack)
                            {
                                transform.forward = GetTargetDir(m_player.transform);//플레이어 방향 봐주기
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
                //  dist = m_player.transform.position - transform.position;
                //    dist.y = 0f;
                //if (Mathf.Approximately(dist.sqrMagnitude, Mathf.Pow(m_navMeshAgent.stoppingDistance, 2f)) || dist.sqrMagnitude < Mathf.Pow(m_navMeshAgent.stoppingDistance, 2f))
                if (CheckArea(m_player.transform.position, m_navAgent.stoppingDistance))
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
        //  m_animCtr = GetComponent<MonsterAnimController>();
    }
  ///  private void Start()
  //  {

   // }
    /*
    protected override void Update()
    {
        RageCool += Time.deltaTime;
        if (m_state != MonsterState.Die)
            BehaviourProcess();
    }*/
}
