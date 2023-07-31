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
    public float Heal { get; set; }
    public float LastFire { get; set; }
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
    public int TurretMaxBuild { get; set; }
    public int BarricadeMaxBuild { get; set; }
    public float ObjectRegen { get; set; }
    public float BarricadeRegen { get; set; }
    public float BonusHP { get; set; }
    public float TurretHP { get; set; }
    public float BarricadeHP { get; set; }
    public float ObjectHP { get; set; }
    public float CyberWear { get; set; }
    public float publicBuffDamage { get; set; }
    public int BuffArmorPierce { get; set; }
    public float TurretRigist { get; set; }
    public float BarricadeRigist { get; set; }
    public float ObjectRigist { get; set; }
    public float TurretDamage { get; set; }
    public float TurretRange { get; set; }
    public float TurretAttackSpeed { get; set; }
    public int TurretArmorPierce { get; set; }
    public float BarricadeDefence { get; set; }
    public float ObjectDefence { get; set; }
    public float ReflectDamge { get; set; }
    public int MaxMachineLearning { get; set; }
    public string AbilityTypeChecker { get; set; }
    public TableSkillStat() { }
    public TableSkillStat(string skillName, int Id, string image, SkillType skilltype, SkillWeaponType skillweapon, string skillInfo, float damage, float atkSpeed, float Reload, float speed, int criRate, float cridam, float mag, float defence, float damageRigist, float hp, float knockbackrate, float heal, float lastfire, int pierce, int boom, int skillpoint, float armorpierce, float remove, int drain, float crush, int burn, bool isactive, int skillPos, int nextID, int prevID, int skillGrade, int turretmaxbuild, int barricademaxbuld, float objectregen, float barricaderegen, float bonushp, float turrethp, float barricadehp, float objecthp, float cyberwear, float publicbuffdamge, int buffarmorpierce, float turretrigist, float barricaderigist, float objectrigist, float turretdamage, float turretrange, float turretattackspeed, int turretarmorpierce, float barricadedefence, float objectdefence, float reflectdamage, int maxmachinelearning, string abilityTypeChecker)
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
        this.TurretMaxBuild = turretmaxbuild;
        this.BarricadeMaxBuild = barricademaxbuld;
        this.ObjectRegen = objectregen;
        this.BarricadeRegen = barricaderegen;
        this.BonusHP = bonushp;
        this.TurretHP = turrethp;
        this.BarricadeHP = barricadehp;
        this.ObjectHP = objecthp;
        this.CyberWear = cyberwear;
        this.publicBuffDamage = publicbuffdamge;
        this.BuffArmorPierce = buffarmorpierce;
        this.TurretRigist = turretrigist;
        this.BarricadeRigist = barricaderigist;
        this.ObjectRigist = objectrigist;
        this.TurretDamage = turretdamage;
        this.TurretRange = turretrange;
        this.TurretArmorPierce = turretarmorpierce;
        this.BarricadeDefence = barricadedefence;
        this.ObjectDefence = objectdefence;
        this.ReflectDamge = reflectdamage;
        this.MaxMachineLearning = maxmachinelearning;
        this.AbilityTypeChecker = abilityTypeChecker;
    }
    public TableSkillStat GetSkillData(int id)
    {
        TableSkillStat skilldata = null;
        skilldata = Skilldata.Instance.m_dic[id];
        /*
        TableSkillStat stat = new TableSkillStat(skilldata.SkillName, skilldata.ID, skilldata.Image, skilldata.SkillType, skilldata.SkillWeaponType, skilldata.SkillInfo, skilldata.Damage, skilldata.AtkSpeed, skilldata.Reload, skilldata.Speed, skilldata.CriRate,
            skilldata.Mag, skilldata.CriDamage, skilldata.Defence, skilldata.DamageRigist, skilldata.HP, skilldata.KnockBackRate, skilldata.Heal, skilldata.LastFire, skilldata.Pierce, skilldata.Boom, skilldata.SkillPoint, skilldata.ArmorPierce
            ,skilldata.Remove, skilldata.Drain,skilldata.Crush, skilldata.Burn, skilldata.isActive, skilldata.SkillPos,skilldata.NextID,skilldata.PrevID,skilldata.SkillGrade,skilldata.TurretMaxBuild,skilldata.BarricadeMaxBuild,skilldata.ObjectRegen,skilldata.BarricadeRegen,skilldata.BonusHP,skilldata.TurretHP,skilldata.BarricadeHP,skilldata.ObjectHP,skilldata.CyberWear,skilldata.publicBuffDamage,skilldata.BuffArmorPierce,skilldata.TurretRigist,skilldata.BarricadeRigist,skilldata.ObjectRigist,skilldata.TurretDamage,skilldata.TurretRange,skilldata.TurretAttackSpeed,skilldata.TurretArmorPierce,skilldata.BarricadeDefence,skilldata.ObjectDefence,skilldata.ReflectDamge,skilldata.MaxMachineLearning);*/
        return skilldata;
    }

}
class Skilldata : Singleton<Skilldata>
{
    public Dictionary<int, TableSkillStat> m_dic = new Dictionary<int, TableSkillStat>();
    public static List<int> AgilityList = new List<int>();
    public static List<int> StrengthList = new List<int>();
    public static List<int> UtilityList = new List<int>();

