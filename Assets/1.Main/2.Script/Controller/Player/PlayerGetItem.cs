using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;

public enum PlayerItemType
{
    HealPack,
    Barricade,
    Turret
}
public class PlayerGetItem : MonoBehaviour
{

    #region Constants and Fields
    [SerializeField]
    QuickSlot m_slot;
    StoreUI m_storeUI;
    GunManager m_weaponmanager;
    ArmorManager m_armormanager;
    PlayerController m_player;
    CapsuleCollider m_collider;
    
    int m_playerMoney = 0;
    int m_healpackCount = 0;
    int m_barricadeCount = 0;
    int m_turretCount = 0;
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
        UGUIManager.Instance.GetScreenHUD().SetMoney(m_playerMoney);
       // UIManager.Instance.MoneyUI(m_playerMoney); //구버전 NGUI
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
            AddItem(PlayerItemType.HealPack);
        }
        else if (itemtype.Equals("Barricade"))
        {
            AddItem(PlayerItemType.Barricade);
        }
        else if (itemtype.Equals("Turret"))
        {
            AddItem(PlayerItemType.Turret);
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
    public bool HaveEnoughItem(PlayerItemType type)
    {
        if (type.Equals(PlayerItemType.HealPack))
        {
            if (m_healpackCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "의 갯수가 모자랍니다.");
                return false;
            }
        }
        else if (type.Equals(PlayerItemType.Barricade))
        {
            if (m_barricadeCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "의 갯수가 모자랍니다.");
                return false;
            }
        }
        else if (type.Equals(PlayerItemType.Turret))
        {
            if (m_turretCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "의 갯수가 모자랍니다.");
                return false;
            }
        }
        return true;
    }
    public void UseItem(PlayerItemType type)
    {
        if (type.Equals(PlayerItemType.HealPack))
        {
            if(m_healpackCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "의 갯수가 모자랍니다.");
                return;
            }
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(1, m_healpackCount, "HealPack");
            m_healpackCount--;
        }
        else if (type.Equals(PlayerItemType.Barricade))
        {
            if (m_barricadeCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "의 갯수가 모자랍니다.");
                return;
            }
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(2, m_barricadeCount, "Barricade");
            m_barricadeCount--;
        }
        else if (type.Equals(PlayerItemType.Turret))
        {
            if (m_turretCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "의 갯수가 모자랍니다.");
                return;
            }
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(3, m_turretCount, "Turret");
            m_turretCount--;
        }
    }
    void AddItem(PlayerItemType type)
    {
        if(type.Equals(PlayerItemType.HealPack))
        {
            //  m_slot.SetItem(1, "HealPack");기존 사용 UI
            m_healpackCount++;
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(1, m_healpackCount, "HealPack");
        }
        else if(type.Equals(PlayerItemType.Barricade))
        {
            //  m_slot.SetItem(2, "Barricade");기존 사용 UI
            m_barricadeCount++;
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(2, m_barricadeCount, "Barricade");
        }
        else if (type.Equals(PlayerItemType.Turret))
        {
            //  m_slot.SetItem(3, "Turret");기존 사용 UI
            m_turretCount++;
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(3, m_turretCount, "Turret");
        }
    }
    /*
    void AddHealPack(int Id) //퀵슬롯 클래스에 아이템 획득 신호 전달
    {
      //  m_slot.SetItem(1, "HealPack");기존 사용 UI
        m_healpackCount++;
        UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(1, m_healpackCount, "HealPack");
    }
    void AddBarricade(int Id)//퀵슬롯 클래스에 아이템 획득 신호 전달
    {
        //  m_slot.SetItem(2, "Barricade");기존 사용 UI
        m_barricadeCount++;
        UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(2, m_barricadeCount, "HealPack");
    }
    void AddTurret(int Id)//퀵슬롯 클래스에 아이템 획득 신호 전달
    {
        //  m_slot.SetItem(3, "Turret");기존 사용 UI
        m_turretCount++;
        UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(3, m_turretCount, "HealPack");
    }*/
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
        //m_slot.SetPlayer(player);
    }
    public void UsingCheatKey()
    {
        MoneyChange(100000);
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
