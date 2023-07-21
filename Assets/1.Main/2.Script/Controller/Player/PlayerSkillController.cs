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
    } //������ ����� ��ų�� Ư��ȭ���� ��ġ�ϴ��� Ȯ�ο�
    public bool IsCanActive(int point,int skillgrade,SkillType skilltype) //��ų ���� �������� Ȯ�����ִ� �޼���
    {
        if (m_skillPoint >= point)
        {
            CheckActivedSkillLV(skilltype); //���� Ȱ��ȭ�Ǿ��ִ� ��ų����Ʈ�� �ܰ躰 ���� Ȯ��
            if(skillgrade == 2 && m_lowLvCount < 8)
            {
                UGUIManager.Instance.SystemMessageSendMessage("1�ܰ� ��ų�� 8�� �̻� ���� �� ���� �����մϴ�.");
                return false;
            }
            else if (skillgrade == 3 && m_midLvCount < 6)
            {
                UGUIManager.Instance.SystemMessageSendMessage("2�ܰ� ��ų�� 6�� �̻� ���� �� ���� �����մϴ�.");
                return false;
            }
            else if (skillgrade == 4 && m_highLvCount < 3)
            {
                UGUIManager.Instance.SystemMessageSendMessage("Ư�� ��ų ��� ���� �� ���� �����մϴ�.");
                return false;
            }
            else
            return true;
        }
        else
        {
            UGUIManager.Instance.SystemMessageSendMessage("��ų ����Ʈ�� ���ڶ��ϴ�."); 
            return false;
        }
    }
    public bool IsActived(int id) //����Ʈ ��� �� ��ų�� ��Ƽ�� �������� �ƴ��� Ȯ�����ִ� �޼ҵ�
    {
        if (m_skillList.Contains(id)) return true;
        else return false;
    }
    public bool IsCanOpen(int grade,SkillType skilltype)
    {
        CheckActivedSkillLV(skilltype);
        if (grade == 3 && m_midLvCount < 6)
        {
            UGUIManager.Instance.SystemMessageSendMessage("2�ܰ� ��ų�� 6�� �̻� ���� �� ���� �����մϴ�.");
            return false;
        }
        else if (grade == 4 && m_highLvCount < 3)
        {
            UGUIManager.Instance.SystemMessageSendMessage("Ư����ų�� ��� ������ �� ���� �����մϴ�.");
            return false;
        }
        else return true;
    }
    public PlayerAbilityType GetPlayerAbilityState() //�÷��̾��� Ư��ȭ�� �����Ƽ Ÿ���� ������
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
    int m_lowLvCount = 0; //���� �÷��̾ �����ϰ��ִ� ��ų�� ��޺� ���� üũ��
    int m_midLvCount = 0;
    int m_highLvCount = 0;


    public List<int> m_skillList = new List<int>(); //���� Ȱ��ȭ�� ��ų ����Ʈ �����
    #endregion

    #region publicMethod
    public void ActiveSkill(int id) //��ų ��⸦ �õ��Ѵٸ� �� ��ų�� �ʿ� ����Ʈ Ȯ���Ͽ� ����ϸ� ��� ���. + ��ųUI���� ��ų����Ʈ�� Ȯ���ϱ� ������ ���⼭ ����
    {
        m_skillPoint -= m_skilldata.GetSkillData(id).SkillPoint;
        m_skillList.Add(id);
        RefreshSKillData();
        PushSkillUpSignal();
    }
    public void SetWeaponType(WeaponType type)  //���� ���� �� ���� ȣ���Ͽ� ���� �������� ����� ��ų�� ȣȯ�Ǵ��� �Ǵ��ϱ� ����.
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
    public void LevelUP() //������ �� ��ų����Ʈ ���� ��� �ִ� 30���� ���� �ϵ���.
    {
        if (SPCount < 30)
        {
            m_skillPoint++;
            SPCount++;
        }
        else if (SPCount == 30)
        {
            UGUIManager.Instance.SystemMessageSendMessage("�ִ� ��ų ����Ʈ�� �����߽��ϴ�.");
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
        for (int i = 0; i <  m_skillList.Count; i++) //���� Ȱ��ȭ�� ��ų����Ʈ��ŭ �ݺ��Ͽ� �� ID�� ������ ���� ��ų�� ã�ƿ�
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
        m_player.SkillUpInitstatus(); //�÷��̾�� �ѿ� ��ų�ö󰬴ٴ� ���� ����.
        m_gunmanager.SkillUpSignal();
    }
    void ResetDatas() //������ ������ �ִ� ���� �ʱ�ȭ.
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
    void RefreshSKillData() //��ų������ ������ ������ ������ �۾�.
    {
        ResetDatas(); //�����͸� �������ְ�
        for (int i = 0; i < m_skillList.Count; i++) //����Ʈ ���̸�ŭ ����
        {
            if (isActiveType(m_skilldata.GetSkillData(m_skillList[i]).SkillWeaponType, m_weapontype)) //���� ���� ������ ��ų�� �䱸�ϴ� ������ ������ ���ٸ� ���� ������.
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
    } //�÷��̾�� ������ ����
    private void Awake()
    {

        m_skilldata = new TableSkillStat();
        m_player = GetComponent<PlayerController>();
        m_gunmanager = GetComponent<GunManager>();
        m_abilityType = PlayerAbilityType.None;
       // m_skillPoint = 100; �׽�Ʈ��
    }
    #endregion
}
