using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PanelItemInfo : MonoBehaviour
{
    public TextMeshProUGUI[] m_infoText;
    public TextMeshProUGUI m_ItemInfo;
    public TextMeshProUGUI m_ItemName;
    [SerializeField]
    Image m_image;
    StoreUI m_store;
    int id;


    void SetInfo(int m_id, ItemType m_type)
    {
        if (m_type.Equals(ItemType.Item))
        {
            Debug.Log("아이템 들어옴");
            m_ItemName.text = m_store.m_itemdata[m_id].type;
            m_ItemInfo.text = m_store.m_itemdata[m_id].ItemInfo;
            m_image.sprite = ImageLoader.Instance.GetImage(m_store.m_itemdata[m_id].type);
            if (m_ItemName.Equals("HealPack"))
            {
                m_infoText[0].text = ("회복량 : " + m_store.m_itemdata[m_id].Heal + "%");
            }
            else if (m_ItemName.Equals("Barricade"))
            {
                m_infoText[0].text = ("체력 : " + m_store.m_itemdata[m_id].HP);
                m_infoText[1].text = ("방어력 : " + m_store.m_itemdata[m_id].Defence);
            }
        }
        else if (m_type.Equals(ItemType.Weapon))
        {
            m_image.sprite = ImageLoader.Instance.GetImage(m_store.m_weapondata[m_id].Image);
            m_ItemName.text = m_store.m_weapondata[m_id].Image;
            m_ItemInfo.text = m_store.m_weapondata[m_id].Info;
            m_infoText[0].text = "공격력 : "+ m_store.m_weapondata[m_id].Damage;
            if (m_store.m_weapondata[m_id].weaponType.Equals(WeaponType.ShotGun))
            {
                m_infoText[0].text = "공격력 : " + m_store.m_weapondata[m_id].Damage + " * " + m_store.m_weapondata[m_id].Shotgun;
            }
            m_infoText[1].text = "공격속도 : " + m_store.m_weapondata[m_id].AtkSpeed;
            m_infoText[2].text = "재장전시간 : " + m_store.m_weapondata[m_id].ReloadTime;
            m_infoText[3].text = "장탄량 : " + m_store.m_weapondata[m_id].Mag;
            m_infoText[4].text = "속도보너스 : " + m_store.m_weapondata[m_id].Speed;
            m_infoText[5].text = "체력보너스 : " + m_store.m_weapondata[m_id].HP;
            m_infoText[6].text = "방어력 : " + m_store.m_weapondata[m_id].Defence;
            m_infoText[7].text = "크리티컬확률 : " + m_store.m_weapondata[m_id].CriRate;
            m_infoText[8].text = "크리데미지 : " + m_store.m_weapondata[m_id].CriDamage;
            m_infoText[9].text = "넉백확률 : " + m_store.m_weapondata[m_id].KnockBack;
            m_infoText[10].text = "넉백파워 : " + m_store.m_weapondata[m_id].KnockBackDist;
            m_infoText[11].text = "사거리 : " + m_store.m_weapondata[m_id].AttackDist;
        }
        else if (m_type.Equals(ItemType.Armor))
        {
            m_image.sprite = ImageLoader.Instance.GetImage(m_store.m_armordata[m_id].Image);
            m_ItemName.text = m_store.m_armordata[m_id].Image;
            m_ItemInfo.text = m_store.m_armordata[m_id].Info;
            m_infoText[0].text = "방어력 : " + m_store.m_armordata[m_id].Defence;
            if (m_store.m_armordata[m_id].Type.Equals("Helmet"))
            {
                m_infoText[1].text = "치명타확률 : " + m_store.m_armordata[m_id].CriRate + "%";
            }
            else if (m_store.m_armordata[m_id].Type.Equals("Armor"))
            {
                m_infoText[1].text = "체력보너스 : " + m_store.m_armordata[m_id].HP;
            }
            else if (m_store.m_armordata[m_id].Type.Equals("Glove"))
            {
                m_infoText[1].text = "공격속도보너스 : " + m_store.m_armordata[m_id].AttackSpeed;
            }
            else if (m_store.m_armordata[m_id].Type.Equals("Boots"))
            {
                m_infoText[1].text = "이동속도보너스 : " + m_store.m_armordata[m_id].Speed;    
            }
            else if (m_store.m_armordata[m_id].Type.Equals("Pants"))
            {
                m_infoText[1].text = "공격력보너스 : " + m_store.m_armordata[m_id].Damage;
            }
            
           
      
        }
    }
    private void ResetData()
    {
        for(int i = 0; i < m_infoText.Length; i++)
        {
            if (m_infoText[i].text != null)
            {
                m_infoText[i].text = null;
            }
        }
        if (m_ItemName.text != null)
        {
            m_ItemName.text = null;
        }
            
        if (m_ItemInfo.text != null)
        {
            m_ItemInfo.text = null;
        }  
        if (m_image.sprite != null)
        {
            m_image.sprite = null;
        }
    }
    public void SetStoreUI(StoreUI store)
    {
        m_store = store;
    }
    public void ActiveUI(int ID, ItemType type)
    {
        gameObject.SetActive(true);
        if (id.Equals(ID)) //계속해서 호출되는 경우가 계속 있어서 메모리 낭비 방지를 위해 같은 ID일땐 기존 데이터 게속사용.
        {
            return;
        }
        ResetData();
        id = ID;
        SetInfo(ID, type);
    }
    public void DeActiveUI()
    {
         gameObject.SetActive(false);
    }

}
