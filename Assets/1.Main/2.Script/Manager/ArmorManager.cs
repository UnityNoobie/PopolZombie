using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour //장착 방어구의 정보를 관리하는 클래스. 단 현재 ArmorData가 아닌 단일 변수를 통해 정보를 제어하고 있어 추후 리펙토링 해야함.
{
    #region Constants and Fields
    [SerializeField]
    GameObject[] m_wearArmors = new GameObject[5]; //방어구 목록
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
    Dictionary<string, ArmorData> wearArmor = new Dictionary<string, ArmorData>(); //장착하고 있는 방어구 데이터 저장
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
    void ResetStatus() //데이터 초기화
    { 
        Defence = 0;
        Damage = 0;
        ReloadTime = 0;
        AttackSpeed = 0; 
        CriRate = 0;
        Speed = 0;
    }
    void SetArmorData() // 방어구의 정보를 저장해주기.
    {
        ResetStatus();//일단 TableArmor에 새롭게 저장을 해야하기 때문에 값을 다 초기화해줌.

        if (m_wearArmors[0] != null) //딕셔너리에 저장되어있는 값들을 받아와 적용해주어야 할 값들을 더해줌.
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
    
    public void ChangeArmor(int Id) // 방어구 획득시 대상 방어구의 아이디를 받아와 교체 실시.
    {
        ArmorData m_armorData =  armorData.GetArmorData(Id);//방어궁의 정보를 가져옴
        UGUIManager.Instance.GetStatusUI().SetSlot(Id, m_armorData.Image, m_armorData.armorType, ItemType.Armor); //스테이터스 UI에 정보 전달
        
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
        SetArmorData(); //방어구의 교체가 일어났으니 스탯 재정비.
    }
    public void SetHelmet(int id, int grade) //헬멧 장착을 위한 메서드
    {
        if (m_wearArmors[0] != null)
        {
            m_wearArmors[0].SetActive(false); //헬멧이 장착되어있는 상태이면 기존 헬멧 꺼주기
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//딕셔너리에 정보 넣어주기.
        m_wearArmors[0] = m_helmets[grade];
        m_wearArmors[0].SetActive(true); //헬멧을 장착아머0번에 넣어주고 액티브 켜주기.
        
    }
    public void SetGloves(int id,int grade)//글러브 장착을 위한 메서드
    {
        if (m_wearArmors[1] != null)
        {
            m_wearArmors[1].SetActive(false); //헬멧이 장착되어있는 상태이면 기존 헬멧 꺼주기
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//딕셔너리에 정보 넣어주기.
        m_wearArmors[1] = m_gloves[grade];
        m_wearArmors[1].SetActive(true); //헬멧을 장착아머0번에 넣어주고 액티브 켜주기.
    }
    public void SetArmors(int id, int grade)//방어구 장착을 위한 메서드
    {
        if (m_wearArmors[2] != null)
        {
            m_wearArmors[2].SetActive(false); //헬멧이 장착되어있는 상태이면 기존 헬멧 꺼주기
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//딕셔너리에 정보 넣어주기.
        m_wearArmors[2] = m_armors[grade];
        m_wearArmors[2].SetActive(true); //헬멧을 장착아머0번에 넣어주고 액티브 켜주기.
    }
    public void SetPants(int id, int grade)//바지 장착을 위한 메서드
    {
        if (m_wearArmors[3] != null)
        {
            m_wearArmors[3].SetActive(false); //헬멧이 장착되어있는 상태이면 기존 헬멧 꺼주기
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//딕셔너리에 정보 넣어주기.
        m_wearArmors[3] = m_pants[grade];
        m_wearArmors[3].SetActive(true); //헬멧을 장착아머0번에 넣어주고 액티브 켜주기.
    }
    public void SetBoots(int id, int grade)//신발 장착을 위한 메서드
    {
        if (m_wearArmors[4] != null)
        {
            m_wearArmors[4].SetActive(false); //헬멧이 장착되어있는 상태이면 기존 헬멧 꺼주기
        }
        wearArmor[TableArmorStat.Instance.m_armorData[id].Type] = TableArmorStat.Instance.m_armorData[id];//딕셔너리에 정보 넣어주기.
        m_wearArmors[4] = m_boots[grade];
        m_wearArmors[4].SetActive(true); //헬멧을 장착아머0번에 넣어주고 액티브 켜주기.
    }

    void Start()
    {
        m_player = GetComponent<PlayerController>();
        armorData = new ArmorData();
    }
    #endregion
}
