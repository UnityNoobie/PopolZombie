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


    int count = 0; //이하 스킬찍는거 테스트용.
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
    public void LevelUP() //레벨업 시 스킬포인트 증가 기능 최대 30까지 가능 하도록.
    {
        if(SPCount < 30)
        {
            m_skillPoint++;
            SPCount++;
        }
    }
    void RefreshSKillData() //스킬데이터 가져올 때마다 수행할 작업.
    {
        ResetDatas(); //데이터를 리셋해주고
        for(int i = 0; i < m_skillList.Count; i++) //리스트 길이만큼 실행
        {
            if (isActiveType(m_skilldata.GetSkillData(m_skillList[i]).SkillWeaponType, m_weapontype)) //현재 무기 정보와 스킬이 요구하는 무기의 종류가 같다면 스탯 더해줌.
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
    } //플레이어에게 데이터 전송
    public void ActiveSkill(int id) //스킬 찍기를 시도한다면 그 스킬의 필요 포인트 확인하여 충분하면 찍는 방식.
    {
        //Debug.Log(m_skilldata.GetSkillData(id).SkillName);
        if(m_skillPoint >= m_skilldata.GetSkillData(id).SkillPoint)
        {
            m_skillPoint -= m_skilldata.GetSkillData(id).SkillPoint;
            m_skillList.Add(id);
        }
        RefreshSKillData();
        m_player.SkillUpInitstatus(); //스킬렙만 올리니 전달 안되는 현상발생. 추가로 스텟정보 바로 넘겨주기.
    }
    public void SetWeaponType(WeaponType type)  //무기 변경 시 마다 호출하여 현재 착용중인 무기와 스킬이 호환되는지 판단하기 위함.
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
        if (Input.GetKeyDown(KeyCode.K)) //테스트용 입니당.
        {
            m_skillPoint = 100;
          //  Debug.Log(testskillid[count]);
            UIManager.Instance.SystemMessageCantOpen("스킬 코드" + testskillid[count] + " 권총, 개인화기 강화 트리입니다 잘 적용되는지 확인좀.");
            ActiveSkill(testskillid[count]);
            count++;
        }
        if (Input.GetKeyDown(KeyCode.L)) //테스트용 입니당2.
        {
            m_skillPoint = 100;
            //  Debug.Log(testskillid[count]);
            UIManager.Instance.SystemMessageCantOpen("스킬 코드" + testbodyid[bcount] + " 근접, 중화기 강화 트리입니다 잘 적용되는지 확인좀.");
            ActiveSkill(testbodyid[bcount]);
            bcount++;
        }
    }
}
