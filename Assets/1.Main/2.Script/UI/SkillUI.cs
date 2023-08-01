using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    #region Constants and Fields
    TableSkillStat m_stat = new TableSkillStat();
    PlayerSkillController m_player;
    public Button m_skillOpen;
    public Button m_masterOpen;
    public Button m_Agility;
    public Button m_Strength;
    public Button m_Utility;
    ActiveSkill m_active;
    SkillInfo m_skillInfo;
    SkillSlot m_masterSlot;
    SkillSlot[] m_midSlots;
    SkillSlot[] m_lowSlots;
    SkillSlot[] m_highSlots;
    Transform m_low;
    Transform m_mid;
    Transform m_high;
    Transform m_master;
    TextMeshProUGUI m_skillPoints;


    SkillType m_Type;

    
    int m_count;
    List<int> m_SkillList = new List<int>();

    #endregion

    #region Methods
    public void SetTransform() // 하위 오브젝트에서 필요한 좌표, 컴포넌트 읽어옴
    {
        m_low = Utill.GetChildObject(gameObject, "Lv1");
        m_mid = Utill.GetChildObject(gameObject, "Lv2");
        m_high = Utill.GetChildObject(gameObject, "Lv3");
        m_master = Utill.GetChildObject(gameObject, "Master");
        m_active = Utill.GetChildObject(gameObject,"ActiveSkill").GetComponent<ActiveSkill>();
        m_skillInfo = Utill.GetChildObject(gameObject,"SkillInfo").GetComponent<SkillInfo>();
        m_skillOpen = Utill.GetChildObject(gameObject, "Lv3_Active").GetComponent<Button>();
        m_masterOpen = Utill.GetChildObject(gameObject, "Lv4_Active").GetComponent<Button>();
        m_Agility = Utill.GetChildObject(gameObject, "Button_Agility").GetComponent<Button>();
        m_Strength = Utill.GetChildObject(gameObject, "Button_Strength").GetComponent<Button>();
        m_Utility = Utill.GetChildObject(gameObject, "Button_Utility").GetComponent<Button>();
        m_skillPoints = Utill.GetChildObject(gameObject,"SkillPoint").GetComponent<TextMeshProUGUI>();

        m_lowSlots = m_low.GetComponentsInChildren<SkillSlot>(true);
        m_midSlots = m_mid.GetComponentsInChildren<SkillSlot>(true);
        m_highSlots = m_high.GetComponentsInChildren<SkillSlot>(true);
        m_masterSlot = m_master.GetComponentInChildren<SkillSlot>(true);
        SetButtonAddListener();
    }
    void OpenSlots(int grade) //슬롯 OnOff기능
    {
        if (grade == 3)
        {
            m_skillOpen.gameObject.SetActive(false);
            m_high.gameObject.SetActive(true);
        }
        else if (grade == 4)
        {
            m_masterOpen.gameObject.SetActive(false);
            m_master.gameObject.SetActive(true);
        }
    }
    void CloseSlots(int grade) //특성 오픈 가능 여부 확인하여 닫아줌
    {
        if (grade == 3)
        {
            m_skillOpen.gameObject.SetActive(true);
            m_high.gameObject.SetActive(false);
        }
        else if (grade == 4)
        {
            m_masterOpen.gameObject.SetActive(true);
            m_master.gameObject.SetActive(false);
        }
    }

    void TryOpenSkill() //특성화 스킬 슬롯의 개방 가능 여부 판단하여 열어줌
    {
        if (m_player.IsCanOpen(3, m_Type))
        {
            OpenSlots(3);
            if (m_Type.Equals(SkillType.Shooter))
            {
                m_highSlots[0].ChoiceSkillTypes(m_player,PlayerAbilityType.Pistol, "PistolLv3", "권총 강화",this);
                m_highSlots[1].ChoiceSkillTypes(m_player, PlayerAbilityType.SMG, "SMGLv3", "기관단총 강화", this);
                m_highSlots[2].ChoiceSkillTypes(m_player, PlayerAbilityType.Rifle, "RifleLv3", "돌격소총 강화", this);
            }
            else if(m_Type.Equals(SkillType.Physical))
            {
                m_highSlots[0].ChoiceSkillTypes(m_player, PlayerAbilityType.ShotGun, "ShotGunLv3", "샷건 강화", this);
                m_highSlots[1].ChoiceSkillTypes(m_player, PlayerAbilityType.MG, "MachineGunLv3", "기관총 강화", this);
                m_highSlots[2].ChoiceSkillTypes(m_player, PlayerAbilityType.Melee, "AxeLv3", "근접무기 강화", this);
            }
            else if (m_Type.Equals(SkillType.Utility))
            {
                m_highSlots[0].ChoiceSkillTypes(m_player, PlayerAbilityType.Turret, "GunTurret", "포탑 강화", this);
                m_highSlots[1].ChoiceSkillTypes(m_player, PlayerAbilityType.Barricade, "Barricade", "바리케이드 강화", this);
                m_highSlots[2].ChoiceSkillTypes(m_player, PlayerAbilityType.CyberWear, "CyberWear", "아군 강화", this);
            }
        }
    }
    public void UpdateSP(PlayerSkillController player) //스킬 포인트 갱신
    {
         m_player = player;
        int point = m_player.GetPlayerSP();
        m_skillPoints.text =  "보유스킬포인트 : " + point;
    }
    public void TypeChoiceFinished() // 특성 선택이 완료되었을때 정보를 받아와 세팅.
    {
        SetSlotList(m_Type);
    }
    void TryOpenMasterSkillSlot() // 마스터 특성칸 열기 시도함.
    {
        if (m_player.IsCanOpen(4, m_Type)) //플레이어 스킬 컨트롤러를 통해 가능한지 여부 판단하여 작동.
        {
            OpenSlots(4);
            SetSlotList(m_Type);
            m_player.ActivedMasterSkill();
        }
    }
    void SetAgilitySlot() // 스킬 리스트를 사격술 리스트로.
    {
        m_Type = SkillType.Shooter;
        SetSlotList(m_Type);
    }
    void SetStrengthSlot()// 스킬 리스트를 신체 리스트로.
    {
        m_Type = SkillType.Physical;
        SetSlotList(m_Type);
    }
    void SetUtilitySlot()// 스킬 리스트를 유틸리티 리스트로.
    {
        m_Type = SkillType.Utility;
        SetSlotList(m_Type);
    }
    void SetSlotList(SkillType type) //스킬 타입에 따라 리스트의 종류를 바꾸어주고 습득 가능한 스킬을 세팅해줌.
    {
        ResetList();
        m_Type = type;
        m_count = 0;
        if(m_Type.Equals(SkillType.Shooter))
        {
            m_SkillList = Skilldata.AgilityList;
        }
        else if(m_Type.Equals(SkillType.Physical))
        {
            m_SkillList = Skilldata.StrengthList;
        }
        else if(m_Type.Equals(SkillType.Utility))
        {
            m_SkillList = Skilldata.UtilityList;
        }
        for (int i = 0; i < m_SkillList.Count; i++)
        {
            if (m_stat.GetSkillData(m_SkillList[i]).SkillGrade == 1)
            {
                if(m_stat.GetSkillData(m_SkillList[i]).PrevID == 0 || m_player.IsActived(m_stat.GetSkillData(m_SkillList[i]).PrevID))
                {
                    m_lowSlots[m_stat.GetSkillData(m_SkillList[i]).SkillPos % 3].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                }//스킬등급이 1이면서 이전 스킬이 기본등급(이전 스킬 ID값이 0임)이거나 이전스킬이 Active상태이면 스킬 ID정보값 보내주기
            }
            else if (m_stat.GetSkillData(m_SkillList[i]).SkillGrade == 2)
            {
                if (m_stat.GetSkillData(m_SkillList[i]).PrevID == 0 || m_player.IsActived(m_stat.GetSkillData(m_SkillList[i]).PrevID))
                {
                    m_midSlots[m_stat.GetSkillData(m_SkillList[i]).SkillPos % 3].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                }//스킬등급이 2이면서 이전 스킬이 기본등급(이전 스킬 ID값이 0임)이거나 이전스킬이 Active상태이면 스킬 ID정보값 보내주기
            }
            else if (m_stat.GetSkillData(m_SkillList[i]).SkillGrade == 3)
            {
                switch (m_player.GetPlayerAbilityState())
                {
                    case PlayerAbilityType.None: //플레이어가 특성이 켜져있지 않다면 슬롯들을 꺼주기
                        CloseSlots(3);
                        break;
                    case PlayerAbilityType.Pistol:
                        if(m_stat.GetSkillData(m_SkillList[i]).SkillWeaponType == SkillWeaponType.Pistol)
                        {
                            OpenSlots(3);
                            m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                            m_count++;
                        }
                        break;
                    case PlayerAbilityType.SMG:
                       
                        if (m_stat.GetSkillData(m_SkillList[i]).SkillWeaponType == SkillWeaponType.SMG)
                        {
                            OpenSlots(3);
                            m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                            m_count++;
                        }
                        break;
                    case PlayerAbilityType.Rifle:
                        
                        if (m_stat.GetSkillData(m_SkillList[i]).SkillWeaponType == SkillWeaponType.Rifle)
                        {
                            OpenSlots(3);
                            m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                            m_count++;
                        }
                        break;
                    case PlayerAbilityType.MG:
                       
                        if (m_stat.GetSkillData(m_SkillList[i]).SkillWeaponType == SkillWeaponType.MachineGun)
                        {
                            OpenSlots(3);
                            m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                            m_count++;
                        }
                        break;
                    case PlayerAbilityType.ShotGun:
                        
                        if (m_stat.GetSkillData(m_SkillList[i]).SkillWeaponType == SkillWeaponType.ShotGun)
                        {
                            OpenSlots(3);
                            m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                            m_count++;
                        }
                        break;

                    case PlayerAbilityType.Melee:
                        if (m_stat.GetSkillData(m_SkillList[i]).SkillWeaponType == SkillWeaponType.Melee)
                        {
                            OpenSlots(3);
                            m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                            m_count++;
                        }
                        break;
                    case PlayerAbilityType.Barricade:
                        {
                            if (m_stat.GetSkillData(m_SkillList[i]).AbilityTypeChecker.Equals("Barricade"))
                            {
                                OpenSlots(3);
                                m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                                m_count++;
                            }
                        }
                        break;
                    case PlayerAbilityType.Turret:
                        {
                            if (m_stat.GetSkillData(m_SkillList[i]).AbilityTypeChecker.Equals("Turret"))
                            {
                                OpenSlots(3);
                                m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                                m_count++;
                            }
                        }
                        break;
                    case PlayerAbilityType.CyberWear:
                        {
                            if (m_stat.GetSkillData(m_SkillList[i]).AbilityTypeChecker.Equals("CynerWare"))
                            {
                                OpenSlots(3);
                                m_highSlots[m_count].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                                m_count++;
                            }
                        }
                        break;
                }
            }
            else if (m_stat.GetSkillData(m_SkillList[i]).SkillGrade == 4)
            {

                if (!m_player.IsMasterSkillActived())
                {
                     CloseSlots(4);
                }
                else
                {
                    OpenSlots(4);
                    m_player.GetComponent<PlayerController>().SetTitle(m_stat.GetSkillData(m_SkillList[i]).SkillName);
                    m_masterSlot.SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                }
            }
        }

    }
    void ResetList() //현재 스킬 리스트를 초기화.
    {
        m_SkillList = new List<int>();
    }
    public void ActiveSkill(PlayerSkillController skill) //스킬창 열때 플레이어 정보도 호출함
    {
        m_player = skill;
        m_player.SetSkillUI(this);
        gameObject.SetActive(true);
        UpdateSP(skill);
    }
    public void DeActiveSkill() //UI종료
    {
        gameObject.SetActive(false);
    }
    void SetButtonAddListener() //AddListener을 통해 버튼 UI에 기능 부여.
    {
        m_skillOpen.onClick.AddListener(TryOpenSkill);
        m_masterOpen.onClick.AddListener(TryOpenMasterSkillSlot);
        m_Agility.onClick.AddListener(SetAgilitySlot);
        m_Strength.onClick.AddListener(SetStrengthSlot);
        m_Utility.onClick.AddListener(SetUtilitySlot);
    }

    private void Start()
    {
        SetAgilitySlot(); //스타트 할 때에 기본 리스트는 사격술 카테고리가 오게끔 초기화해줌.
    }
    #endregion
}
