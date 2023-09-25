using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Gun;
using static PlayerStriker;

public class Melee : MonoBehaviour
{
   
    [SerializeField]
    PlayerController m_player;
    public MeleeState meleeState { get; set; }
    public WeaponType m_type { get; set; }
    public int grade;
    void OnEnable()
    {
        UGUIManager.Instance.GetScreenHUD().SetWeaponInfo(m_type + ".LV" + grade);
        //UIManager.Instance.WeaponInfoUI(m_type + ".LV" + grade); // ±¸¹öÀü
        meleeState = MeleeState.Ready;
    }
}


