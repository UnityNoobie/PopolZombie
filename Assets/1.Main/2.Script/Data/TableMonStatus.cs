using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;



public enum MonsterType
{
    Normal,
    EungAe,
    Heavy,
    Dog,
    Spiter,
    Boss,
    Max
}

[System.Serializable]
public class MonStat
{
    public MonsterType type { get; set; }
    public string name { get; set; }
    public float hp { get; set; }
    public float hpMax { get; set; }
    public float atkSpeed { get; set; }
    public float damage { get; set; }
    public float defense { get; set; }
    public float speed { get; set; }
    public float attackDist { get; set; }
    public float knockbackRegist { get; set; }
    public float Score { get; set; }
    public float coin { get;set; }
    public float exp { get; set; }
    public string hitSound { get; set; }
    public string atkSound { get; set; }
    public string dieSound { get; set; }
    public string skillSound { get; set; }
    public string stepSound { get; set; }

    public MonStat()
    {

    }
    public MonStat(MonsterType type,string name, float hp, float atkSpeed, float damage, float defense, float speed, float attackDist, float knockbackRegist, float score,float coin, float texp,string hitsound,string atksound,string diesound,string skillsound ,string stepsound)
    {
        this.type = type;
        this.name = name;
        this.hp = hpMax = hp;
        this.atkSpeed = atkSpeed;
        this.damage = damage;
        this.defense = defense;
        this.speed = speed;
        this.attackDist = attackDist;
        this.knockbackRegist = knockbackRegist;
        this.Score = score;
        this.coin = coin;
        this.exp = texp;
        this.hitSound = hitsound;
        this.atkSound = atksound;
        this.dieSound = diesound;
        this.skillSound = skillsound;
        this.stepSound = stepsound;
    }
    public MonStat GetMonStat(MonsterType type)//체력 공속 공격력 방어력 이동속도 사정거리 넉백저항 점수 코인 경험치 순서로
    {
        MonStat stat = null;
        stat = TableMonsterStat.Instance.m_mondic[type];
        return stat;
    }
}
public class TableMonsterStat : Singleton<TableMonsterStat> 
{
    public Dictionary<MonsterType, MonStat> m_mondic = new Dictionary<MonsterType, MonStat>();
    
    public void Load()
    {
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("MonStatus"));
        for(int i = 0; i < 6; i++)
        {
            MonStat data = new MonStat();
            string str = TableLoader.Instance.GetString("Type", i);
            switch (str)
            {
                case "Normal":
                    data.type = MonsterType.Normal;
                    break;
                case "EnunAe":
                    data.type = MonsterType.EungAe;
                    break;
                case "Heavy":
                    data.type = MonsterType.Heavy;
                    break;
                case "Dog":
                    data.type = MonsterType.Dog;
                    break;
                case "Spiter":
                    data.type = MonsterType.Spiter;
                    break;
                case "Boss":
                    data.type = MonsterType.Boss;
                    break;
                case null:
                    Console.WriteLine("타입확인좀");
                    break;      
            }
            data.name = TableLoader.Instance.GetString("Name", i);
            data.hp = TableLoader.Instance.GetFloat("HP", i);
            data.hpMax = TableLoader.Instance.GetFloat("HPMax",i);
            data.atkSpeed = TableLoader.Instance.GetFloat("AtkSpeed", i);
            data.damage = TableLoader.Instance.GetFloat("Damage", i);
            data.defense = TableLoader.Instance.GetFloat("Defence", i);
            data.speed = TableLoader.Instance.GetFloat("Speed", i);
            data.attackDist = TableLoader.Instance.GetFloat("AtkDist", i);
            data.knockbackRegist = TableLoader.Instance.GetFloat("KnockbackRigist", i);
            data.Score = TableLoader.Instance.GetFloat("Score", i);
            data.coin = TableLoader.Instance.GetFloat("Coin", i);
            data.exp = TableLoader.Instance.GetFloat("Exp", i);
            data.hitSound = TableLoader.Instance.GetString("SoundHit", i);
            data.atkSound = TableLoader.Instance.GetString("SoundAtk", i);
            data.dieSound = TableLoader.Instance.GetString("SoundDeath", i);
            data.skillSound = TableLoader.Instance.GetString("SoundSkill", i);
            data.stepSound = TableLoader.Instance.GetString("SoundStep", i);
            m_mondic.Add(data.type,data);
        }
        TableLoader.Instance.Clear();
    }
}
