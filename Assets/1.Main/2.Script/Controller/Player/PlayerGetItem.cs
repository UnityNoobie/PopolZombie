using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;

public class PlayerGetItem : MonoBehaviour
{

    #region Constants and Fields
    [SerializeField]
    QuickSlot m_slot;
    StoreUI m_storeUI;
    StatusUI m_statusUI;
    GunManager m_weaponmanager;
    ArmorManager m_armormanager;
    PlayerController m_player;
    CapsuleCollider m_collider;
    
    int m_playerMoney = 0;
    #endregion

    #region Property
    public WeaponData m_weapondata { get; set; }
    #endregion



    #region Methods

    #region public return Method
    public bool HaveEnoughMoney(int price) //플레이어가 충분한 재화를 가지고 있는지 반환해주는 메소드
    {
        if (price <= m_playerMoney)
        {
            return true;
        }
        return false;
    }
    #endregion
    void MoneyChange(int money) //플레이어 재화 변경
    {
        m_playerMoney += money;
        UIManager.Instance.MoneyUI(m_playerMoney);
    }
    public void GetMoney(int money) //재화 획득 시 호출되어 재화변경 메소드 실행
    {
        MoneyChange(money);
    }
    public void BuyItem(int Id, string itemtype,int price) //아이템 구매 성공 시 호출되어 아이템 타입에 따라 관련 메소드 호출.
    {
        if(itemtype.Equals("Gun") || itemtype.Equals("Melee"))
        {
            ChangeWeapon(Id);
        }
        else if(itemtype.Equals("Armor"))
        {
            ChangeArmor(Id);
        }
        else if (itemtype.Equals("HealPack"))
        {
            AddHealPack(Id);
        }
        else if (itemtype.Equals("Barricade"))
        {
            AddBarricade(Id);
        }
        else if (itemtype.Equals("Turret"))
        {
            AddTurret(Id);
        }   
        else if (itemtype.Equals("Generator"))
        {
            if(Id == 38)
            {
                ObjectManager.Instance.GetGenerator().IncreaseMaxHp(TableItemData.Instance.itemData[Id].HP);
            }
            else if(Id == 39)
            {
                ObjectManager.Instance.GetGenerator().IncreaseDefence(TableItemData.Instance.itemData[Id].Defence);
            }
            else if (Id == 40)
            {
                ObjectManager.Instance.GetGenerator().IncreaseHPRegen(TableItemData.Instance.itemData[Id].Heal);
            }
        }
        MoneyChange(price);
    }
    void AddHealPack(int Id) //퀵슬롯 클래스에 아이템 획득 신호 전달
    {
        m_slot.SetItem(1, "HealPack");
    }
    void AddBarricade(int Id)//퀵슬롯 클래스에 아이템 획득 신호 전달
    {
        m_slot.SetItem(2, "Barricade");
    }
    void AddTurret(int Id)//퀵슬롯 클래스에 아이템 획득 신호 전달
    {
        m_slot.SetItem(3, "Turret");
    }
    void ChangeArmor(int id) //방어구 제어 클래스에 정보 전달.
    {
        m_armormanager.ChangeArmor(id);
    }
    void ChangeWeapon(int Id) //무기 제어 클래스에 정보 전달
    {
        m_weaponmanager.ChangeWeapon(Id);
    }
    void SetPlayer(PlayerController player) // 플레이어 지정
    {
        m_slot.SetPlayer(player);
    }
    public void UsingCheatKey()
    {
        m_playerMoney = 100000;
    }
    private void OnTriggerEnter(Collider other)//상점 진입 감지를 위한 메소드
    {
        if (other.CompareTag("Store")) //상점 에어리어에 들어갔을경우
        {
            m_storeUI.ActiveStoreUI(other.name,m_player);
        }
    }
    private void OnTriggerExit(Collider other) //상점 영역 밖으로 나가는것을 감지
    {
        if (other.CompareTag("Store"))
        {
            m_storeUI.CloseStore();
        }
    }

    void SetTransform() // 좌표 지정
    {
        m_player = GetComponent<PlayerController>();
        m_weaponmanager = GetComponent<GunManager>();
        m_armormanager = GetComponent<ArmorManager>();
        m_collider = GetComponent<CapsuleCollider>();
        m_statusUI = UGUIManager.Instance.GetStatusUI();
        m_storeUI = UGUIManager.Instance.GetStoreUI();
    }
    void Awake()    
    {
        SetTransform();
        SetPlayer(m_player);
    }
    void Start()
    {
      //  GetMoney(50000);//테스트용
    }
    #endregion
}
