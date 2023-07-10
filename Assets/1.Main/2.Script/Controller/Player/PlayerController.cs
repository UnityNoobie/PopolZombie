using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Gun;
using static InvBaseItem;
using static PlayerStriker;
using static UnityEditor.Experimental.GraphView.GraphView;


public class PlayerController : MonoBehaviour
{

    #region Constant and Fields

    public ArmorManager m_armorManager { get; set; }
    [SerializeField]
    HUDText m_hudText;
    [SerializeField]
    UILabel m_hudLabel;
    [SerializeField] //�÷��̾��� �������ͽ� üũ
    Status m_status;
    [SerializeField]
    GameObject m_PlayerHuD;
    StatusUI m_statusUI;
    [SerializeField]
    QuickSlot m_quickSlot;
    [SerializeField]
    UpdateManager m_updateManager;
    AudioSource m_audio;
    GunManager m_manager;
    PlayerShooter m_shooter;
    Transform m_leftDir;
    Transform m_hitPos;
    MeleeType m_meleetype;
    Gun m_gun;
    WeaponData m_weaponData { get; set; }
    NavMeshAgent m_navAgent;
    PlayerAnimController m_animCtr;
    PlayerSkillController m_skill;
    [SerializeField]
    GameObject m_area;
    [SerializeField]
    Vector3 m_dir;
    int hp;
    int m_comboIndex;
    int m_experience;
    int m_levelexp;
    int m_score;
    int m_level;
    string m_knickName;
    string m_title = "";
    bool m_hastitle = false;
    bool m_skillactive = false;
    bool m_isactive = false;
    float gameDuration = 0;
    float m_stamina = 10f;
    public float pDefence;
    bool isbuild = false;
    #endregion
    //�� ���� �ӽ�����
    #region ArmorData
    int armDefence;
    float armDamage;
    float hpPer;
    float armAttackSpeed;
    int armCriRate;
    float armSpeed;
    #endregion
    //��ų ���� �ӽ� ����
    #region SkillData
    float skillamage;
    float skillAtkSpeed;
    float skillReload;
    float skillSpeed;
    int skillCriRate;
    float skillCriDamage;
    float skillMag;
    float skillDefence;
    float skillDamageRigist;
    float skillHP;
    float skillKnockBackRate;
    float skillHeal;
    int skillLastFire;
    int skillPierce;
    int skillBoom;
    float skillArmorPierce;
    float skillRemove;
    int skillDrain;
    float skillCrush;
    int skillBurn;
    bool crush;
    #endregion

    #region Property
    public enum PlayerState //�÷��̾��� ���� �˸�
    {
        alive,
        dead,
        Invincible
    }
    public Status GetStatus{get { return m_status; } set { m_status = value; } }
    public MeleeState meleeState { get; set; }
    public PlayerState m_Pstate { get; set; }
    public PlayerAnimController.Motion GetMotion { get { return m_animCtr.GetMotion; } }
    #endregion

    #region PlayerTitle
    public bool HasTitle()
    {
        return m_hastitle;
    }
    public void SetTitle(string title)
    {
        m_title = title;
        m_hastitle = true;
    }
    #endregion

    #region MeleeAttackProcess //��������
    List<PlayerAnimController.Motion> m_comboList = new List<PlayerAnimController.Motion>() { PlayerAnimController.Motion.Combo1, PlayerAnimController.Motion.Combo2 };
    Queue<KeyCode> m_keyBuffer = new Queue<KeyCode>(); // ���� ���� �޺��ý��� ������ ���� ť
    /*
    public bool IsAttack
    {
        get
        {
            if (GetMotion == PlayerAnimController.Motion.Combo1 ||
                GetMotion == PlayerAnimController.Motion.Combo2)
                return true;
            return false;
        }
    }*/
    public void SetAttack()
    {
       if(m_Pstate != PlayerState.dead &&  GetMotion.Equals(PlayerAnimController.Motion.MeleeIdle) || m_Pstate != PlayerState.dead && GetMotion.Equals(PlayerAnimController.Motion.Idle))
        { // �غ�����̰� Idle�̳� MeleeIdle�̶�� �޺�1 ���� .meleeState.Equals(MeleeState.Ready)&&
            meleeState = MeleeState.Attack;
            m_animCtr.Play(PlayerAnimController.Motion.Combo1);
        }
        else
        {
           m_keyBuffer.Enqueue(KeyCode.Mouse0); //Ű���ۿ� �־��ֱ�
            if (IsInvoking("ReleaseKeyBuffer")) // Ű���� ����������̶�� ������ֱ�.
            {   
                CancelInvoke();
            }
            Invoke("ReleaseKeyBuffer", 1 / m_status.atkSpeed); // ���ݼӵ��� ����� �ð� ���� Ű���� ����ó��.
        }
    }
    #endregion

