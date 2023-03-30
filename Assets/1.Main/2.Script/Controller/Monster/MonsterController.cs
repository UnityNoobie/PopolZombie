using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public enum MonsterState
    {
        Chase,
        Attack,
        Die,
        Idle,
        Skill,
        Rage,
        Damaged,
        Max
    }
    [SerializeField]
    Renderer[] m_Renderers;
    [SerializeField]
    protected  PlayerController m_player; //타겟을 지정하기 위함
    [Header("몬스터 능력치")]
    [SerializeField]
    protected MonStatus m_status;
    [SerializeField, HideInInspector]
    MonStat m_monStat;
    protected NavMeshAgent m_navAgent;
    protected  MonsterAnimController m_animctr;
    HUDController m_hudPanel;
    TweenMove m_tweenmove;
    protected Animator m_animFloat;
    public Transform DummyHud;
    [SerializeField]
    protected MonsterState m_state;

    protected float timeafterAttack;
    protected float m_idleDuration = 0.5f;
    protected float m_idleTime;
    bool isChase;
    int m_delayFrame;
    //public float defence;
    Coroutine m_damagedCoroutine;
    Coroutine m_motionDelaycoroutine;
    int count = 1;
    public MonStat GetStat { get { return m_monStat; } set { m_monStat = value; } }
    public MonStatus GetStatus { get { return m_status; } set { m_status = value; } }

    public MonsterType Type { get; set; }
    IEnumerator Coroutine_SetDamagedColor()
    {
        SetRimLight(Color.red);
        yield return new WaitForSeconds(1f);
        SetRimLight(Color.black);
    }
    IEnumerator Coroutine_MotionDelay() //스킬마다 딜레이를 다르게 책정하기 위해 
    {
        for (int i = 0; i < m_delayFrame; i++)
        {
            yield return null;
        }
        SetIdle(0.1f);
        m_delayFrame = 0;
    }
    protected IEnumerator Coroutine_SerchTargetPath(int frame)
    {
        while (m_state == MonsterState.Chase)
        {
            m_navAgent.SetDestination(m_player.transform.position);
            for (int i = 0; i < frame; i++)
                yield return null;
        }
    }
    #region AnimEvent
    protected virtual void AnimEvent_SetDie()
    {
       // Debug.Log(this.name + " 가 사망했습니다.");
 
        MonsterManager.Instance.ResetMonster(this, m_hudPanel);
    }
    protected virtual void AnimEvent_SetAttack()
    {
        float damage =  CalculationDamage.NormalDamage(m_status.damage, m_player.GetStatus.defense);
      //  Debug.Log("데미지 : " + damage + " 몬스터 공격력 : " + m_status.damage + " 플레이어 방어력 : " + m_player.GetStatus.defense);
        var dir = m_player.transform.position - transform.position;
        float sqrAttackDist = Mathf.Pow(m_status.attackDist, 2f);
        if (Mathf.Approximately(dir.sqrMagnitude, sqrAttackDist) || dir.sqrMagnitude < sqrAttackDist)
        {      
            var dot = Vector3.Dot(transform.forward, dir.normalized);
            if (dot >= 0.5f) //공격시 지정한 범위 안쪽을 향한 공간일때만 데미지를 가하는 식으로.
            {
                m_player.GetDamage(damage);               
            }
        }
    }
    void AnimEvent_AttackFinished()
    {
            SetIdle(0.1f);
    }
    void AnimEvent_HitFinished()
    {
          if(m_status.hp > 0)
        {
            m_motionDelaycoroutine = StartCoroutine(Coroutine_MotionDelay());
        }
        else
        {
            StopAllCoroutines();
        }
       
    }
    #endregion
    void HPControl(int value)
    {
        m_status.hp += value;
        if (m_status.hp > m_status.hpMax) { m_status.hp = m_status.hpMax; }
      //  hp = Mathf.CeilToInt(m_status.hp);
    }
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        m_animFloat = GetComponent<Animator>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_animctr = GetComponent<MonsterAnimController>();
        DummyHud = Utill.GetChildObject(gameObject, "Dummy");
        m_Renderers = GetComponentsInChildren<Renderer>();
        m_tweenmove = GetComponent<TweenMove>();
        SetRimLight(Color.black);
    }
    public void SetMonster(PlayerController player, HUDController hud)
    {
        m_player = player;
        m_hudPanel = hud;
        SetState(MonsterState.Chase);
        StartCoroutine(Coroutine_SerchTargetPath(10));
    }
    public void InitMonster(MonsterType type) //몬스터의 타입을 받아옴
    {//체력 공속 공격력 방어력 이동속도 순서로
        Type = type;
    }
    public void InitStatus(MonsterType type, string name, float hp, float atkSpeed, float dmg, float def, float speed, float attackDist, float StatScale, float knockBackRigist,float Score,float coin,float exp) //이 몬스터의 스탯을 적용.
    {
        m_status = new MonStatus(type, name, hp * StatScale, atkSpeed * StatScale, dmg * StatScale, def * StatScale, speed * StatScale, attackDist,knockBackRigist,Score*StatScale,coin*StatScale,exp*StatScale);
        m_status.hp = m_status.hpMax; //최대체력 설정.
        m_navAgent.stoppingDistance = m_status.attackDist;
        timeafterAttack = m_status.atkSpeed;
        m_animFloat.SetFloat("Speed", m_status.speed / 5f);
        m_navAgent.speed = m_status.speed;
    }
    public void SetStatus(MonsterType type, float StatScale) //몬스터의 타입과 라운드에 따른 StatScale을 받아 몬스터의 정보를 가져와 InitStatus로 보내주기
    {
        var monStat = m_monStat.SetMonStat(Type);
        InitStatus(monStat.type, monStat.name, monStat.hp, monStat.atkSpeed, monStat.damage, monStat.defense, monStat.speed, monStat.attackDist, StatScale,monStat.knockbackRegist,monStat.Score,monStat.coin,monStat.exp);
       
    }
    public void SetState(MonsterState state)
    {
        m_state = state;
    }
    protected void SetRimLight(Color color)
    {
        for (int i = 0; i < m_Renderers.Length; i++)
            m_Renderers[i].material.SetColor("_FresnelColor", color);
    }

    protected void SetDie()
    {
        // Debug.Log(count+"번째 게임오브젝트 :"+gameObject + "의 SetDie실행!! 현재 상태 : " + m_state);
        StopAllCoroutines();
        UIManager.Instance.ScoreChange(Random.Range(m_status.score / 2, m_status.score / 0.7f));
        UIManager.Instance.MoneyChange(Random.Range(m_status.coin / 2, m_status.coin / 0.7f));
        m_player.IncreaseExperience(m_status.score);
        m_animctr.Play(MonsterAnimController.Motion.Die);
        SetState(MonsterState.Die); //상태를 죽은상태로
        gameObject.tag = "Die";//죽었을때 태그를 Die로 설정하여 시체가 뒤의 좀비 타격방지.
        m_navAgent.ResetPath(); //더 추적금지!! 
                                //   Debug.Log(count + "번째 게임오브젝트 :" + gameObject + "SetDie종료!! 현재 상태 : " + m_state + "태그는? :" +gameObject.tag);
                                //    count++;
    }
    protected virtual void SetIdle(float duration)
    {
        SetState(MonsterState.Idle);
        m_animctr.Play(MonsterAnimController.Motion.Idle);
        SetIdleDuration(duration);
    }
    protected virtual void SetIdleDuration(float duration)
    {
        m_idleTime = m_idleDuration - duration;
    }
    protected virtual bool CheckArea(Vector3 target, float area)
    {
        var dir = target - transform.position;
        dir.y = 0;
        float sqrArea = Mathf.Pow(area, 2f);
        if (Mathf.Approximately(dir.sqrMagnitude, sqrArea) || dir.sqrMagnitude < sqrArea)
        {
            return true;
        }
        return false;
    }
    protected virtual float GetTargetAngle(Transform target)
    {
        var dir = target.transform.position - transform.position;
        dir.y = 0;
        return Vector3.Angle(transform.forward, dir);
    }
    protected virtual Vector3 GetTargetDir(Transform target)
    {
        var dir = target.position - transform.position;
        dir.y = 0f;
        return dir.normalized;
    }
    protected bool FindTarget(Transform target, float dist)
    {
        RaycastHit hit;
        var dir = target.position - transform.position;
        dir.y = 0f;
        var layer = (1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawRay(transform.position + Vector3.up * 1.2f, dir.normalized * dist, Color.magenta);
        if (Physics.Raycast(transform.position + Vector3.up * 1.2f, dir.normalized, out hit, dist, layer))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
        }
        return false;
    }
    public virtual void BehaviourProcess()
    {
        if (!gameObject.activeSelf || m_state.Equals(MonsterState.Die)) return;
        
        timeafterAttack += Time.deltaTime;

        switch (m_state)
        {
            case MonsterState.Idle:
                m_navAgent.ResetPath();
                m_navAgent.isStopped = true;
                m_idleTime += Time.deltaTime;    
                if (m_idleTime >= m_idleDuration) //대기시간을 초과한다면
                {
                    m_idleTime = 0f; //대기시간 초기화
                    if (FindTarget(m_player.transform,50)) //관측 가능한 거리에 플레이어가 있다면.
                    {
                        if (CheckArea(m_player.transform.position, m_status.attackDist)) // 사정거리 내에 위치해 있으면
                        {
                            if (m_status.atkSpeed <= timeafterAttack)
                            {
                                transform.forward = GetTargetDir(m_player.transform);//플레이어 방향 봐주기
                                timeafterAttack = 0f;
                                m_navAgent.ResetPath();
                                SetState(MonsterState.Attack);
                                m_animctr.Play(MonsterAnimController.Motion.Attack);
                            }   
                            return;
                        }
                        SetState(MonsterState.Chase); //상태를 추적으로 변경
                        m_animctr.Play(MonsterAnimController.Motion.Chase);
                        m_navAgent.isStopped = false; //다시 움직이기 시작s
                        m_navAgent.stoppingDistance = m_status.attackDist -1f; //사정거리 안에 들어오면 멈추도록
                        StartCoroutine(Coroutine_SerchTargetPath(5));
                        return;
                    }
                    else
                    {
                        m_navAgent.isStopped = false; //다시 움직이기 시작
                        SetState(MonsterState.Chase); //상태를 추적으로 변경
                        m_animctr.Play(MonsterAnimController.Motion.Chase);
                        m_navAgent.stoppingDistance = m_status.attackDist -1f;
                        StartCoroutine(Coroutine_SerchTargetPath(60)); //거리안에 적이 없다면 느린 간격으로 추적 실행
                    }
                }
                break;
            case MonsterState.Chase:
                if (CheckArea(m_player.transform.position, m_navAgent.stoppingDistance))
                {
                    SetIdle(0.1f);
                }
                break;

        }

    }
    public virtual void SetDamage(AttackType type, float damage, PlayerController player)
        {
        if (m_status.hp <= 0) //hp가 0이하일때는 적용 x // 바로 리턴을 하였는데 죽어도 안죽는 현상 발생 다시 수정함.
        {
           SetDie();
            return;
        }   
        int dmg = Mathf.CeilToInt(damage); // 데미지를 인트로 바꾸어 받기
        HPControl(-dmg);
        m_damagedCoroutine = StartCoroutine("Coroutine_SetDamagedColor");
        m_hudPanel.DisplayDamage(type, dmg, m_status.hp / m_status.hpMax);
        if (player.GetStatus.KnockBackPer > m_status.KnockbackRigist) //넉백이 될때만 아래 적용 넉백없는 공격은 무시하고 돌진하도록!
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
           var dir = (transform.position - player.transform.position);
           dir.y = 0f;
           m_tweenmove.m_from = transform.position;
           m_tweenmove.m_to = m_tweenmove.m_from + dir.normalized * player.GetStatus.KnockBackDist;
           m_tweenmove.m_duration = player.GetStatus.KnockBackDist;
           m_tweenmove.Play();
        }
        if (m_status.hp <= 0) //피해를 받은 후 피가 0 이하일때 사망처리
        {
            SetDie();
        }
           
        StartCoroutine(Coroutine_SetDamagedColor());
    }
    /*
        protected virtual void Update()
        {
         if(m_status.hp > 0)
            BehaviourProcess();
        }*/
}
