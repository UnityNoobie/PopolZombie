using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAbilityType
{
    None,
    Pistol,
    SMG,
    Rifle,
    MG,
    ShotGun,
    Melee,
    Turret,
    Barricade,
    CyberWear
}
public class PlayerSkillController : MonoBehaviour
{

    #region PlayerSkillDatas
    float Damage;
    float AtkSpeed;
    float Reload;
    float Speed;
    int CriRate;
    float CriDamage;
    float Mag;
    float Defence;
    float DamageRigist;
    float HP;
    float KnockBackRate;
    float Heal;
    int LastFire;
    int Pierce;
    int Boom;
    float ArmorPierce;
    float Remove;
    int Drain;
    float Crush;
    int Burn;
    #endregion

    #region PublicReturnMethod
    public bool isActiveType(SkillWeaponType skillweapon, WeaponType weaponType)
    {
        switch (skillweapon)
        {
            case SkillWeaponType.Every: return true;
            case SkillWeaponType.Personal:
                if (weaponType.Equals(WeaponType.Pistol) || weaponType.Equals(WeaponType.SubMGun) || weaponType.Equals(WeaponType.Rifle)) return true;
                else return false;
            case SkillWeaponType.Heavy:
                if (weaponType.Equals(WeaponType.Bat) || weaponType.Equals(WeaponType.Axe) || weaponType.Equals(WeaponType.MachineGun) || weaponType.Equals(WeaponType.ShotGun)) return true;
                else return false;
            case SkillWeaponType.Pistol:
                if (weaponType.Equals(WeaponType.Pistol)) return true;
                else return false;
            case SkillWeaponType.SMG:
                if (weaponType.Equals(WeaponType.SubMGun)) return true;
                else return false;
            case SkillWeaponType.Rifle:
                if (weaponType.Equals(WeaponType.Rifle)) return true;
                else return false;
            case SkillWeaponType.MachineGun:
                if (weaponType.Equals(WeaponType.MachineGun)) return true;
                else return false;
            case SkillWeaponType.ShotGun:
                if (weaponType.Equals(WeaponType.ShotGun)) return true;
                else return false;
            case SkillWeaponType.Melee:
                if (weaponType.Equals(WeaponType.Bat) || weaponType.Equals(WeaponType.Axe)) return true;
                else return false;
        }
        return false;
    } //장착한 무기와 스킬의 특성화값이 일치하는지 확인용
    public bool IsCanActive(int point,int skillgrade,SkillType skilltype) //스킬 습득 가능한지 확인해주는 메서드
    {
        if (m_skillPoint >= point)
        {
            CheckActivedSkillLV(skilltype); //현재 활성화되어있는 스킬리스트로 단계별 갯수 확인
            if(skillgrade == 2 && m_lowLvCount < 8)
            {
                UGUIManager.Instance.SystemMessageSendMessage("1단계 스킬에 8개 이상 습득 후 습득 가능합니다.");
                return false;
            }
            else if (skillgrade == 3 && m_midLvCount < 6)
            {
                UGUIManager.Instance.SystemMessageSendMessage("2단계 스킬에 6개 이상 습득 후 습득 가능합니다.");
                return false;
            }
            else if (skillgrade == 4 && m_highLvCount < 3)
            {
                UGUIManager.Instance.SystemMessageSendMessage("특성 스킬 모두 습득 시 습득 가능합니다.");
                return false;
            }
            else
            return true;
        }
        else
        {
            UGUIManager.Instance.SystemMessageSendMessage("스킬 포인트가 모자랍니다."); 
            return false;
        }
    }
    public bool IsActived(int id) //리스트 등록 시 스킬이 액티브 상태인지 아닌지 확인해주는 메소드
    {
        if (m_skillList.Contains(id)) return true;
        else return false;
    }
    public bool IsCanOpen(int grade,SkillType skilltype)
    {
        CheckActivedSkillLV(skilltype);
        if (grade == 3 && m_midLvCount < 6)
        {
            UGUIManager.Instance.SystemMessageSendMessage("2단계 스킬에 6개 이상 습득 후 습득 가능합니다.");
            return false;
        }
        else if (grade == 4 && m_highLvCount < 3)
        {
            UGUIManager.Instance.SystemMessageSendMessage("특성스킬을 모두 마스터 후 습득 가능합니다.");
            return false;
        }
        else return true;
    }
    public PlayerAbilityType GetPlayerAbilityState() //플레이어의 특성화한 어빌리티 타입을 보내줌
    {
        return m_abilityType;
    }
    public int GetPlayerSP()
    {
        return m_skillPoint;
    }

    #endregion

    #region Property
  
    public TableSkillStat m_skilldata{get;set;}
    public TableUtilitySkillStat m_utilldata { get; set;}
    #endregion

    #region Constants and Fields
    WeaponType m_weapontype;
    SkillWeaponType m_skillweapon;
    PlayerAbilityType m_abilityType;
    PlayerController m_player;
    GunManager m_gunmanager;
    SkillUI m_skillUI;
    int m_skillPoint = 0;
    int SPCount = 0;
    int m_lowLvCount = 0; //현재 플레이어가 습득하고있는 스킬의 등급별 갯수 체크용
    int m_midLvCount = 0;
    int m_highLvCount = 0;


    public List<int> m_skillList = new List<int>(); //현재 활성화한 스킬 리스트 저장용
    #endregion