    #region Coroutine  //�ڷ�ƾ
    IEnumerator Coroutine_Invincible(float time)
    {
        m_Pstate = PlayerState.Invincible;
        //meleeState = MeleeState.Ready;
        yield return new WaitForSeconds(time);
        m_Pstate = PlayerState.alive;
    }

    IEnumerator Coroutine_SustainedHeal() //��ų ����� �� ��������
    {
        while (true)
        {
            SkillHeal(Mathf.CeilToInt(skillHeal * m_status.hpMax / 100));
            yield return new WaitForSeconds(2);
        }
    }
    Coroutine CheckCoroutine;
    #endregion

    #region AnimEvent // �ִϸ��̼� �̺�Ʈ
    // ��� �̺�Ʈ
    void AnimEvent_Dead()
    {
        gameObject.SetActive(false);
        m_PlayerHuD.SetActive(false);
    }
    //���� ���� ���� �̺�Ʈ
    void AnimEvent_AttackStart()
    {
        m_area.SetActive(true);
        PlaySwingSound();
    }
    //���� ���� �̺�Ʈ
    void AnimEvent_GrabWeapon()
    {
        m_gun.gunstate = GunState.Ready;
    }
    //���� ���� ���μ��� �����ϴ� �̺�Ʈ
    void AnimEvent_MeleeAttack()
    {

        float damage = 0;
        var area = m_area.GetComponent<AttackAreaUnitFind>();
        var areaList = area.m_unitList;
        for (int i = 0; i < areaList.Count; i++)
        {         
            if (areaList[i].CompareTag("Zombie"))
            {
                var mon = areaList[i].GetComponent<MonsterController>();
                var type = GunManager.AttackProcess(mon, m_status.damage, m_status.criRate, m_status.criAttack, m_status.ArmorPierce, out damage);
                if (crush)//�м⽺ų�� ����������..
                {
                    mon.Crush(skillCrush);
                }
                mon.SetDamage(type, damage, this, false);
                if (m_meleetype.Equals(MeleeType.Axe))
                    mon.PlayHitSound("SFX_AxeHit");
                else
                    mon.PlayHitSound("SFX_BatHit");
                if (m_status.Drain != 0)
                {
                    //Debug.Log(Mathf.CeilToInt((damage * m_status.Drain) / 100) + " ȸ��!!");
                    SkillHeal(Mathf.CeilToInt((damage * m_status.Drain) / 100));
                }

            }
        }
    }
    void AnimEvent_FootStep() //���ڱ� �Ҹ� �����. ����Ʈ�� ��� �����̶� �����ߴµ� ��, �� �̵��� ����ϴ� �������� �۵��Ͽ���.
    {
        SoundManager.Instance.PlaySFX("SFX_PlayerFootStep", m_audio);
    }
    //���� ���� ���� �� ����Ǵ� �ڵ�
    public void AnimEvnet_MeleeFinished()
    {
        m_area.SetActive(false);
        m_area.GetComponent<AttackAreaUnitFind>().Resetlist();
        meleeState = MeleeState.Ready;
        bool isCombo = false;
        if (m_keyBuffer.Count > 0)
        {
            var key = m_keyBuffer.Dequeue();
            isCombo = true;
            if (m_keyBuffer.Count > 0)
            {
                isCombo = false;
                ReleaseKeyBuffer();
            }
        }
        if (isCombo)
        {
            m_comboIndex++;
            if (m_comboIndex >= m_comboList.Count)
            {
                m_comboIndex = 0;
            }
            m_animCtr.Play(m_comboList[m_comboIndex]);
        }
        else
        {
            m_comboIndex = 0;
            m_animCtr.Play(PlayerAnimController.Motion.MeleeIdle);
        }
    }
    //�޺� ������ ���� Ű���� �ʱ�ȭ
    void ReleaseKeyBuffer()
    {
        m_keyBuffer.Clear();
    }
    //���� �������� �� ����
    public void SetGun(Gun gun)
    {
        m_gun = gun.GetComponent<Gun>();
    }
    #endregion

