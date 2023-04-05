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
    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }
    public void SetItem(string image)
    {
        m_icon.mainTexture = ImageLoader.Instance.GetImage(image).texture;
        count++;
        m_lable.text = count.ToString();
        isNull = false;
    }
    public void SetWeapon(string image)
    {
        m_icon.mainTexture = ImageLoader.Instance.GetImage(image).texture;
        m_lable.text = image;
        isNull = false;
    }
    public void AddItem()
    {
        count++;
        m_lable.text = count.ToString();
    }
    public void UseItem(string item)
    {
        if (count <= 0)
        {
            UIManager.Instance.SystemMessageItem(item);
            return;
        }
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
    void Setheal(int heal)
    {
        m_player.GetHeal(heal);
    }
    private void Start()
    {
        isNull = true;
    }

    public void SetSlot(string name,int num)
    {
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
