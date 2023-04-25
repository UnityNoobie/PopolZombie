using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum SkillType
{
    Shooter,
    Physical,
    Utility
}
public enum SkillWeaponType
{
    Every,
    Personal,
    Heavy,
    Pistol,
    SMG,
    Rifle,
    MachineGun,
    ShotGun,
    Melee
}
public class TableSkillStat
{
    //Skill	ID	Image	SkillType	WeaponType	Info	Damage	AtkSpeed	Reload	Speed	CriRate	CriDamage	Mag	Defence	DamageRigist	KnockBackRate	HP	Heal	LastFire	Pierce	Boom	
    public string SkillName { get; set; }
    public int ID { get; set; }
    public string Image { get; set; }
    public SkillType SkillType { get; set; }
    public SkillWeaponType SkillWeaponType { get; set; }

    public string SkillInfo { get; set; }
    public float Damage { get; set; }
    public float AtkSpeed { get; set; }
    public float Reload { get; set; }
    public float Speed { get; set; }
    public int CriRate { get; set; }
    public float CriDamage { get; set; }
    public float Mag { get; set; }
    public float Defence { get; set; }
    public float DamageRigist { get; set; }
    public float HP { get; set; }
    public float KnockBackRate { get; set; }
    public int Heal { get; set; }
    public int LastFire { get; set; }
    public int Pierce { get; set; }
    public int Boom { get; set; }
    public int SkillPoint { get; set; }
    public float ArmorPierce { get; set; }
    public float Remove { get; set; }
    public int Drain { get; set; }
    public float Crush { get; set; }
    public int Burn { get; set; }
    public bool isActive { get; set; }

    public int SkillPos { get; set; }

    public int NextID { get; set; }
    public int PrevID { get; set; }
    public int SkillGrade { get; set; }
    public TableSkillStat() { }
    public TableSkillStat(string skillName, int Id, string image, SkillType skilltype, SkillWeaponType skillweapon, string skillInfo, float damage, float atkSpeed, float Reload, float speed, int criRate, float cridam, float mag, float defence, float damageRigist, float hp, float knockbackrate, int heal, int lastfire, int pierce, int boom, int skillpoint, float armorpierce, float remove, int drain, float crush, int burn, bool isactive, int skillPos, int nextID, int prevID, int skillGrade)
    {
        // int burn, bool isactive
        this.SkillName = skillName;
        this.ID = Id;
        this.Image = image;
        this.SkillType = skilltype;
        this.SkillWeaponType = skillweapon;
        this.SkillInfo = skillInfo;
        this.Damage = damage;
        this.AtkSpeed = atkSpeed;
        this.Reload = Reload;
        this.Speed = speed;
        this.CriRate = criRate;
        this.Mag = mag;
        this.CriDamage = cridam;
        this.Defence = defence;
        this.DamageRigist = damageRigist;
        this.HP = hp;
        this.KnockBackRate = knockbackrate;
        this.Heal = heal;
        this.LastFire = lastfire;
        this.Pierce = pierce;
        this.Boom = boom;
        this.SkillPoint = skillpoint;
        this.ArmorPierce = armorpierce;
        this.Remove = remove;
        this.Drain = drain;
        this.Crush = crush;
        this.Burn = burn;
        this.isActive = isactive;
        this.SkillPos = skillPos;
        this.NextID = nextID;
        this.PrevID = prevID;
        this.SkillGrade = skillGrade;
    }
    public TableSkillStat GetSkillData(int id)
    {
        TableSkillStat skilldata = null;
        skilldata = Skilldata.Instance.m_dic[id];
        TableSkillStat stat = new TableSkillStat(skilldata.SkillName, skilldata.ID, skilldata.Image, skilldata.SkillType, skilldata.SkillWeaponType, skilldata.SkillInfo, skilldata.Damage, skilldata.AtkSpeed, skilldata.Reload, skilldata.Speed, skilldata.CriRate,
            skilldata.Mag, skilldata.CriDamage, skilldata.Defence, skilldata.DamageRigist, skilldata.HP, skilldata.KnockBackRate, skilldata.Heal, skilldata.LastFire, skilldata.Pierce, skilldata.Boom, skilldata.SkillPoint, skilldata.ArmorPierce
            ,skilldata.Remove, skilldata.Drain,skilldata.Crush, skilldata.Burn, skilldata.isActive, skilldata.SkillPos,skilldata.NextID,skilldata.PrevID,skilldata.SkillGrade);
        return skilldata;
    }

}
public class SKillPos
{
    public int Pos { get; set; }
    public int ID { get; set; }
}
class Skilldata : Singleton<Skilldata>
{
    public TableSkillStat m_stat { get; set; }
    public Dictionary<int, TableSkillStat> m_dic = new Dictionary<int, TableSkillStat>();
    public static List<int> AgilityList = new List<int>();
    public static List<int> StrengthList = new List<int>();
    public static List<int> UtilityList = new List<int>();

