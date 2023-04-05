using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileCtr : MonoBehaviour
{
    MonsterController m_hitmon;
    PlayerController m_atkPlayer;
    List<GameObject> m_hitLists;
   // float atkdamage;
    public void SetPlayerProjectile(PlayerController player)
    {
        m_hitLists = new List<GameObject>();
        m_atkPlayer = player;
    }

    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("파티클 시스템 들어옴 1 : " + other.name);
        if (other.CompareTag("Zombie"))
        {
            //Debug.Log("파티클 시스템 들어옴 2 : " + other.name);
            float damage = 0;
            m_hitmon = other.GetComponent<MonsterController>();
            var type = GunManager.AttackProcess(m_hitmon, m_atkPlayer.GetStatus.damage, m_atkPlayer.GetStatus.criRate, m_atkPlayer.GetStatus.criAttack,m_atkPlayer.GetStatus.ArmorPierce, out damage);
            m_hitmon.SetDamage(type, damage, m_atkPlayer, false);
            Debug.Log("데미지는 : " + damage);
        }
    }
}
