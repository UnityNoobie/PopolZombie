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
        UIManager.Instance.WeaponInfoUI(m_type + ".LV" + grade);
        meleeState = MeleeState.Ready;
    }
}