    /*
   public void AA()
   {
     List<SKillPos> AgilityPos = new List<SKillPos>()
     {
      new SKillPos(){ ID = AgilityList[0], Pos = m_stat.GetSkillData(AgilityList[0]).SkillPos},
      new SKillPos(){ ID = AgilityList[1], Pos = m_stat.GetSkillData(AgilityList[1]).SkillPos   },
      new SKillPos(){ ID = AgilityList[2], Pos = m_stat.GetSkillData(AgilityList[2]).SkillPos   },
      new SKillPos(){ ID = AgilityList[3], Pos = m_stat.GetSkillData(AgilityList[3]).SkillPos   },
      new SKillPos(){ ID = AgilityList[4], Pos = m_stat.GetSkillData(AgilityList[4]).SkillPos   },
      new SKillPos(){ ID = AgilityList[5], Pos = m_stat.GetSkillData(AgilityList[5]).SkillPos   },
      new SKillPos(){ ID = AgilityList[6], Pos = m_stat.GetSkillData(AgilityList[6]).SkillPos   },
      new SKillPos(){ ID = AgilityList[7], Pos = m_stat.GetSkillData(AgilityList[7]).SkillPos   },
      new SKillPos(){ ID = AgilityList[8], Pos = m_stat.GetSkillData(AgilityList[8]).SkillPos   },
      new SKillPos(){ ID = AgilityList[9], Pos = m_stat.GetSkillData(AgilityList[9]).SkillPos   },
      new SKillPos(){ ID = AgilityList[10], Pos = m_stat.GetSkillData(AgilityList[10]).SkillPos   },
      new SKillPos(){ ID = AgilityList[11], Pos = m_stat.GetSkillData(AgilityList[11]).SkillPos   },
      new SKillPos(){ ID = AgilityList[12], Pos = m_stat.GetSkillData(AgilityList[12]).SkillPos   },
      new SKillPos(){ ID = AgilityList[13], Pos = m_stat.GetSkillData(AgilityList[13]).SkillPos   },
      new SKillPos(){ ID = AgilityList[14], Pos = m_stat.GetSkillData(AgilityList[14]).SkillPos   },
      new SKillPos(){ ID = AgilityList[15], Pos = m_stat.GetSkillData(AgilityList[15]).SkillPos   },
      new SKillPos(){ ID = AgilityList[16], Pos = m_stat.GetSkillData(AgilityList[16]).SkillPos},
      new SKillPos(){ ID = AgilityList[17], Pos = m_stat.GetSkillData(AgilityList[17]).SkillPos},
      new SKillPos(){ ID = AgilityList[18], Pos = m_stat.GetSkillData(AgilityList[18]).SkillPos   },
      new SKillPos(){ ID = AgilityList[19], Pos = m_stat.GetSkillData(AgilityList[19]).SkillPos   },
      new SKillPos(){ ID = AgilityList[20], Pos = m_stat.GetSkillData(AgilityList[20]).SkillPos   },
      new SKillPos(){ ID = AgilityList[21], Pos = m_stat.GetSkillData(AgilityList[21]).SkillPos   },
      new SKillPos(){ ID = AgilityList[22], Pos = m_stat.GetSkillData(AgilityList[22]).SkillPos   },
      new SKillPos(){ ID = AgilityList[23], Pos = m_stat.GetSkillData(AgilityList[23]).SkillPos   },
      new SKillPos(){ ID = AgilityList[24], Pos = m_stat.GetSkillData(AgilityList[24]).SkillPos   },
      new SKillPos(){ ID = AgilityList[25], Pos = m_stat.GetSkillData(AgilityList[25]).SkillPos   },
      new SKillPos(){ ID = AgilityList[26], Pos = m_stat.GetSkillData(AgilityList[26]).SkillPos   },
      new SKillPos(){ ID = AgilityList[27], Pos = m_stat.GetSkillData(AgilityList[27]).SkillPos   },
      new SKillPos(){ ID = AgilityList[28], Pos = m_stat.GetSkillData(AgilityList[28]).SkillPos   },
      new SKillPos(){ ID = AgilityList[29], Pos = m_stat.GetSkillData(AgilityList[29]).SkillPos   },
      new SKillPos(){ ID = AgilityList[30], Pos = m_stat.GetSkillData(AgilityList[30]).SkillPos}
     };
       List<SKillPos> StrengthPos = new List<SKillPos>()
     {
      new SKillPos(){ ID = StrengthList[0], Pos = m_stat.GetSkillData(StrengthList[0]).SkillPos},
      new SKillPos(){ ID = StrengthList[1], Pos = m_stat.GetSkillData(StrengthList[1]).SkillPos   },
      new SKillPos(){ ID = StrengthList[2], Pos = m_stat.GetSkillData(StrengthList[2]).SkillPos   },
      new SKillPos(){ID = StrengthList[3], Pos = m_stat.GetSkillData(StrengthList[3]).SkillPos},
      new SKillPos(){ ID =StrengthList[4], Pos = m_stat.GetSkillData(StrengthList[4]).SkillPos   },
      new SKillPos(){ ID = StrengthList[5], Pos = m_stat.GetSkillData(StrengthList[5]).SkillPos   },
      new SKillPos(){ ID = StrengthList[6], Pos = m_stat.GetSkillData(StrengthList[6]).SkillPos   },
      new SKillPos(){ ID = StrengthList[7], Pos = m_stat.GetSkillData(StrengthList[7]).SkillPos   },
      new SKillPos(){ ID = StrengthList[8], Pos = m_stat.GetSkillData(StrengthList[8]).SkillPos   },
      new SKillPos(){ ID = StrengthList[9], Pos = m_stat.GetSkillData(StrengthList[9]).SkillPos   },
      new SKillPos(){ ID = StrengthList[10], Pos = m_stat.GetSkillData(StrengthList[10]).SkillPos   },
      new SKillPos(){ ID = StrengthList[11], Pos = m_stat.GetSkillData(StrengthList[11]).SkillPos   },
      new SKillPos(){ ID = StrengthList[12], Pos = m_stat.GetSkillData(StrengthList[12]).SkillPos   },
      new SKillPos(){ ID = StrengthList[13], Pos = m_stat.GetSkillData(StrengthList[13]).SkillPos   },
      new SKillPos(){ ID = StrengthList[14], Pos = m_stat.GetSkillData(StrengthList[14]).SkillPos   },
      new SKillPos(){ ID = StrengthList[15], Pos = m_stat.GetSkillData(StrengthList[15]).SkillPos   },
      new SKillPos(){ ID = StrengthList[16], Pos = m_stat.GetSkillData(StrengthList[16]).SkillPos},
      new SKillPos(){ ID = StrengthList[17], Pos = m_stat.GetSkillData(StrengthList[17]).SkillPos},
      new SKillPos(){ID = StrengthList[18], Pos = m_stat.GetSkillData(StrengthList[18]).SkillPos  },
      new SKillPos(){ ID = StrengthList[19], Pos = m_stat.GetSkillData(StrengthList[19]).SkillPos   },
      new SKillPos(){ ID = StrengthList[20], Pos = m_stat.GetSkillData(StrengthList[20]).SkillPos   },
      new SKillPos(){ ID = StrengthList[21], Pos = m_stat.GetSkillData(StrengthList[21]).SkillPos   },
      new SKillPos(){ ID = StrengthList[22], Pos = m_stat.GetSkillData(StrengthList[22]).SkillPos   },
      new SKillPos(){ ID = StrengthList[23], Pos = m_stat.GetSkillData(StrengthList[23]).SkillPos   },
      new SKillPos(){ ID = StrengthList[24], Pos = m_stat.GetSkillData(StrengthList[24]).SkillPos   },
      new SKillPos(){ ID = StrengthList[25], Pos = m_stat.GetSkillData(StrengthList[25]).SkillPos   },
      new SKillPos(){ ID = StrengthList[26], Pos = m_stat.GetSkillData(StrengthList[26]).SkillPos   },
      new SKillPos(){ ID = StrengthList[27], Pos = m_stat.GetSkillData(StrengthList[27]).SkillPos   },
      new SKillPos(){ID = StrengthList[28], Pos = m_stat.GetSkillData(StrengthList[28]).SkillPos   },
      new SKillPos(){ID = StrengthList[29], Pos = m_stat.GetSkillData(StrengthList[29]).SkillPos  },
      new SKillPos(){ ID = StrengthList[30], Pos = m_stat.GetSkillData(StrengthList[30]).SkillPos}
     };

       SkillManager.Instance.SetListPos(AgilityPos, StrengthPos);
   }*/
    public void Load()
    {
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("Ability"));
        m_dic.Clear();
        for (int i = 0; i < 93; i++)
        {
            TableSkillStat data = new TableSkillStat();
            data.SkillName = TableLoader.Instance.GetString("Skill", i);
            data.ID = TableLoader.Instance.GetInteger("ID", i);
            data.Image = TableLoader.Instance.GetString("Image", i);
            string skillweapon = TableLoader.Instance.GetString("WeaponType", i);
            if (skillweapon.Equals("Every"))
            {
                data.SkillWeaponType = SkillWeaponType.Every;
            }
            else if (skillweapon.Equals("Personal"))
            {
                data.SkillWeaponType = SkillWeaponType.Personal;
            }
            else if (skillweapon.Equals("Heavy"))
            {
                data.SkillWeaponType = SkillWeaponType.Heavy;
            }
            else if (skillweapon.Equals("Pistol"))
            {
                data.SkillWeaponType = SkillWeaponType.Pistol;
            }
            else if (skillweapon.Equals("SMG"))
            {
                data.SkillWeaponType = SkillWeaponType.SMG;
            }
            else if (skillweapon.Equals("Rifle"))
            {
                data.SkillWeaponType = SkillWeaponType.Rifle;
            }
            else if (skillweapon.Equals("ShotGun"))
            {
                data.SkillWeaponType = SkillWeaponType.ShotGun;
            }
            else if (skillweapon.Equals("MachineGun"))
            {
                data.SkillWeaponType = SkillWeaponType.MachineGun;
            }
            else if (skillweapon.Equals("Melee"))
            {
                data.SkillWeaponType = SkillWeaponType.Melee;
            }
            else
            {
                Debug.Log("대상 무기타입이 잘못됨.");
            }
            data.SkillInfo = TableLoader.Instance.GetString("Info", i);
            data.Damage = TableLoader.Instance.GetFloat("Damage", i);
            data.AtkSpeed = TableLoader.Instance.GetFloat("AtkSpeed", i);
            data.Reload = TableLoader.Instance.GetFloat("Reload", i);
            data.Speed = TableLoader.Instance.GetFloat("Speed", i);
            data.CriRate = TableLoader.Instance.GetInteger("CriRate", i);
            data.CriDamage = TableLoader.Instance.GetFloat("CriDamage", i);
            data.Mag = TableLoader.Instance.GetFloat("Mag", i);
            data.Defence = TableLoader.Instance.GetFloat("Defence", i);
            data.DamageRigist = TableLoader.Instance.GetFloat("DamageRigist", i);
            data.KnockBackRate = TableLoader.Instance.GetFloat("KnockBackRate", i);
            data.HP = TableLoader.Instance.GetFloat("HP", i);
            data.Heal = TableLoader.Instance.GetInteger("Heal", i);
            data.LastFire = TableLoader.Instance.GetInteger("LastFire", i);
            data.Pierce = TableLoader.Instance.GetInteger("Pierce", i);
            data.Boom = TableLoader.Instance.GetInteger("Boom", i);
            data.SkillPoint = TableLoader.Instance.GetInteger("Point", i);
            data.ArmorPierce = TableLoader.Instance.GetFloat("ArmorPierce", i);
            data.Remove = TableLoader.Instance.GetFloat("Remove", i);
            data.Burn = TableLoader.Instance.GetInteger("Burn", i);
            data.Drain = TableLoader.Instance.GetInteger("Drain", i);
            data.Crush = TableLoader.Instance.GetFloat("Crush", i);
            data.SkillPos = TableLoader.Instance.GetInteger("SkillPos", i);
            data.isActive = false;
            data.NextID = TableLoader.Instance.GetInteger("NextSkill", i);
            data.PrevID = TableLoader.Instance.GetInteger("PreSkill", i);
            data.SkillGrade = TableLoader.Instance.GetInteger("SkillGrade", i);
            string skilltype = TableLoader.Instance.GetString("SkillType", i);
            if (skilltype.Equals("Shooter"))
            {
                data.SkillType = SkillType.Shooter;
                AgilityList.Add(data.ID);   
            }
            else if (skilltype.Equals("Physical"))
            {
                data.SkillType = SkillType.Physical;
                StrengthList.Add(data.ID);
            }
            else if (skilltype.Equals("Utility"))
            {
                data.SkillType = SkillType.Utility;
                UtilityList.Add(data.ID);
            }
            else
            {
                Debug.Log("특성타입 입력이 잘못되었습니다.");
            }
            m_dic.Add(data.ID, data);
        }
        TableLoader.Instance.Clear();
       // AA();
    }
}

