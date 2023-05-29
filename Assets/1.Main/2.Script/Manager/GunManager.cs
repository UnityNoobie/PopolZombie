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
    string m_weaponType; //현재 장착중인 무기를 표시하기 위함.]]
    PlayerGetItem m_getitem;
    [SerializeField]
    PlayerShooter shooter;  //현재 적용중인 무기타입 체크용.
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
    Dictionary<string, GameObject> weaponDic = new Dictionary<string, GameObject>(); //무기의 정보를 Dictionary에서 관리.      
    #endregion

    #region property
    public TableGunstat m_gunStatus { get; set; }                                                                          
    public WeaponData m_weapondata { get; set; }
    #endregion

    #region Coroutine
    public IEnumerator ChangeWeaponRoutine(string name, WeaponType type, int ID, int grade, string atkType, string image, StatusUI statusui)
    {
        // Debug.Log(image);
        if (atkType.Equals("Gun")) //무기의 공격타입이 총기라면 
        {
            isGun = true;
            if (striker.enabled) //근접공격스크립트 켜져있으면 꺼주기.
            {
                striker.enabled = false;
            }
            if (!shooter.enabled)
            {
                shooter.enabled = true;  //슈터가 꺼져있으면 켜주기
            }
            PlayerShooter.isActive = false; //슈터 동작멈챠
            isChange = true; //무기 변경할때 다시 변경 못하는 식으로
            m_animCtr.Play("PutWeapon"); //애니메이션 실행
            yield return new WaitForSeconds(weaponChangeTime);
            m_animCtr.Play("GrabWeapon");
            GunChange(name, type, ID, grade);
            statusui.SetSlot(ID, image, ArmorType.Max, ItemType.Weapon);
            yield return new WaitForSeconds(weaponChangeTime);
            isChange = false;
            PlayerShooter.isActive = true; // 재장전 다 끝난뒤에 실행되도록
        }
        else if (atkType.Equals("Melee"))
        {

            isGun = false;
            if (shooter.enabled) //슈터가 켜져있으면 꺼주기
            {
                shooter.enabled = false;
            }
            if (!striker.enabled)
            {
                striker.enabled = true; //스트라이커가 꺼져있으면 켜주기
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
        } //ID값 베이스로 무기 변경하는 메소드
        public static AttackType AttackProcess(MonsterController mon, float Pdamage, float criper, float cridam,float armorpierce, out float damage) // 기본 공격과 치명타의 구분을 위한 메서드 damage를 넘겨줌
        {
            AttackType attackType = AttackType.Normal; //기본적으로는 치명타상태가 아닌 일반적인 공격으로 계산
            damage = 0f;
            damage = CalculationDamage.NormalDamage(Pdamage, mon.GetStatus.defense,armorpierce); //공격의 경우 총의 공격력과 총알에 맞은 콜라이더의 방어력 정보를 얻어와 적용.
            if (CalculationDamage.CriticalDecision(criper))   //크리티컬의 유무를 확인하기.
            {
                attackType = AttackType.Critical; //공격의 타입을 크리티컬로 지정
                damage = CalculationDamage.CriticalDamage(damage, cridam); // 크리티컬의 데미지값을 얻어옴
            }
            return attackType; // 공격타입을 반환.
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
            TableGunstat.Instance.Load(); //종 테이블 정보를 로드해서 미리 가져다 두기.
            m_player = GetComponent<PlayerController>();
            shooter = GetComponent<PlayerShooter>();
            m_animCtr = GetComponent<PlayerAnimController>();
            striker = GetComponent<PlayerStriker>();
            m_gunStatus = new TableGunstat();
            m_weapondata = new WeaponData();
        }

        private void Start()
        {
            for (int i = 0; i < Weapons.Length; i++) //딕셔너리에 총의 이름과 개체 적용.
            {
                weaponDic.Add(Weapons[i].name, Weapons[i]);
            }
            m_getitem = GetComponent<PlayerGetItem>();
            m_getitem.BuyItem(testweapon, m_weapondata.GetWeaponStatus(testweapon).ItemType,0);
        }
    #endregion
}
