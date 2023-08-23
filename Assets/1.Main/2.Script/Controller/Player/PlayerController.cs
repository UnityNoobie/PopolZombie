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
    [SerializeField] //플레이어의 스테이터스 체크
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
    public enum PlayerState //플레이어의 상태 알림
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

    #region MeleeAttackProcess //근접공격
    List<PlayerAnimController.Motion> m_comboList = new List<PlayerAnimController.Motion>() { PlayerAnimController.Motion.Combo1, PlayerAnimController.Motion.Combo2 };
    Queue<KeyCode> m_keyBuffer = new Queue<KeyCode>(); // 근접 공격 콤보시스템 구현을 위한 큐
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

    // 근접 공격 메소드.
    public void SetAttack()
    {
       if(m_Pstate != PlayerState.dead &&  GetMotion.Equals(PlayerAnimController.Motion.MeleeIdle) || m_Pstate != PlayerState.dead && GetMotion.Equals(PlayerAnimController.Motion.Idle))
        { // 준비상태이고 Idle이나 MeleeIdle이라면 콤보1 실행 .meleeState.Equals(MeleeState.Ready)&&
            meleeState = MeleeState.Attack;
            m_animCtr.Play(PlayerAnimController.Motion.Combo1);
        }
        else
        {
           m_keyBuffer.Enqueue(KeyCode.Mouse0); //키버퍼에 넣어주기
            if (IsInvoking("ReleaseKeyBuffer")) // 키버퍼 삭제대기중이라면 취소해주기.
            {   
                CancelInvoke();
            }
            Invoke("ReleaseKeyBuffer", 1 / m_status.atkSpeed); // 공격속도에 비례한 시간 이후 키버퍼 삭제처리.
        }
    }
    #endregion

    #region Coroutine  //코루틴
    //부활 등의 이벤트에서 캐릭터를 무적 상태로 만들어줌.
    IEnumerator Coroutine_Invincible(float time)
    {
        m_Pstate = PlayerState.Invincible;
        //meleeState = MeleeState.Ready;
        yield return new WaitForSeconds(time);
        m_Pstate = PlayerState.alive;
    }
    //스킬 찍었을 시 지속힐 적용용 코루틴
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

    #region AnimEvent // 애니메이션 이벤트
    // 사망 이벤트
    void AnimEvent_Dead()
    {
        GameManager.Instance.PlayerDeath(this);
        gameObject.SetActive(false);
        m_PlayerHuD.SetActive(false);
    }
    //근접 공격 시작 이벤트
    void AnimEvent_AttackStart()
    {
        m_area.SetActive(true);
        PlaySwingSound();
    }
    //무기 변경 이벤트
    void AnimEvent_GrabWeapon()
    {
        m_gun.gunstate = GunState.Ready;
    }
    //근접 공격 프로세스 실행하는 이벤트
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
                if (crush)//분쇄스킬이 찍혀있으면..
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
                    //Debug.Log(Mathf.CeilToInt((damage * m_status.Drain) / 100) + " 회복!!");
                    SkillHeal(Mathf.CeilToInt((damage * m_status.Drain) / 100));
                }

            }
        }
    }

    //발자국 소리 재생용 이벤트.
    void AnimEvent_FootStep() 
    {
        SoundManager.Instance.PlaySFX("SFX_PlayerFootStep", m_audio);
    }
    //근접 공격 종료 후 실행되는 코드
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

    //콤보 공격을 위한 키버퍼 초기화
    void ReleaseKeyBuffer()
    {
        m_keyBuffer.Clear();
    }
    //현재 장착중인 총 세팅
    public void SetGun(Gun gun)
    {
        m_gun = gun.GetComponent<Gun>();
    }

    #endregion

    #region SFXPlay // SFX재생 메소드
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

    #region PlayerInput  //플레이어의 움직임 관련 메소드
   
    //움직임 구현기능. 8방향 다리모양 다른식으로 세분화
    void MoveAnimCtr(Vector3 dir) 
    {
        var fdot = Vector3.Dot(gameObject.transform.forward, dir.normalized);
        var dot = Vector3.Dot(m_leftDir.forward, dir.normalized);
        float force = 0f;
        if(dir != Vector3.zero)
        {
           if(dot > 0f) //왼쪽
            {
                force = -dot;
                m_animCtr.SetFloat("Horizontal", force);
            }
            else//오른쪽
            {
                force = -dot;
                m_animCtr.SetFloat("Horizontal", force);
            }
           if(fdot > 0f) //앞쪽
            {
                force = fdot;
                m_animCtr.SetFloat("Vertical", force);
            }
            else //뒷쪽
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
    //플레이어 전방 시선처리
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
    //플레이어가 처치한 적의 수를 카운트
    public void KillCount()
    {
        killCount++;
    } 
    //성공적으로 오브젝트 건설에 성공했을 때 호출. 소지 아이템 개수 --
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
    // 현재 상태를 건설 / 평상시 전환
    public void BuildingConvert()
    {
        isbuild = !isbuild;
    }
    
    //플레이어의 전체적인 조작을 담당하는 메소드
    public void BehaviorProcess()
    {
        // 사망시 작동 x
        if (m_Pstate.Equals(PlayerState.dead)) return;

        // 현재 상태가 오브젝트 설치중일 때 실행.
        if (isbuild)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                ObjectManager.Instance.RotationChanger();
            }
        }

        // 플레이어 스킬창
        if (Input.GetKeyDown(KeyCode.K)) 
        {    
            m_skillactive = !m_skillactive;
            UGUIManager.Instance.SkillUIChange(m_skillactive,m_skill);
            UGUIManager.Instance.PlayClickSFX();
        }

        // 인벤토리 온오프
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            m_isactive = !m_isactive; //불값으로 액티브 변경. 
            UGUIManager.Instance.GetStatusUI().SetActive(m_isactive);
            UGUIManager.Instance.PlayClickSFX();
        }

        // 구급상자 사용
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            if (GetHPValue() >= 0.95)
            {
                UGUIManager.Instance.SystemMessageItem("HealPack");
                return;
            }
            m_quickSlot.UseQuickSlotITem(1, "HealPack");
        }

        // 바리케이드 설치
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

        // 포탑 설치
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

        // 마우스 클릭을 활용한 공격, 오브젝트 설치
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            GameObject hitObject = hit.collider.gameObject;
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) //마우스 포인터가 UI 위에 있을 시 공격 안하게 만드는 기능.
            {
                if (!isbuild) //건설상태가 아닐경우
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
        // 총기 사용중일 때 재장전 메소드 실행.
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
            m_dir = m_dir / 1.5f; //대각선 이동 시 너무 빠른 움직이 발생하여 적용
        }
        if (m_dir != Vector3.zero)
        {
            m_navAgent.ResetPath();
        }
        //달리기 기능 테스트
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
        MoveAnimCtr(m_dir); //방향, 속도별 애니메이션 조절
        PlayerLookAt();     //플레이어 마우스방향 주시기능
        m_navAgent.Move(m_dir * m_status.speed/20 * Time.deltaTime); //실질적으로 이동을 시키는 코드. 네비메쉬에이전트를 활용하여 움직임 처리.
    }

    #endregion

    #region AboutStatus  //플레이어 스테이터스 관련 메소드
    //방어구 데이터. 데이터 양이 적어 임시 처리 하였음. 추후 수정 해야함.

    public void SetArmData(WearArmorData data) //방어구 스탯 저장 후 플레이어 스탯 갱신
    {
        m_armorData = data;
        SetStatus(m_weaponData.ID);
    }
    public void SetSkillData(TableSkillStat stat) //플레이어가 스킬을 활성화 했을 때 호출. 스탯정보를 가져오고 적용.
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
        if (m_status.SkillHeal > 0)// 스킬의 지속힐이 0 보다 크다면 힐 코루틴 시작.
        {
            if (CheckCoroutine != null) //기존에 실행중이라면 멈추고 다시실행
            {
                StopCoroutine(CheckCoroutine);
            }
            CheckCoroutine = StartCoroutine(Coroutine_SustainedHeal());
        }
    }
    public void LevelUp() //플레이어 레벨업
    {
        m_status.level++;
    }
    public int GetScore() //플레이어가 획득한 점수 정보 가져오기
    {
        return m_score;
    }
    public void AddScore(int score) //점수 추가.
    {
        m_score += score;
        UIManager.Instance.ScoreChange(m_score);
    }
    void HPControl(int value) //HP 컨트롤 메소드
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
        float hpvalue = 1; //HP상승 시 현재 hp의 value값을 바탕으로 비율 유지하도록 하기 위해
        if (m_skillStat.Remove != 0 && m_weaponData.Speed < 0) //스킬 데이터로 이동속도 패널티감소가 있고 무기의 이속감소 효과가 있을 경우 이속올려줌
        {
            penaltyRemove = penaltyRemove - (penaltyRemove * m_skillStat.Remove);
        }
        hpvalue = m_status.hp / m_status.hpMax;
        if(m_skillStat.CyberWear > 0)
        m_status = new Status(200 * (1 + (m_skillStat.HP + m_weaponData.HP + m_armorData.HP + (m_skillStat.ObjectHP * (1 + m_skillStat.CyberWear)))) * hpvalue, 200 * (1 + (m_skillStat.HP + m_weaponData.HP + m_armorData.HP + (m_skillStat.ObjectHP *(1 + m_skillStat.CyberWear)))), 10f + m_skillStat.CriRate + m_weaponData.CriRate + m_armorData.CriRate, 50f + (m_skillStat.CriDamage + m_weaponData.CriRate), 0 + (m_skillStat.AtkSpeed + m_weaponData.AtkSpeed) * (1 + m_armorData.AttackSpeed), (1 + (m_skillStat.Damage + m_armorData.Damage + m_skillStat.publicBuffDamage * (1 + m_skillStat.CyberWear))) * m_weaponData.Damage, 0 + m_skillStat.Defence + m_weaponData.Defence + m_armorData.Defence + m_skillStat.ObjectDefence * (1 + m_skillStat.CyberWear), 130 * (1 + m_skillStat.Speed + penaltyRemove + m_armorData.Speed), m_weaponData.Mag * Mathf.CeilToInt(1 + m_skillStat.Mag), m_weaponData.ReloadTime - (m_weaponData.ReloadTime * m_skillStat.Reload), m_weaponData.KnockBack + m_skillStat.KnockBackRate, m_weaponData.KnockBackDist, m_weaponData.AttackDist, m_weaponData.Shotgun,m_status.level, m_skillStat.DamageRigist + m_skillStat.ObjectRigist * (1 + m_skillStat.CyberWear), m_skillStat.LastFire, m_skillStat.Pierce, m_skillStat.Boom, m_skillStat.ArmorPierce+ m_skillStat.BuffArmorPierce * (1 + m_skillStat.CyberWear), m_skillStat.Remove, m_skillStat.Drain, m_skillStat.Crush, m_skillStat.Burn, m_skillStat.Heal + m_skillStat.ObjectRegen * (1 + m_skillStat.CyberWear), m_knickName, m_title);
        else
        m_status = new Status(200 * (1 + (m_skillStat.HP + m_weaponData.HP + m_armorData.HP)) * hpvalue, 200 * (1 + (m_skillStat.HP + m_weaponData.HP + m_armorData.HP)), 10f + m_skillStat.CriRate+ m_weaponData.CriRate + m_armorData.CriRate, 50f + (m_skillStat.CriDamage + m_weaponData.CriRate), 0 + (m_skillStat.AtkSpeed + m_weaponData.AtkSpeed) * (1 + m_armorData.AttackSpeed),(1+ (m_skillStat.Damage + m_armorData.Damage + m_skillStat.publicBuffDamage)) * m_weaponData.Damage, 0 + m_skillStat.Defence + m_weaponData.Defence + m_armorData.Defence, 130 * (1 + m_skillStat.Speed +penaltyRemove + m_armorData.Speed), m_weaponData.Mag * Mathf.CeilToInt(1+ m_skillStat.Mag), m_weaponData.ReloadTime - (m_weaponData.ReloadTime * m_skillStat.Reload) ,m_weaponData.KnockBack + m_skillStat.KnockBackRate, m_weaponData.KnockBackDist,m_weaponData.AttackDist,m_weaponData.Shotgun, m_status.level, m_skillStat.DamageRigist, m_skillStat.LastFire, m_skillStat.Pierce, m_skillStat.Boom, m_skillStat.ArmorPierce + m_skillStat.BuffArmorPierce , m_skillStat.Remove, m_skillStat.Drain, m_skillStat.Crush, m_skillStat.Burn, m_skillStat.Heal, m_knickName,m_title);
        HPControl(0); //바뀐 hp정보 UI에 보내주기 위해 0을 보내줌
        SetHudText(); //레벨 갱신을 위해 정보 업로드
        SetAnimSpeed(); //캐릭터의 속도에 비례하여애니메이션 속도 조절용
        m_area.transform.localScale = new Vector3(m_status.AtkDist, 1f, m_status.AtkDist);
    }
    void SetAnimSpeed()
    {
        m_animCtr.SetFloat("MeleeSpeed", m_status.atkSpeed);
        m_animCtr.SetFloat("MoveSpeed", m_status.speed / 100);
    }
    public void SetStatus(int ID) //무기 변경 시 사용하는 메소드로 무기의 데이터를 받아와준 후 스텟설정
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
        m_skill.SetWeaponType(m_weaponData.weaponType); //무기타입 전송
    }
    //다른 클래스에 플레이어컨트롤러 정보 전달
    void SetPlayer()
    {
        UGUIManager.Instance.SetPlayer(this);
        UpdateManager.Instance.SetPlayer(this); 
        GameManager.Instance.SetGameObject(gameObject);
    }

    #endregion

    #region HUD && PlayerState  //플레이어의 HUD와 State 관련 메소드

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
        if (m_status.hp <= 0 || m_Pstate == PlayerState.dead || m_Pstate == PlayerState.Invincible) //피가 0이거나 죽었을땐 적용 X
            return;
        damage = CalculationDamage.NormalDamage(damage, m_status.defense, 0f, m_status.DamageRigist);
        PlayDamagedSound();
        int mondamage = Mathf.CeilToInt(damage - (damage * m_status.DamageRigist)); //피해 저항 적용
        HPControl(-mondamage);
        var hiteffect = TableEffect.Instance.m_tableData[6].Prefab[2];
        var effect = EffectPool.Instance.Create(hiteffect);
        effect.transform.position = m_hitPos.position;
        effect.SetActive(true);
        m_hudText.Add(-damage, Color.red, 0f);
        UIManager.Instance.DamagedUI();
        if (m_status.hp <= 0)
        {
            Dead(); //hp가 0이하일때 사망처리
        }
    }
    void SkillHeal(int healvalue) // 스킬 중 지속적으로 회복하기 위해 사용하는 메소드.
    {
        HPControl(healvalue);
        m_hudText.Add(healvalue, Color.green, 0f);
    }
    public void GetHeal(float heal) //
    {
        if (m_status.hp >= m_status.hpMax || m_Pstate == PlayerState.dead) return; //이미 풀피이거나 죽었을경우 실행 X
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
        if (m_status.hp >= m_status.hpMax) //최대체력 초과한 힐은 적용 X
        {
            m_status.hp = m_status.hpMax;
        }
    }
    void Dead() //죽었을경우 플레이어 액티브 종료
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
    public void Revive() //부활기능
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
        SetStatus(m_weaponData.ID); //스테이터스 재정비 합니다잉
        GameManager.Instance.SetGameObject(gameObject);
    }

    void LevelUP()
    {
        PlayLvUpSound();
        LevelUp();
        m_levelexp = Levelexp();
        HPControl(Mathf.CeilToInt((m_status.hpMax - m_status.hp) / 2)); //풀피로 만들어줌 은 너무 사기이므로 잃은 피해의 50%로 수정.
        UIManager.Instance.LevelUPUI(m_status.level);
        SetHudText();
        m_skill.LevelUP();
        StartCoroutine(Coroutine_Invincible(2f)); //렙업시무적
        var levelupEffect = TableEffect.Instance.m_tableData[6].Prefab[0];
        var effect = EffectPool.Instance.Create(levelupEffect);
        effect.transform.position = gameObject.transform.position;
        effect.transform.localScale = new Vector3(2f, 2f, 2f);
        effect.SetActive(true);
    }   
    public void SetHudText() //HUD상에 닉네임, 레벨, 타이틀 표기
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

    #region Level,Exp  //경험치, 레벨업 관련 메소드
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
        SetStatus(1); // 시작시 기본 권총으로
        m_status.hp = m_status.hpMax; //시작 시 hp 설정.
        m_status.level = 1;
        m_levelexp = Levelexp();
        HPControl(0);
        SetHudText();
    }
    #endregion

    #endregion
}

