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
  
   

    public void ChangeMelee(GameObject newWeapon, WeaponType type, int ID, int grade)  //�׽�Ʈ��
    {
        if (GunManager.currentWeapon != null) //���Ⱑ ����ִ� ���°� �ƴ϶��!
        {
            GunManager.currentWeapon.gameObject.SetActive(false); //���� ��Ƽ�� ���ֱ�
        }
        melee = newWeapon.GetComponent<Melee>(); //���� �޾ƿ� �� ������Ʈ�� ����
        GunManager.currentWeapon = melee.GetComponent<Transform>();
        melee.grade = grade;
        melee.m_type = type;
        melee.gameObject.SetActive(true); //���� �����·� ����
        m_player.SetStatus(ID);// �÷��̾��� �������ͽ��� �缳��

    }
    /*
     private void OnDisable()
     {

         if (GunManager.currentWeapon != null)
         GunManager.currentWeapon.gameObject.SetActive(false); //���� ���� ����
     }

     private void OnEnable()
     {
         GunManager.m_animCtr.SetFloat("MeleeSpeed", m_player.GetStatus.atkSpeed);
         if (GunManager.currentWeapon != null)
         {
             GunManager.currentWeapon.gameObject.SetActive(true); //���� ���� �ѱ�
         }

     }*/
    private void Update()
    {
        if (isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) //���콺 �����Ͱ� UI ���� ���� �� ���� ���ϰ� ����� ���. �ٸ� ���� NGUI������ ������ �ȵ�.
                {
                    m_player.SetAttack();
                }
            }
        }
    }
}
