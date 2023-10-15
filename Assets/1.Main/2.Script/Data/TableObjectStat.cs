using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TableObjectStat : Singleton<TableObjectStat>
{
    public Dictionary<ObjectType,ObjectStat> m_objectData = new Dictionary<ObjectType,ObjectStat>();
    public void Load()
    {
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("Object"));
        m_objectData.Clear();
        for(int i = 0; i < 3; i++)
        {
            ObjectStat data = new ObjectStat();
            data.ID = TableLoader.Instance.GetInteger("ID", i);
            if(TableLoader.Instance.GetString("ObjectType", i) == "Barricade")
            {
                data.Objecttype = ObjectType.Barricade;
            }
            else if (TableLoader.Instance.GetString("ObjectType", i) == "Turret")
            {
                data.Objecttype = ObjectType.Turret;
            }
            else if (TableLoader.Instance.GetString("ObjectType", i) == "Generator")
            {
                data.Objecttype = ObjectType.Generator;
            }
            data.HP = data.MaxHP = TableLoader.Instance.GetFloat("HP", i);
            data.Defence = TableLoader.Instance.GetFloat("Defence", i);
            data.DamageRigist = TableLoader.Instance.GetFloat("DamageRigist", i);
            data.Damage = TableLoader.Instance.GetFloat("Damage", i);
            data.FireRate = TableLoader.Instance.GetFloat("FireRate", i);
            data.Range = TableLoader.Instance.GetFloat("Range", i);
            data.CriRate = TableLoader.Instance.GetFloat("CriRate", i);
            data.CriDamage = TableLoader.Instance.GetFloat("CriDamage", i);
            data.ArmorPierce = TableLoader.Instance.GetFloat("ArmorPierce", i);
            data.DamageReflect = TableLoader.Instance.GetInteger("Reflect", i);
            data.MaxBuild = TableLoader.Instance.GetInteger("MaxBuild", i);
            data.Info = TableLoader.Instance.GetString("Info", i);
            m_objectData.Add(data.Objecttype, data);
        }
        TableLoader.Instance.Clear();
    }
}
public class ObjectStat
{
    public int ID { get; set; }
    public ObjectType Objecttype { get; set; }
    public float HP { get; set; }
    public float MaxHP { get; set; }
    public float Defence { get; set; }
    public float DamageRigist { get; set; }
    public float Damage { get; set; }
    public float FireRate { get; set; }
    public float Range { get; set; }
    public float CriRate { get; set; }
    public float CriDamage { get; set; }
    public float ArmorPierce { get; set; }
    public float DamageReflect { get; set; }
    public int MaxBuild { get; set; }
    public string Info { get; set; }
    public float Regen { get; set; }
    public ObjectStat() { }

    public ObjectStat(int id,ObjectType type ,float hp, float maxhp, float defence, float rigist,float damage,float firerate,float range, float crirate, float cridam, float armorpierce, float reflect, int maxbuild, string info,float regen )
    {
        this.ID = id;
        this.Objecttype = type;
        this.HP = hp;
        this.MaxHP = maxhp;
        this.Defence = defence;
        this.DamageRigist = rigist;
        this.Damage = damage;
        this.FireRate = firerate;
        this.Range = range;
        this.CriRate = crirate;
        this.CriDamage = cridam;
        this.ArmorPierce = armorpierce;
        this.DamageReflect = reflect;
        this.MaxBuild = maxbuild;
        this.Info = info;
        this.Regen = regen;
    }
    public ObjectStat GetObjectStatus(ObjectType type)
    {
        ObjectStat data = null;
        data = TableObjectStat.Instance.m_objectData[type];
        ObjectStat stat = new ObjectStat(data.ID, data.Objecttype, data.HP, data.MaxHP, data.Defence, data.DamageRigist, data.Damage, data.FireRate, data.Range, data.CriRate, data.CriDamage, data.ArmorPierce, data.DamageReflect, data.MaxBuild, data.Info,0);
        return stat;
    }
}
