using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Gun;

public class PlayerShooter : MonoBehaviour
{
    #region Constants and Fields
    public Gun gun;
    public Transform m_gunpos;
    public Transform leftMount;
    public Transform rightMount;
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    Transform m_PistolleftPos;
    [SerializeField]
    Transform m_SMGLeftPos;
    Transform m_leftMount;
    [SerializeField]
    Transform m_Pivot;
    Animator m_anim;


    public static bool isActive = true;
    #endregion

    #region Property
    public WeaponType m_type { get; set; }
    #endregion

    #region Methods
    public void ChangeGun(GameObject newWeapon, WeaponData data)   //게임오브젝트와 무기 정보를 받아와 총기 변경 진행.
    {
        if (GunManager.currentWeapon != null) //무기가 비어있는 상태가 아니라면
        {
          GunManager.currentWeapon.gameObject.SetActive(false); //무기 엑티브 꺼주기
        }
        if (data.weaponType.Equals(WeaponType.Pistol))     //무기가 권총, 기관단총일 경우 IK 애니메이션 적용하기 위해 포지션 설정.
        {
            m_leftMount = m_PistolleftPos;
        }
        else if (data.weaponType.Equals(WeaponType.SubMGun))
        {
            m_leftMount = m_SMGLeftPos;
        }
         gun = newWeapon.GetComponent<Gun>(); //총을 받아온 총 오브젝트로 변경
         GunManager.currentWeapon = gun.GetComponent<Transform>();
         m_player.SetStatus(data.ID);// 플레이어의 스테이터스를 재설정
         gun.SetGun(data);
         m_player.SetGun(gun);
         CheckSkillInfo();   
         gun.gameObject.SetActive(true); //총을 사용상태로 변경
    }
    public void CheckSkillInfo() //스킬정보 확인
    {
        gun.isfirst = true;
        gun.ResetBoolin();
        gun.CheckBoolin();
    }
    public void AttackProcess() //공격프로세스
    {
        gun.Fire();
    }
    public void ReloadProcess() //장전프로세스
    {
        gun.Reload();
    }
    private void Start()
    {
        m_anim = GetComponent<Animator>();
        m_player = GetComponent<PlayerController>();
        if (GunManager.currentWeapon != null)
            GunManager.currentWeapon = gun.GetComponent<Transform>();
    }
    private void OnAnimatorIK() //무기 타입별 IK애니메이션 조절
    {
        if(gun!= null&&(gun.gunstate.Equals(GunState.Ready)&&( gun.m_type == WeaponType.Pistol || gun.m_type == WeaponType.SubMGun)))
        {
            m_anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            m_anim.SetIKPosition(AvatarIKGoal.LeftHand, m_leftMount.position);
            m_anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            m_anim.SetIKPosition(AvatarIKGoal.RightHand, m_Pivot.position);
        }
    }
    #endregion
}
