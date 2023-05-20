using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
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
    
    void SetPos() //�������� �����Ǿ��ִ� ��ų���� �θ� ��ġ �޾ƿ��� �� OutOfIndex ��� ���ͼ� ����
    {
       // m_contents = Utill.GetChildObject(gameObject, "Content");
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
    }
    void FindSlots() //��ġ���� �ڽĿ�����Ʈ�� ���Ը�� ��������
    {
      //  m_SkillSlot = m_contents.GetComponentsInChildren<SkillSlot>();
        m_lowSlots = m_low.GetComponentsInChildren<SkillSlot>();
        m_midSlots = m_mid.GetComponentsInChildren<SkillSlot>();
        m_highSlots = m_high.GetComponentsInChildren<SkillSlot>();
        m_masterSlot = m_master.GetComponentInChildren<SkillSlot>();
    }
    void OpenSlots(int grade) //���� OnOff���
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
    void CloseSlots(int grade)
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

    void TryOpenSkill()
    {
        if (m_player.IsCanOpen(3, m_Type))
        {
            OpenSlots(3);
            if (m_Type.Equals(SkillType.Shooter))
            {
                m_highSlots[0].ChoiceSkillTypes(m_player,PlayerAbilityType.Pistol, "PistolLv3", "���� ��ȭ",this);
                m_highSlots[1].ChoiceSkillTypes(m_player, PlayerAbilityType.SMG, "SMGLv3", "������� ��ȭ", this);
                m_highSlots[2].ChoiceSkillTypes(m_player, PlayerAbilityType.Rifle, "RifleLv3", "���ݼ��� ��ȭ", this);
            }
            else if(m_Type.Equals(SkillType.Physical))
            {
                m_highSlots[0].ChoiceSkillTypes(m_player, PlayerAbilityType.ShotGun, "ShotGunLv3", "���� ��ȭ", this);
                m_highSlots[1].ChoiceSkillTypes(m_player, PlayerAbilityType.MG, "MachineGunLv3", "����� ��ȭ", this);
                m_highSlots[2].ChoiceSkillTypes(m_player, PlayerAbilityType.Melee, "AxeLv3", "�������� ��ȭ", this);
            }
        }
    }
    public void RefreshSP(PlayerSkillController player)
    {
         m_player = player;
        int point = m_player.GetPlayerSP();
        m_skillPoints.text =  "������ų����Ʈ : " + point;
    }
    public void ChoiceFinished()
    {
        SetSlotList(m_Type);
    }
    void TryOpenMaster()
    {
        if (m_player.IsCanOpen(4, m_Type))
        {
            OpenSlots(4);
            SetSlotList(m_Type);
        }
    }
    void SetAgilitySlot()
    {
        m_Type = SkillType.Shooter;
        SetSlotList(m_Type);
    }
    void SetStrengthSlot()
    {
        m_Type = SkillType.Physical;
        SetSlotList(m_Type);
    }
    void SetUtilitySlot()
    {
        UIManager.Instance.SystemMessageCantOpen("��ƿ��Ƽ ��ƿ�� ���� �̱����Դϴ�.");
    }
    void SetSlotList(SkillType type)
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
                }//��ų����� 1�̸鼭 ���� ��ų�� �⺻���(���� ��ų ID���� 0��)�̰ų� ������ų�� Active�����̸� ��ų ID������ �����ֱ�
            }
            else if (m_stat.GetSkillData(m_SkillList[i]).SkillGrade == 2)
            {
                if (m_stat.GetSkillData(m_SkillList[i]).PrevID == 0 || m_player.IsActived(m_stat.GetSkillData(m_SkillList[i]).PrevID))
                {
                    m_midSlots[m_stat.GetSkillData(m_SkillList[i]).SkillPos % 3].SetSkillSlot(m_SkillList[i], m_player, m_player.IsActived(m_SkillList[i]), m_active, m_skillInfo);
                }//��ų����� 2�̸鼭 ���� ��ų�� �⺻���(���� ��ų ID���� 0��)�̰ų� ������ų�� Active�����̸� ��ų ID������ �����ֱ�
            }
            else if (m_stat.GetSkillData(m_SkillList[i]).SkillGrade == 3)
            {
                switch (m_player.GetPlayerAbilityState())
                {
                    case PlayerAbilityType.None: //�÷��̾ Ư���� �������� �ʴٸ� ���Ե��� ���ֱ�
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
                }
            }
            else if (m_stat.GetSkillData(m_SkillList[i]).SkillGrade == 4)
            {
                if (!m_player.IsCanOpen(4, m_stat.GetSkillData(m_SkillList[i]).SkillType))
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
    void ResetList()
    {
       // m_SkillList.Clear();
        m_SkillList = new List<int>();
    }
    public void ActiveSkill(PlayerSkillController skill) //��ųâ ���� �÷��̾� ������ ȣ����
    {
        m_player = skill;
        m_player.SetStore(this);
        gameObject.SetActive(true);
        RefreshSP(skill);
    }
    public void DeActiveSkill()
    {
        gameObject.SetActive(false);
    }
    void GetComponentsSlots(GameObject parent)
    {
        parent.GetComponentsInChildren<SkillSlot>();
    }



    private void Start()
    {
        SetPos();
        FindSlots(); //���� �׽�Ʈ���� �ּ�ó���Ѱ� ���� �������
        m_skillOpen.onClick.AddListener(TryOpenSkill);
        m_masterOpen.onClick.AddListener(TryOpenMaster);
        m_Agility.onClick.AddListener(SetAgilitySlot);
        m_Strength.onClick.AddListener(SetStrengthSlot);
        m_Utility.onClick.AddListener(SetUtilitySlot);
        SetAgilitySlot();   
    }
}
