using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    [SerializeField]
    QuickSlot_slot[] slots;
    PlayerController m_player;
    public void SetPlayer(PlayerController player)
    {
        m_player = player;
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].SetPlayer(m_player);
        }
    }
    public void SetItem(int num, string name)
    {
        slots[num].SetSlot(name,num);
    }
    public void UseQuickSlotITem(int num, string name)
    {
        slots[num].UseItem(name);
    }
    public bool CheckItemCount(int num)
    {
       return slots[num].HaveEnoughItem();
    }
}
