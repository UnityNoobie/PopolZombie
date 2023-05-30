using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    #region Constants and Fields
    PanelItemInfo m_panelItemInfo;
    PlayerController m_player;
    PlayerGetItem m_playeritem;
    TextMeshProUGUI[] m_status;
    TextMeshProUGUI[] m_useAbleItem;
    TextMeshProUGUI m_name;
    SlotStatus[] m_itemSlots;
    Transform m_slotPos;
    Transform m_StatusPos;
    #endregion

    #region Methods
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        if(active)
          SetStatus();
    }

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
        m_playeritem = m_player.GetComponent<PlayerGetItem>();
        FindNSetSlots();
    }
    public void FindNSetSlots()
    {
        m_panelItemInfo = Utill.GetChildObject(gameObject, "ItemInfo").GetComponent<PanelItemInfo>();
        m_slotPos = Utill.GetChildObject(gameObject, "Panel_ItemSlot");
        m_StatusPos = Utill.GetChildObject(gameObject, "Panel_Info");
        m_name = Utill.GetChildObject(gameObject, "PlayerName").GetComponent<TextMeshProUGUI>();
        m_status = m_StatusPos.GetComponentsInChildren<TextMeshProUGUI>();
        m_useAbleItem = m_slotPos.GetComponentsInChildren<TextMeshProUGUI>();
        m_itemSlots = new SlotStatus[6];
        m_itemSlots[0] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Helmet").GetComponent<SlotStatus>();
        m_itemSlots[1] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Armor").GetComponent<SlotStatus>();
        m_itemSlots[2] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Pants").GetComponent<SlotStatus>();
        m_itemSlots[3] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Glove").GetComponent<SlotStatus>();
        m_itemSlots[4] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Boots").GetComponent<SlotStatus>();
        m_itemSlots[5] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Weapon").GetComponent<SlotStatus>();
    }
    public void SetStatus()
    {
        m_status[0].text = "체력 : "+ m_player.GetStatus.hp + " / " + m_player.GetStatus.hpMax;
        m_status[1].text = "공격력 : "+ m_player.GetStatus.damage;
        m_status[2].text = "방어력 : "+ m_player.GetStatus.defense;
        m_status[3].text = "공격속도 : " + m_player.GetStatus.atkSpeed;
        m_status[4].text = "데미지 감소 : " + m_player.GetStatus.DamageRigist+"%";
        m_status[5].text = "크리티컬 확률 : " + m_player.GetStatus.criRate + "%";
        m_status[6].text = "초당 회복량 : " + m_player.GetStatus.SkillHeal;
        m_status[7].text = "크리 데미지 : " + m_player.GetStatus.criAttack + "%";
        m_status[8].text = "데미지 흡혈 : " + m_player.GetStatus.Drain + "%";
        m_status[9].text = "넉백 확률 : " + m_player.GetStatus.KnockBackPer;
        m_status[10].text = "이동속도 : " + m_player.GetStatus.speed;
        m_status[11].text = "넉백 파워 : " + m_player.GetStatus.KnockBackDist;
        if (m_player.HasTitle())
        {
            m_name.text = "["+m_player.GetStatus.Title+"]\nLV" + m_player.GetStatus.level + " " + m_player.GetStatus.KnickName;
        }
        else
        {
            m_name.text = "LV" + m_player.GetStatus.level + " " + m_player.GetStatus.KnickName;
        }
      
    }
    public void SetSlot(int id, string imagename, ArmorType armortype,ItemType itemtype)
    {
        if (itemtype.Equals(ItemType.Armor))
        {
            switch (armortype)
            {
                case ArmorType.Helmet:
                    m_itemSlots[0].SetSlotItem(id, imagename, itemtype, m_panelItemInfo);
                    break;
                case ArmorType.Armor:
                    m_itemSlots[1].SetSlotItem(id, imagename, itemtype, m_panelItemInfo);
                    break;
                case ArmorType.Glove:
                    m_itemSlots[3].SetSlotItem(id, imagename, itemtype, m_panelItemInfo);
                    break;
                case ArmorType.Pants:
                    m_itemSlots[2].SetSlotItem(id, imagename, itemtype, m_panelItemInfo);
                    break;
                case ArmorType.Boots:
                    m_itemSlots[4].SetSlotItem(id, imagename, itemtype, m_panelItemInfo);
                    break;
            }
        }
        else if (itemtype.Equals(ItemType.Weapon))
        {
            m_itemSlots[5].SetSlotItem(id, imagename, itemtype, m_panelItemInfo);
        }  
    }
    #endregion
}
