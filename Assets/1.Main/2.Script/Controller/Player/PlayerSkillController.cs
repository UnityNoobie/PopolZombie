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

    #region Constants and Fields

    WeaponType m_weapontype;
    PlayerAbilityType m_abilityType;

    PlayerController m_player;
    PlayerObjectController m_playerObject;
    TableSkillStat m_playerSkillstat;
    GunManager m_gunmanager;
    SkillUI m_skillUI;
    int m_skillPoint = 0;

    //int SPCount = 0; //최대 획득 포인트 제한용 이었으나 밸런스 이유로 적용 X

    int m_lowLvCount = 0; //현재 플레이어가 습득하고있는 스킬의 등급별 갯수 체크용
    int m_midLvCount = 0;
    int m_highLvCount = 0;

    bool m_ismasterskillactived = false;


    public List<int> m_skillList = new List<int>(); //현재 활성화한 스킬 리스트 저장용
    #endregion

    

    #region Property
  
    public TableSkillStat m_skilldata{get;set;}

    #endregion

    #region Methods

    #region PublicReturnMethod
    public bool isActiveType(SkillWeaponType skillweapon, WeaponType weaponType) //스킬의 스테이터스 적용 대상과 맞는지 확인용. 큰 데이터가 아니라 Switch문 사용하였음.
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
    public bool IsCanActive(int point, int skillgrade, SkillType skilltype) //스킬 습득 가능한지 확인해주는 메서드
    {
        if (m_skillPoint >= point)
        {
            CheckActivedSkillLV(skilltype); //현재 활성화되어있는 스킬리스트로 단계별 갯수 확인
            if (skillgrade == 2 && m_lowLvCount < 8)
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
    public bool IsCanOpen(int grade, SkillType skilltype) //하위스킬의 갯수를 체크하여 상위 스킬을 습득할 수 있는지 확인
    {
        CheckActivedSkillLV(skilltype);
        if (grade == 3 && m_midLvCount < 6)
        {
            UGUIManager.Instance.SystemMessageSendMessage("2단계 스킬에 6개 이상 습득 후 습득 가능합니다.");
            return false;
        }
        else if (grade == 4 && m_highLvCount < 3)
        {
            UGUIManager.Instance.SystemMessageSendMessage("특성 스킬 모두 습득 시 습득 가능합니다.");
            return false;
        }
        else return true;
    }
    public void ActivedMasterSkill() //마스터스킬의 엑티브 여부 결정
    {
        m_ismasterskillactived = true;
    }
    public bool IsMasterSkillActived() //마스터스킬의 액티브 여부 리턴
    {
        return m_ismasterskillactived;
    }
    public PlayerAbilityType GetPlayerAbilityState() //플레이어의 특성화한 어빌리티 타입을 보내줌
    {
        return m_abilityType;
    }
    public int GetPlayerSP() // 플레이어의 SP 반환
    {
        return m_skillPoint;
    }
    public TableSkillStat GetPlayerSkillData() // 스킬 데이터 반환
    {
        UpdateSkillData();
        return m_playerSkillstat;
    }

    #endregion

    #region publicMethod
    public void ActiveSkill(int id) //스킬 찍기를 시도한다면 그 스킬의 필요 포인트 확인하여 충분하면 찍는 방식. + 스킬UI에서 스킬포인트를 확인하기 때문에 여기서 삭제
    {
        m_skillPoint -= m_skilldata.GetSkillData(id).SkillPoint;
        m_skillList.Add(id);
        UpdateSkillData();
        PushSkillUpSignal();
    }
    public void SetWeaponType(WeaponType type)  //무기 변경 시 마다 호출하여 현재 착용중인 무기와 스킬이 호환되는지 판단하기 위함.
    {
        m_weapontype = type;
        UpdateSkillData();
    }

    public void SetSkillUI(SkillUI ui) //UI설정
    {
        m_skillUI = ui;
    }
    public void SetAblityType(PlayerAbilityType type) //플레이어의 특성화된 어빌리티 타입 받아옴
    {
        m_abilityType = type;
    }
    public void LevelUP() //레벨업 시 스킬포인트 증가 기능 최대 30까지 가능 하도록. 했었으나 무한라운드니까 특성 스킬 이하는 무한으로 해볼까 함.
    {
        m_skillPoint++;
        /*
        if (SPCount < 30)
        {
            m_skillPoint++;
            SPCount++;
        }
        else if (SPCount == 30)
        {
            UGUIManager.Instance.SystemMessageSendMessage("최대 스킬 포인트에 도달했습니다.");
        }*/ // 폐기
    }
    #endregion

    #region privateMethod
    void UpdateSP() //스킬 포인트 갱신.
    {
        m_skillUI.UpdateSP(this);
    }
    void CheckActivedSkillLV(SkillType type) //현재 습득한 스킬들의 타입에 따라, 등급에 따라 분류하고 해당 조건의 스킬을 배운 수를 체크해줌.
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
   
    void PushSkillUpSignal() // 스킬 레벨업 시 관련 메소드에 정보 전달.
    {
        UpdateSP();
        m_gunmanager.SkillUpSignal();
        m_playerObject.UpdateObjectStat();
        UGUIManager.Instance.GetStatusUI().SetStatus(); // 신 인벤토리
    }

    void ResetDatas() //이전에 가지고 있던 스킬정보 초기화.
    {
        m_playerSkillstat = new TableSkillStat();
    }
    void UpdateSkillData() //스킬데이터를 최신화해주고 정보 전달.
    {
        ResetDatas(); //데이터를 리셋해주고
        for (int i = 0; i < m_skillList.Count; i++) //리스트 길이만큼 실행
        {
            if (isActiveType(m_skilldata.GetSkillData(m_skillList[i]).SkillWeaponType, m_weapontype)) //현재 무기 정보와 스킬이 요구하는 무기의 종류가 같다면 스탯 더해줌.
            {
                m_skilldata = m_skilldata.GetSkillData(m_skillList[i]);
                m_playerSkillstat.Damage += m_skilldata.Damage;
                m_playerSkillstat.AtkSpeed += m_skilldata.AtkSpeed;
                m_playerSkillstat.Reload += m_skilldata.Reload;
                m_playerSkillstat.Speed += m_skilldata.Speed;
                m_playerSkillstat.CriRate += m_skilldata.CriRate;
                m_playerSkillstat.CriDamage += m_skilldata.CriDamage;
                m_playerSkillstat.Mag += m_skilldata.Mag;
                m_playerSkillstat.Defence += m_skilldata.Defence;
                m_playerSkillstat.DamageRigist += m_skilldata.DamageRigist;
                m_playerSkillstat.KnockBackRate += m_skilldata.KnockBackRate;
                m_playerSkillstat.HP += m_skilldata.HP;
                m_playerSkillstat.Heal += m_skilldata.Heal;
                m_playerSkillstat.LastFire += m_skilldata.LastFire;
                m_playerSkillstat.Pierce += m_skilldata.Pierce;
                m_playerSkillstat.Boom += m_skilldata.Boom;
                m_playerSkillstat.SkillPoint += m_skilldata.SkillPoint;
                m_playerSkillstat.ArmorPierce += m_skilldata.ArmorPierce;
                m_playerSkillstat.Remove += m_skilldata.Remove;
                m_playerSkillstat.Drain += m_skilldata.Drain;
                m_playerSkillstat.Crush += m_skilldata.Crush;
                m_playerSkillstat.Burn += m_skilldata.Burn;
                m_playerSkillstat.TurretMaxBuild += m_skilldata.TurretMaxBuild;
                m_playerSkillstat.BarricadeMaxBuild += m_skilldata.BarricadeMaxBuild;
                m_playerSkillstat.ObjectRegen += m_skilldata.ObjectRegen;
                m_playerSkillstat.BarricadeRegen += m_skilldata.BarricadeRegen;
                m_playerSkillstat.BonusHP += m_skilldata.BonusHP;
                m_playerSkillstat.TurretHP += m_skilldata.TurretHP;
                m_playerSkillstat.BarricadeHP += m_skilldata.BarricadeHP;
                m_playerSkillstat.ObjectHP += m_skilldata.ObjectHP;
                m_playerSkillstat.CyberWear += m_skilldata.CyberWear;
                m_playerSkillstat.publicBuffDamage += m_skilldata.publicBuffDamage;
                m_playerSkillstat.BuffArmorPierce += m_skilldata.BuffArmorPierce;
                m_playerSkillstat.TurretRigist += m_skilldata.TurretRigist;
                m_playerSkillstat.BarricadeRigist += m_skilldata.BarricadeRigist;
                m_playerSkillstat.ObjectRigist += m_skilldata.ObjectRigist;
                m_playerSkillstat.TurretDamage += m_skilldata.TurretDamage;
                m_playerSkillstat.TurretRange += m_skilldata.TurretRange;
                m_playerSkillstat.TurretAttackSpeed += m_skilldata.TurretAttackSpeed;
                m_playerSkillstat.TurretArmorPierce = m_skilldata.TurretArmorPierce;
                m_playerSkillstat.BarricadeDefence += m_skilldata.BarricadeDefence;
                m_playerSkillstat.ObjectDefence += m_skilldata.ObjectDefence;
                m_playerSkillstat.ReflectDamge += m_skilldata.ReflectDamge;
                m_playerSkillstat.MaxMachineLearning += m_skilldata.MaxMachineLearning;
                /*
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
                */ //이전버전
            }
        }
        m_player.SetSkillData(m_playerSkillstat);//플레이어에게 데이터 전송
    } 
    void SetTransform() // 클래스에서 사용할 컴포넌트들을 가져와줌.
    {
        m_player = GetComponent<PlayerController>();
        m_skilldata = new TableSkillStat();
        m_gunmanager = GetComponent<GunManager>();
        m_playerObject = GetComponent<PlayerObjectController>();
        m_abilityType = PlayerAbilityType.None;
    }
    private void Awake() 
    {
        SetTransform();
         m_skillPoint = 100;// 테스트
        ObjectManager.Instance.SetPlayer(this);
    }
    #endregion
    #endregion
}
