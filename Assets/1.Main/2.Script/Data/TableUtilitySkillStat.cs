using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class TableUtilitySkillStat
{
    public string SkillName { get; set; }
    public int ID { get; set; }
    public string Image { get; set; }
    public SkillType SkillType { get; set; }
    public string ObjectType { get; set; }
    public string SkillInfo { get; set; }
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
    public int SkillPos { get; set; }
    public int SkillGrade { get; set; }
    public int NextID { get; set; }
    public int PrevID { get; set; }

    public TableUtilitySkillStat() { }
    public TableUtilitySkillStat(string skillName, int id, string image, SkillType type,string objecttype, string skillinfo, int turretmaxbuild, int barricademaxbuld, float objectregen, float barricaderegen, float bonushp, float turrethp, float barricadehp, float objecthp, float cyberwear, float publicbuffdamge, int buffarmorpierce, float turretrigist, float barricaderigist, float objectrigist, float turretdamage, float turretrange, float turretattackspeed, int turretarmorpierce, float barricadedefence, float objectdefence, float reflectdamage, int maxmachinelearning, int skillpos, int skillgrade, int nextid, int previd)
    {
        this.SkillName = skillName;
        this.ID = id;
        this.Image = image;
        this.SkillType = type;
        this.ObjectType = objecttype;
        this.SkillInfo = skillinfo;
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
        this.SkillPos = skillpos;
        this.SkillGrade = skillgrade;
        this.NextID = nextid;
        this.PrevID = previd;
    }
    public TableUtilitySkillStat GetUtillSkillData(int id)
    {
        TableUtilitySkillStat skilldata = null;
        skilldata = UtillData.Instance.m_utillData[id];
        return skilldata;
    }
}
public class UtillData : Singleton<UtillData>
{
     public Dictionary<int,TableUtilitySkillStat> m_utillData = new Dictionary<int, TableUtilitySkillStat> ();
    public static List<int> UtilityList = new List<int>();
    public void Load()
    {
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("UtillAbility"));
        m_utillData.Clear ();
        for(int i = 0; i < 31; i++)
        {
            TableUtilitySkillStat data = new TableUtilitySkillStat();
            data.SkillName = TableLoader.Instance.GetString("Skill", i);
            data.ID = TableLoader.Instance.GetInteger("ID", i);
            data.Image = TableLoader.Instance.GetString("Image", i);
            data.SkillType = SkillType.Utility;
            data.ObjectType = TableLoader.Instance.GetString("ObjectType", i);
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
            data.SkillPos = TableLoader.Instance.GetInteger("SkillPos", i);
            data.SkillGrade = TableLoader.Instance.GetInteger("SkillGrade", i);
            data.NextID = TableLoader.Instance.GetInteger("NextSkill", i);
            data.PrevID = TableLoader.Instance.GetInteger("PreSkill", i);
            UtilityList.Add(data.ID);
            m_utillData.Add(data.ID, data);
        }
        TableLoader.Instance.Clear();
    }
   

}*/



