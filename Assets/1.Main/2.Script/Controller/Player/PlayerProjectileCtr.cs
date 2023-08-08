using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileCtr : MonoBehaviour //플레이어의 투사체 관리
{
    MonsterController m_hitmon;
    PlayerController m_atkPlayer;
    List<GameObject> m_hitLists;
    public void SetPlayerProjectile(PlayerController player) //투사체에 플레이어 정보 전달.
    {
        m_hitLists = new List<GameObject>();
        m_atkPlayer = player;
    }

    private void OnParticleCollision(GameObject other) //파티클시스템의 Collision 에서 충돌 판정.
    {
        if (other.CompareTag("Zombie"))
        {
            float damage = 0;
            m_hitmon = other.GetComponent<MonsterController>();
            Status status = m_atkPlayer.GetStatus;
            var type = GunManager.AttackProcess(m_hitmon, status.damage, status.criRate, status.criAttack, status.ArmorPierce, out damage);
            m_hitmon.SetDamage(type, damage, m_atkPlayer, false,m_atkPlayer);
        }
    }
}
