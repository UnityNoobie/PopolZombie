using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileCtr : MonoBehaviour //�÷��̾��� ����ü ����
{
    MonsterController m_hitmon;
    PlayerController m_atkPlayer;
    List<GameObject> m_hitLists;
    public void SetPlayerProjectile(PlayerController player) //����ü�� �÷��̾� ���� ����.
    {
        m_hitLists = new List<GameObject>();
        m_atkPlayer = player;
    }

    private void OnParticleCollision(GameObject other) //��ƼŬ�ý����� Collision ���� �浹 ����.
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
