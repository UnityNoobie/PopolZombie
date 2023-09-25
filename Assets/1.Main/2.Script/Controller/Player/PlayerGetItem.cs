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
    public bool HaveEnoughMoney(int price) //�÷��̾ ����� ��ȭ�� ������ �ִ��� ��ȯ���ִ� �޼ҵ�
    {
        if (price <= m_playerMoney)
        {
            return true;
        }
        return false;
    }
    #endregion
    void MoneyChange(int money) //�÷��̾� ��ȭ ����
    {
        m_playerMoney += money;
        UGUIManager.Instance.GetScreenHUD().SetMoney(m_playerMoney);
       // UIManager.Instance.MoneyUI(m_playerMoney); //������ NGUI
    }
    public void GetMoney(int money) //��ȭ ȹ�� �� ȣ��Ǿ� ��ȭ���� �޼ҵ� ����
    {
        MoneyChange(money);
    }
    public void BuyItem(int Id, string itemtype,int price) //������ ���� ���� �� ȣ��Ǿ� ������ Ÿ�Կ� ���� ���� �޼ҵ� ȣ��.
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
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "�� ������ ���ڶ��ϴ�.");
                return false;
            }
        }
        else if (type.Equals(PlayerItemType.Barricade))
        {
            if (m_barricadeCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "�� ������ ���ڶ��ϴ�.");
                return false;
            }
        }
        else if (type.Equals(PlayerItemType.Turret))
        {
            if (m_turretCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "�� ������ ���ڶ��ϴ�.");
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
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "�� ������ ���ڶ��ϴ�.");
                return;
            }
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(1, m_healpackCount, "HealPack");
            m_healpackCount--;
        }
        else if (type.Equals(PlayerItemType.Barricade))
        {
            if (m_barricadeCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "�� ������ ���ڶ��ϴ�.");
                return;
            }
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(2, m_barricadeCount, "Barricade");
            m_barricadeCount--;
        }
        else if (type.Equals(PlayerItemType.Turret))
        {
            if (m_turretCount <= 0)
            {
                UGUIManager.Instance.SystemMessageSendMessage(type.ToString() + "�� ������ ���ڶ��ϴ�.");
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
            //  m_slot.SetItem(1, "HealPack");���� ��� UI
            m_healpackCount++;
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(1, m_healpackCount, "HealPack");
        }
        else if(type.Equals(PlayerItemType.Barricade))
        {
            //  m_slot.SetItem(2, "Barricade");���� ��� UI
            m_barricadeCount++;
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(2, m_barricadeCount, "Barricade");
        }
        else if (type.Equals(PlayerItemType.Turret))
        {
            //  m_slot.SetItem(3, "Turret");���� ��� UI
            m_turretCount++;
            UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(3, m_turretCount, "Turret");
        }
    }
    /*
    void AddHealPack(int Id) //������ Ŭ������ ������ ȹ�� ��ȣ ����
    {
      //  m_slot.SetItem(1, "HealPack");���� ��� UI
        m_healpackCount++;
        UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(1, m_healpackCount, "HealPack");
    }
    void AddBarricade(int Id)//������ Ŭ������ ������ ȹ�� ��ȣ ����
    {
        //  m_slot.SetItem(2, "Barricade");���� ��� UI
        m_barricadeCount++;
        UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(2, m_barricadeCount, "HealPack");
    }
    void AddTurret(int Id)//������ Ŭ������ ������ ȹ�� ��ȣ ����
    {
        //  m_slot.SetItem(3, "Turret");���� ��� UI
        m_turretCount++;
        UGUIManager.Instance.GetScreenHUD().UpdateQuickSlotItem(3, m_turretCount, "HealPack");
    }*/
    void ChangeArmor(int id) //�� ���� Ŭ������ ���� ����.
    {
        m_armormanager.ChangeArmor(id);
    }
    void ChangeWeapon(int Id) //���� ���� Ŭ������ ���� ����
    {
        m_weaponmanager.ChangeWeapon(Id);
    }
    void SetPlayer(PlayerController player) // �÷��̾� ����
    {
        //m_slot.SetPlayer(player);
    }
    public void UsingCheatKey()
    {
        MoneyChange(100000);
    }
    private void OnTriggerEnter(Collider other)//���� ���� ������ ���� �޼ҵ�
    {
        if (other.CompareTag("Store")) //���� ���� �������
        {
            m_storeUI.ActiveStoreUI(other.name,m_player);
        }
    }
    private void OnTriggerExit(Collider other) //���� ���� ������ �����°��� ����
    {
        if (other.CompareTag("Store"))
        {
            m_storeUI.CloseStore();
        }
    }

    void SetTransform() // ��ǥ ����
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
      //  GetMoney(50000);//�׽�Ʈ��
    }
    #endregion
}
