using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    PanelItemInfo m_info;
    #endregion

    #region Methods
    public void SetStore(StoreUI store, BuyItems buyItem,PanelItemInfo info)
    {
        m_store = store;
        m_buyItem = buyItem;
        m_info = info;
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
                UGUIManager.Instance.SystemMessageSendMessage("���� �����մϴ�.");
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)//���콺�� ���� �ö������
    {
        if (m_image.sprite != null && eventData.pointerEnter.CompareTag("Slot")) //eventData.pointerEnter.CompareTag("Slot")
        {
            m_info.ActiveUI(itemID, m_type);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) //���Կ��� ���������.
    {
        if (eventData.pointerEnter.CompareTag("Slot"))
        {
            m_info.DeActiveUI();
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
            m_name = itemType + ".LV"+ m_store.m_itemdata[ID].Grade;
            m_text.text =  (m_name + "\n ���� : " +price) ;
        }
        else if (type.Equals(ItemType.Armor))
        {
            price = m_store.m_armordata[ID].Price;
            itemType = m_store.m_armordata[ID].ItemType;
            m_name = m_store.m_armordata[ID].Type + ".LV" + m_store.m_armordata[ID].Grade;
            m_text.text = (m_name + "\n ���� : " + m_store.m_armordata[ID].Price);
        }
        else if (type.Equals(ItemType.Weapon))
        {
            price = m_store.m_weapondata[ID].Price;
            itemType = m_store.m_weapondata[ID].ItemType;
            m_name = m_store.m_weapondata[ID].Type + ".LV" + m_store.m_weapondata[ID].Grade;
            m_text.text = (m_name + "\n ���� : " + m_store.m_weapondata[ID].Price);
        }
    }
    public void ResetSlot()
    {
        m_image = Utill.GetChildObject(gameObject,"ItemImage").GetComponent<Image>(); //GetComponentsintChileren���� ã���� ���ӿ�����Ʈ�� Image�� ������ ������ �̷������� ó����
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_image.sprite = null;
        m_text.text = null;
    }
    #endregion

}
