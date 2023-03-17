using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;






public class TestSkillData
{
    public float hp{get;set;}
    public float criPer { get; set; }
    public float criDam { get; set; }
    public float atkSpeed { get; set; }
    public float atk { get; set; }
    public float def { get; set; }
    public float Speed { get; set; }
    public TestSkillData()
    {

    }
    public TestSkillData(float hp, float criPer, float criDam, float atkSpeed, float atk, float def, float speed)
    {
        this.hp = hp;
        this.criPer = criPer;
        this.criDam = criDam;
        this.atkSpeed = atkSpeed;
        this.atk = atk;
        this.def = def;
        this.Speed = speed;
       
    }
    public TestSkillData SkillData(WeaponType weapon) //체력, 크리확률, 크리뎀, 공격속도, 공격력, 방어력, 이동속도
    {
        TestSkillData Item = null;
        switch(weapon)
        {
            case WeaponType.Pistol:
                Item = new TestSkillData(0f, 50f, 150f, 0f, 1.8f, 0f, 0.5f);
                    break;
            case WeaponType.SubMGun:
                Item = new TestSkillData(0f, 20f, 30f, 3f, 1.2f, 0f, 0.3f);
                break;
            case WeaponType.Rifle:
                Item = new TestSkillData(0f, 30f, 50f, 1f, 1.5f, 0f, 0.1f);
                break;
            case WeaponType.MachineGun:
                Item = new TestSkillData(0.2f, 10f, 30f, 2f, 1.2f, 10f, -0.2f);
                break;
            case WeaponType.ShotGun:
                Item = new TestSkillData(0.5f, 10f, 20f, 0f, 2f, 30f, 0f);
                break;
            case WeaponType.Bat:
                Item = new TestSkillData(1f, 10f, 20f, 0.5f, 1.5f, 50f, 0f);
                break;
            case WeaponType.Axe:
                Item = new TestSkillData(1f, 10f, 20f, 0.5f, 1.5f, 50f, 0f);
                break;

        }
        return Item;

    }
}

/*
public class TestItemData
{
    public float hp { get; set; }
    public float criPer { get; set; }
    public float criDam { get; set; }
    public float atkSpeed { get; set; }
    public float atk { get; set; }
    public float def { get; set; }
    public float Speed { get; set; }
    public int Maxammo { get; set; }
    public float reloadTime { get; set; }
    public TestItemData() { }
    public TestItemData(float hp, float criPer, float criDam, float atkSpeed, float atk, float def,float speed, int MaxAmmo, float reloadTime)
    {
        this.hp = hp;
        this.criPer = criPer;
        this.criDam = criDam;
        this.atkSpeed = atkSpeed;
        this.atk = atk;
        this.def = def;
        this.Speed = speed;
        this.Maxammo = MaxAmmo;
        this.reloadTime = reloadTime;
    }


     public TestItemData ItemData(WeaponType weapon)
    {
        TestItemData skill = null; //체력 , 치명타확률, 치명타 데미지, 공격속도, 공격력, 방어력, 이동속도, 장탄량, 장전속도
        switch (weapon)
        {
            case WeaponType.Pistol:
                skill = new TestItemData(0f, 50f, 150f, 2f, 40f, 0f, 0.5f, 7,1f);
                break;
            case WeaponType.SubMGun:
                skill = new TestItemData(0f, 10f, 20f, 4f, 25f, 0f, 0.1f,40,1.5f);
                break;
            case WeaponType.Rifle:
                skill = new TestItemData(0f, 20f, 30f, 3f, 50f, 0f, 0.1f,30,2f);
                break;
            case WeaponType.MachineGun:
                skill = new TestItemData(0.2f, 0f, 10f, 4f, 30f, 10f, 0f,100,3f);
                break;
            case WeaponType.ShotGun:
                skill = new TestItemData(0.5f, 0f, 20f, 0.5f, 50f, 30f, 0f,8,1f);
                break;
           
        }
        return skill;
    }

}
*/