    #region publicMethod
    public void ActiveSkill(int id) //스킬 찍기를 시도한다면 그 스킬의 필요 포인트 확인하여 충분하면 찍는 방식. + 스킬UI에서 스킬포인트를 확인하기 때문에 여기서 삭제
    {
        m_skillPoint -= m_skilldata.GetSkillData(id).SkillPoint;
        m_skillList.Add(id);
        RefreshSKillData();
        PushSkillUpSignal();
    }
    public void SetWeaponType(WeaponType type)  //무기 변경 시 마다 호출하여 현재 착용중인 무기와 스킬이 호환되는지 판단하기 위함.
    {
        m_weapontype = type;
        RefreshSKillData();
    }

    public void SetStore(SkillUI ui)
    {
        m_skillUI = ui;
    }
    public void SetAblityType(PlayerAbilityType type)
    {
        m_abilityType = type;
    }
    public void LevelUP() //레벨업 시 스킬포인트 증가 기능 최대 30까지 가능 하도록.
    {
        if (SPCount < 30)
        {
            m_skillPoint++;
            SPCount++;
        }
        else if (SPCount == 30)
        {
            UGUIManager.Instance.SystemMessageSendMessage("최대 스킬 포인트에 도달했습니다.");
        }
    }
    #endregion

    #region privateMethod
    void RefreshSP()
    {
        m_skillUI.RefreshSP(this);
    }
    void CheckActivedSkillLV(SkillType type)
    {
        m_lowLvCount = 0;
        m_midLvCount = 0;
        m_highLvCount = 0;
        for (int i = 0; i <  m_skillList.Count; i++) //현재 활성화된 스킬리스트만큼 반복하여 각 ID의 레벨별 습득 스킬을 찾아옴
        {
            if (m_skilldata.GetSkillData(m_skillList[i]).SkillType.Equals(type))
            {
                if (m_skilldata.GetSkillData(m_skillList[i]).SkillGrade == 1)
                {
                    m_lowLvCount++;
                }
                else if (m_skilldata.GetSkillData(m_skillList[i]).SkillGrade == 2)
                {
                    m_midLvCount++;
                }
                else if (m_skilldata.GetSkillData(m_skillList[i]).SkillGrade == 3)
                {
                    m_highLvCount++;
                }
            }
        }
    }
   
    void PushSkillUpSignal()
    {
        RefreshSP();
        m_player.SkillUpInitstatus(); //플레이어와 총에 스킬올라갔다는 정보 전달.
        m_gunmanager.SkillUpSignal();
    }
    void ResetDatas() //이전에 가지고 있던 정보 초기화.
    {
        Damage = 0;
        AtkSpeed = 0;
        Reload = 0;
        Speed = 0;
        CriRate = 0;
        CriDamage = 0;
        Mag = 0;
        Defence = 0;
        DamageRigist = 0;
        HP = 0;
        KnockBackRate = 0;
        Heal = 0;
        LastFire = 0;
        Pierce = 0;
        Boom = 0;
        ArmorPierce = 0;
        Remove = 0;
        Drain = 0;
        Crush = 0;
        Burn = 0;
    }
    void RefreshSKillData() //스킬데이터 가져올 때마다 수행할 작업.
    {
        ResetDatas(); //데이터를 리셋해주고
        for (int i = 0; i < m_skillList.Count; i++) //리스트 길이만큼 실행
        {
            if (isActiveType(m_skilldata.GetSkillData(m_skillList[i]).SkillWeaponType, m_weapontype)) //현재 무기 정보와 스킬이 요구하는 무기의 종류가 같다면 스탯 더해줌.
            {
                Damage += m_skilldata.GetSkillData(m_skillList[i]).Damage;
                AtkSpeed += m_skilldata.GetSkillData(m_skillList[i]).AtkSpeed;
                Reload += m_skilldata.GetSkillData(m_skillList[i]).Reload;
                Speed += m_skilldata.GetSkillData(m_skillList[i]).Speed;
                CriRate += m_skilldata.GetSkillData(m_skillList[i]).CriRate;
                CriDamage += m_skilldata.GetSkillData(m_skillList[i]).CriDamage;
                Mag += m_skilldata.GetSkillData(m_skillList[i]).Mag;
                Defence += m_skilldata.GetSkillData(m_skillList[i]).Defence;
                DamageRigist += m_skilldata.GetSkillData(m_skillList[i]).DamageRigist;
                HP += m_skilldata.GetSkillData(m_skillList[i]).HP;
                KnockBackRate += m_skilldata.GetSkillData(m_skillList[i]).KnockBackRate;
                Heal += m_skilldata.GetSkillData(m_skillList[i]).Heal;
                LastFire += m_skilldata.GetSkillData(m_skillList[i]).LastFire;
                Pierce += m_skilldata.GetSkillData(m_skillList[i]).Pierce;
                Boom += m_skilldata.GetSkillData(m_skillList[i]).Boom;
                ArmorPierce += m_skilldata.GetSkillData(m_skillList[i]).ArmorPierce;
                Remove += m_skilldata.GetSkillData(m_skillList[i]).Remove;
                Drain += m_skilldata.GetSkillData(m_skillList[i]).Drain;
                Crush += m_skilldata.GetSkillData(m_skillList[i]).Crush;
                Burn += m_skilldata.GetSkillData(m_skillList[i]).Burn;
            }
        }
        m_player.SetSkillData(Damage, AtkSpeed, Reload, Speed, CriRate, CriDamage, Mag, Defence, DamageRigist, HP, KnockBackRate, Heal, LastFire, Pierce, Boom, ArmorPierce, Remove, Drain, Crush, Burn);
    } //플레이어에게 데이터 전송
    private void Awake()
    {

        m_skilldata = new TableSkillStat();
        m_player = GetComponent<PlayerController>();
        m_gunmanager = GetComponent<GunManager>();
        m_abilityType = PlayerAbilityType.None;
       // m_skillPoint = 100; 테스트용
    }
    #endregion
}
