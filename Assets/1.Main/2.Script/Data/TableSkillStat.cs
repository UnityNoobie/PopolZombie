using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    public int ID { get;set; }
    public string Image { get; set; }
    public SkillType SkillType { get; set; }

    public SkillWeaponType SkillWeaponType { get; set; }
 
    public string SkillInfo { get; set; }
    public float Damage { get; set; }
    public float AtkSpeed { get; set; }
    public float Reload { get; set; }
    public float Speed { get; set; }
    public int CriRate { get; set; }
    public float CriDamage { get;set; }
    public float Mag { get; set; }
    public float Defence { get; set; }
    public float DamageRigist { get; set; }
    public float HP { get;set; }
    public float KnockBackRate { get; set; }
    public int Heal { get; set; }   
    public int LastFire { get; set; }
    public int Pierce { get; set; } 
    public int Boom { get; set; }
    public int SkillPoint { get; set; }
    public float ArmorPierce { get; set; }
    public float Remove { get; set; }
    public int Drain { get; set; }
    public int Crush { get; set; }
    public int Burn { get; set; }
    public bool isActive { get; set; }

    public TableSkillStat() { }
    public TableSkillStat(string skillName, int Id, string image, SkillType skilltype,SkillWeaponType skillweapon, string skillInfo, float damage, float atkSpeed, float Reload, float speed, int criRate, float cridam, float mag, float defence, float damageRigist, float hp, float knockbackrate, int heal, int lastfire, int pierce, int boom, int skillpoint, float armorpierce, float remove, int drain, int crush, int burn, bool isactive)
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
    }
    public TableSkillStat GetSkillData(int id)
    {
        TableSkillStat skilldata = null;
        skilldata = Skilldata.Instance.m_dic[id];
        return skilldata;
    }
}
class Skilldata : Singleton<Skilldata>
{
    public Dictionary<int, TableSkillStat> m_dic = new Dictionary<int, TableSkillStat>();

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
            string skilltype = TableLoader.Instance.GetString("SkillType", i);
            if (skilltype.Equals("Shooter"))
            {
                data.SkillType = SkillType.Shooter;
            }
            else if (skilltype.Equals("Physical")) 
            {
                data.SkillType = SkillType.Physical;
            }
            else if (skilltype.Equals("Utility"))
            {
                data.SkillType = SkillType.Utility;
            }
            else
            {
                Debug.Log("특성타입 입력이 잘못되었습니다.");
            }
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
            data.Crush = TableLoader.Instance.GetInteger("Crush", i);
            data.isActive = false;
            m_dic.Add(data.ID,data);
        }
        TableLoader.Instance.Clear();
    }
}

