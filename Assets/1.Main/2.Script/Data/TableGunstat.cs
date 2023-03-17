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
    public int ID { get; set; } //무기의 ID값.
    public int Grade { get; set; } //무기의 레벨정보 1~3
    public int Damage { get; set; } //무기의 공격력
    public int Mag { get; set; } // 무기의 장탄량
    public int CriRate { get; set; } //무기의 크리티컬확률
    public int CriDamage { get; set; } //무기의 크리티컬 데미지
    public int Defence { get; set; } //무기의 방어력 보너스
    public int Shotgun { get; set; } //샷건의 한발 발사 수
    public int KnockBack { get; set; } //넉백확률
    public float AtkSpeed { get; set; } //공격 속도
    public float ReloadTime { get; set; } //재장전 시간
    public float Speed { get; set; } //이동속도 보너스
    public float HP { get; set; } // 체력 보너스
    public float KnockBackDist { get; set; }
    public float AttackDist { get; set; }
    public string Type { get; set; } // 무기의 종류
    public string AtkType { get; set; }
    public bool isfirst { get; set; } //총을 처음 사용했는지 아닌지에 따라 무기 교체 시 총알 수 확인을 위한 불린값.
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
        this.HP = HP;//아이템 타입, 아이디, 등급, 데미지, 장탄량, 크리티컬확률, 크리티컬데미지, 방어력, 샷건발사수, 넉백확률,공격속도,재장전속도,이동속도,체력 순
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
        //아이템 타입, 아이디, 등급, 데미지, 장탄량, 크리티컬확률, 크리티컬데미지, 방어력, 샷건발사수, 넉백확률,공격속도,재장전속도,이동속도,체력 순서
        //엑셀은 Type	Id	Grade	Damage	AtkSpeed	ReloadTime	Mag	Speed	CriRate	CriDamage	HP	Defence	Shotgun	KnockBack 순서임.
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
            Debug.Log("무기의 타입이 뭔가 이상합니다 선생님");
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
            // Debug.Log("일단 실행은 했습니다잉");
            data.Type = TableLoader.Instance.GetString("Type", i); //총의 타입값을 인스턴스
            data.ID = TableLoader.Instance.GetInteger("Id", i); //총의 아이디값
            data.Grade = TableLoader.Instance.GetInteger("Grade", i); //총의 레벨값
            data.Damage = TableLoader.Instance.GetInteger("Damage", i); //총의 데미지
            data.AtkSpeed = TableLoader.Instance.GetFloat("AtkSpeed", i); //공격속도
            data.ReloadTime = TableLoader.Instance.GetFloat("ReloadTime", i);
            data.Mag = TableLoader.Instance.GetInteger("Mag", i); //장탄량
            data.Speed = TableLoader.Instance.GetFloat("Speed", i); //이동속도 보너스
            data.CriRate = TableLoader.Instance.GetInteger("CriRate", i); //치명타확률
            data.CriDamage = TableLoader.Instance.GetInteger("CriDamage", i); //치명타 데미지        
            data.HP = TableLoader.Instance.GetFloat("HP", i); //체력
            data.Defence = TableLoader.Instance.GetInteger("Defence", i); //방어력
            data.Shotgun = TableLoader.Instance.GetInteger("Shotgun", i); //샷건발사 수
            data.KnockBack = TableLoader.Instance.GetInteger("KnockBack", i); //넉백 확률
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
            // Debug.Log(i + "번째" + data.Type + "타입" + "ID" + data.ID + "Grade" + data.Grade + "Damage :" + data.Damage + "AtkSpeed" + data.AtkSpeed + "공격방식 : " + data.AtkType);
            m_WeaponData.Add(data.ID, data);
        }
        TableLoader.Instance.Clear();
    }


}
