using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public enum ItemType
    {
        Gun,
        Melee,
        Armor,
        HealPack,
        Barricade,
        Max
    }
   
     //Type	Id	grade	Heal	Price	HP	Defence	Info
     public string type { get; set; }
     public int ID { get; set; }
     public int Grade { get; set; } 

     public  int Heal { get; set; }
     public int Price { get;set; }
     public int HP { get; set; }
     public int Defence { get; set; }
     public ItemType Type { get; set; }


     public string ItemInfo { get;set; }
     public ItemData() { }

     public ItemData(string type,int id,int grade,int heal,int price,int hp, int defecne,string itemInfo,ItemType itype)
     {
         this.type = type;
         this.ID = id;
         this.Grade = grade;
         this.Heal = heal;
         this.Price = price;
         this.HP = hp;
         this.Defence = defecne;
         this.ItemInfo = itemInfo;
         this.Type = itype;
     }

     public ItemData GetItemInfo(int ID)
     {
         ItemData Info = null;
         Info = TableItemData.Instance.itemData[ID];
         ItemType itype = new ItemType();
         if (Info.type.Equals("HealPack"))
         {
             itype = ItemType.HealPack;
         }
         else if (Info.type.Equals("Barricade"))
         {
             itype = ItemType.Barricade;
         }
         else { Debug.Log("아이템타입 뭔가 이상"); }

         ItemData iinfo = new ItemData(Info.type, Info.ID,Info.Grade,Info.Heal,Info.Price,Info.HP,Info.Defence,Info.ItemInfo,itype);
         return iinfo;
       //  itemInfo = 
     }
 }
 public class TableItemData : Singleton<TableItemData>
 {
     public Dictionary<int, ItemData> itemData = new Dictionary<int, ItemData>();
    public List<int> m_lowItem = new List<int>();
    public List<int> m_midItem = new List<int>();
    public List<int> m_highItem = new List<int>();
    public void Load()
     {
         TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("ItemInfo"));
         itemData.Clear();
        m_lowItem.Clear();
        m_midItem.Clear();
        m_highItem.Clear();
         for (int i = 0; i < 6; i++)
         {
             ItemData data = new ItemData();
             data.type = TableLoader.Instance.GetString("Type", i);
             data.ID = TableLoader.Instance.GetInteger("Id", i);
             data.Grade = TableLoader.Instance.GetInteger("grade", i);
             data.Heal = TableLoader.Instance.GetInteger("Heal", i);
             data.Price = TableLoader.Instance.GetInteger("Price", i);
             data.HP = TableLoader.Instance.GetInteger("HP", i);
             data.Defence = TableLoader.Instance.GetInteger("Defence", i);
             data.ItemInfo = TableLoader.Instance.GetString("Info", i);
             itemData.Add(data.ID, data);
             if (data.Grade == 1) m_lowItem.Add(data.ID);
             else if (data.Grade == 2) m_midItem.Add(data.ID);
             else if (data.Grade == 3) m_highItem.Add(data.ID);
        }
     }

     
}
