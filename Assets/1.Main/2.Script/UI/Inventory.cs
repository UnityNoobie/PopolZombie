using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GunManager;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject m_slotPrefab;
    [SerializeField]
    UIGrid m_slotGrid;
    const int SLOT_MAX = 9;
    const int COLUMN_COUNT = 3;
    [SerializeField]
    UILabel m_Lv_Nick;
    [SerializeField]
    UILabel m_hp;
    [SerializeField]
    UILabel m_def;
    [SerializeField]
    UILabel m_atk;
    [SerializeField]
    UILabel m_criRate;
    [SerializeField]
    UILabel m_criDam;
    [SerializeField]
    UILabel m_speed;
    [SerializeField]
    UILabel m_atkSpeed;
    [SerializeField]
    UILabel m_atkDist;
    [SerializeField]
    UILabel m_KnockBackDist;
    [SerializeField]
    UILabel m_KnockBackRate;
    [SerializeField]
    UILabel m_mainWeapontype;
    [SerializeField]
    UILabel m_subWeaponType;
    [SerializeField]
     UITexture m_mainImage;
    [SerializeField]
    UITexture m_subImage;
    [SerializeField]
    UIManager m_manager;
    [SerializeField]
    UIButton m_exitButton;
    [SerializeField]
    UIButton dragButton;
    [SerializeField]
    UITexture m_helmet;
    [SerializeField]
    UITexture m_armor;
    [SerializeField]
    UITexture m_glove;
    [SerializeField]
    UITexture m_pants;
    [SerializeField]
    UITexture m_boots;

    PlayerController m_player;

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
        //�����ɱ��
    }
    public void GetStatusInfo(PlayerController player)   //�÷��̾��� ���������� ��񺯰� ���� ȿ�� �߻� �� �ҷ�����.
    {
        m_Lv_Nick.text = ("LV30 Hunter");
        m_hp.text = ("ü�� : "+player.GetStatus.hp + " / " + player.GetStatus.hpMax);
       //if (GunManager.currentWeapon !=null && GunManager.currentWeapon.GetComponent<Gun>().m_type.Equals(WeaponType.ShotGun)&& GunManager.isGun == true)
          //  m_atk.text = ("Damage : " + player.GetStatus.damage + " * " + player.GetStatus.ShotGun);
        m_atk.text = ("���ݷ� : " + player.GetStatus.damage);
        m_def.text = ("���� : " + player.GetStatus.defense);
        m_criRate.text = ("ũ��Ȯ�� : " + player.GetStatus.criRate);
        m_criDam.text = ("ũ�������� : " + player.GetStatus.criAttack);
        m_speed.text = ("�̵��ӵ� : "+ player.GetStatus.speed);
        m_atkSpeed.text = ("���ݼӵ�  : " + player.GetStatus.atkSpeed);
        m_atkDist.text = ("��Ÿ� : " + player.GetStatus.AtkDist);
        m_KnockBackDist.text = ("�˹�Ÿ� : " + player.GetStatus.KnockBackDist);
        m_KnockBackRate.text = ("�˹�Ȯ�� : " + player.GetStatus.KnockBackPer);
    }
    public void SetArmorImage(string Image,ArmorType type) //�� ��ü�� �̹��� �����ϴ� ���.
    {
        switch (type)
        { 
            case ArmorType.Helmet:
                m_helmet.mainTexture = ImageLoader.Instance.GetImage(Image).texture;
                break;
            case ArmorType.Armor:
                m_armor.mainTexture = ImageLoader.Instance.GetImage(Image).texture;
                break;
            case ArmorType.Glove:
                m_glove.mainTexture = ImageLoader.Instance.GetImage(Image).texture;
                break;
            case ArmorType.Pants: 
                m_pants.mainTexture = ImageLoader.Instance.GetImage(Image).texture;
                break;
            case ArmorType.Boots:
                m_boots.mainTexture = ImageLoader.Instance.GetImage(Image).texture;
                break;
        }
    }
    public void OffArmorImage(UITexture tex) //�� ������ ���ִ±�� ��������
    {
        tex.mainTexture = null;
    }
    public void SetWeaponInfo(string text) //�κ��丮 ���� �������� ���
    {
       m_mainWeapontype.text = text;
    }
    public void WeaponImage(string name) //�κ��丮 ���� �����̹���
    {
        m_mainImage.mainTexture = ImageLoader.Instance.GetImage(name).texture;
    }


}
