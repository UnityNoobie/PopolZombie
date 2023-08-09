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
    public MeleeState meleeState { get; set; } //���� ���� ��Ȳ
    public static bool isActive = true;
    float m_lastAttackTime;
    public void ChangeMelee(GameObject newWeapon, WeaponData data)  //���� ���� �޼ҵ�
    {
        m_data = new WeaponData();
        m_data = data;
        if (GunManager.currentWeapon != null) //���Ⱑ ����ִ� ���°� �ƴ϶��!
        {
            GunManager.currentWeapon.gameObject.SetActive(false); //���� ��Ƽ�� ���ֱ�
        }
        melee = newWeapon.GetComponent<Melee>(); //���⸦ �޾ƿ� ������Ʈ�� ����
        GunManager.currentWeapon = melee.GetComponent<Transform>();
        melee.grade = m_data.Grade;
        melee.m_type = m_data.weaponType;
        melee.gameObject.SetActive(true); //���� �����·� ����
        m_player.SetStatus(m_data.ID);// �÷��̾��� �������ͽ��� �缳��
    }
    #endregion
}
