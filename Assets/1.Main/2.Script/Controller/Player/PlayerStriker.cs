using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStriker : MonoBehaviour
{
    [SerializeField]
    PlayerController m_player;
    WeaponData m_data;
    
    Melee melee;
    public enum MeleeState
    {
        Ready,
        Attack,
        Dead
    }
    #region Methods
    public MeleeState meleeState { get; set; } //현재 무기 상황
    public static bool isActive = true;
    float m_lastAttackTime;
    public void ChangeMelee(GameObject newWeapon, WeaponData data)  //무기 변경 메소드
    {
        m_data = new WeaponData();
        m_data = data;
        if (GunManager.currentWeapon != null) //무기가 비어있는 상태가 아니라면!
        {
            GunManager.currentWeapon.gameObject.SetActive(false); //무기 엑티브 꺼주기
        }
        melee = newWeapon.GetComponent<Melee>(); //무기를 받아온 오브젝트로 변경
        GunManager.currentWeapon = melee.GetComponent<Transform>();
        melee.grade = m_data.Grade;
        melee.m_type = m_data.weaponType;
        melee.gameObject.SetActive(true); //총을 사용상태로 변경
        m_player.SetStatus(m_data.ID);// 플레이어의 스테이터스를 재설정
    }
    #endregion
}
