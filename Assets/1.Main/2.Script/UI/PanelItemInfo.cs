using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class PanelItemInfo : MonoBehaviour
{

    #region Constants and Fields
    TextMeshProUGUI[] m_infoText;
    TextMeshProUGUI m_ItemInfo;
    TextMeshProUGUI m_ItemName;
    Image m_image;
    StoreUI m_store;
    int id;
    bool isfirst = true;
    #endregion

    #region Methods
    void SetInfo(int m_id, ItemType m_type) //ȭ�鿡 ����� ���� ����. ������ Ÿ�Կ� ���� �ٸ� ������ �����ϸ� ����������� ���� ��� ��â ���
    {
        if (m_type.Equals(ItemType.Item))
        {
            ItemData data = m_store.m_itemdata[m_id];
            ObjectStat stat = new ObjectStat();
            m_ItemName.text = data.Type.ToString();
            m_ItemInfo.text = data.ItemInfo;
            m_image.sprite = ImageLoader.Instance.GetImage(data.Type.ToString());
            if (data.Type.Equals(UseItemType.HealPack))
            {
                m_infoText[0].text = ("ȸ���� : " + data.Heal + "%");
            }
            else if (data.Type.Equals(UseItemType.Barricade))
            {
                stat = ObjectManager.Instance.GetObjectStat(ObjectType.Barricade);
                m_infoText[0].text = ("ü�� : " + stat.HP);
                m_infoText[1].text = ("���� : " + stat.Defence);
                m_infoText[2].text = ("�ִ� ��ġ�� : " + stat.MaxBuild);
            }
            else if (data.Type.Equals(UseItemType.GunTurret))
            {
                stat = ObjectManager.Instance.GetObjectStat(ObjectType.Turret);
                m_infoText[0].text = ("���ݷ� : " + stat.Damage);
                m_infoText[1].text = ("���� : " + stat.Defence); 
                m_infoText[2].text = ("ü�� : " + stat.HP);
                m_infoText[3].text = ("���ݼӵ� : " + stat.FireRate);
                m_infoText[4].text = ("�����Ÿ� : " + stat.Range);
                m_infoText[5].text = ("ũ��Ƽ��Ȯ�� : " + stat.CriRate);
                m_infoText[6].text = ("ũ��Ƽ�õ����� : " + stat.CriDamage);
                m_infoText[7].text = ("�ִ뼳ġ�� : " + stat.MaxBuild +"(��ų ���� �� ��ġ ����)");
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
    void LoadData() //������ ������ �ε�
    {
        isfirst = false;
        if(m_store == null)
           m_store = UGUIManager.Instance.GetStoreUI();
        if(!m_store.isLoaded())
             m_store.LoadInfo();
        m_infoText = Utill.GetChildObject(gameObject, "TextInfos").GetComponentsInChildren<TextMeshProUGUI>();
        m_ItemInfo = Utill.GetChildObject(gameObject, "Text_Info").GetComponent<TextMeshProUGUI>();
        m_ItemName = Utill.GetChildObject(gameObject, "ItemName").GetComponent<TextMeshProUGUI>();
        m_image = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>();
    }
    private void ResetData() //�����Ǿ��ִ� ������ �ʱ�ȭ
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
    public void ActiveUI(int ID, ItemType type) //UI ���ֱ�
    {
        if(ID == -1) return;
        if (isfirst)
        {
            LoadData();
        }
        gameObject.SetActive(true);
        if (id.Equals(ID)) //����ؼ� ȣ��Ǵ� ��찡 ��� �־ �޸� ���� ������ ���� ���� ID�϶� ���� ������ �Լӻ��.
        {
            return;
        }
        ResetData();
        id = ID;
        SetInfo(ID, type);
    }
    public void DeActiveUI() //����
    {
         if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }
    #endregion
}
