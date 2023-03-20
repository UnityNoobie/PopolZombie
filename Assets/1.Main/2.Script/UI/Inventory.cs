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
        //과연될까용
    }
    public void GetStatusInfo(PlayerController player)   //플레이어의 스탯정보를 장비변경 등의 효과 발생 시 불러와줌.
    {
        m_Lv_Nick.text = ("LV30 Hunter");
        m_hp.text = ("체력 : "+player.GetStatus.hp + " / " + player.GetStatus.hpMax);
       //if (GunManager.currentWeapon !=null && GunManager.currentWeapon.GetComponent<Gun>().m_type.Equals(WeaponType.ShotGun)&& GunManager.isGun == true)
          //  m_atk.text = ("Damage : " + player.GetStatus.damage + " * " + player.GetStatus.ShotGun);
        m_atk.text = ("공격력 : " + player.GetStatus.damage);
        m_def.text = ("방어력 : " + player.GetStatus.defense);
        m_criRate.text = ("크리확률 : " + player.GetStatus.criRate);
        m_criDam.text = ("크리데미지 : " + player.GetStatus.criAttack);
        m_speed.text = ("이동속도 : "+ player.GetStatus.speed);
        m_atkSpeed.text = ("공격속도  : " + player.GetStatus.atkSpeed);
        m_atkDist.text = ("사거리 : " + player.GetStatus.AtkDist);
        m_KnockBackDist.text = ("넉백거리 : " + player.GetStatus.KnockBackDist);
        m_KnockBackRate.text = ("넉백확률 : " + player.GetStatus.KnockBackPer);
    }
    public void SetArmorImage(string Image,ArmorType type) //방어구 교체시 이미지 저장하는 기능.
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
    public void OffArmorImage(UITexture tex) //방어구 해제시 꺼주는기능 구현예정
    {
        tex.mainTexture = null;
    }
    public void SetWeaponInfo(string text) //인벤토리 내에 무기정보 출력
    {
       m_mainWeapontype.text = text;
    }
    public void WeaponImage(string name) //인벤토리 내의 무기이미지
    {
        m_mainImage.mainTexture = ImageLoader.Instance.GetImage(name).texture;
    }


}
