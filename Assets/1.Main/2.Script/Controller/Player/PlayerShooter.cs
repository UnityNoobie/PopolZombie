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
    [SerializeField]
    Camera m_Camera;
    Animator m_anim;

  


    public static bool isActive = true;
    #endregion

    #region Property
    public WeaponType m_type { get; set; }
    #endregion

    #region Methods
    public void ChangeGun(GameObject newWeapon,  WeaponType type,int ID, int grade)  //테스트용
    {
        if (GunManager.currentWeapon != null) //무기가 비어있는 상태가 아니라면!
        {
          GunManager.currentWeapon.gameObject.SetActive(false); //무기 엑티브 꺼주기
        }
        if (type.Equals(WeaponType.Pistol))     //무기가 권총, 기관단총일 경우 손 위치 변경하여 모션 자연스럽게
        {
            m_leftMount = m_PistolleftPos;
        }
        else if (type.Equals(WeaponType.SubMGun))
        {
            m_leftMount = m_SMGLeftPos;
        }
         gun = newWeapon.GetComponent<Gun>(); //총을 받아온 총 오브젝트로 변경
         GunManager.currentWeapon = gun.GetComponent<Transform>();
         m_player.SetStatus(ID);// 플레이어의 스테이터스를 재설정
         gun.SetGun(ID,grade,type);
         m_player.SetGun(gun);
         CheckSkillSignal();   
         gun.gameObject.SetActive(true); //총을 사용상태로 변경
    }
    public void CheckSkillSignal()
    {
        gun.isfirst = true;
        gun.ResetBoolin();
        gun.CheckBoolin();
    }
    private void Start()
    {
        m_anim = GetComponent<Animator>();
        m_player = GetComponent<PlayerController>();
        if (GunManager.currentWeapon != null)
            GunManager.currentWeapon = gun.GetComponent<Transform>();
    }
    private void OnAnimatorIK()
    {
        if(gun!= null&&(gun.gunstate.Equals(GunState.Ready)&&( gun.m_type == WeaponType.Pistol || gun.m_type == WeaponType.SubMGun)))
        {
            m_anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            m_anim.SetIKPosition(AvatarIKGoal.LeftHand, m_leftMount.position);
            m_anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            m_anim.SetIKPosition(AvatarIKGoal.RightHand, m_Pivot.position);
        }
    }
        private void Update()
    {
        if (isActive && m_player.m_Pstate != PlayerController.PlayerState.dead)
        {
           // Ray mousepos = m_Camera.ScreenPointToRay(Input.mousePosition);
           // RaycastHit hit;
           
       
            if (Input.GetMouseButtonDown(0))
            {    
                GunManager.m_animCtr.Play("Fire");
            }
            if (Input.GetMouseButton(0))
            {
              if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) //마우스 포인터가 UI 위에 있을 시 공격 안하게 만드는 기능. 다만 현재 NGUI에서는 적용이 안됨.
                {
                    gun.Fire();
                }
                
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (gun.Reload())
                {
                   
                }

            }
        }
       
    }
    #endregion
}
