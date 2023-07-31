using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MonsterController : MonoBehaviour
{
    #region Constants and Fields
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
    ProjectileController m_burn;
    ProjectileController m_crush;
    protected AudioSource m_source;
    protected HUDController m_hudPanel;
    [SerializeField, HideInInspector]
    MonStat m_monStat;
    [SerializeField]
    Renderer[] m_Renderers;
    [SerializeField]
    protected PlayerController m_player; //타겟을 지정하기 위함
    [Header("몬스터 능력치")]
    [SerializeField]
    protected MonStatus m_status;
    [SerializeField]
    protected GameObject m_target;
    protected TweenMove m_tweenmove;
    protected NavMeshAgent m_navAgent;
    protected MonsterAnimController m_animctr;
    protected Animator m_animFloat;
    public Transform DummyHud;
    [SerializeField]
    protected MonsterState m_state;
    protected float timeafterAttack;
    protected float m_idleDuration = 0.5f;
    protected float m_idleTime;
    protected bool isburn;
    protected float m_burnDamage = 5;
    int m_delayFrame;
    float defence = 0;
    #endregion

    #region Property
    public MonStatus GetStatus { get { return m_status; } set { m_status = value; } }

    public MonsterType Type { get; set; }
    #endregion

    #region coroutine
    protected Coroutine m_damagedCoroutine;
    protected Coroutine m_motionDelaycoroutine;
    protected Coroutine m_burnCoroutine;
    protected Coroutine m_crushCoroutine;
    IEnumerator Coroutine_SetDamagedColor() //피해 입을 시 표시용도
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
    protected IEnumerator Couroutine_BurnDamage(float value) // 상태이상 화상 지속딜
    {
        var effectName = TableEffect.Instance.m_tableData[1].Prefab[2];
        var effect = EffectPool.Instance.Create(effectName);
        m_burn = effect.gameObject.GetComponent<ProjectileController>();
        m_burn.SetFollowProjectile(gameObject.GetComponent<MonsterController>(), 0);
        m_burn.transform.position = gameObject.transform.position;
        for (int i = 0; i < 5; i++)
        {
            DamageProcess(Mathf.CeilToInt(((m_status.hpMax * value) / 100)), AttackType.Normal);
            yield return new WaitForSeconds(1);
        }
        m_burnCoroutine = null;
    }
    IEnumerator Coroutine_Crush(float value) //상태이상 적용용도 (슬로우, 방어력 감소)
    {
        var effectName = TableEffect.Instance.m_tableData[7].Prefab[1];
        var effect = EffectPool.Instance.Create(effectName);
        m_crush = effect.gameObject.GetComponent<ProjectileController>();
        m_crush.SetFollowProjectile(gameObject.GetComponent<MonsterController>(), 0);
        m_crush.transform.position = gameObject.transform.position;
        float speed = m_status.speed - m_status.speed * value; //이동속도 감소효과
        m_status.defense = m_status.defense - m_status.defense * value;
        m_navAgent.speed = speed;
        yield return new WaitForSeconds(5);
        m_status.defense = defence;
        m_navAgent.speed = m_status.speed;
        m_crushCoroutine = null;
    }
    protected IEnumerator Coroutine_SerchTargetPath(int frame)
    {
        while (m_state == MonsterState.Chase)
        {
            SetTarget();
            m_navAgent.SetDestination(m_target.transform.position);
            for (int i = 0; i < frame; i++)
                yield return null;
        }
    }
    #endregion

    #region AnimEvent
    protected virtual void AnimEvent_SetDie()
    {
        StopAllCoroutines();
        SetRimLight(Color.black);
        MonsterManager.Instance.ResetMonster(this, m_hudPanel);
    }
    protected virtual void AnimEvent_SetAttack()
    {
        PlayAtkSound();
        /*
        GameObject target = GameManager.Instance.GetTargetObject(transform.position);
        var dir = target.transform.position - transform.position;
        
       float sqrAttackDist = Mathf.Pow(m_status.attackDist, 2f);
       if (Mathf.Approximately(dir.sqrMagnitude, sqrAttackDist) || dir.sqrMagnitude < sqrAttackDist)
       {
           var dot = Vector3.Dot(transform.forward, dir.normalized);
           if (dot >= 0.5f) //공격시 지정한 범위 안쪽을 향한 공간일때만 데미지를 가하는 식으로.
           {
               m_player.GetDamage(m_status.damage);
           }
       }
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_status.attackDist);
        foreach (Collider collider in colliders)
        {
            IDamageAbleObject damageableObject = collider.GetComponent<IDamageAbleObject>();
            PlayerController player = collider.GetComponent<PlayerController>();
            if (damageableObject != null)
            {
                damageableObject.SetDamage(m_status.damage);
            }
            if(player != null)
            {
                player.GetDamage(m_status.damage);
            }
        }*/
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_status.attackDist);
        foreach (Collider collider in colliders)
        {
            IDamageAbleObject damageableObject = collider.GetComponent<IDamageAbleObject>();
            PlayerController player = collider.GetComponent<PlayerController>();
            if (damageableObject != null)
            {
                var dir = m_target.transform.position - transform.position;
                float sqrAttackDist = Mathf.Pow(m_status.attackDist, 2f);
                if (Mathf.Approximately(dir.sqrMagnitude, sqrAttackDist) || dir.sqrMagnitude < sqrAttackDist)
                {
                    var dot = Vector3.Dot(transform.forward, dir.normalized);
                    if (dot >= 0.5f)
                    {
                        damageableObject.SetDamage(m_status.damage,this);
                    }
                }
            }
            /*
            if (player != null)
            {
                var dir = m_target.transform.position - transform.position;
                float sqrAttackDist = Mathf.Pow(m_status.attackDist, 2f);
                if (Mathf.Approximately(dir.sqrMagnitude, sqrAttackDist) || dir.sqrMagnitude < sqrAttackDist)
                {
                    var dot = Vector3.Dot(transform.forward, dir.normalized);
                    if (dot >= 0.5f)
                    {
                        player.SetDamage(m_status.damage, this);
                    }
                }
            }*/
        }
    }
    void AnimEvent_AttackFinished()
    {
        SetIdle(0.1f);
    }
    void AnimEvent_HitFinished()
    {
        if (m_status.hp > 0)
        {
            m_motionDelaycoroutine = StartCoroutine(Coroutine_MotionDelay());
        }
        else
        {
            StopAllCoroutines();
        }
    }
    #endregion

    #region Methods
    void HPControl(int value, AttackType type)
    {
        m_status.hp += value;
        if (m_status.hp > m_status.hpMax) { m_status.hp = m_status.hpMax; }
        m_hudPanel.DisplayDamage(type, -value, m_status.hp / m_status.hpMax);
    }
    // Start is called before the first frame update
    #region SFX

    public virtual void PlayHitSound(string sound) //피격시 소리 재생
    {
        SoundManager.Instance.PlaySFX(sound, m_source);
        SoundManager.Instance.PlaySFX("SFX_ZombieDamaged", m_source);
    }
    void PlayDieSound()
    {
        SoundManager.Instance.PlaySFX("SFX_ZombieDeath", m_source);
    }
    protected virtual void PlayAtkSound()
    {
        SoundManager.Instance.PlaySFX("SFX_ZombieAtk", m_source);
    }
    #endregion
    public void SetMonster(PlayerController player, HUDController hud)
    {
        m_player = player;
        m_hudPanel = hud;
        SetState(MonsterState.Idle);
        StartCoroutine(Coroutine_SerchTargetPath(60));
    }
    public void InitMonster(MonsterType type) //몬스터의 타입을 받아옴
    {//체력 공속 공격력 방어력 이동속도 순서로
        Type = type;
    }
    public void InitStatus(MonStat stat,float StatScale) //이 몬스터의 스탯을 적용.
    {
        m_status = new MonStatus(stat.type, stat.name, stat.hp * StatScale, stat.atkSpeed * StatScale, stat.damage * StatScale, stat.defense * StatScale, stat.speed * StatScale, stat.attackDist,stat.knockbackRegist,stat.Score*StatScale,stat.coin*StatScale,stat.exp *StatScale);
        m_status.hp = m_status.hpMax; //최대체력 설정.
        m_navAgent.stoppingDistance = m_status.attackDist;
        timeafterAttack = m_status.atkSpeed;
        m_animFloat.SetFloat("Speed", m_status.speed / 5f);
        m_navAgent.speed = m_status.speed;
        isburn = false;
    }
    public void SetStatus(MonsterType type, float StatScale) //몬스터의 타입과 라운드에 따른 StatScale을 받아 몬스터의 정보를 가져와 InitStatus로 보내주기
    {
        var monStat = m_monStat.GetMonStat(type);
        defence = monStat.defense; //방어력감소 효과 적용용
        InitStatus(monStat,StatScale);
    }
    public void SetTarget()
    {
        m_target = GameManager.Instance.GetTargetObject(transform.position);
    }

    public void SetState(MonsterState state)
    {
        m_state = state;
    }
    protected void SetRimLight(Color color) //피격시 좀비 색 변경하여 표시
    {
        for (int i = 0; i < m_Renderers.Length; i++)
            m_Renderers[i].material.SetColor("_FresnelColor", color);
    }

    protected virtual void SetDie()
    {
        PlayDieSound();
        StopAllCoroutines();
        int money = Mathf.CeilToInt(Random.Range(m_status.coin / 2, m_status.coin / 0.7f));
        int score = Mathf.CeilToInt(Random.Range(m_status.score / 2, m_status.score / 0.7f));
        int exp = Mathf.CeilToInt(m_status.score);
        m_player.GetComponent<PlayerGetItem>().GetMoney(money);
        m_player.IncreaseExperience(exp);
        m_player.AddScore(score);
        m_animctr.Play(MonsterAnimController.Motion.Die);
        SetState(MonsterState.Die); //상태를 죽은상태로
        gameObject.tag = "Die";//죽었을때 태그를 Die로 설정하여 시체가 뒤의 좀비 타격방지.
        m_navAgent.ResetPath(); //더 추적금지!! 
    }
    protected virtual void SetIdle(float duration)
    {
        if(m_state == MonsterState.Die)  //죽어도 안죽는, 이동하는 현상 발생하여 추가함.
        {
            if(m_animctr.GetMotion != MonsterAnimController.Motion.Die)
            {
                m_animctr.Play("Die", false);
            }
        }
        else
        {
            SetState(MonsterState.Idle);
            m_animctr.Play(MonsterAnimController.Motion.Idle);
            SetIdleDuration(duration);
        } 
    }
    protected virtual void SetIdleDuration(float duration)
    {
        m_idleTime = m_idleDuration - duration;
    }
    protected virtual bool CheckArea(float area)
    {
        SetTarget();
        var dir = m_target.transform.position - transform.position;
        dir.y = 0;
        float sqrArea = Mathf.Pow(area, 2f);
        if (Mathf.Approximately(dir.sqrMagnitude, sqrArea) || dir.sqrMagnitude < sqrArea)
        {
            return true;
        }
        return false;
    }
    protected virtual float GetTargetAngle()
    {
        SetTarget();
        var dir = m_target.transform.position - transform.position;
        dir.y = 0;
        return Vector3.Angle(transform.forward, dir);
    }
    protected virtual Vector3 GetTargetDir()
    {
        SetTarget();
        var dir = m_target.transform.position - transform.position;
        dir.y = 0f;
        return dir.normalized;
    }
    protected bool FindTarget(float dist)
    {
        SetTarget();
        RaycastHit hit;
        var dir = m_target.transform.position - transform.position;
        dir.y = 0f;
        var layer = (1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Player")|1 << LayerMask.NameToLayer("DamageAbleObject"));
        if (Physics.Raycast(transform.position + Vector3.up * 1.2f, dir.normalized, out hit, dist, layer))
        {
            if (hit.transform.CompareTag("Player")|| hit.transform.CompareTag("Generator") || hit.transform.CompareTag("Barricade"))
                return true;
        }
        return false;
    }
    protected virtual void Awake()
    {
        m_animFloat = GetComponent<Animator>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_animctr = GetComponent<MonsterAnimController>();
        DummyHud = Utill.GetChildObject(gameObject, "Dummy");
        m_Renderers = GetComponentsInChildren<Renderer>();
        m_tweenmove = GetComponent<TweenMove>();
        m_source = GetComponent<AudioSource>();
        SetRimLight(Color.black);
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
                    if (FindTarget(30)) //관측 가능한 거리에 공격 가능한 오브젝트가 있다면.
                    {
                        if (CheckArea(m_status.attackDist)) // 사정거리 내에 위치해 있으면
                        {
                            if (m_status.atkSpeed <= timeafterAttack)
                            {
                                transform.forward = GetTargetDir();//타겟 방향 봐주기
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
                        StartCoroutine(Coroutine_SerchTargetPath(60));
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
  
    public virtual void BurnDamage()//처음엔 중첩 안되게 하려 했으나 밸런스상 중첩 가능하게 수정(수치조정)
    {
        StartCoroutine(Couroutine_BurnDamage(m_burnDamage));
    }
    public virtual void Crush(float crushvalue)
    {
        if(m_crushCoroutine == null)
        {
            m_crushCoroutine = StartCoroutine(Coroutine_Crush(crushvalue)); //중첩 실행 시 이펙트가 복사가 된다고?! 를 느끼기 때문에 일단 요로코롬 추후수정
        }
    }
    protected void DamageProcess(int damage,AttackType type) //화상데미지 확장용
    {
        HPControl(-damage,type);
        m_damagedCoroutine = StartCoroutine("Coroutine_SetDamagedColor");
    }
    public bool IsAliveObject()
    {
        if(m_state != MonsterState.Die) return true;
        else return false;
    }
    public virtual void SetDamage(AttackType type, float damage, PlayerController player, bool isburn,IDamageAbleObject obj)
    {
        if (m_status.hp <= 0) //hp가 0이하일때는 적용 x // 바로 리턴을 하였는데 죽어도 안죽는 현상 발생 다시 수정함.
        {
            obj.KillCount();
            SetDie();
            return;
        }   
        int dmg = Mathf.CeilToInt(damage); // 데미지를 인트로 바꾸어 받기
        DamageProcess(dmg, type);
        if(isburn)
        {
            BurnDamage();
        }
        if(player != null)
        if (player.GetStatus.KnockBackPer > Random.Range(0,100 + m_status.KnockbackRigist)) //넉백이 될때만 아래 적용 넉백없는 공격은 무시하고 돌진하도록!
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
            obj.KillCount();
            m_hudPanel.Died();
            SetDie();
        }      
    }
    #endregion
}
