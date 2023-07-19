using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour
{
    PlayerController m_player;
    TowerController m_turret;
    Barricade m_barricade;


    private void Start()
    {
        m_player = GetComponent<PlayerController>();
    }
}
