using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static Gun;

public class GunManager : MonoBehaviour
{
    public enum CurrentSlot
    {
        Main,
        Sub
    }
    [SerializeField]
    GameObject[] Weapons;
    [SerializeField]
    string m_weaponType;
    //���� �������� ���⸦ ǥ���ϱ� ����.]]
    PlayerGetItem m_getitem;
    public PlayerShooter shooter;
    public PlayerStriker striker;
    PlayerController m_player;
    public TableGunstat m_gunStatus { get; set; }
    public bool isMelee;
    float weaponChangeTime = 0.3f;
    int testweapon = 1;
    Dictionary<string, GameObject> weaponDic = new Dictionary<string, GameObject>(); //���� ������ Dictionary���� ����.
                                                                                     // Dictionary<string, GameObject> meleeDic = new Dictionary<string, GameObject>(); //������������ ����
    public WeaponData m_weapondata{ get; set; }
   
    public static Transform currentWeapon;
    public static Transform subWeapon; //�κ��丮 �ý��ۿ��� �ֹ���, �������� �����Ͽ��ϱ�����              ������ �Ҵ� X
    public static bool isGun;
    public static PlayerAnimController m_animCtr;
    public static bool isChange = false;
    public Inventory m_inven;
    public IEnumerator ChangeWeaponRoutine(string name, WeaponType type, int ID, int grade, string atkType, string image)
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
                if(m_animCtr.GetMotion.Equals(PlayerAnimController.Motion.Combo1) || m_animCtr.GetMotion.Equals(PlayerAnimController.Motion.Combo2))
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

        public void ChangeWeapon(int id)
        {
            StartCoroutine(ChangeWeaponRoutine(m_weapondata.GetWeaponStatus(id).Type, m_weapondata.GetWeaponStatus(id).weaponType, id, m_weapondata.GetWeaponStatus(id).Grade, m_weapondata.GetWeaponStatus(id).AtkType, m_weapondata.GetWeaponStatus(id).Image));
        }
        public static AttackType AttackProcess(MonsterController mon, float Pdamage, float criper, float cridam, out float damage) // �⺻ ���ݰ� ġ��Ÿ�� ������ ���� �޼��� damage�� �Ѱ���
        {
            AttackType attackType = AttackType.Normal; //�⺻�����δ� ġ��Ÿ���°� �ƴ� �Ϲ����� �������� ���
            damage = 0f;
            damage = CalculationDamage.NormalDamage(Pdamage, mon.GetStatus.defense); //������ ��� ���� ���ݷ°� �Ѿ˿� ���� �ݶ��̴��� ���� ������ ���� ����.
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
            m_getitem.BuyItem(testweapon, m_weapondata.GetWeaponStatus(testweapon).ItemType);
        }
    /*
        private void Update()
        {
            if (!isChange)
            {

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    testweapon++;
                    if (testweapon > 21) testweapon = 1;
                    m_getitem.BuyItem(testweapon, m_weapondata.GetWeaponStatus(testweapon).ItemType);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    testweapon--;
                    if (testweapon < 1) testweapon = 21;
                    m_getitem.BuyItem(testweapon, m_weapondata.GetWeaponStatus(testweapon).ItemType);
                }
            }
        }*/
}
