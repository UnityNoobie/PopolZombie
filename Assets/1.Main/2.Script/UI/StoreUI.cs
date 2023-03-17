using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum ItemType
{
    Weapon,
    Armor,
    Item
}
public class StoreUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_storePannel;
    [SerializeField]
    Slot[] m_slots;
    [SerializeField]
    Transform m_content;
    [SerializeField]
    GameObject Store;
    [SerializeField]
    Button closebutton;
    [SerializeField]
    GameObject[] Shopareas;
    [SerializeField]
    TextMeshProUGUI m_StoreName;
    [SerializeField]
    BuyItems m_buyItem;
    [SerializeField]
    public PanelItemInfo m_info;
  

    PlayerController m_player;
    bool isOPen;
    public Dictionary<int, ArmorData> m_armordata = new Dictionary<int, ArmorData>();
    public Dictionary<int, WeaponData> m_weapondata = new Dictionary<int, WeaponData>();
    public Dictionary<int, ItemData> m_itemdata = new Dictionary<int, ItemData>();
    List<int> m_armorList = new List<int>();
    List<int> m_weaponList = new List<int>();
    List<int> m_itemList = new List<int>();
    bool m_isActive;
    

    public void CloseAllTabs()
    {
        //if(m_buyItem.gameObject.activeSelf)
        m_buyItem.DeActiveUI();
        //if (m_info.gameObject.activeSelf)
            m_info.DeActiveUI();
      //  if (Store.gameObject.activeSelf)
            CloseStore();
    }
    public void ActiveStoreUI(string StoreType,PlayerController player) //켜주기
    {
        ResetSlot();
        m_player = player;
        if (StoreType.Equals("Item"))
        {
            SetSlotItem(m_itemList,ItemType.Item);
        }
        else if (StoreType.Equals("Armor"))
        {
            SetSlotItem(m_armorList, ItemType.Armor);
        }
        else if (StoreType.Equals("Weapon"))
        {
            SetSlotItem(m_weaponList, ItemType.Weapon);
        }
        m_StoreName.text = StoreType + "Store";
        Store.SetActive(true);
    }
    public void CloseStore() //꺼주기
    {
        Store.SetActive(false);
    }
    void SetItemsInfos() //상점에서 사용해줄 정보를 복사해서 가져와줌.
    {
        m_armordata.Clear();
        m_weapondata.Clear();
        m_itemdata.Clear();
        m_armordata = TableArmorStat.Instance.m_armorData;
        m_weapondata = TableGunstat.Instance.m_WeaponData;
        m_itemdata = TableItemData.Instance.itemData;
    }
    void ResetSlot()
    {
        for(int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i].ResetSlot();
        }
    }
    public void SetItemListTable()
    {
       
        if (MonsterManager.thisRound >= 30) //라운드가 30이상일 때 상점에 상위 아이템 추가
        {
            for (int i = 0; i < TableArmorStat.Instance.m_highArmor.Count; i++)
            {
                m_armorList.Add(TableArmorStat.Instance.m_highArmor[i]);
            }
            for (int i = 0; i < TableGunstat.Instance.m_highWeapon.Count; i++)
            {
                m_weaponList.Add(TableGunstat.Instance.m_highWeapon[i]);
            }
            for (int i = 0; i < TableItemData.Instance.m_highItem.Count; i++)
            {
                m_itemList.Add(TableItemData.Instance.m_highItem[i]);
            }
        }
        else if (MonsterManager.thisRound >= 15) //라운드가 15이상일 때 상점에 중간아이템 추가
        {
            for (int i = 0; i < TableArmorStat.Instance.m_midArmor.Count; i++)
            {
                m_armorList.Add(TableArmorStat.Instance.m_midArmor[i]);
            }
            for (int i = 0; i < TableGunstat.Instance.m_midWeapon.Count; i++)
            {
                m_weaponList.Add(TableGunstat.Instance.m_midWeapon[i]);
            }
            for (int i = 0; i < TableItemData.Instance.m_highItem.Count; i++)
            {
                m_itemList.Add(TableItemData.Instance.m_midItem[i]);
            }
        }
        else //기본 아이템
        {
            m_armorList = TableArmorStat.Instance.m_lowArmor;
            m_weaponList = TableGunstat.Instance.m_lowWeapon;
            m_itemList = TableItemData.Instance.m_lowItem;
        }

    }
    void SetSlotItem(List<int> list,ItemType type)
    {
        if (type.Equals(ItemType.Item))
        {
            for (int i = 0; i < list.Count; i++)
            {
                m_slots[i].SetStoreItem(list[i], m_itemdata[list[i]].type, type, m_player);
            }
        }
        else if (type.Equals(ItemType.Armor))
        {
            for (int i = 0; i < list.Count; i++)
            {
                m_slots[i].SetStoreItem(list[i], m_armordata[list[i]].Image, type, m_player); 
            }
        }
        else if (type.Equals(ItemType.Weapon))
        {
            for (int i = 0; i < list.Count; i++)
            {
                m_slots[i].SetStoreItem(list[i], m_weapondata[list[i]].Image, type, m_player);
            }
        }
    }
    /*   여기서 이벤트트리거를 조작하려했던 흔적들...ㅎ
     
          m_slots[i].AddComponent<EventTrigger>();
                EventTrigger triger = m_slots[i].GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => { m_info.ActiveUI(list[i], m_itemdata[list[i]].type); });
                triger.triggers.Add(entry);

                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((eventData) => { m_info.DeActiveUI(); });
                triger.triggers.Add(entry);

       m_slots[i].AddComponent<EventTrigger>();
                EventTrigger triger = m_slots[i].GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => { m_info.ActiveUI(list[i], m_itemdata[list[i]].type); });
                triger.triggers.Add(entry);

                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((eventData) => { m_info.DeActiveUI(); });
                triger.triggers.Add(entry);
     
     
      m_slots[i].AddComponent<EventTrigger>();
                EventTrigger triger = m_slots[i].GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => { m_info.ActiveUI(list[i], m_itemdata[list[i]].type); });
                triger.triggers.Add(entry);

                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((eventData) => { m_info.DeActiveUI(); });
                triger.triggers.Add(entry);
     
     
     */

   

    void Start()
    {
        m_slots = m_content.GetComponentsInChildren<Slot>();
        SetItemsInfos();
        SetItemListTable();
        for(int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i].SetStore(this, m_buyItem);
        }
        closebutton.onClick.AddListener(CloseStore);
        m_info.SetStoreUI(this);
    }
}
