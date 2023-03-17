using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum AttackType
{
    Normal,
    Critical
}
public enum AttackCCType
{
    Break,
    Pierce
}
[System.Serializable]
public struct Status
{
    public float hp; //현재 체력
    public float hpMax; //최대 체력
    public float criRate; //크리티컬확률
    public float criAttack; //크리공격력
    public float atkSpeed; //공격속도
    public float damage;  //공격력
    public float defense; //방어력
    public float speed; //이동속도
    public int maxammo;
    public float reloadTime;
    public float KnockBackPer;
    public float KnockBackDist;
    public float AtkDist;
    public int ShotGun;
    
    public Status(float hp,float hpMax,  float criRate, float criAttack,float atkSpeed, float damage, float defense, float speed, int maxammo, float reloadTime,float knockbackPer, float knockbackDist, float atkDist, int shotgun)
    {
        this.hp = hp;
        this.hpMax = hpMax; //이 객체의 최대체력과 현재 체력을 받아온 hp값으로 저장
        this.criRate = criRate; //이 객체의 크리티컬 확률을 받아온 값으로 저장
        this.criAttack = criAttack;//이 객체의 크리티컬 데미지를 받아온 값으로 저장
        this.atkSpeed = atkSpeed;
        this.damage = damage;//이 객체의 공격력을 받아온 값으로 저장
        this.defense = defense;//이 객체의 피해감소를 받아온 값으로 저장
        this.speed = speed;
        this.maxammo = maxammo;
        this.reloadTime = reloadTime;
        this.KnockBackDist= knockbackDist;
        this.KnockBackPer= knockbackPer;
        this.AtkDist= atkDist;
        this.ShotGun = shotgun;
    }
}
[System.Serializable]
public struct MonStatus //몬스터용 스테이터스창.
{
    public MonsterType type;
    public string name;
    public float hp;
    public float hpMax;
    public float atkSpeed;
    public float damage;
    public float defense;
    public float speed;
    public float attackDist;
    public float KnockbackRigist;
    public float score;
   
    public MonStatus(MonsterType type,string name, float hp, float atkSpeed, float damage, float defense, float speed, float attackDist,float knockbackrigist,float score) //체력 공속 공격력 방어력 이동속도 사정거리 순서로
    {
        this.type = type;
        this.name = name;
        this.hp = hpMax = hp;
        this.atkSpeed = atkSpeed;
        this.damage = damage;
        this.defense = defense;
        this.speed = speed;
        this.attackDist = attackDist;
        this.KnockbackRigist= knockbackrigist;
        this.score = score;
    }

}
