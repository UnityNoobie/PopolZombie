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
        UIManager.Instance.MoneyUI(m_playerMoney);
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
    void AddHealPack(int Id) //������ Ŭ������ ������ ȹ�� ��ȣ ����
    {
        m_slot.SetItem(1, "HealPack");
    }
    void AddBarricade(int Id)//������ Ŭ������ ������ ȹ�� ��ȣ ����
    {
        m_slot.SetItem(2, "Barricade");
    }
    void AddTurret(int Id)//������ Ŭ������ ������ ȹ�� ��ȣ ����
    {
        m_slot.SetItem(3, "Turret");
    }
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
        m_slot.SetPlayer(player);
    }
    public void UsingCheatKey()
    {
        m_playerMoney = 100000;
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
      //  GetMoney(50000);//�׽�Ʈ��
    }
    #endregion
}
