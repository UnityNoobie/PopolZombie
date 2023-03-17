using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Gun;
using static PlayerStriker;


public class PlayerController : MonoBehaviour
{
    public ArmorManager m_armorManager { get; set; }
    public static float Money;
    public static float Score;
    [SerializeField]
    HUDText m_hudText;
    [SerializeField] //플레이어의 스테이터스 체크
    Status m_status;
    [SerializeField]
    GameObject m_PlayerHuD;
    [SerializeField]
    Inventory m_inven;
    [SerializeField]
    QuickSlot m_quickSlot;
    GunManager m_manager;
    Transform m_leftDir;
    Transform m_hitPos;
    Gun m_gun;
    WeaponData m_weaponData { get; set; }
    public TestSkillData m_SkillData;
    NavMeshAgent m_navAgent;
    PlayerAnimController m_animCtr;
    bool isComboError;
    [SerializeField]
    GameObject m_area;
    [SerializeField]
    Vector3 m_dir;

   


    int hp;
    int m_comboIndex;
    public float pDefence;
    public int ID;
    float timeAfterAttack;
    #region ArmorData
    int armDefence;
    float armDamage;
    float hpPer;
    float armAttackSpeed;
    int armCriRate;
    float armSpeed;
    #endregion
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
    List<PlayerAnimController.Motion> m_comboList = new List<PlayerAnimController.Motion>() { PlayerAnimController.Motion.Combo1, PlayerAnimController.Motion.Combo2 };
    Queue<KeyCode> m_keyBuffer = new Queue<KeyCode>(); // 콤보를 위한 키코드
    #region MeleeAttackProcess
    public bool IsAttack
    {
        get
        {
            if (GetMotion == PlayerAnimController.Motion.Combo1 ||
                GetMotion == PlayerAnimController.Motion.Combo2)
                return true;
            return false;
        }
    }
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


    #region Coroutine
    IEnumerator Coroutine_Invincible(float time)
    {
        m_Pstate = PlayerState.Invincible;
        //meleeState = MeleeState.Ready;
        yield return new WaitForSeconds(time);
        m_Pstate = PlayerState.alive;
    }
    #endregion


    #region AnimEvent
    void AnimEvent_Dead()
    {
        gameObject.SetActive(false);
        m_PlayerHuD.SetActive(false);
    }
    void AnimEvent_AttackStart()
    {
        m_area.SetActive(true);
    }
    void AnimEvent_GrabWeapon()
    {
        m_gun.gunstate = GunState.Ready;
    }
    void AnimEvent_MeleeAttack()
    {  
        float damage = 0;
        var area = m_area.GetComponent<AttackAreaUnitFind>(); ;
        var areaList = area.m_unitList;
        for (int i = 0; i < areaList.Count; i++)
        {
           // Debug.Log(areaList[i].name + "몇번째? : "+i);
          
            if (areaList[i].CompareTag("Zombie"))
            {
                var mon = areaList[i].GetComponent<MonsterController>();
                var type = GunManager.AttackProcess(mon, m_status.damage, m_status.criRate, m_status.criAttack, out damage);
                mon.SetDamage(type, damage, this);
            }
        }

        // areaList.Clear(); //공격 후 초기화하는 방식?
        //        m_area.transform.localScale = new Vector3(m_player.GetStatus.AtkDist, 1f, m_player.GetStatus.AtkDist);
    }
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
    void ReleaseKeyBuffer()
    {
        m_keyBuffer.Clear();
    }
    public void SetGun(Gun gun)
    {
        m_gun = gun.GetComponent<Gun>();
    }
    #endregion
    #region PlayerInput

    void MoveAnimCtr(Vector3 dir) //움직임 구현기능. 8방향 다리모양 다른식으로 세분화해보리기!
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
    public void PlayerLookAt() //플레이어 전방 시선처리
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
    void BehaviorProcess()
    {
        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(m_dir.x !=0 && m_dir.z !=0)
        {
            m_dir = m_dir / 1.5f; //일단 대각선이동은 이렇게 처리 추후 함수적용.
        }
        if (m_dir != Vector3.zero)
        {
            m_navAgent.ResetPath();
        }
        MoveAnimCtr(m_dir);
        m_navAgent.Move(m_dir * m_status.speed/20 * Time.deltaTime);
    }

    #endregion

