using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

public class PlayerGetItem : MonoBehaviour
{
    [SerializeField]
    Inventory m_inven;
    [SerializeField]
    QuickSlot m_slot;
    [SerializeField]
    StoreUI m_storeUI;
    GunManager m_weaponmanager;
    ArmorManager m_armormanager;
    PlayerController m_player;
    CapsuleCollider m_collider;
    public WeaponData m_weapondata { get; set; }

    public void BuyItem(int Id, string itemtype)
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
        m_armormanager.ChangeArmor(id);
    }
    void ChangeWeapon(int Id)
    {
        m_weaponmanager.ChangeWeapon(Id);
    }

    void SetPlayer(PlayerController player) // 플레이어 지정
    {
        m_inven.SetPlayer(player);
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
            BuyItem(37, "HealPack");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BuyItem(40, "Barricade");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if(m_player.GetHPValue() >= 0.95)
            {
                UIManager.Instance.SystemMessageCantUse("HealPack");
                return;
            }
            m_slot.UseQuickSlotITem(1, "HealPack");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            m_slot.UseQuickSlotITem(2, "Barricade");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("3렙방어구 장착");
            BuyItem(24, "Armor");
            BuyItem(30, "Armor");
            BuyItem(33, "Armor");
            BuyItem(36, "Armor");
            BuyItem(27, "Armor");

        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("1렙방어구 장착");
            BuyItem(22, "Armor");
            BuyItem(28, "Armor");
            BuyItem(31, "Armor");
            BuyItem(34, "Armor");
            BuyItem(25, "Armor");
        }
    }

    void Awake()    
    {
        TableItemData.Instance.Load();
        m_player = GetComponent<PlayerController>();
        m_weaponmanager = GetComponent<GunManager>();
        m_armormanager = GetComponent<ArmorManager>();
        SetPlayer(m_player);
        m_collider = GetComponent<CapsuleCollider>();
       
    }
}
