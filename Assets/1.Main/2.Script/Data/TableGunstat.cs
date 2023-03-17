using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static TableGunstat;
public enum WeaponType
{
    Pistol,
    SubMGun,
    Rifle,
    MachineGun,
    ShotGun,
    Bat,
    Axe,
    Max
}
public class WeaponData
{
    public int ID { get; set; } //������ ID��.
    public int Grade { get; set; } //������ �������� 1~3
    public int Damage { get; set; } //������ ���ݷ�
    public int Mag { get; set; } // ������ ��ź��
    public int CriRate { get; set; } //������ ũ��Ƽ��Ȯ��
    public int CriDamage { get; set; } //������ ũ��Ƽ�� ������
    public int Defence { get; set; } //������ ���� ���ʽ�
    public int Shotgun { get; set; } //������ �ѹ� �߻� ��
    public int KnockBack { get; set; } //�˹�Ȯ��
    public float AtkSpeed { get; set; } //���� �ӵ�
    public float ReloadTime { get; set; } //������ �ð�
    public float Speed { get; set; } //�̵��ӵ� ���ʽ�
    public float HP { get; set; } // ü�� ���ʽ�
    public float KnockBackDist { get; set; }
    public float AttackDist { get; set; }
    public string Type { get; set; } // ������ ����
    public string AtkType { get; set; }
    public bool isfirst { get; set; } //���� ó�� ����ߴ��� �ƴ����� ���� ���� ��ü �� �Ѿ� �� Ȯ���� ���� �Ҹ���.
    public string Image { get; set; }
    public string Info { get; set; }
    public string ItemType { get; set; }
    public int Price { get; set; }
    public WeaponType weaponType { get; set; }


    public WeaponData() { }
    public WeaponData(string Type, int Id, int Grade, int Damage, int Mag, int CriRate, int CriDamage, int Defence, int Shotgunmag, int knockbackPer, float atkSpeed, float reloadTime, float speed, float HP,float knockBackDist,float atkDist,string atkType, WeaponType type, string image, string Info, string itemtype, int price)
    {
        this.Type = Type;
        this.weaponType = type;
        this.ID = Id;
        this.Grade = Grade;
        this.Damage = Damage;
        this.Mag = Mag;
        this.CriRate = CriRate;
        this.CriDamage = CriDamage;
        this.Defence = Defence;
        this.Shotgun = Shotgunmag;
        this.KnockBack = knockbackPer;
        this.AtkSpeed = atkSpeed;
        this.ReloadTime = reloadTime;
        this.Speed = speed;
        this.HP = HP;//������ Ÿ��, ���̵�, ���, ������, ��ź��, ũ��Ƽ��Ȯ��, ũ��Ƽ�õ�����, ����, ���ǹ߻��, �˹�Ȯ��,���ݼӵ�,�������ӵ�,�̵��ӵ�,ü�� ��
        this.KnockBackDist = knockBackDist;
        this.AtkType = atkType;
        this.AttackDist = atkDist;
        this.isfirst = isfirst;
        this.Price = price;
        this.Info = Info;
        this.ItemType = itemtype;
        this.Image = image;

    }

