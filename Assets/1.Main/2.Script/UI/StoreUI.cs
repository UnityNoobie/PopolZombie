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
    #region Constants and Fields
    Slot[] m_slots;
    Transform m_content;
    Button m_closebutton;
    TextMeshProUGUI m_StoreName;
    BuyItems m_buyItem;
    PanelItemInfo m_info;
    StatusUI m_status;
    PlayerController m_player;
    bool isloaded = false;
    public Dictionary<int, ArmorData> m_armordata = new Dictionary<int, ArmorData>();
    public Dictionary<int, WeaponData> m_weapondata = new Dictionary<int, WeaponData>();
    public Dictionary<int, ItemData> m_itemdata = new Dictionary<int, ItemData>();
    List<int> m_armorList = new List<int>();
    List<int> m_weaponList = new List<int>();
    List<int> m_itemList = new List<int>();
    #endregion

    #region Methods
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
        gameObject.SetActive(true);
    }
    public void CloseStore() //꺼주기
    {
        gameObject.SetActive(false);
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
        if (!isloaded)
        {
            LoadUIInfo();
        }
        for (int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i].ResetSlot();
        }
    }
    public void SetItemListTable()
    {
        if (GameManager.Instance.GetRoundInfo() >= 21) //라운드가 21이상일 때 상점에 상위 아이템 추가
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
        else if (GameManager.Instance.GetRoundInfo() >= 11) //라운드가 11이상일 때 상점에 중간아이템 추가
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
    void SetStore()
    {
        m_content = Utill.GetChildObject(gameObject, "Content");
        m_slots = m_content.GetComponentsInChildren<Slot>(true);
        m_status = UGUIManager.Instance.GetStatusUI();
        m_info = Utill.GetChildObject(gameObject, "ItemInfo").GetComponent<PanelItemInfo>();
        m_buyItem = Utill.GetChildObject(gameObject, "BuyItem").GetComponent<BuyItems>();
        m_StoreName = Utill.GetChildObject(gameObject,"StoreName").GetComponent<TextMeshProUGUI>();
        m_closebutton = Utill.GetChildObject(gameObject,"CloseButton").GetComponent<Button>();
    }
    void LoadUIInfo()
    {
        SetStore();
        SetItemsInfos();
        SetItemListTable();
        for (int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i].SetStore(this, m_buyItem, m_info);
        }
        m_closebutton.onClick.AddListener(CloseStore);
        isloaded = true;
    }
    public void LoadInfo()
    {
        if(!isloaded)
        {
            LoadUIInfo();
        }
    }
    public bool isLoaded()
    {
        return isloaded;
    }
    void Awake()
    {
        if (!isloaded)
        {
            LoadUIInfo();
        }
    }
    #endregion
}
