using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour //���� ���� ������ �����ϴ� Ŭ����. �� ���� ArmorData�� �ƴ� ���� ������ ���� ������ �����ϰ� �־� ���� �����丵 �ؾ���.
{
    #region Constants and Fields
    [SerializeField]
    GameObject[] m_wearArmors = new GameObject[5]; //����� ���
    [SerializeField]
    GameObject[] m_helmets;
    [SerializeField]
    GameObject[] m_gloves;
    [SerializeField]
    GameObject[] m_armors;
    [SerializeField]
    GameObject[] m_pants;
    [SerializeField]
    GameObject[] m_boots;
    Dictionary<ArmorType, ArmorData> wearArmor = new Dictionary<ArmorType, ArmorData>(); //�����ϰ� �ִ� �� ������ ����
    PlayerController m_player;
    WearArmorData m_wearArmorData;
    ArmorData armorData { get; set; }
    #endregion

    #region Methods
    void ResetStatus() //������ �ʱ�ȭ
    { 
        m_wearArmorData = new WearArmorData();
    }
    void SetArmorData() // ���� ������ �������ֱ�.
    {
        ResetStatus();//�ϴ� TableArmor�� ���Ӱ� ������ �ؾ��ϱ� ������ ���� �� �ʱ�ȭ����.
        foreach(ArmorData data in wearArmor.Values) //�������� �������� �����ϰ� �ִ� ��ųʸ����� ���� ã�Ƽ� ������.
        {
            m_wearArmorData.Defence += data.Defence;
            m_wearArmorData.Damage += data.Damage;
            m_wearArmorData.HP += data.HP;
            m_wearArmorData.AttackSpeed += data.AttackSpeed;
            m_wearArmorData.CriRate += data.CriRate;
            m_wearArmorData.Speed += data.Speed;
        }
        m_player.SetArmData(m_wearArmorData);
    }

    public int GetEquipArmorData(ArmorType type)
    {
        if (!wearArmor.ContainsKey(type)) return -1;
        return wearArmor[type].Id;
    }
    public void ChangeArmor(int Id) // �� ȹ��� ��� ���� ���̵� �޾ƿ� ��ü �ǽ�.
    {
        ArmorData m_armorData =  armorData.GetArmorData(Id);//������ ������ ������
        UGUIManager.Instance.GetStatusUI().SetSlot(Id, m_armorData.Image, m_armorData.armorType, ItemType.Armor); //�������ͽ� UI�� ���� ����
        wearArmor[m_armorData.armorType] = m_armorData; //��ųʸ��� ���� ���� ���� ����.
        if (m_armorData.armorType.Equals(ArmorType.Helmet))
        {
            SetHelmet(Id, m_armorData.Grade - 1);
        }
        else if (m_armorData.armorType.Equals(ArmorType.Glove)) 
        {
            SetGloves(Id, m_armorData.Grade - 1);
        }
        else if (m_armorData.armorType.Equals(ArmorType.Armor))
        {
            SetArmors(Id, m_armorData.Grade - 1);
        }
        else if (m_armorData.armorType.Equals(ArmorType.Pants))
        {
            SetPants(Id, m_armorData.Grade - 1);
        }
        else if (m_armorData.armorType.Equals(ArmorType.Boots))
        {
            SetBoots(Id, m_armorData.Grade - 1);
        }
        SetArmorData(); //���� ��ü�� �Ͼ���� ���� ������.
    }
    void SetHelmet(int id, int grade) //��� ������ ���� �޼���
    {
        if (m_wearArmors[0] != null)
        {
            m_wearArmors[0].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        m_wearArmors[0] = m_helmets[grade];
        m_wearArmors[0].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
    }
    void SetGloves(int id,int grade)//�۷��� ������ ���� �޼���
    {
        if (m_wearArmors[1] != null)
        {
            m_wearArmors[1].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        m_wearArmors[1] = m_gloves[grade];
        m_wearArmors[1].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
    }
    void SetArmors(int id, int grade)//�� ������ ���� �޼���
    {
        if (m_wearArmors[2] != null)
        {
            m_wearArmors[2].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        m_wearArmors[2] = m_armors[grade];
        m_wearArmors[2].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
    }
    void SetPants(int id, int grade)//���� ������ ���� �޼���
    {
        if (m_wearArmors[3] != null)
        {
            m_wearArmors[3].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        m_wearArmors[3] = m_pants[grade];
        m_wearArmors[3].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
    }
    void SetBoots(int id, int grade)//�Ź� ������ ���� �޼���
    {
        if (m_wearArmors[4] != null)
        {
            m_wearArmors[4].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        m_wearArmors[4] = m_boots[grade];
        m_wearArmors[4].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
    }

    void Start()
    {
        m_player = GetComponent<PlayerController>();
        armorData = new ArmorData();
    }
    #endregion
}
