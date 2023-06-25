using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ArmorType
{
    Helmet,
    Glove,
    Armor,
    Pants,
    Boots,
    Max
}
public class WearArmorData
{
    public int Defence { get; set; }
    public float Damage { get; set; }
    public float HP { get; set; }
    public float AttackSpeed { get; set; }
    public int CriRate { get; set; }
    public float Speed { get; set; }

    public WearArmorData() { }

    public WearArmorData(int defence, float damage, float hp, float attackSpeed,int criRate, float speed)
    {
        this.Defence = defence;
        this.Damage = damage;
        this.HP = hp;
        this.AttackSpeed = attackSpeed;
        this.CriRate = criRate;
        this.Speed = speed;
    }
    public WearArmorData SetWearArmorData(int defence, float damage, float hp, float attackSpeed, int criRate, float speed)
    {
        WearArmorData wearArmorData; //정보를 비워주고 받아온 정보를 새로 넣어줌.
        wearArmorData = new WearArmorData(defence, damage, hp, attackSpeed, criRate, speed);
        return wearArmorData;
    }

}
public class ArmorData
{//Type	Id	grade	Defence	Damage	ReloadTime	AttackSpeed	CriRate	Speed

    public string Type { get; set; }
    public string Image { get; set; }

    public string Info { get; set; }
    public string ItemType { get; set; }
    public int Id { get; set; }
    public int Grade { get; set; }
    public int Defence { get; set; }
    public int CriRate { get; set; }
    public int Price { get; set; }
    public float Damage { get; set; }
    public float HP { get; set; }
    public float AttackSpeed { get; set; }
 
    public float Speed { get; set; }
    
    public ArmorType armorType { get; set; }

    public ArmorData() { }

    public ArmorData(string type, int id, int grade, int defence, float damage, float HP, float attackSpeed, int criRate, float speed, ArmorType armortype,string image, string Info,string itemtype,int price)
    {
        this.Type = type;
        this.Id = id;
        this.Grade = grade;
        this.Defence = defence;
        this.Damage = damage;
        this.HP = HP;
        this.AttackSpeed = attackSpeed;
        this.CriRate = criRate;
        this.Speed = speed;
        this.armorType = armortype;
        this.Price = price;
        this.Info = Info;
        this.ItemType = itemtype;
        this.Image = image;
       
    }
    public ArmorData GetArmorData(int ID) //방어구는 5종류까지 착용 가능하기 때문에 int배열로 장착중인 방어구들의 ID를 한번에 받아와 처리하는 방식으로 처리를 하려고 함. 
    { //는 폐기 후 아이템의 정보는 한개씩 가져오되 다른 메서드 구현하여 방어구 수치값들만 +하여 적용하는 방식으로 하면 될것 같음.
        ArmorData armordata = null;
        ArmorType type = new ArmorType();
        armordata = TableArmorStat.Instance.m_armorData[ID];
        
        if (armordata.Type == "Helmet")
        {
            type = ArmorType.Helmet;
        }
        else if (armordata.Type == "Glove")
        {
            type = ArmorType.Glove;
        }
        else if (armordata.Type == "Armor")
        {
            type = ArmorType.Armor;
        }
        else if (armordata.Type == "Pants")
        {
            type = ArmorType.Pants;
        }
        else if (armordata.Type == "Boots")
        {
            type = ArmorType.Boots;
        }
        else
        {
            Debug.Log("방어구의 타입이 뭔가 이상합니다 선생님");
        }//tring type, int id, int grade, int defence, float damage, float reloadTime, float attackSpeed, int criRate, float speed, ArmorType armortype
        ArmorData armorstat = new ArmorData(armordata.Type, armordata.Id, armordata.Grade, armordata.Defence, armordata.Damage, armordata.HP, armordata.AttackSpeed, armordata.CriRate, armordata.Speed, type,armordata.Image,armordata.Info,armordata.ItemType,armordata.Price);
        return armorstat;
    }
   

}
public class TableArmorStat : Singleton<TableArmorStat>
{
    // Start is called before the first frame update


    public Dictionary<int, ArmorData> m_armorData = new Dictionary<int, ArmorData>();
    public List<int> m_lowArmor = new List<int>();
    public List<int> m_midArmor = new List<int>();
    public List<int> m_highArmor = new List<int>();
    public void Load() //테이블로더에서 방어구 정보를 받아와 딕셔너리에 저장하기 위한 용도.
    {

        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("ArmorStat"));
        m_armorData.Clear();
        m_lowArmor.Clear();
        m_midArmor.Clear();
        m_highArmor.Clear();
        for (int i = 0; i < 15; i++)
        {
            ArmorData data = new ArmorData();
            data.Type = TableLoader.Instance.GetString("Type", i); //아머의 타입값
            data.Id = TableLoader.Instance.GetInteger("Id", i); //아머의 아이디값
            data.Grade = TableLoader.Instance.GetInteger("grade", i); //아머의 레벨값
            data.Damage = TableLoader.Instance.GetFloat("Damage", i); //아머의 데미지
            data.AttackSpeed = TableLoader.Instance.GetFloat("AttackSpeed", i); //공격속도 보너스
            data.HP = TableLoader.Instance.GetFloat("HP", i); // 장전시간 보너스
            data.Speed = TableLoader.Instance.GetFloat("Speed", i); //이동속도 보너스
            data.CriRate = TableLoader.Instance.GetInteger("CriRate", i); //치명타확률 보너스
            data.Defence = TableLoader.Instance.GetInteger("Defence", i); //방어력
            data.Price = TableLoader.Instance.GetInteger("Price", i);
            data.Image = TableLoader.Instance.GetString("Image", i);
            data.Info = TableLoader.Instance.GetString("Info", i);
            data.ItemType = TableLoader.Instance.GetString("ItemType", i);
            if (data.Type == "Helmet")
            {
                data.armorType = ArmorType.Helmet;
            }
            else if (data.Type == "Glove")
            {
                data.armorType = ArmorType.Glove;
            }
            else if (data.Type == "Armor")
            {
                data.armorType = ArmorType.Armor;
            }
            else if (data.Type == "Pants")
            {
                data.armorType = ArmorType.Pants;
            }
            else if (data.Type == "Boots")
            {
                data.armorType = ArmorType.Boots;
            }
            if (data.Grade == 1) m_lowArmor.Add(data.Id);
            else if (data.Grade == 2) m_midArmor.Add(data.Id);
            else if (data.Grade == 3) m_highArmor.Add(data.Id);
            m_armorData.Add(data.Id, data);
        }
        TableLoader.Instance.Clear();
    }
}
