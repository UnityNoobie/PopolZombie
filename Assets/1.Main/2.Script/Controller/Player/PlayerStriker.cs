using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStriker : MonoBehaviour
{
    [SerializeField]
    PlayerController m_player;

    
    Melee melee;
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
    
}
