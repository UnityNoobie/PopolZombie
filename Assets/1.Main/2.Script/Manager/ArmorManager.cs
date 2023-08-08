using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour //���� ���� ������ �����ϴ� Ŭ����. �� ���� ArmorData�� �ƴ� ���� ������ ���� ������ �����ϰ� �־� ���� �����丵 �ؾ���.
{
    #region Constants and Fields
    [SerializeField]
    GameObject[] m_wearArmors = new GameObject[5]; //�� ���
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
    Dictionary<string, ArmorData> wearArmor = new Dictionary<string, ArmorData>(); //�����ϰ� �ִ� �� ������ ����
    PlayerController m_player;
    int Defence;
    float Damage;
    float ReloadTime;
    float AttackSpeed;
    int CriRate;
    float Speed;
    ArmorData armorData { get; set; }
    #endregion

    #region Methods
    void ResetStatus() //������ �ʱ�ȭ
    { 
        Defence = 0;
        Damage = 0;
        ReloadTime = 0;
        AttackSpeed = 0; 
        CriRate = 0;
        Speed = 0;
    }
    void SetArmorData() // ���� ������ �������ֱ�.
    {
        ResetStatus();//�ϴ� TableArmor�� ���Ӱ� ������ �ؾ��ϱ� ������ ���� �� �ʱ�ȭ����.

        if (m_wearArmors[0] != null) //��ųʸ��� ����Ǿ��ִ� ������ �޾ƿ� �������־�� �� ������ ������.
        {
            Defence += wearArmor["Helmet"].Defence;
            Damage += wearArmor["Helmet"].Damage;
            ReloadTime += wearArmor["Helmet"].HP;
            AttackSpeed += wearArmor["Helmet"].AttackSpeed;
            CriRate += wearArmor["Helmet"].CriRate;
            Speed += wearArmor["Helmet"].Speed;

        }
        if (m_wearArmors[1] != null)
        {
            Defence += wearArmor["Glove"].Defence;
            Damage += wearArmor["Glove"].Damage;
            ReloadTime += wearArmor["Glove"].HP;
            AttackSpeed += wearArmor["Glove"].AttackSpeed;
            CriRate += wearArmor["Glove"].CriRate;
            Speed += wearArmor["Glove"].Speed;
            
        }
        if (m_wearArmors[2] != null)
        {
            Defence += wearArmor["Armor"].Defence;
            Damage += wearArmor["Armor"].Damage;
            ReloadTime += wearArmor["Armor"].HP;
            AttackSpeed += wearArmor["Armor"].AttackSpeed;
            CriRate += wearArmor["Armor"].CriRate;
            Speed += wearArmor["Armor"].Speed;
            
        }
        if (m_wearArmors[3] != null)
        {
            Defence += wearArmor["Pants"].Defence;
            Damage += wearArmor["Pants"].Damage;
            ReloadTime += wearArmor["Pants"].HP;
            AttackSpeed += wearArmor["Pants"].AttackSpeed;
            CriRate += wearArmor["Pants"].CriRate;
            Speed += wearArmor["Pants"].Speed;
        }
        if (m_wearArmors[4] != null)
        {
            Defence += wearArmor["Boots"].Defence;
            Damage += wearArmor["Boots"].Damage;
            ReloadTime += wearArmor["Boots"].HP;
            AttackSpeed += wearArmor["Boots"].AttackSpeed;
            CriRate += wearArmor["Boots"].CriRate;
            Speed += wearArmor["Boots"].Speed;
        }

        WearArmorData aarmorData = new WearArmorData();
        aarmorData.SetWearArmorData(Defence, Damage, ReloadTime, AttackSpeed, CriRate, Speed);
        m_player.SetArmData(Defence, Damage, ReloadTime, AttackSpeed, CriRate, Speed);
    }
    
    public void ChangeArmor(int Id) // �� ȹ��� ��� ���� ���̵� �޾ƿ� ��ü �ǽ�.
    {
        ArmorData m_armorData =  armorData.GetArmorData(Id);//������ ������ ������
        UGUIManager.Instance.GetStatusUI().SetSlot(Id, m_armorData.Image, m_armorData.armorType, ItemType.Armor); //�������ͽ� UI�� ���� ����
        
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
    public void SetHelmet(int id, int grade) //��� ������ ���� �޼���
    {
        if (m_wearArmors[0] != null)
        {
            m_wearArmors[0].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//��ųʸ��� ���� �־��ֱ�.
        m_wearArmors[0] = m_helmets[grade];
        m_wearArmors[0].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
        
    }
    public void SetGloves(int id,int grade)//�۷��� ������ ���� �޼���
    {
        if (m_wearArmors[1] != null)
        {
            m_wearArmors[1].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//��ųʸ��� ���� �־��ֱ�.
        m_wearArmors[1] = m_gloves[grade];
        m_wearArmors[1].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
    }
    public void SetArmors(int id, int grade)//�� ������ ���� �޼���
    {
        if (m_wearArmors[2] != null)
        {
            m_wearArmors[2].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//��ųʸ��� ���� �־��ֱ�.
        m_wearArmors[2] = m_armors[grade];
        m_wearArmors[2].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
    }
    public void SetPants(int id, int grade)//���� ������ ���� �޼���
    {
        if (m_wearArmors[3] != null)
        {
            m_wearArmors[3].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//��ųʸ��� ���� �־��ֱ�.
        m_wearArmors[3] = m_pants[grade];
        m_wearArmors[3].SetActive(true); //����� �����Ƹ�0���� �־��ְ� ��Ƽ�� ���ֱ�.
    }
    public void SetBoots(int id, int grade)//�Ź� ������ ���� �޼���
    {
        if (m_wearArmors[4] != null)
        {
            m_wearArmors[4].SetActive(false); //����� �����Ǿ��ִ� �����̸� ���� ��� ���ֱ�
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//��ųʸ��� ���� �־��ֱ�.
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
