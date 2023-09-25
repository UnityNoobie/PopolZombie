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
    public void ChangeGun(GameObject newWeapon, WeaponData data)   //���ӿ�����Ʈ�� ���� ������ �޾ƿ� �ѱ� ���� ����.
    {
        if (GunManager.currentWeapon != null) //���Ⱑ ����ִ� ���°� �ƴ϶��
        {
          GunManager.currentWeapon.gameObject.SetActive(false); //���� ��Ƽ�� ���ֱ�
        }
        if (data.weaponType.Equals(WeaponType.Pistol))     //���Ⱑ ����, ��������� ��� IK �ִϸ��̼� �����ϱ� ���� ������ ����.
        {
            m_leftMount = m_PistolleftPos;
        }
        else if (data.weaponType.Equals(WeaponType.SubMGun))
        {
            m_leftMount = m_SMGLeftPos;
        }
         gun = newWeapon.GetComponent<Gun>(); //���� �޾ƿ� �� ������Ʈ�� ����
         GunManager.currentWeapon = gun.GetComponent<Transform>();
         m_player.SetStatus(data.ID);// �÷��̾��� �������ͽ��� �缳��
         gun.SetGun(data);
         m_player.SetGun(gun);
         CheckSkillInfo();   
         gun.gameObject.SetActive(true); //���� �����·� ����
    }
    public void CheckSkillInfo() //��ų���� Ȯ��
    {
        gun.isfirst = true;
        gun.ResetBoolin();
        gun.CheckBoolin();
    }
    public void AttackProcess() //�������μ���
    {
        gun.Fire();
    }
    public void ReloadProcess() //�������μ���
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
    private void OnAnimatorIK() //���� Ÿ�Ժ� IK�ִϸ��̼� ����
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
