using System.Collections;
using System.Collections.Generic;
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
    public MonsterType type { get; }
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
    public MonStat()
    {

    }
    public MonStat(MonsterType type,string name, float hp, float atkSpeed, float damage, float defense, float speed, float attackDist, float knockbackRegist, float score,float coin, float texp)
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
        this.exp = exp;
    }
    public MonStat SetMonStat(MonsterType type)//체력 공속 공격력 방어력 이동속도 사정거리 넉백저항 점수 코인 경험치 순서로
    {
        MonStat monstat = null;
        if (type == MonsterType.Normal)
            monstat = new MonStat(type, "일반좀비", 50, 2f, 10f, 10f, 4f, 4f,20f,10f,10f, 10f);
        else if (type == MonsterType.EungAe)
            monstat = new MonStat(type, "응애좀비", 20, 1.5f, 10f, 5f,6f, 3f,0f,12f, 13f, 10f);
        else if (type == MonsterType.Heavy)
            monstat = new MonStat(type, "헤비좀비", 100, 3f, 30f, 30f,3f, 4f,50f,15f, 15f, 10f);
        else if (type == MonsterType.Dog)
            monstat = new MonStat(type, "댕댕이좀비", 15, 1f, 30f, 5f, 7f, 3f,0f, 12f, 13f, 10f);
        else if (type == MonsterType.Spiter)
            monstat = new MonStat(type, "스피터좀비", 70, 5f, 20f, 20f, 3.5f, 12f,0f,20f, 15f, 10f);
        else if (type == MonsterType.Boss)
            monstat = new MonStat(type, "보스좀비", 1000, 3f, 60f, 60f, 4f, 9f,200f,200f, 2000f, 100f);
        else Debug.Log(type);

        return monstat;

    }
}
