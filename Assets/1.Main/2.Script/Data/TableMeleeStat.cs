using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MeleeType
{
    Bat,
    Axe,
    Max
}











public class TableMeleeStat : Singleton<TableMeleeStat>
{
    // type	Id	Grade	Damage	AtkSpeed	AtkRange	Speed	CriRate	CriDamage	HP	Defence	KnockBack	KnockBackDist	Break	AtkType
    public Dictionary<int, MeleeData> m_meleeData = new Dictionary<int, MeleeData>();

    public void Load()
    {
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("MeleeStat"));
        m_meleeData.Clear();
        for(int i = 0; i < 6; i++)
        {
            MeleeData data = new MeleeData();
            //type Id  Grade Damage  AtkSpeed AtkRange    Speed CriRate CriDamage HP  Defence KnockBack   KnockBackDist Break   AtkType

            data.Type = TableLoader.Instance.GetString("Type", i);
            data.Id = TableLoader.Instance.GetInteger("Id", i);
            data.Grade = TableLoader.Instance.GetInteger("Grade", i);
            data.Damage = TableLoader.Instance.GetInteger("Damage", i);
            data.AtkSpeed = TableLoader.Instance.GetFloat("AtkSpeed", i);
            data.AtkRange = TableLoader.Instance.GetFloat("AtkRange", i);
            data.Speed = TableLoader.Instance.GetFloat("Speed", i);
            data.CriRate = TableLoader.Instance.GetInteger("CriRate", i);
            data.CriDamage = TableLoader.Instance.GetInteger("CriDamage", i);
            data.HP = TableLoader.Instance.GetFloat("HP", i);
            data.Defence = TableLoader.Instance.GetInteger("Defence", i);
            data.KnockBackPer = TableLoader.Instance.GetInteger("KnockBack", i);
            data.KnockBackDist = TableLoader.Instance.GetFloat("KnockBackDist", i);
            data.ArmorBreak = TableLoader.Instance.GetInteger("Break", i);
            data.AtkType = TableLoader.Instance.GetString("AtkType", i);
           // Debug.Log("아이템 타입 : " + data.Type + "아이디 : " + data.Id + "데미지 : " + data.Damage + "사거리 : " + data.AtkRange);
            m_meleeData.Add(i, data);
        }
        TableLoader.Instance.Clear();
    }
}
public class MeleeData
{
    //type Id  Grade Damage  AtkSpeed AtkRange    Speed CriRate CriDamage HP  Defence KnockBack   KnockBackDist Break   AtkType

    public string Type { get; set; }
    public string AtkType { get; set; }
    public int Id { get; set; }
    public int Grade { get; set; }
    public int Damage { get; set; }
    public int CriRate { get; set; }
    public int CriDamage { get; set; }
    public int Defence { get; set; }
    public int KnockBackPer { get; set; }
    public int ArmorBreak { get; set; }
    public float AtkSpeed { get; set; }
    public float AtkRange { get; set; }
    public float Speed { get; set; } 
    public float HP { get; set; }
    public float KnockBackDist { get; set; }
    public MeleeType meleeType { get; set; }
    public MeleeData() { }

    public MeleeData(string type, string atkType, int id, int grade, int damage, int criRate, int criDamage, int defence, int knockBackPer, int armorBreak, float atkSpeed, float atkRange, float speed, float hP, float knockBackDist,MeleeType meleetype)
    {
        this.Type = type;
        this.AtkType = atkType;
        this.Id = id;
        this.Grade = grade;
        this.Damage = damage;
        this.CriRate = criRate;
        this.CriDamage = criDamage;
        this.Defence = defence;
        this.KnockBackPer = knockBackPer;
        this.ArmorBreak = armorBreak;
        this.AtkSpeed = atkSpeed;
        this.AtkRange = atkRange;
        this.Speed = speed;
        this.HP = hP;
        this.KnockBackDist = knockBackDist;
        this.meleeType = meleetype;
    }

    public MeleeData GetMeleeStatus(int id)
    {
        MeleeData data = null;
        data = TableMeleeStat.Instance.m_meleeData[id];
        MeleeType type = new MeleeType();

        if (data.Type.Equals("Axe"))
        {
            type = MeleeType.Axe;
        }
        else if (data.Type.Equals("Bat"))
        {
            type = MeleeType.Bat;
        }
        else
        {
            Debug.Log("무기의 타입이 이상합니다.");
        }
        //string type, string atkType, int id, int grade, int damage, int criRate, int criDamage, int defence, int knockBackPer, int armorBreak, float atkSpeed, float atkRange, float speed, float hP, float knockBackDist
        MeleeData melee = new MeleeData(data.Type,data.AtkType,data.Id,data.Grade,data.Damage,data.CriRate,data.CriDamage,data.Defence,data.KnockBackPer,data.ArmorBreak,data.AtkSpeed,data.AtkRange,data.Speed,data.HP,data.KnockBackDist,type);

        return melee;

    }







}








