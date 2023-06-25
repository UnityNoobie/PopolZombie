using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItemData;

public class Slot : MonoBehaviour, IPointerUpHandler,IPointerEnterHandler, IPointerExitHandler
{
    #region Constants and Fields
    Image m_image;
    TextMeshProUGUI m_text;
    PlayerController m_player;
    BuyItems m_buyItem;
    bool isEmpty = true;
    int itemID;
    int price;
    string itemType;
    ItemType m_type;
    string m_name;
    StoreUI m_store;
    ArmorType m_armortype;
    PanelItemInfo m_info;
    PanelItemInfo m_equipItem;
    #endregion

    #region Methods
    public void SetStore(StoreUI store, BuyItems buyItem,PanelItemInfo info,PanelItemInfo equip)
    {
        m_store = store;
        m_buyItem = buyItem;
        m_info = info;
        m_equipItem = equip;
    }
    public void BuyItem()
    {
        m_player.GetComponent<PlayerGetItem>().BuyItem(itemID, itemType,-price);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        UGUIManager.Instance.PlayClickSFX();
        if (!isEmpty)
        {
            if (m_player.GetComponent<PlayerGetItem>().HaveEnoughMoney(price))
            {
                m_buyItem.ActiveUI(this, price, m_name);
            }
            else
            {
                UGUIManager.Instance.SystemMessageSendMessage("돈이 부족합니다.");
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)//마우스가 위에 올라왔을때
    {
        if (m_image.sprite != null && eventData.pointerEnter.CompareTag("Slot")) //eventData.pointerEnter.CompareTag("Slot")
        {
            m_info.ActiveUI(itemID, m_type);
            if(m_type != ItemType.Item)
            {
                m_equipItem.ActiveUI(UGUIManager.Instance.GetStatusUI().GetEquipItemID(m_armortype, m_type), m_type);
            }
            
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) //슬롯에서 나갔을경우.
    {
        if (eventData.pointerEnter.CompareTag("Slot"))
        {
            m_info.DeActiveUI();
            m_equipItem.DeActiveUI();
       }
    }
    public void SetStoreItem(int ID,string image,ItemType type,PlayerController player)
    {
        m_image.sprite = ImageLoader.Instance.GetImage(image);
        m_player = player;
        itemID = ID;
        m_type = type;
        isEmpty = false;

        if (type.Equals(ItemType.Item))
        {
            m_armortype = ArmorType.Max;
            price = m_store.m_itemdata[ID].Price;
            itemType = m_store.m_itemdata[ID].type;
            m_name = itemType + ".LV"+ m_store.m_itemdata[ID].Grade;
            m_text.text =  (m_name + "\n 가격 : " +price) ;
        }
        else if (type.Equals(ItemType.Armor))
        {
            price = m_store.m_armordata[ID].Price;
            itemType = m_store.m_armordata[ID].ItemType;
            m_name = m_store.m_armordata[ID].Type + ".LV" + m_store.m_armordata[ID].Grade;
            m_armortype = m_store.m_armordata[ID].armorType;
            m_text.text = (m_name + "\n 가격 : " + m_store.m_armordata[ID].Price);
        }
        else if (type.Equals(ItemType.Weapon))
        {
            m_armortype = ArmorType.Max;
            price = m_store.m_weapondata[ID].Price;
            itemType = m_store.m_weapondata[ID].ItemType;
            m_name = m_store.m_weapondata[ID].Type + ".LV" + m_store.m_weapondata[ID].Grade;
            m_text.text = (m_name + "\n 가격 : " + m_store.m_weapondata[ID].Price);
        }
    }
    public void ResetSlot()
    {
        m_image = Utill.GetChildObject(gameObject,"ItemImage").GetComponent<Image>(); 
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_image.sprite = null;
        m_text.text = null;
    }
    #endregion

}