    #region AboutStatus
    
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
    void InitStatus(float skillhp, float itemhp, float S_CriPer, float I_Criper, float S_CriDam, float I_CriDam, float S_atkSpeed, float I_atkSpeed, float S_Atk, float I_Atk, float S_Def, float I_Def, float S_speed, float I_speed, int maxammo, float reloadtime,float knockbackPer, float knockbackDist,float atkDist, int shotgun) 
    {//SetStatus에서 스킬과 아이템의 추가값들을 받아옴.
     //스테이터스 입력값의 순서 : 최대체력 , 크리티컬 확률, 크리티컬 추가 데미지,공격속도, 공격력, 방어력, 이동속도
        m_status = new Status(hp,200 * (1 + (skillhp + itemhp + hpPer)), 10f + S_CriPer + I_Criper + armCriRate, 50f + S_CriDam + I_CriDam, 0 + (S_atkSpeed + I_atkSpeed) * (1 + armAttackSpeed), (S_Atk + armDamage) * I_Atk, 0 + S_Def + I_Def + armDefence, 130 * (1 + S_speed + I_speed + armSpeed), maxammo, reloadtime ,knockbackPer,knockbackDist,atkDist,shotgun);
        // m_status.hpMax = Mathf.CeilToInt(m_status.hpMax); //최대체력 가져오기
        HPControl(0);
        m_animCtr.SetFloat("MeleeSpeed", m_status.atkSpeed);
        m_animCtr.SetFloat("MoveSpeed", m_status.speed / 100);
        m_inven.GetStatusInfo(this);
        pDefence = m_status.defense;  //방어력 가져오기
        m_area.transform.localScale = new Vector3(m_status.AtkDist, 1f, m_status.AtkDist);
        
    }
    public void SetStatus(int ID)
    {
        SetWeaponID(ID);
        //스테이터스 입력값의 순서 : 스킬, 아이템 최대체력 ,스킬아이템 크리티컬 확률, 스킬, 아이템 크리티컬 추가 데미지,스킬 아이템 공격속도, 스킬아이템 공격력, 스킬 아이템 방어력 , 이동속도
        InitStatus(m_SkillData.hp, m_weaponData.HP, m_SkillData.criPer, m_weaponData.CriRate, m_SkillData.criDam, m_weaponData.CriDamage, m_SkillData.atkSpeed, m_weaponData.AtkSpeed, m_SkillData.atk, m_weaponData.Damage, m_SkillData.def, m_weaponData.Defence, m_SkillData.Speed, m_weaponData.Speed, m_weaponData.Mag, m_weaponData.ReloadTime,m_weaponData.KnockBack,m_weaponData.KnockBackDist,m_weaponData.AttackDist,m_weaponData.Shotgun);
     //   m_animCtr.SetFloat("MoveSpeed", m_status.speed / 150); //이동속도별 다리움직임 속도조절용.
    }   //넉백을 아직은 무기에서만 적용을 하여 추후 더 넣어줘야함!
    void SetWeaponID(int ID)
    {
        //   m_gunData = m_gunData.GetWeaponStatus(ID);
        m_weaponData = m_weaponData.GetWeaponStatus(ID);
        m_SkillData = m_SkillData.SkillData(m_weaponData.weaponType);
    }

    #endregion
    #region HUD && PlayerState

    public float GetValue()
    {
        float value = m_status.hp / m_status.hpMax;

        return value;
    }
    public void GetDamage(float damage)
    {
        if (m_status.hp <= 0 || m_Pstate == PlayerState.dead || m_Pstate == PlayerState.Invincible) //피가 0이거나 죽었을땐 적용 X
            return;
        int mondamage = Mathf.CeilToInt(damage);
        HPControl(-mondamage);
        var hiteffect = TableEffect.Instance.m_tableData[6].Prefab[2];
        var effect = EffectPool.Instance.Create(hiteffect);
        effect.transform.position = m_hitPos.position;
        effect.SetActive(true);
        m_hudText.Add(-damage,Color.red,0f);
        UIManager.Instance.DamagedUI();
        if (m_status.hp <= 0)
        {
            Dead(); //hp가 0이하일때 사망처리
        }
    }
    public void GetHeal(float heal)
    {
        if (m_status.hp >= m_status.hpMax || m_Pstate == PlayerState.dead) return; //이미 풀피이거나 죽었을경우 실행 X
        float percentHeal = m_status.hpMax * (heal / 100);
        Debug.Log(percentHeal);
        int healvalue = Mathf.CeilToInt(percentHeal);
        HPControl(healvalue);
        m_hudText.Add(heal, Color.green, 0f);
        var healeffect = TableEffect.Instance.m_tableData[6].Prefab[1];
        var effect = EffectPool.Instance.Create(healeffect);
        effect.transform.position = gameObject.transform.position;
        effect.SetActive(true);
        UIManager.Instance.HealUI();
        if (m_status.hp >= m_status.hpMax) //최대체력 초과한 힐은 적용 X 추후 회복량 자체 줄이는식으로 수정할것    *****
        {
            m_status.hp = m_status.hpMax;
        }
    }
    void Dead() //죽었을경우 플레이어 액티브 종료
    {
        ReleaseKeyBuffer();
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
        SetStatus(m_weaponData.ID); //스테이터스 재정비 합니다잉

    }

    #endregion

    private void Awake()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_SkillData = new TestSkillData();
           //임시용으로 해둔것이기 떄문에 추후 수정필수 m_skilldata, m_ItemData 등등 무조건!!!! 해야함!!
        m_animCtr = GetComponent<PlayerAnimController>();
        m_leftDir = Utill.GetChildObject(gameObject, "LeftDir");
        m_weaponData = new WeaponData();
        m_armorManager= GetComponent<ArmorManager>();
        m_manager = GetComponent<GunManager>();
        isComboError = false;
    }
    
    private void Start()
    {
        SetStatus(8); // 시작시 기본 라이플로 작동
        m_status.hp = m_status.hpMax; //시작 시 hp 설정.
        HPControl(0);
        m_hitPos = Utill.GetChildObject(gameObject, "Dummy_Pos");
    }
    private void Update()
    {
        if(m_Pstate != PlayerState.dead) 
        {
            BehaviorProcess();
            PlayerLookAt();
        }
        if(Input.GetKeyDown(KeyCode.F)) //이하 테스트기능
        {
            GetDamage(50); 
        }
    }


}

