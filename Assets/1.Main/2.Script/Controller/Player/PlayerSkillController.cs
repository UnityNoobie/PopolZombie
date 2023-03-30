using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int Heal;
    int LastFire;
    int Pierce;
    int Boom;
    float ArmorPierce;
    float Remove;
    int Drain;
    int Crush;
    int Burn;
    #endregion

    #region SkillTypeChecker
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
    }
    #endregion
    WeaponType m_weapontype;
    SkillWeaponType m_skillweapon;
    public TableSkillStat m_skilldata{get;set;}
    PlayerController m_player;
    int m_skillPoint = 0;
    int SPCount = 0;
    List<int> m_skillList = new List<int>();


    int count = 0; //���� ��ų��°� �׽�Ʈ��.
    int bcount = 0;
    int[] testskillid =
    {
        11001,
        11002,
        11003,
        11004,
        11011,
        11012,
        11013,
        11014,
        11021,
        11022,
        11023,
        11024,
        12001,
        12002,
        12003,
        12011,
        12012,
        12013,
        12021,
        12022,
        12023,
        13001,
        13002,
        13003,
        13011,
        13012,
        13013,
        13021,
        13022,
        13023,
        14111
    };
    int[] testbodyid =
    {
        21001,
21002,
21003,
21004,
21011,
21012,
21013,
21014,
21021,
21022,
21023,
21024,
22001,
22002,
22003,
22011,
22012,
22013,
22021,
22022,
22023,
23001,
23002,
23003,
23011,
23012,
23013,
23021,
23022,
23023,
24111
    };
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
    public void LevelUP() //������ �� ��ų����Ʈ ���� ��� �ִ� 30���� ���� �ϵ���.
    {
        if(SPCount < 30)
        {
            m_skillPoint++;
            SPCount++;
        }
    }
    void RefreshSKillData() //��ų������ ������ ������ ������ �۾�.
    {
        ResetDatas(); //�����͸� �������ְ�
        for(int i = 0; i < m_skillList.Count; i++) //����Ʈ ���̸�ŭ ����
        {
            if (isActiveType(m_skilldata.GetSkillData(m_skillList[i]).SkillWeaponType, m_weapontype)) //���� ���� ������ ��ų�� �䱸�ϴ� ������ ������ ���ٸ� ���� ������.
            {
                Damage += m_skilldata.GetSkillData(m_skillList[i]).Damage;
                AtkSpeed += m_skilldata.GetSkillData(m_skillList[i]).ArmorPierce;
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
        m_player.SetSkillData(Damage,AtkSpeed,Reload,Speed,CriRate,CriDamage,Mag,Defence,DamageRigist,HP,KnockBackRate,Heal,LastFire,Pierce,Boom, ArmorPierce,Remove,Drain, Crush, Burn); 
    } //�÷��̾�� ������ ����
    public void ActiveSkill(int id) //��ų ��⸦ �õ��Ѵٸ� �� ��ų�� �ʿ� ����Ʈ Ȯ���Ͽ� ����ϸ� ��� ���.
    {
        //Debug.Log(m_skilldata.GetSkillData(id).SkillName);
        if(m_skillPoint >= m_skilldata.GetSkillData(id).SkillPoint)
        {
            m_skillPoint -= m_skilldata.GetSkillData(id).SkillPoint;
            m_skillList.Add(id);
        }
        RefreshSKillData();
        m_player.SkillUpInitstatus(); //��ų���� �ø��� ���� �ȵǴ� ����߻�. �߰��� �������� �ٷ� �Ѱ��ֱ�.
    }
    public void SetWeaponType(WeaponType type)  //���� ���� �� ���� ȣ���Ͽ� ���� �������� ����� ��ų�� ȣȯ�Ǵ��� �Ǵ��ϱ� ����.
    {
        m_weapontype = type;
        RefreshSKillData();
    }

    private void Awake()
    {
        m_skilldata = new TableSkillStat();
        m_player = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) //�׽�Ʈ�� �Դϴ�.
        {
            m_skillPoint = 100;
          //  Debug.Log(testskillid[count]);
            UIManager.Instance.SystemMessageCantOpen("��ų �ڵ�" + testskillid[count] + " ����, ����ȭ�� ��ȭ Ʈ���Դϴ� �� ����Ǵ��� Ȯ����.");
            ActiveSkill(testskillid[count]);
            count++;
        }
        if (Input.GetKeyDown(KeyCode.L)) //�׽�Ʈ�� �Դϴ�2.
        {
            m_skillPoint = 100;
            //  Debug.Log(testskillid[count]);
            UIManager.Instance.SystemMessageCantOpen("��ų �ڵ�" + testbodyid[bcount] + " ����, ��ȭ�� ��ȭ Ʈ���Դϴ� �� ����Ǵ��� Ȯ����.");
            ActiveSkill(testbodyid[bcount]);
            bcount++;
        }
    }
}
