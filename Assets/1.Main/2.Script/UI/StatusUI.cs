using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ItemData;

public class StatusUI : MonoBehaviour
{
    #region Constants and Fields
    PanelItemInfo m_panelItemInfo;
    PlayerController m_player;
    PlayerGetItem m_playeritem;
    PlayerObjectController m_playerObject;
    TextMeshProUGUI[] m_status;
    TextMeshProUGUI[] m_useAbleItem;
    TextMeshProUGUI m_name;
    SlotStatus[] m_itemSlots;
    Transform m_slotPos;
    Transform m_StatusPos;
    Status m_stat;
    #endregion

    #region Methods
    public void SetActive(bool active) //액티브 설정
    {
        gameObject.SetActive(active);
        if(active)
          SetStatus();
    }

    public void SetPlayer(PlayerController player) //플레이어 정보 설정
    {
        m_player = player;
        m_playeritem = m_player.GetComponent<PlayerGetItem>();
        m_playerObject = m_player.GetComponent<PlayerObjectController>();
        FindNSetSlots();
    }
    public void FindNSetSlots() //좌표 ㅅ설정
    {
        m_panelItemInfo = Utill.GetChildObject(gameObject, "ItemInfo").GetComponent<PanelItemInfo>();
        m_slotPos = Utill.GetChildObject(gameObject, "Panel_ItemSlot");
        m_StatusPos = Utill.GetChildObject(gameObject, "Panel_Info");
        m_name = Utill.GetChildObject(gameObject, "PlayerName").GetComponent<TextMeshProUGUI>();
        m_status = m_StatusPos.GetComponentsInChildren<TextMeshProUGUI>();
        m_useAbleItem = new TextMeshProUGUI[4];
        m_useAbleItem[0] = Utill.GetChildObject(m_slotPos.gameObject, "MaxBarricade").GetComponent<TextMeshProUGUI>();
        m_useAbleItem[1] = Utill.GetChildObject(m_slotPos.gameObject, "ActiveBarricade").GetComponent<TextMeshProUGUI>();
        m_useAbleItem[2] = Utill.GetChildObject(m_slotPos.gameObject, "MaxTurret").GetComponent<TextMeshProUGUI>();
        m_useAbleItem[3] = Utill.GetChildObject(m_slotPos.gameObject, "ActiveTurret").GetComponent<TextMeshProUGUI>();
        m_itemSlots = new SlotStatus[6];
        m_itemSlots[0] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Helmet").GetComponent<SlotStatus>();
        m_itemSlots[1] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Armor").GetComponent<SlotStatus>();
        m_itemSlots[2] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Pants").GetComponent<SlotStatus>();
        m_itemSlots[3] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Glove").GetComponent<SlotStatus>();
        m_itemSlots[4] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Boots").GetComponent<SlotStatus>();
        m_itemSlots[5] = Utill.GetChildObject(m_slotPos.gameObject, "Slot_Weapon").GetComponent<SlotStatus>();
    }
    public void SetStatus() //UI에 현재 스테이터스 정보 표기
    {
        m_stat = new Status();
        m_stat = m_player.GetStatus;
        m_status[0].text = "체력 : "+ m_stat.hp.ToString() + " / " + m_stat.hpMax;
        m_status[1].text = "공격력 : "+ m_stat.damage;
        m_status[2].text = "방어력 : "+ m_stat.defense;
        m_status[3].text = "공격속도 : " + m_stat.atkSpeed.ToString("F1");
        m_status[4].text = "데미지 감소 : " + m_stat.DamageRigist * 100+"%";
        m_status[5].text = "크리티컬 확률 : " + m_stat.criRate + "%";
        m_status[6].text = "초당 회복량 : " + m_stat.SkillHeal;
        m_status[7].text = "크리 데미지 : " + m_stat.criAttack + "%";
        m_status[8].text = "데미지 흡혈 : " + m_stat.Drain + "%";
        m_status[9].text = "넉백 확률 : " + m_stat.KnockBackPer;
        m_status[10].text = "이동속도 : " + m_stat.speed;
        m_status[11].text = "넉백 파워 : " + m_stat.KnockBackDist;

        int[] objectdata = new int[4];
        objectdata= m_playerObject.GetObjectBuildData();
        m_useAbleItem[0].text = "최대 바리케이드 : " + objectdata[0];
        m_useAbleItem[1].text = "설치 바리케이드 : " + objectdata[1];
        m_useAbleItem[2].text = "최대 포탑 : " + objectdata[2];
        m_useAbleItem[3].text = "설치 포탑 : " + objectdata[3];


        if (m_player.HasTitle())
        {
            m_name.text = "["+m_stat.Title+"]\nLV" + m_stat.level + " [" + m_stat.KnickName+"]";
        }
        else
        {
            m_name.text = "LV" + m_stat.level + " [" + m_stat.KnickName + "]";
        }
      
    }
    public int GetEquipItemID(ArmorType armortype, ItemType itemtype) //착용중인 아이템 정보 반환
    {
        if (itemtype.Equals(ItemType.Armor))
        {
            switch (armortype)
            {
                case ArmorType.Helmet:
                    return m_itemSlots[0].GetEquipItemId();    
                case ArmorType.Armor:
                    return m_itemSlots[1].GetEquipItemId();              
                case ArmorType.Pants:
                    return m_itemSlots[2].GetEquipItemId();
                case ArmorType.Glove:
                    return m_itemSlots[3].GetEquipItemId();
                case ArmorType.Boots:
                    return m_itemSlots[4].GetEquipItemId();
            }
        }
        else if (itemtype.Equals(ItemType.Weapon))
        {
            return m_itemSlots[5].GetEquipItemId();
        }
        return -1;

    }
    public void SetSlot(int id, string imagename, ArmorType armortype,ItemType itemtype) //슬롯 설정
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
