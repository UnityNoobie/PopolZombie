using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotStatus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    PanelItemInfo m_infoUI;
    TextMeshProUGUI m_itemName;
    Image m_itemImage;
    ItemType m_type;
    int m_id;



    public void SetSlotItem(int id, string image, ItemType type,PanelItemInfo infoui)
    {
        m_itemImage = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>();
        m_itemName = Utill.GetChildObject(gameObject,"Info").GetComponent<TextMeshProUGUI>();
        m_itemName.text = image;
        m_itemImage.sprite = ImageLoader.Instance.GetImage(image);
        m_type = type;
        m_id = id;
        m_infoUI = infoui;
    }
    public void OnPointerEnter(PointerEventData pointer)
    {
        m_infoUI.ActiveUI(m_id,m_type);
    }
    public void OnPointerExit(PointerEventData pointer)
    {
        if(pointer.pointerEnter.CompareTag("Slot"))
        m_infoUI.DeActiveUI();
    }
    
}
