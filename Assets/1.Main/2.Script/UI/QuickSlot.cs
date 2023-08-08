using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    [SerializeField]
    QuickSlot_slot[] slots;
    PlayerController m_player;
    public void SetPlayer(PlayerController player) //플레이어 정보 설정.
    {
        m_player = player;
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].SetPlayer(m_player);
        }
    }
    public void SetItem(int num, string name) //슬롯에 아이템 지정.
    {
        slots[num].SetSlot(name,num);
    }
    public void UseQuickSlotITem(int num, string name) //슬롯 아이템 사용
    {
        slots[num].UseItem(name);
    }
    public bool CheckItemCount(int num) //아이템의 갯수를 체크해서 충분한지 부족한지 반환
    {
       return slots[num].HaveEnoughItem();
    }
}
