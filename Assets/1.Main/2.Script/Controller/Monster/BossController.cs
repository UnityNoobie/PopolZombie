using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    bool isSkill()//��ų üĿ
    {
        int skillRate = Random.Range(0, 100);
        if (skillRate <= 60) return false;
        else return true;
    }
    #endregion

    #region Coroutine
    IEnumerator Coroutine_Rage()// ���������� �г�����.
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
        SetIdle(0.3f);
        isRage= false;
    }
    #endregion
    #region Methods

    #region AnimEvent
    void AnimEvent_CancleSkill() //�˹�, ��� ���� �̽��� ��ų ����� �ߴܵǾ��� �� ȣ��.
    {
        StopSkill();
    }
    void AnimEvent_Skill1()    //���� ��ų 1 ����Ʈ ����
    {
        StopSkill();
        PlaySkill1();
        var effectName = TableEffect.Instance.m_tableData[8].Prefab[0];
        var effect = EffectPool.Instance.Create(effectName);
        m_skill = effect.GetComponent<ProjectileController>();
        m_skill.SetProjectileWithChild(gameObject.GetComponent<BossController>(), 1.5f);
        m_skill.transform.position = gameObject.transform.position;
        m_skill.transform.forward = gameObject.transform.forward;
        m_skill.transform.localScale = new Vector3(2f, 2f, 2f);
    }
    void AnimEvent_Skill2()//���� ��ų 2 ����Ʈ ����
    {
        StopSkill();
        PlaySkill2();
        var effectName = TableEffect.Instance.m_tableData[8].Prefab[1];
        var effect = EffectPool.Instance.Create(effectName);
        m_skill2 = effect.GetComponent<ProjectileController>();
        m_skill2.SetProjectile(gameObject.GetComponent<BossController>(), 0.1f);
        m_skill2.transform.position = gameObject.transform.position;
        m_skill2.transform.localScale = new Vector3(5f, 5f, 5f);
    }
    void AnimEvent_Rage() //���� �г� ����
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
    void StopSkill() //��ų ��� ���
    {
        m_skill1Pos.gameObject.SetActive(false);
        m_skill2Pos.gameObject.SetActive(false);
    }
    #endregion
   
    public override void BurnDamage() //ó���� ��ø �ȵǰ� �Ϸ� ������ �뷱���� ��ø �����ϰ� ����(��ġ����)
    {
        StartCoroutine(Couroutine_BurnDamage(m_burnDamage / 10));
    }
    public override void SetDamage(AttackType type, float damage, PlayerController player, bool isburn, IDamageAbleObject obj) //�ǰ� ���μ���
    {
   
        if (m_status.hp <= 0) //hp�� 0�����϶��� ���� x // �ٷ� ������ �Ͽ��µ� �׾ ���״� ���� �߻� �ٽ� ������.
        {
            SetDie();
            return;
        }
        int dmg = Mathf.CeilToInt(damage); // �������� ��Ʈ�� �ٲپ� �ޱ�
        DamageProcess(dmg, type);
        if (isburn)
        {
            BurnDamage();
        }
        if(player != null)
        {
            if (player.GetStatus.KnockBackPer > Random.Range(0, 100 + m_status.KnockbackRigist)) //�˹��� �ɶ��� �Ʒ� ���� �˹���� ������ �����ϰ� �����ϵ���!
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
        if (m_status.hp <= 0) //���ظ� ���� �� �ǰ� 0 �����϶� ���ó��
        {
            obj.KillCount();
            m_hudPanel.Died();
            SetDie();
        }
    }
    void SetAttack() //���� ����.
    {
        SetState(MonsterState.Attack); //���� ���·� ����
        if (isSkill())  //40�ۼ�Ʈ Ȯ���� ��ų �����ϵ���.
        {
            if (Random.Range(0, 3) <= 1) //��ų 1�� 2�� �������� ����.
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
        else
        { //�Ϲ� ���� ����
          //  m_navAgent.ResetPath();
            PlaySwingSound();
            m_animctr.Play(MonsterAnimController.Motion.Attack);
        }
    }
    #region SFXPlay
    public override void PlayHitSound(string sound) //�ǰݽ� �Ҹ� ���
    {
        SoundManager.Instance.PlaySFX(sound, m_source);
        SoundManager.Instance.PlaySFX(m_status.hitSound, m_source);
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
    public override void BehaviourProcess() //���ѻ��¸ӽ� Ȱ���� ���� ����.
    {
        if (!gameObject.activeSelf || m_state.Equals(MonsterState.Die)) return; //�����ְų� ��������̸� ���� X
        RageCool += Time.deltaTime;
        timeafterAttack += Time.deltaTime;
        if(!isRage && m_status.hp / m_status.hpMax < 0.5 && RageCool > 20 && m_state != MonsterState.Attack) //�г����� ����.
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

                if (m_idleTime >= m_idleDuration) //���ð��� �ʰ��Ѵٸ�
                {
                    m_idleTime = 0f; //���ð� �ʱ�ȭ
                    if (FindTarget(30)) //���� ������ �Ÿ��� �÷��̾ �ִٸ�.
                    {
                        if (CheckArea(m_status.attackDist)) // �����Ÿ� ���� ��ġ�� ������
                        {
                            if (m_status.atkSpeed <= timeafterAttack)
                            {
                                transform.forward = GetTargetDir();//�÷��̾� ���� ���ֱ�
                                timeafterAttack = 0f;
                                m_navAgent.ResetPath();
                                SetAttack();
                            }
                            return;
                        }
                        SetState(MonsterState.Chase); //���¸� �������� ����
                        m_animctr.Play(MonsterAnimController.Motion.Chase);
                        m_navAgent.isStopped = false; //�ٽ� �����̱� ����s
                        m_navAgent.stoppingDistance = m_status.attackDist - 1f; //�����Ÿ� �ȿ� ������ ���ߵ���
                        StartCoroutine(Coroutine_SerchTargetPath(10));
                        return;
                    }
                    else
                    {
                        m_navAgent.isStopped = false; //�ٽ� �����̱� ����
                        SetState(MonsterState.Chase); //���¸� �������� ����
                        m_animctr.Play(MonsterAnimController.Motion.Chase);
                        m_navAgent.stoppingDistance = m_status.attackDist - 1f;
                        StartCoroutine(Coroutine_SerchTargetPath(60)); //�Ÿ��ȿ� ���� ���ٸ� ���� �������� ���� ����
                    }
                }
                break;
                
            case MonsterState.Chase:
                if (CheckArea(m_navAgent.stoppingDistance))
                {
                    SetIdle(0.05f);
                }
                break;
        }
    }
    protected override void SetTransform() // ��ġ ����
    {
        base.SetTransform();
        m_skill1Pos = Utill.GetChildObject(gameObject, "AttackArea");
        m_skill = GetComponent<ProjectileController>();
        m_skill1Pos.gameObject.SetActive(false);
        m_skill2Pos = Utill.GetChildObject(gameObject, "Circle");
        m_skill2Pos.gameObject.SetActive(false);
        RageCool = 15;
    }
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion
}
