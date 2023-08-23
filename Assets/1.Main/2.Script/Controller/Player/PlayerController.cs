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


public class PlayerController : MonoBehaviour ,IDamageAbleObject
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
    [SerializeField]
    QuickSlot m_quickSlot;
    TableSkillStat m_skillStat;
    WearArmorData m_armorData;
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
    PlayerObjectController m_playerObject;
    [SerializeField]
    GameObject m_area;
    [SerializeField]
    Vector3 m_dir;

    int m_comboIndex;
    int m_experience;
    int m_levelexp;
    int m_score;
    int killCount = 0;
    string m_knickName;
    string m_title = "";
    bool m_hastitle = false;
    bool m_skillactive = false;
    bool m_isactive = false;
    float m_stamina = 10f;
    bool isbuild = false;
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
    #region Methods

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

    // ���� ���� �޼ҵ�.
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
    //��Ȱ ���� �̺�Ʈ���� ĳ���͸� ���� ���·� �������.
    IEnumerator Coroutine_Invincible(float time)
    {
        m_Pstate = PlayerState.Invincible;
        //meleeState = MeleeState.Ready;
        yield return new WaitForSeconds(time);
        m_Pstate = PlayerState.alive;
    }
    //��ų ����� �� ������ ����� �ڷ�ƾ
    IEnumerator Coroutine_SustainedHeal() 
    {
        while (true)
        {
            SkillHeal(Mathf.CeilToInt(m_status.SkillHeal * m_status.hpMax / 100));
            yield return new WaitForSeconds(2);
        }
    }
   
    Coroutine CheckCoroutine;
    #endregion

    #region AnimEvent // �ִϸ��̼� �̺�Ʈ
    // ��� �̺�Ʈ
    void AnimEvent_Dead()
    {
        GameManager.Instance.PlayerDeath(this);
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
                    mon.Crush(m_skillStat.Crush);
                }
                mon.SetDamage(type, damage, this, false, this);
                if (m_meleetype.Equals(MeleeType.Axe))
                    mon.PlayHitSound(m_weaponData.AtkSound);
                else
                    mon.PlayHitSound(m_weaponData.AtkSound);
                if (m_status.Drain != 0)
                {
                    //Debug.Log(Mathf.CeilToInt((damage * m_status.Drain) / 100) + " ȸ��!!");
                    SkillHeal(Mathf.CeilToInt((damage * m_status.Drain) / 100));
                }

            }
        }
    }

    //���ڱ� �Ҹ� ����� �̺�Ʈ.
    void AnimEvent_FootStep() 
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
   
    //������ �������. 8���� �ٸ���� �ٸ������� ����ȭ
    void MoveAnimCtr(Vector3 dir) 
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
    //�÷��̾� ���� �ü�ó��
    public void PlayerLookAt() 
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
    //�÷��̾ óġ�� ���� ���� ī��Ʈ
    public void KillCount()
    {
        killCount++;
    } 
    //���������� ������Ʈ �Ǽ��� �������� �� ȣ��. ���� ������ ���� --
    public void ObjcetBuildSuccesed(int num)
    {
        if(num == 2)
        {
            m_quickSlot.UseQuickSlotITem(2, "Barricade");
        }
        else if(num == 3)
        {
            m_quickSlot.UseQuickSlotITem(3, "Turret");
        }
    } 
    // ���� ���¸� �Ǽ� / ���� ��ȯ
    public void BuildingConvert()
    {
        isbuild = !isbuild;
    }
    
    //�÷��̾��� ��ü���� ������ ����ϴ� �޼ҵ�
    public void BehaviorProcess()
    {
        // ����� �۵� x
        if (m_Pstate.Equals(PlayerState.dead)) return;

        // ���� ���°� ������Ʈ ��ġ���� �� ����.
        if (isbuild)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                ObjectManager.Instance.RotationChanger();
            }
        }

        // �÷��̾� ��ųâ
        if (Input.GetKeyDown(KeyCode.K)) 
        {    
            m_skillactive = !m_skillactive;
            UGUIManager.Instance.SkillUIChange(m_skillactive,m_skill);
            UGUIManager.Instance.PlayClickSFX();
        }

        // �κ��丮 �¿���
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            m_isactive = !m_isactive; //�Ұ����� ��Ƽ�� ����. 
            UGUIManager.Instance.GetStatusUI().SetActive(m_isactive);
            UGUIManager.Instance.PlayClickSFX();
        }

        // ���޻��� ���
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            if (GetHPValue() >= 0.95)
            {
                UGUIManager.Instance.SystemMessageItem("HealPack");
                return;
            }
            m_quickSlot.UseQuickSlotITem(1, "HealPack");
        }

        // �ٸ����̵� ��ġ
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            if (m_quickSlot.CheckItemCount(2) && m_playerObject.IsCanBuildObject(2))
            {
                BuildingConvert();
                if (isbuild)
                {
                    ObjectManager.Instance.SetPreviewObject(3);
                    ObjectManager.Instance.StartPreviewBuild();
                }
                else
                {
                    ObjectManager.Instance.StopBuilding();
                }
            }
        }

        // ��ž ��ġ
        if (Input.GetKeyDown(KeyCode.Alpha4)) 
        {
            if (m_quickSlot.CheckItemCount(3) && m_playerObject.IsCanBuildObject(3))
            {
                BuildingConvert();
                if (isbuild)
                {
                    ObjectManager.Instance.SetPreviewObject(4);
                    ObjectManager.Instance.StartPreviewBuild();
                }
                else
                {
                    ObjectManager.Instance.StopBuilding();
                }
            }
        }

        // ���콺 Ŭ���� Ȱ���� ����, ������Ʈ ��ġ
        if (Input.GetMouseButton(0))
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
                    ObjectManager.Instance.BuildObject();
                }
            } 
        }
        // �ѱ� ������� �� ������ �޼ҵ� ����.
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
        //�޸��� ��� �׽�Ʈ
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            m_stamina -= 2 * Time.deltaTime;
            if(m_stamina < 0)
            {
                m_stamina = 0;
                return;
            }
            m_dir = new Vector3(m_dir.x * 1.5f, 0f, m_dir.z * 1.5f);
        }
        else
        {
            m_stamina += Time.deltaTime;
        }
        MoveAnimCtr(m_dir); //����, �ӵ��� �ִϸ��̼� ����
        PlayerLookAt();     //�÷��̾� ���콺���� �ֽñ��
        m_navAgent.Move(m_dir * m_status.speed/20 * Time.deltaTime); //���������� �̵��� ��Ű�� �ڵ�. �׺�޽�������Ʈ�� Ȱ���Ͽ� ������ ó��.
    }

    #endregion

    #region AboutStatus  //�÷��̾� �������ͽ� ���� �޼ҵ�
    //�� ������. ������ ���� ���� �ӽ� ó�� �Ͽ���. ���� ���� �ؾ���.

    public void SetArmData(WearArmorData data) //�� ���� ���� �� �÷��̾� ���� ����
    {
        m_armorData = data;
        SetStatus(m_weaponData.ID);
    }
    public void SetSkillData(TableSkillStat stat) //�÷��̾ ��ų�� Ȱ��ȭ ���� �� ȣ��. ���������� �������� ����.
    {
        m_skillStat = stat;
        InitStatus();
        if (stat.Crush > 0)
        {
            crush = true;
        }
        else
        {
            crush = false;
        }
        if (m_status.SkillHeal > 0)// ��ų�� �������� 0 ���� ũ�ٸ� �� �ڷ�ƾ ����.
        {
            if (CheckCoroutine != null) //������ �������̶�� ���߰� �ٽý���
            {
                StopCoroutine(CheckCoroutine);
            }
            CheckCoroutine = StartCoroutine(Coroutine_SustainedHeal());
        }
    }
    public void LevelUp() //�÷��̾� ������
    {
        m_status.level++;
    }
    public int GetScore() //�÷��̾ ȹ���� ���� ���� ��������
    {
        return m_score;
    }
    public void AddScore(int score) //���� �߰�.
    {
        m_score += score;
        UIManager.Instance.ScoreChange(m_score);
    }
    void HPControl(int value) //HP ��Ʈ�� �޼ҵ�
    {
        m_status.hp += value;   
        if(m_status.hp > m_status.hpMax) 
            m_status.hp = m_status.hpMax; 
        if(m_status.hp < 0)
            m_status.hp = 0;
        UIManager.Instance.HPBar(m_status.hp,m_status.hpMax);
    }
    void InitStatus() 
    {
        float penaltyRemove = m_weaponData.Speed;
        float hpvalue = 1; //HP��� �� ���� hp�� value���� �������� ���� �����ϵ��� �ϱ� ����
        if (m_skillStat.Remove != 0 && m_weaponData.Speed < 0) //��ų �����ͷ� �̵��ӵ� �г�Ƽ���Ұ� �ְ� ������ �̼Ӱ��� ȿ���� ���� ��� �̼ӿ÷���
        {
            penaltyRemove = penaltyRemove - (penaltyRemove * m_skillStat.Remove);
        }
        hpvalue = m_status.hp / m_status.hpMax;
        if(m_skillStat.CyberWear > 0)
        m_status = new Status(200 * (1 + (m_skillStat.HP + m_weaponData.HP + m_armorData.HP + (m_skillStat.ObjectHP * (1 + m_skillStat.CyberWear)))) * hpvalue, 200 * (1 + (m_skillStat.HP + m_weaponData.HP + m_armorData.HP + (m_skillStat.ObjectHP *(1 + m_skillStat.CyberWear)))), 10f + m_skillStat.CriRate + m_weaponData.CriRate + m_armorData.CriRate, 50f + (m_skillStat.CriDamage + m_weaponData.CriRate), 0 + (m_skillStat.AtkSpeed + m_weaponData.AtkSpeed) * (1 + m_armorData.AttackSpeed), (1 + (m_skillStat.Damage + m_armorData.Damage + m_skillStat.publicBuffDamage * (1 + m_skillStat.CyberWear))) * m_weaponData.Damage, 0 + m_skillStat.Defence + m_weaponData.Defence + m_armorData.Defence + m_skillStat.ObjectDefence * (1 + m_skillStat.CyberWear), 130 * (1 + m_skillStat.Speed + penaltyRemove + m_armorData.Speed), m_weaponData.Mag * Mathf.CeilToInt(1 + m_skillStat.Mag), m_weaponData.ReloadTime - (m_weaponData.ReloadTime * m_skillStat.Reload), m_weaponData.KnockBack + m_skillStat.KnockBackRate, m_weaponData.KnockBackDist, m_weaponData.AttackDist, m_weaponData.Shotgun,m_status.level, m_skillStat.DamageRigist + m_skillStat.ObjectRigist * (1 + m_skillStat.CyberWear), m_skillStat.LastFire, m_skillStat.Pierce, m_skillStat.Boom, m_skillStat.ArmorPierce+ m_skillStat.BuffArmorPierce * (1 + m_skillStat.CyberWear), m_skillStat.Remove, m_skillStat.Drain, m_skillStat.Crush, m_skillStat.Burn, m_skillStat.Heal + m_skillStat.ObjectRegen * (1 + m_skillStat.CyberWear), m_knickName, m_title);
        else
        m_status = new Status(200 * (1 + (m_skillStat.HP + m_weaponData.HP + m_armorData.HP)) * hpvalue, 200 * (1 + (m_skillStat.HP + m_weaponData.HP + m_armorData.HP)), 10f + m_skillStat.CriRate+ m_weaponData.CriRate + m_armorData.CriRate, 50f + (m_skillStat.CriDamage + m_weaponData.CriRate), 0 + (m_skillStat.AtkSpeed + m_weaponData.AtkSpeed) * (1 + m_armorData.AttackSpeed),(1+ (m_skillStat.Damage + m_armorData.Damage + m_skillStat.publicBuffDamage)) * m_weaponData.Damage, 0 + m_skillStat.Defence + m_weaponData.Defence + m_armorData.Defence, 130 * (1 + m_skillStat.Speed +penaltyRemove + m_armorData.Speed), m_weaponData.Mag * Mathf.CeilToInt(1+ m_skillStat.Mag), m_weaponData.ReloadTime - (m_weaponData.ReloadTime * m_skillStat.Reload) ,m_weaponData.KnockBack + m_skillStat.KnockBackRate, m_weaponData.KnockBackDist,m_weaponData.AttackDist,m_weaponData.Shotgun, m_status.level, m_skillStat.DamageRigist, m_skillStat.LastFire, m_skillStat.Pierce, m_skillStat.Boom, m_skillStat.ArmorPierce + m_skillStat.BuffArmorPierce , m_skillStat.Remove, m_skillStat.Drain, m_skillStat.Crush, m_skillStat.Burn, m_skillStat.Heal, m_knickName,m_title);
        HPControl(0); //�ٲ� hp���� UI�� �����ֱ� ���� 0�� ������
        SetHudText(); //���� ������ ���� ���� ���ε�
        SetAnimSpeed(); //ĳ������ �ӵ��� ����Ͽ��ִϸ��̼� �ӵ� ������
        m_area.transform.localScale = new Vector3(m_status.AtkDist, 1f, m_status.AtkDist);
    }
    void SetAnimSpeed()
    {
        m_animCtr.SetFloat("MeleeSpeed", m_status.atkSpeed);
        m_animCtr.SetFloat("MoveSpeed", m_status.speed / 100);
    }
    public void SetStatus(int ID) //���� ���� �� ����ϴ� �޼ҵ�� ������ �����͸� �޾ƿ��� �� ���ݼ���
    {
        SetWeaponID(ID);
        InitStatus();
    }   
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
    //�ٸ� Ŭ������ �÷��̾���Ʈ�ѷ� ���� ����
    void SetPlayer()
    {
        UGUIManager.Instance.SetPlayer(this);
        UpdateManager.Instance.SetPlayer(this); 
        GameManager.Instance.SetGameObject(gameObject);
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
    public void SetDamage(float damage,MonsterController mon)
    {
        if (m_status.hp <= 0 || m_Pstate == PlayerState.dead || m_Pstate == PlayerState.Invincible) //�ǰ� 0�̰ų� �׾����� ���� X
            return;
        damage = CalculationDamage.NormalDamage(damage, m_status.defense, 0f, m_status.DamageRigist);
        PlayDamagedSound();
        int mondamage = Mathf.CeilToInt(damage - (damage * m_status.DamageRigist)); //���� ���� ����
        HPControl(-mondamage);
        var hiteffect = TableEffect.Instance.m_tableData[6].Prefab[2];
        var effect = EffectPool.Instance.Create(hiteffect);
        effect.transform.position = m_hitPos.position;
        effect.SetActive(true);
        m_hudText.Add(-damage, Color.red, 0f);
        UIManager.Instance.DamagedUI();
        if (m_status.hp <= 0)
        {
            Dead(); //hp�� 0�����϶� ���ó��
        }
    }
    void SkillHeal(int healvalue) // ��ų �� ���������� ȸ���ϱ� ���� ����ϴ� �޼ҵ�.
    {
        HPControl(healvalue);
        m_hudText.Add(healvalue, Color.green, 0f);
    }
    public void GetHeal(float heal) //
    {
        if (m_status.hp >= m_status.hpMax || m_Pstate == PlayerState.dead) return; //�̹� Ǯ���̰ų� �׾������ ���� X
        float percentHeal = m_status.hpMax * (heal / 100);
        int healvalue = Mathf.CeilToInt(percentHeal);
        PlayHealSound();
        HPControl(healvalue);
        m_hudText.Add(healvalue, Color.green, 0f);
        var healeffect = TableEffect.Instance.m_tableData[6].Prefab[1];
        var effect = EffectPool.Instance.Create(healeffect);
        effect.transform.position = gameObject.transform.position;
        effect.SetActive(true);
        UIManager.Instance.HealUI();
        if (m_status.hp >= m_status.hpMax) //�ִ�ü�� �ʰ��� ���� ���� X
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
        GameManager.Instance.SetGameObject(gameObject);
    }

    void LevelUP()
    {
        PlayLvUpSound();
        LevelUp();
        m_levelexp = Levelexp();
        HPControl(Mathf.CeilToInt((m_status.hpMax - m_status.hp) / 2)); //Ǯ�Ƿ� ������� �� �ʹ� ����̹Ƿ� ���� ������ 50%�� ����.
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
    public void SetHudText() //HUD�� �г���, ����, Ÿ��Ʋ ǥ��
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
    
    void SetTransform()
    {
        m_skill = GetComponent<PlayerSkillController>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_audio = GetComponent<AudioSource>();
        m_animCtr = GetComponent<PlayerAnimController>();
        m_playerObject = GetComponent<PlayerObjectController>();
        m_leftDir = Utill.GetChildObject(gameObject, "LeftDir");
        m_weaponData = new WeaponData();
        m_armorManager = GetComponent<ArmorManager>();
        m_manager = GetComponent<GunManager>();
        m_hitPos = Utill.GetChildObject(gameObject, "Dummy_Pos");
        m_armorData = new WearArmorData();
        m_shooter = GetComponent<PlayerShooter>();
    }
    #endregion

    #region UnityMethods

    
    private void Awake()
    {
        SetTransform();
        SetPlayerKnickName(GameManager.Instance.GetNickname());
    }
    private void Start()
    {
        SetPlayer();
        SetStatus(1); // ���۽� �⺻ ��������
        m_status.hp = m_status.hpMax; //���� �� hp ����.
        m_status.level = 1;
        m_levelexp = Levelexp();
        HPControl(0);
        SetHudText();
    }
    #endregion

    #endregion
}

