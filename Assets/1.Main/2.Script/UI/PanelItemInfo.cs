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
            Debug.Log("������ ����");
            m_ItemName.text = m_store.m_itemdata[m_id].type;
            m_ItemInfo.text = m_store.m_itemdata[m_id].ItemInfo;
            m_image.sprite = ImageLoader.Instance.GetImage(m_store.m_itemdata[m_id].type);
            if (m_ItemName.Equals("HealPack"))
            {
                m_infoText[0].text = ("ȸ���� : " + m_store.m_itemdata[m_id].Heal + "%");
            }
            else if (m_ItemName.Equals("Barricade"))
            {
                m_infoText[0].text = ("ü�� : " + m_store.m_itemdata[m_id].HP);
                m_infoText[1].text = ("���� : " + m_store.m_itemdata[m_id].Defence);
            }
        }
        else if (m_type.Equals(ItemType.Weapon))
        {
            m_image.sprite = ImageLoader.Instance.GetImage(m_store.m_weapondata[m_id].Image);
            m_ItemName.text = m_store.m_weapondata[m_id].Image;
            m_ItemInfo.text = m_store.m_weapondata[m_id].Info;
            m_infoText[0].text = "���ݷ� : "+ m_store.m_weapondata[m_id].Damage;
            if (m_store.m_weapondata[m_id].weaponType.Equals(WeaponType.ShotGun))
            {
                m_infoText[0].text = "���ݷ� : " + m_store.m_weapondata[m_id].Damage + " * " + m_store.m_weapondata[m_id].Shotgun;
            }
            m_infoText[1].text = "���ݼӵ� : " + m_store.m_weapondata[m_id].AtkSpeed;
            m_infoText[2].text = "�������ð� : " + m_store.m_weapondata[m_id].ReloadTime;
            m_infoText[3].text = "��ź�� : " + m_store.m_weapondata[m_id].Mag;
            m_infoText[4].text = "�ӵ����ʽ� : " + m_store.m_weapondata[m_id].Speed;
            m_infoText[5].text = "ü�º��ʽ� : " + m_store.m_weapondata[m_id].HP;
            m_infoText[6].text = "���� : " + m_store.m_weapondata[m_id].Defence;
            m_infoText[7].text = "ũ��Ƽ��Ȯ�� : " + m_store.m_weapondata[m_id].CriRate;
            m_infoText[8].text = "ũ�������� : " + m_store.m_weapondata[m_id].CriDamage;
            m_infoText[9].text = "�˹�Ȯ�� : " + m_store.m_weapondata[m_id].KnockBack;
            m_infoText[10].text = "�˹��Ŀ� : " + m_store.m_weapondata[m_id].KnockBackDist;
            m_infoText[11].text = "��Ÿ� : " + m_store.m_weapondata[m_id].AttackDist;
        }
        else if (m_type.Equals(ItemType.Armor))
        {
            m_image.sprite = ImageLoader.Instance.GetImage(m_store.m_armordata[m_id].Image);
            m_ItemName.text = m_store.m_armordata[m_id].Image;
            m_ItemInfo.text = m_store.m_armordata[m_id].Info;
            m_infoText[0].text = "���� : " + m_store.m_armordata[m_id].Defence;
            if (m_store.m_armordata[m_id].Type.Equals("Helmet"))
            {
                m_infoText[1].text = "ġ��ŸȮ�� : " + m_store.m_armordata[m_id].CriRate + "%";
            }
            else if (m_store.m_armordata[m_id].Type.Equals("Armor"))
            {
                m_infoText[1].text = "ü�º��ʽ� : " + m_store.m_armordata[m_id].HP;
            }
            else if (m_store.m_armordata[m_id].Type.Equals("Glove"))
            {
                m_infoText[1].text = "���ݼӵ����ʽ� : " + m_store.m_armordata[m_id].AttackSpeed;
            }
            else if (m_store.m_armordata[m_id].Type.Equals("Boots"))
            {
                m_infoText[1].text = "�̵��ӵ����ʽ� : " + m_store.m_armordata[m_id].Speed;    
            }
            else if (m_store.m_armordata[m_id].Type.Equals("Pants"))
            {
                m_infoText[1].text = "���ݷº��ʽ� : " + m_store.m_armordata[m_id].Damage;
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
        if (id.Equals(ID)) //����ؼ� ȣ��Ǵ� ��찡 ��� �־ �޸� ���� ������ ���� ���� ID�϶� ���� ������ �Լӻ��.
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
