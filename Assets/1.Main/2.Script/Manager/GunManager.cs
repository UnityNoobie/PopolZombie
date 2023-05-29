using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static Gun;

public class GunManager : MonoBehaviour
{
    #region Constants and Fields
    public enum CurrentSlot
    {
        Main,
        Sub
    }
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
    public static Transform currentWeapon;
    public static bool isGun;
    public static PlayerAnimController m_animCtr;
    public static bool isChange = false;
    Dictionary<string, GameObject> weaponDic = new Dictionary<string, GameObject>(); //������ ������ Dictionary���� ����.      
    #endregion

    #region property
    public TableGunstat m_gunStatus { get; set; }                                                                          
    public WeaponData m_weapondata { get; set; }
    #endregion

    #region Coroutine
    public IEnumerator ChangeWeaponRoutine(string name, WeaponType type, int ID, int grade, string atkType, string image, StatusUI statusui)
    {
        // Debug.Log(image);
        if (atkType.Equals("Gun")) //������ ����Ÿ���� �ѱ��� 
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
            GunChange(name, type, ID, grade);
            statusui.SetSlot(ID, image, ArmorType.Max, ItemType.Weapon);
            yield return new WaitForSeconds(weaponChangeTime);
            isChange = false;
            PlayerShooter.isActive = true; // ������ �� �����ڿ� ����ǵ���
        }
        else if (atkType.Equals("Melee"))
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
            MeleeChange(name, type, ID, grade);
            yield return new WaitForSeconds(weaponChangeTime);
            m_animCtr.Play("MeleeIdle");
            isChange = false;
            PlayerStriker.isActive = true;
            m_player.SetStatus(ID);
        }
        UIManager.Instance.WeaponImage(image);
    }
    #endregion

    #region Methods
    public void SkillUpSignal()
    {
        if (shooter.enabled)
        {
            shooter.CheckSkillSignal();
        }
    }
    

        public void ChangeWeapon(int id,StatusUI statusui)
        {
            StartCoroutine(ChangeWeaponRoutine(m_weapondata.GetWeaponStatus(id).Type, m_weapondata.GetWeaponStatus(id).weaponType, id, m_weapondata.GetWeaponStatus(id).Grade, m_weapondata.GetWeaponStatus(id).AtkType, m_weapondata.GetWeaponStatus(id).Image,statusui));
        } //ID�� ���̽��� ���� �����ϴ� �޼ҵ�
        public static AttackType AttackProcess(MonsterController mon, float Pdamage, float criper, float cridam,float armorpierce, out float damage) // �⺻ ���ݰ� ġ��Ÿ�� ������ ���� �޼��� damage�� �Ѱ���
        {
            AttackType attackType = AttackType.Normal; //�⺻�����δ� ġ��Ÿ���°� �ƴ� �Ϲ����� �������� ���
            damage = 0f;
            damage = CalculationDamage.NormalDamage(Pdamage, mon.GetStatus.defense,armorpierce); //������ ��� ���� ���ݷ°� �Ѿ˿� ���� �ݶ��̴��� ���� ������ ���� ����.
            if (CalculationDamage.CriticalDecision(criper))   //ũ��Ƽ���� ������ Ȯ���ϱ�.
            {
                attackType = AttackType.Critical; //������ Ÿ���� ũ��Ƽ�÷� ����
                damage = CalculationDamage.CriticalDamage(damage, cridam); // ũ��Ƽ���� ���������� ����
            }
            return attackType; // ����Ÿ���� ��ȯ.
        }
        void MeleeChange(string Type, WeaponType type, int ID, int grade)
        {
            striker.ChangeMelee(weaponDic[Type], type, ID, grade);
        }
        void GunChange(string Type, WeaponType type, int ID, int grade)
        {
            shooter.ChangeGun(weaponDic[Type], type, ID, grade);
        }
        private void Awake()
        {
            TableGunstat.Instance.Load(); //�� ���̺� ������ �ε��ؼ� �̸� ������ �α�.
            m_player = GetComponent<PlayerController>();
            shooter = GetComponent<PlayerShooter>();
            m_animCtr = GetComponent<PlayerAnimController>();
            striker = GetComponent<PlayerStriker>();
            m_gunStatus = new TableGunstat();
            m_weapondata = new WeaponData();
        }

        private void Start()
        {
            for (int i = 0; i < Weapons.Length; i++) //��ųʸ��� ���� �̸��� ��ü ����.
            {
                weaponDic.Add(Weapons[i].name, Weapons[i]);
            }
            m_getitem = GetComponent<PlayerGetItem>();
            m_getitem.BuyItem(testweapon, m_weapondata.GetWeaponStatus(testweapon).ItemType,0);
        }
    #endregion
}
