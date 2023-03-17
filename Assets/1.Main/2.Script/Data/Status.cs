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
    
    public Status(float hp,float hpMax,  float criRate, float criAttack,float atkSpeed, float damage, float defense, float speed, int maxammo, float reloadTime,float knockbackPer, float knockbackDist, float atkDist, int shotgun)
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
   
    public MonStatus(MonsterType type,string name, float hp, float atkSpeed, float damage, float defense, float speed, float attackDist,float knockbackrigist,float score) //ü�� ���� ���ݷ� ���� �̵��ӵ� �����Ÿ� ������
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
