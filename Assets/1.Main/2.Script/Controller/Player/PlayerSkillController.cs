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
    /*
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
    int Burn;*/ //�������� ���X
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
    public TableSkillStat GetPlayerSkillData()
    {
        RefreshSKillData();
        return m_playerSkillstat;
    }

    #endregion

    #region Property
  
    public TableSkillStat m_skilldata{get;set;}

    #endregion

    #region Constants and Fields
    WeaponType m_weapontype;
    PlayerAbilityType m_abilityType;
    PlayerController m_player;
    PlayerObjectController m_playerObject;
    TableSkillStat m_playerSkillstat;
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
        m_gunmanager.SkillUpSignal();
        m_playerObject.ObjectUpgrade();
    }
    void ResetDatas() //������ ������ �ִ� ���� �ʱ�ȭ.
    {
        m_playerSkillstat = new TableSkillStat();
        /*
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
        Burn = 0;*/
    }
    void RefreshSKillData() //��ų������ ������ ������ ������ �۾�.
    {
        ResetDatas(); //�����͸� �������ְ�
        for (int i = 0; i < m_skillList.Count; i++) //����Ʈ ���̸�ŭ ����
        {
            if (isActiveType(m_skilldata.GetSkillData(m_skillList[i]).SkillWeaponType, m_weapontype)) //���� ���� ������ ��ų�� �䱸�ϴ� ������ ������ ���ٸ� ���� ������.
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
                */ //��������
            }
        }
        m_player.SetSkillData(m_playerSkillstat);
    } //�÷��̾�� ������ ����
    private void Awake()
    {
        m_skilldata = new TableSkillStat();
        m_player = GetComponent<PlayerController>();
        m_gunmanager = GetComponent<GunManager>();
        m_playerObject = GetComponent<PlayerObjectController>();
        m_abilityType = PlayerAbilityType.None;
        m_skillPoint = 100;// �׽�Ʈ��
        ObjectManager.Instance.SetPlayer(this);
    }
    #endregion
}