    public void Load()
    {
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("Ability"));
        m_dic.Clear();
        AgilityList.Clear();
        StrengthList.Clear();
        UtilityList.Clear();
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
            data.Heal = TableLoader.Instance.GetFloat("Heal", i);
            data.LastFire = TableLoader.Instance.GetFloat("LastFire", i);
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
            data.SkillInfo = TableLoader.Instance.GetString("Info", i);
            data.TurretMaxBuild = TableLoader.Instance.GetInteger("TurretMaxBuild", i);
            data.BarricadeMaxBuild = TableLoader.Instance.GetInteger("BarricadeMaxBuild", i);
            data.ObjectRegen = TableLoader.Instance.GetFloat("ObjectRegen", i);
            data.BarricadeRegen = TableLoader.Instance.GetFloat("BarricadeRegen", i);
            data.BonusHP = TableLoader.Instance.GetFloat("BonusHP", i);
            data.TurretHP = TableLoader.Instance.GetFloat("TurretHP", i);
            data.BarricadeHP = TableLoader.Instance.GetFloat("BarricadeHP", i);
            data.ObjectHP = TableLoader.Instance.GetFloat("ObjectHP", i);
            data.CyberWear = TableLoader.Instance.GetFloat("Cyberwear", i);
            data.publicBuffDamage = TableLoader.Instance.GetFloat("BuffDamage", i);
            data.BuffArmorPierce = TableLoader.Instance.GetInteger("BuffArmorPierce", i);
            data.TurretRigist = TableLoader.Instance.GetFloat("TurretRigist", i);
            data.ObjectRigist = TableLoader.Instance.GetFloat("ObjectRigist", i);
            data.TurretDamage = TableLoader.Instance.GetFloat("TurretDamage", i);
            data.TurretRange = TableLoader.Instance.GetFloat("TurretRange", i);
            data.TurretAttackSpeed = TableLoader.Instance.GetFloat("TurretAttackSpeed", i);
            data.BarricadeDefence = TableLoader.Instance.GetFloat("BarricadeDefence", i);
            data.ObjectDefence = TableLoader.Instance.GetFloat("ObjectDefence", i);
            data.ReflectDamge = TableLoader.Instance.GetFloat("ReflectDamage", i);
            data.MaxMachineLearning = TableLoader.Instance.GetInteger("MachineLearing", i);
            data.AbilityTypeChecker = TableLoader.Instance.GetString("AbilityType", i);
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
    }
}

