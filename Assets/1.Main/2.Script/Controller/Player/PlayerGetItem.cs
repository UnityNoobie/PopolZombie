using System.Collections;
using System.Collections.Generic;
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

    #region public return Method
    public bool HaveEnoughMoney(int price)
    {
        if(price <= m_playerMoney)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Methods
    void MoneyChange(int money)
    {
        m_playerMoney += money;
        UIManager.Instance.MoneyUI(m_playerMoney);
    }
    public void GetMoney(int money)
    {
        MoneyChange(money);
    }
    public void BuyItem(int Id, string itemtype,int price)
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
        MoneyChange(price);
    }
    void AddHealPack(int Id)
    {
        m_slot.SetItem(1, "HealPack");
    }
    void AddBarricade(int Id)
    {
        m_slot.SetItem(2, "Barricade");
    }
    void AddTurret(int Id)
    {
        m_slot.SetItem(3, "Turret");
    }
    void ChangeArmor(int id)
    {
        m_armormanager.ChangeArmor(id,m_statusUI);
    }
    void ChangeWeapon(int Id)
    {

        m_weaponmanager.ChangeWeapon(Id,m_statusUI);
    }

    void SetPlayer(PlayerController player) // 플레이어 지정
    {
        m_slot.SetPlayer(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Store")) //상점 에어리어에 들어갔을경우
        {
            m_storeUI.ActiveStoreUI(other.name,m_player);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Store"))
        {
            m_storeUI.CloseStore();
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (m_player.GetHPValue() >= 0.95)
            {
                UGUIManager.Instance.SystemMessageItem("HealPack");
                return;
            }
            m_slot.UseQuickSlotITem(1, "HealPack");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_slot.UseQuickSlotITem(2, "Barricade");
        }
    }
    void GetUIPos()
    {
        m_statusUI = UGUIManager.Instance.GetStatusUI();
        m_storeUI = UGUIManager.Instance.GetStoreUI();
    }
    void Awake()    
    {
        TableItemData.Instance.Load();
        m_player = GetComponent<PlayerController>();
        m_weaponmanager = GetComponent<GunManager>();
        m_armormanager = GetComponent<ArmorManager>();
        m_collider = GetComponent<CapsuleCollider>();
        GetUIPos();
        SetPlayer(m_player);
       // m_playerMoney = 10000;//테스트용
    }
    #endregion
}
