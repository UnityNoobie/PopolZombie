using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static Gun;

public class GunManager : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    GameObject[] Weapons;
    [SerializeField]
    string m_weaponType; //���� �������� ���⸦ ǥ���ϱ� ����.]]
    PlayerGetItem m_getitem;
    [SerializeField]
    PlayerShooter shooter;  //���� �������� ����Ÿ�� üũ��.
    [SerializeField]
    PlayerStriker striker;
    PlayerController m_player;
    public bool isMelee;
    float weaponChangeTime = 0.3f;
    int testweapon = 1;
    int currentWeaponId;
    public static Transform currentWeapon;
    public static bool isGun;
    public static PlayerAnimController m_animCtr;
    public static bool isChange = false;
    Dictionary<string, GameObject> weaponDic = new Dictionary<string, GameObject>(); //������ ������Ʈ ������ Dictionary���� ����.      
    #endregion

    #region property
    public TableGunstat m_gunStatus { get; set; }                                                                          
    public WeaponData m_weapondata { get; set; }
    #endregion

    #region Coroutine
    public IEnumerator ChangeWeaponRoutine(WeaponData data) //���⺯���� �ڷ�ƾ���� ����
    {
        currentWeaponId = data.ID;
        if (data.AtkType.Equals("Gun")) //������ ����Ÿ���� �ѱ��� 
        {
            isGun = true;
            if (striker.enabled) //�������ݽ�ũ��Ʈ ���������� ���ֱ�.
            {
                striker.enabled = false;
            }
            if (!shooter.enabled)
            {
                shooter.enabled = true;  //���Ͱ� ���������� ���ֱ�
            }
            PlayerShooter.isActive = false; //���� ���۸�í
            isChange = true; //���� �����Ҷ� �ٽ� ���� ���ϴ� ������
            m_animCtr.Play("PutWeapon"); //�ִϸ��̼� ����
            yield return new WaitForSeconds(weaponChangeTime);
            m_animCtr.Play("GrabWeapon");
            GunChange(data);
            UGUIManager.Instance.GetStatusUI().SetSlot(data.ID, data.Image, ArmorType.Max, ItemType.Weapon);
            yield return new WaitForSeconds(weaponChangeTime);
            isChange = false;
            PlayerShooter.isActive = true; // ������ �� �����ڿ� ����ǵ���
        }
        else if (data.AtkType.Equals("Melee"))
        {

            isGun = false;
            if (shooter.enabled) //���Ͱ� ���������� ���ֱ�
            {
                shooter.enabled = false;
            }
            if (!striker.enabled)
            {
                striker.enabled = true; //��Ʈ����Ŀ�� ���������� ���ֱ�
            }
            if (m_animCtr.GetMotion.Equals(PlayerAnimController.Motion.Combo1) || m_animCtr.GetMotion.Equals(PlayerAnimController.Motion.Combo2))
            {
                m_player.AnimEvnet_MeleeFinished(); 
            }
            PlayerStriker.isActive = false;
            isChange = true;
            m_animCtr.Play("MeleeArm");
            yield return new WaitForSeconds(weaponChangeTime);
            MeleeChange(data);
            UGUIManager.Instance.GetStatusUI().SetSlot(data.ID, data.Image, ArmorType.Max, ItemType.Weapon);
            yield return new WaitForSeconds(weaponChangeTime);
            m_animCtr.Play("MeleeIdle");
            isChange = false;
            PlayerStriker.isActive = true;
            m_player.SetStatus(data.ID);
        }
        UIManager.Instance.WeaponImage(data.Image);
    }
    #endregion

    #region Methods
   
    public bool IsGun() //������ ���°� ������ �������� ��ȯ
    {
        return isGun;
    }
    public int GetWeaponId() //���� �������� ������ ID�� ��ȯ
    {
        return currentWeaponId;
    }

    public void SkillUpSignal() //��ų ������ �������� �� �������ͽ� ������ ���� ȣ��
    {
        if (shooter.enabled)
        {
            shooter.CheckSkillInfo();
        }
    }
    
    public void ChangeWeapon(int id) //���⺯�� ��ƾ ����
        {
            StartCoroutine(ChangeWeaponRoutine(m_weapondata.GetWeaponStatus(id)));
        } //ID�� ���̽��� ���� �����ϴ� �޼ҵ�
    public static AttackType AttackProcess(MonsterController mon, float Pdamage, float criper, float cridam,float armorpierce, out float damage) // �⺻ ���ݰ� ġ��Ÿ�� ������ ���� �޼��� damage�� �Ѱ���
        {
            AttackType attackType = AttackType.Normal; //�⺻�����δ� ġ��Ÿ���°� �ƴ� �Ϲ����� �������� ���
            damage = 0f;
            damage = CalculationDamage.NormalDamage(Pdamage, mon.GetStatus.defense,armorpierce,0); //������ ��� ���� ���ݷ°� �Ѿ˿� ���� �ݶ��̴��� ���� ������ ���� ����.
            if (CalculationDamage.CriticalDecision(criper))   //ũ��Ƽ���� ������ Ȯ���ϱ�.
            {
                attackType = AttackType.Critical; //������ Ÿ���� ũ��Ƽ�÷� ����
                damage = CalculationDamage.CriticalDamage(damage, cridam); // ũ��Ƽ���� ���������� ����
            }
            return attackType; // ����Ÿ���� ��ȯ.
        }
    void MeleeChange(WeaponData data) //��������� ����
        {
            striker.ChangeMelee(weaponDic[data.Type],data);
        }
    void GunChange(WeaponData data) //�ѱ�� ����
        {
            shooter.ChangeGun(weaponDic[data.Type],data);
        }
    void SetTransform() //��ǥ����
    {
        m_player = GetComponent<PlayerController>();
        shooter = GetComponent<PlayerShooter>();
        m_animCtr = GetComponent<PlayerAnimController>();
        striker = GetComponent<PlayerStriker>();
        m_getitem = GetComponent<PlayerGetItem>();
        m_gunStatus = new TableGunstat();
        m_weapondata = new WeaponData();

    }
    private void Awake()
        {
        SetTransform();
        }
    private void Start()
        {
            for (int i = 0; i < Weapons.Length; i++) //��ųʸ��� ���� �̸��� ��ü ����.
            {
                weaponDic.Add(Weapons[i].name, Weapons[i]);
            }
            m_getitem.BuyItem(testweapon, m_weapondata.GetWeaponStatus(testweapon).ItemType,0);
        }
    #endregion
}
