using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStriker : MonoBehaviour
{
    [SerializeField]
    PlayerController m_player;

    
    public Melee melee;
    public enum MeleeState
    {
        Ready,
        Attack,
        Dead
    }

    public MeleeState meleeState { get; set; }
    public static bool isActive = true;
    float m_lastAttackTime;
  
   

    public void ChangeMelee(GameObject newWeapon, WeaponType type, int ID, int grade)  //테스트용
    {
        if (GunManager.currentWeapon != null) //무기가 비어있는 상태가 아니라면!
        {
            GunManager.currentWeapon.gameObject.SetActive(false); //무기 엑티브 꺼주기
        }
        melee = newWeapon.GetComponent<Melee>(); //총을 받아온 총 오브젝트로 변경
        GunManager.currentWeapon = melee.GetComponent<Transform>();
        melee.grade = grade;
        melee.m_type = type;
        melee.gameObject.SetActive(true); //총을 사용상태로 변경
        m_player.SetStatus(ID);// 플레이어의 스테이터스를 재설정

    }
    /*
     private void OnDisable()
     {

         if (GunManager.currentWeapon != null)
         GunManager.currentWeapon.gameObject.SetActive(false); //현재 무기 끄기
     }

     private void OnEnable()
     {
         GunManager.m_animCtr.SetFloat("MeleeSpeed", m_player.GetStatus.atkSpeed);
         if (GunManager.currentWeapon != null)
         {
             GunManager.currentWeapon.gameObject.SetActive(true); //현재 무기 켜기
         }

     }*/
    private void Update()
    {
        if (isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) //마우스 포인터가 UI 위에 있을 시 공격 안하게 만드는 기능. 다만 현재 NGUI에서는 적용이 안됨.
                {
                    m_player.SetAttack();
                }
            }
        }
    }
}
