using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum AttackType
{
    Normal,
    Critical
}
[System.Serializable]
public struct Status //�÷��̾��� �������ͽ�
{

    public float hp; //���� ü��
    public float hpMax; //�ִ� ü��
    public float criRate; //ũ��Ƽ��Ȯ��
    public float criAttack; //ũ�����ݷ�
    public float atkSpeed; //���ݼӵ�
    public float damage;  //���ݷ�
    public float defense; //����
    public float speed; //�̵��ӵ�
    public int maxammo;
    public float reloadTime;
    public float KnockBackPer;
    public float KnockBackDist;
    public float AtkDist;
    public int ShotGun;
    public int level;
    public float DamageRigist;
    public float LastFire;
    public int Pierce;
    public int Boom;
    public float ArmorPierce;
    public float Remove;
    public int Drain;
    public float Crush;
    public int Burn;
    public float SkillHeal;
    public string KnickName;
    public string Title;

    public Status(float hp,float hpMax,  float criRate, float criAttack,float atkSpeed, float damage, float defense, float speed, int maxammo, float reloadTime,float knockbackPer, float knockbackDist, float atkDist, int shotgun,int Level,float damagerigist,float lastfire, int pierce,int boom,float armorpierce,float remove,int drain,float crush,int burn,float heal,string knickName,string title)
    {
        this.hp = hp;
        this.hpMax = hpMax; //�� ��ü�� �ִ�ü�°� ���� ü���� �޾ƿ� hp������ ����
        this.criRate = criRate; //�� ��ü�� ũ��Ƽ�� Ȯ���� �޾ƿ� ������ ����
        this.criAttack = criAttack;//�� ��ü�� ũ��Ƽ�� �������� �޾ƿ� ������ ����
        this.atkSpeed = atkSpeed;
        this.damage = damage;//�� ��ü�� ���ݷ��� �޾ƿ� ������ ����
        this.defense = defense;//�� ��ü�� ���ذ��Ҹ� �޾ƿ� ������ ����
        this.speed = speed;
        this.maxammo = maxammo;
        this.reloadTime = reloadTime;
        this.KnockBackDist= knockbackDist;
        this.KnockBackPer= knockbackPer;
        this.AtkDist= atkDist;
        this.ShotGun = shotgun;
        this.level = Level;
        this.DamageRigist = damagerigist;
        this.LastFire= lastfire;
        this.Pierce= pierce;
        this.Boom= boom;
        this.ArmorPierce = armorpierce;
        this.Remove= remove;
        this.Drain= drain;
        this.Crush= crush;
        this.Burn= burn;
        this.SkillHeal = heal;
        this.KnickName = knickName;
        this.Title = title;
    }
}
[System.Serializable]
public struct MonStatus //���Ϳ� �������ͽ�â.
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
    public float coin;
    public float exp;
    public string hitSound;
    public string atkSound;
    public string deathSound;
   
    public MonStatus(MonsterType type,string name, float hp, float atkSpeed, float damage, float defense, float speed, float attackDist,float knockbackrigist,float score,float coin,float exp,string hitsound, string atksound, string deathsound) 
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
        this.coin = coin;
        this.exp = exp;
        this.hitSound = hitsound;
        this.atkSound = atksound;
        this.deathSound = deathsound;
    }

}