    #region SFXPlay // SFX��� �޼ҵ�
    void PlayDamagedSound()
    {
        SoundManager.Instance.PlaySFX("SFX_PlayerDamaged", m_audio);
    }
    void PlaySwingSound()
    {
        SoundManager.Instance.PlaySFX("SFX_MeleeSwing", m_audio);
        SoundManager.Instance.PlaySFX("SFX_MeleeAttack", m_audio);
    }
    void PlayHealSound()
    {
        SoundManager.Instance.PlaySFX("SFX_PlayerHeal", m_audio);
    }
    void PlayDieSound()
    {
        SoundManager.Instance.PlaySFX("SFX_PlayerDeath", m_audio);
    }
    void PlayReviveSound()
    {
        SoundManager.Instance.PlaySFX("SFX_PlayerRevive", m_audio);
    }
    void PlayLvUpSound()
    {
        SoundManager.Instance.PlaySFX("SFX_PlayerLevelUp", m_audio);
    }
    #endregion 

    #region PlayerInput  //�÷��̾��� ������ ���� �޼ҵ�

    void SetPlayer()
    {
        UGUIManager.Instance.SetPlayer(this);
        m_updateManager.SetPlayerController(this);
        GameManager.Instance.SetGameObject(gameObject);
    }
    void MoveAnimCtr(Vector3 dir) //������ �������. 8���� �ٸ���� �ٸ������� ����ȭ�غ�����!
    {
        var fdot = Vector3.Dot(gameObject.transform.forward, dir.normalized);
        var dot = Vector3.Dot(m_leftDir.forward, dir.normalized);
        float force = 0f;
        if(dir != Vector3.zero)
        {
           if(dot > 0f) //����
            {
                force = -dot;
                m_animCtr.SetFloat("Horizontal", force);
            }
            else//������
            {
                force = -dot;
                m_animCtr.SetFloat("Horizontal", force);
            }
           if(fdot > 0f) //����
            {
                force = fdot;
                m_animCtr.SetFloat("Vertical", force);
            }
            else //����
            {
                force = fdot;
                m_animCtr.SetFloat("Vertical", force);
            }
        }
        else
        {
            m_animCtr.SetFloat("Horizontal", 0f);
            m_animCtr.SetFloat("Vertical", 0f);
        }
        
    }
    public void PlayerLookAt() //�÷��̾� ���� �ü�ó��
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
        }
    }
    public void BehaviorProcess()
    {
        if (m_Pstate.Equals(PlayerState.dead)) return; // ����� �۵� x
        if (isbuild)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                ObjectManager.Instance.RotationChanger();
            }
        }
        if (Input.GetKeyDown(KeyCode.K)) //�÷��̾ ��ųâ�� �����ϱ� ����
        {    
            m_skillactive = !m_skillactive;
            UGUIManager.Instance.SkillUIChange(m_skillactive,m_skill);
            UGUIManager.Instance.PlayClickSFX();
        }
        if (Input.GetKeyDown(KeyCode.I)) //�κ��丮 �¿���
        {
            m_isactive = !m_isactive; //�Ұ����� ��Ƽ�� ����. 
            UGUIManager.Instance.GetStatusUI().SetActive(m_isactive);
            UGUIManager.Instance.PlayClickSFX();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // ���޻��� ���
        {
            if (GetHPValue() >= 0.95)
            {
                UGUIManager.Instance.SystemMessageItem("HealPack");
                return;
            }
            m_quickSlot.UseQuickSlotITem(1, "HealPack");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isbuild = !isbuild;
            if (isbuild)
            {
                ObjectManager.Instance.StartPreviewBuild();
            }
            else
            {
                ObjectManager.Instance.StopBuilding();
            }
            //m_quickSlot.UseQuickSlotITem(2, "Barricade");
        }
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            GameObject hitObject = hit.collider.gameObject;
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) //���콺 �����Ͱ� UI ���� ���� �� ���� ���ϰ� ����� ���.
            {
                if (!isbuild) //�Ǽ����°� �ƴҰ��
                {
                    if (m_manager.IsGun())
                    {
                        m_shooter.AttackProcess();
                    }
                    else
                    {
                        SetAttack();
                    }
                }
                else if(isbuild)
                {
                    ObjectManager.Instance.BuildBarricade();
                    isbuild = false;
                }
                 
            } 
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (m_manager.IsGun())
            {
                m_shooter.ReloadProcess();
            }
        }
        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(m_dir.x !=0 && m_dir.z !=0)
        {
            m_dir = m_dir / 1.5f; //�밢�� �̵� �� �ʹ� ���� ������ �߻��Ͽ� ����
        }
        if (m_dir != Vector3.zero)
        {
            m_navAgent.ResetPath();
        }
        if (Input.GetKey(KeyCode.LeftShift)) //�޸��� ��� �׽�Ʈ
        {
            m_dir = new Vector3(m_dir.x * 1.5f, 0f, m_dir.z * 1.5f);
        }
        MoveAnimCtr(m_dir); //����, �ӵ��� �ִϸ��̼� ����
        PlayerLookAt();     //�÷��̾� ���콺���� �ֽñ��
        m_navAgent.Move(m_dir * m_status.speed/20 * Time.deltaTime);
    }

    #endregion

    #region AboutStatus  //�÷��̾� �������ͽ� ���� �޼ҵ�
    private void ResetData()
    {
        armDefence = 0;
        armDamage = 0;
        hpPer = 0;
        armAttackSpeed = 0;
        armCriRate = 0;
        armSpeed = 0;
    }
    public void SetArmData(int defence, float damage, float hp, float attackSpeed, int criRate, float speed)
    {
        ResetData();
        armDefence = defence;
        armDamage = damage;
        hpPer = hp;
        armAttackSpeed = attackSpeed;
        armCriRate = criRate;
        armSpeed = speed;
        SetStatus(m_weaponData.ID);
    }
    public void SetSkillData(float damage, float atkspeed, float reload, float speed, int crirate, float cridamage, float mag, float defence, float damagerigist, float hp, float knockbackrate, float heal, int lastfire, int pierce, int boom, float armorPierce, float Remove, int Drain, float Crush, int Burn)
    {
        skillamage = damage;
        skillAtkSpeed = atkspeed;
        skillReload = reload;
        skillSpeed = speed;
        skillCriRate = crirate;
        skillCriDamage = cridamage;
        skillMag = mag;
        skillDefence = defence;
        skillDamageRigist = damagerigist;
        skillHP = hp;
        skillKnockBackRate = knockbackrate;
        skillHeal = heal;
        skillLastFire = lastfire;
        skillPierce = pierce;
        skillBoom = boom;
        skillArmorPierce = armorPierce;
        skillRemove = Remove;
        skillDrain = Drain;
        skillCrush = Crush;
        skillBurn = Burn;
        if(skillCrush > 0)
        {
            crush = true;
        }
        else
        {
           crush = false;
        }
        if (skillHeal > 0)// ��ų�� �������� 0 ���� ũ�ٸ� �� �ڷ�ƾ ����.
        {
            if (CheckCoroutine != null) //������ �������̶�� ���߰� �ٽý���
            {
                StopCoroutine(CheckCoroutine);
            }
            CheckCoroutine = StartCoroutine(Coroutine_SustainedHeal());
        }
        //CheckBoolin();
    }
    public void LevelUp()
    {
        m_level++;
        m_status.level = m_level;
    }
    public int GetLVInfo()
    {
        return m_level;
    }
    public int GetScore()
    {
        return m_score;
    }
    public void AddScore(int score)
    {
        m_score += score;
        UIManager.Instance.ScoreChange(m_score);
    }
    void HPControl(int value)
    {
        m_status.hp += value;   
        if(m_status.hp > m_status.hpMax) 
            m_status.hp = m_status.hpMax; 
        if(m_status.hp < 0)
            m_status.hp = 0;
        hp = Mathf.CeilToInt(m_status.hp);
        UIManager.Instance.HPBar(m_status.hp,m_status.hpMax);
    }
    void InitStatus(float itemhp, float I_Criper, float I_CriDam, float I_atkSpeed, float I_Atk, float I_Def, float I_speed, int maxammo, float reloadtime,float knockbackPer, float knockbackDist,float atkDist, int shotgun,int level) 
    {//SetStatus���� ��ų�� �������� �߰������� �޾ƿ�.
     //(float skillhp, float itemhp, float S_CriPer, float I_Criper, float S_CriDam, float I_CriDam, float S_atkSpeed, float I_atkSpeed, float S_Atk, float I_Atk, float S_Def, float I_Def, float S_speed, float I_speed, int maxammo, float reloadtime,float knockbackPer, float knockbackDist,float atkDist, int shotgun,int level) 
     //�������ͽ� �Է°��� ���� : �ִ�ü�� , ũ��Ƽ�� Ȯ��, ũ��Ƽ�� �߰� ������,���ݼӵ�, ���ݷ�, ����, �̵��ӵ�
        m_status = new Status(hp,200 * (1 + (skillHP + itemhp + hpPer)), 10f + skillCriRate+ I_Criper + armCriRate, 50f + (skillCriDamage + I_CriDam), 0 + (skillAtkSpeed + I_atkSpeed) * (1 + armAttackSpeed),(1+ (skillamage + armDamage)) * I_Atk, 0 + skillDefence + I_Def + armDefence, 130 * (1 + skillSpeed + I_speed + armSpeed), maxammo * Mathf.CeilToInt(1+skillMag), reloadtime - (reloadtime * skillReload) ,knockbackPer+skillKnockBackRate,knockbackDist,atkDist,shotgun,level,skillDamageRigist,skillLastFire,skillPierce,skillBoom,skillArmorPierce,skillRemove,skillDrain,skillCrush,skillBurn,skillHeal,m_knickName,m_title);
        // m_status.hpMax = Mathf.CeilToInt(m_status.hpMax); //�ִ�ü�� ��������
        HPControl(0);
        m_animCtr.SetFloat("MeleeSpeed", m_status.atkSpeed);
        m_animCtr.SetFloat("MoveSpeed", m_status.speed / 100);
      //  m_inven.GetStatusInfo(this); �� �κ��丮
        m_statusUI.SetStatus(); // �� �κ��丮
        pDefence = m_status.defense;  //���� ��������
        m_area.transform.localScale = new Vector3(m_status.AtkDist, 1f, m_status.AtkDist);
    }
    public void SkillUpInitstatus()
    {
        InitStatus(m_weaponData.HP, m_weaponData.CriRate, m_weaponData.CriDamage, m_weaponData.AtkSpeed, m_weaponData.Damage, m_weaponData.Defence, m_weaponData.Speed, m_weaponData.Mag, m_weaponData.ReloadTime, m_weaponData.KnockBack, m_weaponData.KnockBackDist, m_weaponData.AttackDist, m_weaponData.Shotgun, m_level);
        SetHudText();
    }
    public void SetStatus(int ID)
    {
        SetWeaponID(ID);
        //�������ͽ� �Է°��� ���� : ��ų, ������ �ִ�ü�� ,��ų������ ũ��Ƽ�� Ȯ��, ��ų, ������ ũ��Ƽ�� �߰� ������,��ų ������ ���ݼӵ�, ��ų������ ���ݷ�, ��ų ������ ���� , �̵��ӵ�
        float penaltyRemove = m_weaponData.Speed;
        if (skillRemove != 0 && m_weaponData.Speed < 0) //��ų �����ͷ� �̵��ӵ� �г�Ƽ���Ұ� �ְ� ������ �̼Ӱ��� ȿ���� ���� ��� �̼ӿ÷���
        {
            penaltyRemove = penaltyRemove - (penaltyRemove * skillRemove);
        }
        InitStatus(m_weaponData.HP,  m_weaponData.CriRate, m_weaponData.CriDamage, m_weaponData.AtkSpeed,  m_weaponData.Damage, m_weaponData.Defence, penaltyRemove/*�г�Ƽ���� ��ų�� �ֱ� ������ ���� ����*/, m_weaponData.Mag, m_weaponData.ReloadTime,m_weaponData.KnockBack,m_weaponData.KnockBackDist,m_weaponData.AttackDist,m_weaponData.Shotgun,m_level);
     //   m_animCtr.SetFloat("MoveSpeed", m_status.speed / 150); //�̵��ӵ��� �ٸ������� �ӵ�������.
    }   //�˹��� ������ ���⿡���� ������ �Ͽ� ���� �� �־������!
    void SetWeaponID(int ID)
    {
        m_weaponData = m_weaponData.GetWeaponStatus(ID);
        if (m_weaponData.weaponType.Equals(WeaponType.Axe))
        {
            m_meleetype = MeleeType.Axe;
        }
        else if(m_weaponData.weaponType.Equals(WeaponType.Bat))
        {
            m_meleetype = MeleeType.Bat;
        }
        else
        {
            m_meleetype = MeleeType.Max;
        }
        m_skill.SetWeaponType(m_weaponData.weaponType); //����Ÿ�� ����
    }

    #endregion

    #region HUD && PlayerState  //�÷��̾��� HUD�� State ���� �޼ҵ�

    public void SetPlayerKnickName(string nick)
    {
        m_knickName = nick;
    }
    public float GetHPValue()
    {
        float value = m_status.hp / m_status.hpMax;
        return value;
    }
    public void GetDamage(float damage)
    {
        if (m_status.hp <= 0 || m_Pstate == PlayerState.dead || m_Pstate == PlayerState.Invincible) //�ǰ� 0�̰ų� �׾����� ���� X
            return;
        damage = CalculationDamage.NormalDamage(damage, m_status.defense, 0f);
        PlayDamagedSound();
        int mondamage = Mathf.CeilToInt(damage - (damage*skillDamageRigist)); //���� ���� ����
        HPControl(-mondamage);
        var hiteffect = TableEffect.Instance.m_tableData[6].Prefab[2];
        var effect = EffectPool.Instance.Create(hiteffect);
        effect.transform.position = m_hitPos.position;
        effect.SetActive(true);
        m_hudText.Add(-damage,Color.red,0f);
        UIManager.Instance.DamagedUI();
        if (m_status.hp <= 0)
        {
            Dead(); //hp�� 0�����϶� ���ó��
        }
    }
    void SkillHeal(int healvalue)
    {
        HPControl(healvalue);
        m_hudText.Add(healvalue, Color.green, 0f);
    }
    public void GetHeal(float heal)
    {
        if (m_status.hp >= m_status.hpMax || m_Pstate == PlayerState.dead) return; //�̹� Ǯ���̰ų� �׾������ ���� X
        float percentHeal = m_status.hpMax * (heal / 100);
        PlayHealSound();
        int healvalue = Mathf.CeilToInt(percentHeal);
        HPControl(healvalue);
        m_hudText.Add(healvalue, Color.green, 0f);
        var healeffect = TableEffect.Instance.m_tableData[6].Prefab[1];
        var effect = EffectPool.Instance.Create(healeffect);
        effect.transform.position = gameObject.transform.position;
        effect.SetActive(true);
        UIManager.Instance.HealUI();
        if (m_status.hp >= m_status.hpMax) //�ִ�ü�� �ʰ��� ���� ���� X ���� ȸ���� ��ü ���̴½����� �����Ұ�    *****
        {
            m_status.hp = m_status.hpMax;
        }
    }
    void Dead() //�׾������ �÷��̾� ��Ƽ�� ����
    {
        ReleaseKeyBuffer();
        PlayDieSound();
        GameManager.Instance.DestroyTarget(gameObject);
        if (GetMotion.Equals(PlayerAnimController.Motion.Combo1))
        {
            AnimEvnet_MeleeFinished();
        }
        m_Pstate = PlayerState.dead;
        meleeState = MeleeState.Dead;
        m_animCtr.Play("Die");
        UIManager.Instance.DiedUI();
    }
    public void Revive() //��Ȱ���
    {
        PlayReviveSound();
        HPControl(Mathf.CeilToInt(m_status.hpMax));
        Debug.Log(m_status.atkSpeed);
        gameObject.SetActive(true);
        m_PlayerHuD.gameObject.SetActive(true);
        UIManager.Instance.ReviveUI();
        StartCoroutine(Coroutine_Invincible(2f));
        var reviveeffect = TableEffect.Instance.m_tableData[6].Prefab[0];
        var effect = EffectPool.Instance.Create(reviveeffect);
        effect.transform.position = gameObject.transform.position;
        effect.transform.localScale = new Vector3(2f, 2f, 2f);
        effect.SetActive(true);
        SetStatus(m_weaponData.ID); //�������ͽ� ������ �մϴ���
    }

    void LevelUP()
    {
        PlayLvUpSound();
        LevelUp();
        m_levelexp = Levelexp();
        HPControl(Mathf.CeilToInt(m_status.hpMax)); //Ǯ�Ƿ� �������
        UIManager.Instance.LevelUPUI(m_status.level);
        SetHudText();
        m_skill.LevelUP();
        StartCoroutine(Coroutine_Invincible(2f)); //�����ù���
        var levelupEffect = TableEffect.Instance.m_tableData[6].Prefab[0];
        var effect = EffectPool.Instance.Create(levelupEffect);
        effect.transform.position = gameObject.transform.position;
        effect.transform.localScale = new Vector3(2f, 2f, 2f);
        effect.SetActive(true);
    }   
    public void SetHudText()
    {
        if (HasTitle())
        {
            m_hudLabel.text = "[00FFFF][" + m_status.Title + "]\n[FFFF00]LV" + m_status.level + "[FFFFFF]" + m_status.KnickName;
        }
        else
        {
            m_hudLabel.text = "[FFFF00]LV." + m_status.level + "[FFFFFF]" + m_knickName;
        }
    }
    void SetUIInfo()
    {
        m_statusUI = UGUIManager.Instance.GetStatusUI();
    }

    #endregion

    #region Level,Exp  //����ġ, ������ ���� �޼ҵ�
    public void IncreaseExperience(int exp)
    {
        m_experience += exp;
        if (m_experience >= m_levelexp)
        {
            m_experience -= m_levelexp;
            LevelUP();
        }
        UIManager.Instance.EXPUI(m_experience, m_levelexp);
    } 
    int Levelexp()
    {
        int lvupexp = 100 + (80 * m_status.level);
        return lvupexp;
    }

    #endregion

    #region UnityMethods

    
    private void Awake()
    {
        m_skill = GetComponent<PlayerSkillController>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_audio = GetComponent<AudioSource>();
        m_animCtr = GetComponent<PlayerAnimController>();
        m_leftDir = Utill.GetChildObject(gameObject, "LeftDir");
        m_weaponData = new WeaponData();
        m_armorManager= GetComponent<ArmorManager>();
        m_manager = GetComponent<GunManager>();
        m_hitPos = Utill.GetChildObject(gameObject, "Dummy_Pos");
        m_level = 1;
        m_levelexp = Levelexp();
        m_shooter = GetComponent<PlayerShooter>();
        SetPlayerKnickName(GameManager.Instance.GetNickname());
    }
    private void Start()
    {
        SetPlayer();
        SetUIInfo();
        SetStatus(1); // ���۽� �⺻ ��������
        m_status.hp = m_status.hpMax; //���� �� hp ����.
        m_status.level = 1;
        HPControl(0);
        SetHudText();
    }
    private void Update()
    {
        BehaviorProcess();
    }
    #endregion
}