    public WeaponData GetWeaponStatus(int ID)
    {
        //������ Ÿ��, ���̵�, ���, ������, ��ź��, ũ��Ƽ��Ȯ��, ũ��Ƽ�õ�����, ����, ���ǹ߻��, �˹�Ȯ��,���ݼӵ�,�������ӵ�,�̵��ӵ�,ü�� ����
        //������ Type	Id	Grade	Damage	AtkSpeed	ReloadTime	Mag	Speed	CriRate	CriDamage	HP	Defence	Shotgun	KnockBack ������.
        WeaponData status = null;
        status = TableGunstat.Instance.m_WeaponData[ID];
        // GunData var = TableGunstat.Instance.m_GunData[ID];
        // status = TableGunstat.Instance.m_GunData[ID];
        WeaponType type = new WeaponType();
        if (status.Type.Equals("Pistol"))
        {
            type = WeaponType.Pistol;
        }
        else if (status.Type.Equals("SubMGun"))
        {
            type = WeaponType.SubMGun;
        }
        else if (status.Type.Equals("Rifle"))
        {
            type = WeaponType.Rifle;
        }
        else if (status.Type.Equals("MachineGun"))
        {
            type = WeaponType.MachineGun;
        }
        else if (status.Type.Equals("ShotGun"))
        {
            type = WeaponType.ShotGun;
        }
        else if (status.Type.Equals("Bat"))
        {
            type = WeaponType.Bat;
        }
        else if (status.Type.Equals("Axe"))
        {
            type = WeaponType.Axe;
        }
        else
        {
            Debug.Log("������ Ÿ���� ���� �̻��մϴ� ������");
        }
        WeaponData gunstat = new WeaponData(status.Type, status.ID, status.Grade, status.Damage, status.Mag, status.CriRate, status.CriDamage, status.Defence, status.Shotgun, status.KnockBack, status.AtkSpeed, status.ReloadTime, status.Speed, status.HP, status.KnockBackDist,status.AttackDist, status.AtkType, type, status.Image, status.Info, status.ItemType, status.Price);
        return gunstat;
    }
}
public class TableGunstat : Singleton<TableGunstat>
{
    // Start is called before the first frame update
    
    
    public  Dictionary<int, WeaponData> m_WeaponData = new Dictionary<int, WeaponData>();
    public List<int> m_lowWeapon = new List<int>();
    public List<int> m_midWeapon = new List<int>();
    public List<int> m_highWeapon = new List<int>();
    public void Load()
    {

        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("WeaponStat"));
        m_WeaponData.Clear();
        for (int i = 0; i < 21; i++)
        {
            WeaponData data = new WeaponData();
            // Debug.Log("�ϴ� ������ �߽��ϴ���");
            data.Type = TableLoader.Instance.GetString("Type", i); //���� Ÿ�԰��� �ν��Ͻ�
            data.ID = TableLoader.Instance.GetInteger("Id", i); //���� ���̵�
            data.Grade = TableLoader.Instance.GetInteger("Grade", i); //���� ������
            data.Damage = TableLoader.Instance.GetInteger("Damage", i); //���� ������
            data.AtkSpeed = TableLoader.Instance.GetFloat("AtkSpeed", i); //���ݼӵ�
            data.ReloadTime = TableLoader.Instance.GetFloat("ReloadTime", i);
            data.Mag = TableLoader.Instance.GetInteger("Mag", i); //��ź��
            data.Speed = TableLoader.Instance.GetFloat("Speed", i); //�̵��ӵ� ���ʽ�
            data.CriRate = TableLoader.Instance.GetInteger("CriRate", i); //ġ��ŸȮ��
            data.CriDamage = TableLoader.Instance.GetInteger("CriDamage", i); //ġ��Ÿ ������        
            data.HP = TableLoader.Instance.GetFloat("HP", i); //ü��
            data.Defence = TableLoader.Instance.GetInteger("Defence", i); //����
            data.Shotgun = TableLoader.Instance.GetInteger("Shotgun", i); //���ǹ߻� ��
            data.KnockBack = TableLoader.Instance.GetInteger("KnockBack", i); //�˹� Ȯ��
            data.KnockBackDist = TableLoader.Instance.GetFloat("KnockBackDist", i);
            data.AtkType = TableLoader.Instance.GetString("AtkType", i);
            data.AttackDist = TableLoader.Instance.GetFloat("AttackDist", i);
            data.Price = TableLoader.Instance.GetInteger("Price", i);
            data.Image = TableLoader.Instance.GetString("Image", i);
            data.Info = TableLoader.Instance.GetString("Info", i);
            data.ItemType = TableLoader.Instance.GetString("ItemType", i);
            if (data.Grade == 1) m_lowWeapon.Add(data.ID);
            else if (data.Grade == 2) m_midWeapon.Add(data.ID);
            else if (data.Grade == 3) m_highWeapon.Add(data.ID);
            // Debug.Log(i + "��°" + data.Type + "Ÿ��" + "ID" + data.ID + "Grade" + data.Grade + "Damage :" + data.Damage + "AtkSpeed" + data.AtkSpeed + "���ݹ�� : " + data.AtkType);
            m_WeaponData.Add(data.ID, data);
        }
        TableLoader.Instance.Clear();
    }


}
