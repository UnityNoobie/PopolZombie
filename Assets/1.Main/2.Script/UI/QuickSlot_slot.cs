using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot_slot : MonoBehaviour
{
    [SerializeField]
    UITexture m_icon;
    [SerializeField]
    UILabel m_lable;
    PlayerController m_player;
    bool isNull;
    int count = 0;
    string itemName = "";
    public void SetPlayer(PlayerController player) //플레이어 정보 설정
    {
        m_player = player;
    }
    public void SetItem(string image) //아이템 정보 받아오고 갯수 증가
    {
        m_icon.mainTexture = ImageLoader.Instance.GetImage(image).texture;
        count++;
        m_lable.text = count.ToString();
        isNull = false;
    }
    public void SetWeapon(string image) // 무기정보 받기 카운트증가 x
    {
        m_icon.mainTexture = ImageLoader.Instance.GetImage(image).texture;
        m_lable.text = image;
        isNull = false;
    }
    public void AddItem() // 아이템 수량 증가
    {
        count++;
        m_lable.text = count.ToString();
    }
    public bool HaveEnoughItem() //충분한 갯수를 가지고 있는지 반환
    {
        if(count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void UseItem(string item) //아이템 사용
    {
        if (item.Equals("HealPack")) Setheal(30);
        count--;
        m_lable.text = count.ToString();
        if (count <= 0)
        {
            isNull = true;
            m_icon.mainTexture = null;
            m_lable.text = null;
        }
    }
    void Setheal(int heal) //플레이어 회복
    {
        m_player.GetHeal(heal);
    }
    private void Start()
    {
        isNull = true;
    }

    public void SetSlot(string name,int num) //슬롯 설정
    {
        itemName = name;
        if (num == 0)
        {
            SetWeapon(name);
        }
        else if (isNull == true)
        {
            SetItem(name);
        }
        else
        {
            AddItem();
        }
    }

}
