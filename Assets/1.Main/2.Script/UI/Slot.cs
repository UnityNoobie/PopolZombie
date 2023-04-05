using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] //
    Image m_image;
    [SerializeField]
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
    public void SetStore(StoreUI store, BuyItems buyItem)
    {
        m_store = store;
        m_buyItem = buyItem;
    }
    public void BuyItem()
    {
        UIManager.Instance.MoneyChange(-price);
        m_player.GetComponent<PlayerGetItem>().BuyItem(itemID, itemType);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isEmpty)
        {
            if (price <= Mathf.CeilToInt(PlayerController.Money))
            {
                m_buyItem.ActiveUI(this, price, m_name);
            }
            else
            {
                UIManager.Instance.SystemMessageCantOpen("돈이 부족합니다.");
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)//마우스가 위에 올라왔을때
    {
        if (m_image.sprite != null && eventData.pointerEnter.CompareTag("Slot")) //eventData.pointerEnter.CompareTag("Slot")
        {
            m_store.m_info.ActiveUI(itemID, m_type);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) //슬롯에서 나갔을경우.
    {
        if (eventData.pointerEnter.CompareTag("Slot"))
        {
            m_store.m_info.DeActiveUI();
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
            price = m_store.m_itemdata[ID].Price;
            itemType = m_store.m_itemdata[ID].type;
            m_name = m_store.m_itemdata[ID].Type + ".LV"+ m_store.m_itemdata[ID].Grade;
            m_text.text =  (m_name + "\n 가격 : " +price) ;
        }
        else if (type.Equals(ItemType.Armor))
        {
            price = m_store.m_armordata[ID].Price;
            itemType = m_store.m_armordata[ID].ItemType;
            m_name = m_store.m_armordata[ID].Type + ".LV" + m_store.m_armordata[ID].Grade;
            m_text.text = (m_name + "\n 가격 : " + m_store.m_armordata[ID].Price);
        }
        else if (type.Equals(ItemType.Weapon))
        {
            price = m_store.m_weapondata[ID].Price;
            itemType = m_store.m_weapondata[ID].ItemType;
            m_name = m_store.m_weapondata[ID].Type + ".LV" + m_store.m_weapondata[ID].Grade;
            m_text.text = (m_name + "\n 가격 : " + m_store.m_weapondata[ID].Price);
        }
    }
    public void ResetSlot()
    {
        m_image.sprite = null;
        m_text.text = null;
    }

}
