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
    protected  PlayerController m_player; //Ÿ���� �����ϱ� ����
    [Header("���� �ɷ�ġ")]
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
    IEnumerator Coroutine_MotionDelay() //��ų���� �����̸� �ٸ��� å���ϱ� ���� 
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
       // Debug.Log(this.name + " �� ����߽��ϴ�.");
 
        MonsterManager.Instance.ResetMonster(this, m_hudPanel);
    }
    protected virtual void AnimEvent_SetAttack()
    {
        float damage =  CalculationDamage.NormalDamage(m_status.damage, m_player.GetStatus.defense);
      //  Debug.Log("������ : " + damage + " ���� ���ݷ� : " + m_status.damage + " �÷��̾� ���� : " + m_player.GetStatus.defense);
        var dir = m_player.transform.position - transform.position;
        float sqrAttackDist = Mathf.Pow(m_status.attackDist, 2f);
        if (Mathf.Approximately(dir.sqrMagnitude, sqrAttackDist) || dir.sqrMagnitude < sqrAttackDist)
        {      
            var dot = Vector3.Dot(transform.forward, dir.normalized);
            if (dot >= 0.5f) //���ݽ� ������ ���� ������ ���� �����϶��� �������� ���ϴ� ������.
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
    public void InitMonster(MonsterType type) //������ Ÿ���� �޾ƿ�
    {//ü�� ���� ���ݷ� ���� �̵��ӵ� ������
        Type = type;
    }
    public void InitStatus(MonsterType type, string name, float hp, float atkSpeed, float dmg, float def, float speed, float attackDist, float StatScale, float knockBackRigist,float Score,float coin,float exp) //�� ������ ������ ����.
    {
        m_status = new MonStatus(type, name, hp * StatScale, atkSpeed * StatScale, dmg * StatScale, def * StatScale, speed * StatScale, attackDist,knockBackRigist,Score*StatScale,coin*StatScale,exp*StatScale);
        m_status.hp = m_status.hpMax; //�ִ�ü�� ����.
        m_navAgent.stoppingDistance = m_status.attackDist;
        timeafterAttack = m_status.atkSpeed;
        m_animFloat.SetFloat("Speed", m_status.speed / 5f);
        m_navAgent.speed = m_status.speed;
    }
    public void SetStatus(MonsterType type, float StatScale) //������ Ÿ�԰� ���忡 ���� StatScale�� �޾� ������ ������ ������ InitStatus�� �����ֱ�
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
        // Debug.Log(count+"��° ���ӿ�����Ʈ :"+gameObject + "�� SetDie����!! ���� ���� : " + m_state);
        StopAllCoroutines();
        UIManager.Instance.ScoreChange(Random.Range(m_status.score / 2, m_status.score / 0.7f));
        UIManager.Instance.MoneyChange(Random.Range(m_status.coin / 2, m_status.coin / 0.7f));
        m_player.IncreaseExperience(m_status.score);
        m_animctr.Play(MonsterAnimController.Motion.Die);
        SetState(MonsterState.Die); //���¸� �������·�
        gameObject.tag = "Die";//�׾����� �±׸� Die�� �����Ͽ� ��ü�� ���� ���� Ÿ�ݹ���.
        m_navAgent.ResetPath(); //�� ��������!! 
                                //   Debug.Log(count + "��° ���ӿ�����Ʈ :" + gameObject + "SetDie����!! ���� ���� : " + m_state + "�±״�? :" +gameObject.tag);
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
                if (m_idleTime >= m_idleDuration) //���ð��� �ʰ��Ѵٸ�
                {
                    m_idleTime = 0f; //���ð� �ʱ�ȭ
                    if (FindTarget(m_player.transform,50)) //���� ������ �Ÿ��� �÷��̾ �ִٸ�.
                    {
                        if (CheckArea(m_player.transform.position, m_status.attackDist)) // �����Ÿ� ���� ��ġ�� ������
                        {
                            if (m_status.atkSpeed <= timeafterAttack)
                            {
                                transform.forward = GetTargetDir(m_player.transform);//�÷��̾� ���� ���ֱ�
                                timeafterAttack = 0f;
                                m_navAgent.ResetPath();
                                SetState(MonsterState.Attack);
                                m_animctr.Play(MonsterAnimController.Motion.Attack);
                            }   
                            return;
                        }
                        SetState(MonsterState.Chase); //���¸� �������� ����
                        m_animctr.Play(MonsterAnimController.Motion.Chase);
                        m_navAgent.isStopped = false; //�ٽ� �����̱� ����s
                        m_navAgent.stoppingDistance = m_status.attackDist -1f; //�����Ÿ� �ȿ� ������ ���ߵ���
                        StartCoroutine(Coroutine_SerchTargetPath(5));
                        return;
                    }
                    else
                    {
                        m_navAgent.isStopped = false; //�ٽ� �����̱� ����
                        SetState(MonsterState.Chase); //���¸� �������� ����
                        m_animctr.Play(MonsterAnimController.Motion.Chase);
                        m_navAgent.stoppingDistance = m_status.attackDist -1f;
                        StartCoroutine(Coroutine_SerchTargetPath(60)); //�Ÿ��ȿ� ���� ���ٸ� ���� �������� ���� ����
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
        if (m_status.hp <= 0) //hp�� 0�����϶��� ���� x // �ٷ� ������ �Ͽ��µ� �׾ ���״� ���� �߻� �ٽ� ������.
        {
           SetDie();
            return;
        }   
        int dmg = Mathf.CeilToInt(damage); // �������� ��Ʈ�� �ٲپ� �ޱ�
        HPControl(-dmg);
        m_damagedCoroutine = StartCoroutine("Coroutine_SetDamagedColor");
        m_hudPanel.DisplayDamage(type, dmg, m_status.hp / m_status.hpMax);
        if (player.GetStatus.KnockBackPer > m_status.KnockbackRigist) //�˹��� �ɶ��� �Ʒ� ���� �˹���� ������ �����ϰ� �����ϵ���!
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
        if (m_status.hp <= 0) //���ظ� ���� �� �ǰ� 0 �����϶� ���ó��
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
