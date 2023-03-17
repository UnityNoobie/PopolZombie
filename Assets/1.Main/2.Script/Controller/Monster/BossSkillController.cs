using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillController : MonoBehaviour
{
    BossController m_boss;
    PlayerController m_playerCtr;
 


    public void SetProjectile(BossController boss)
    {
        m_boss = boss;
    }
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("���⵵�ȵ�?");
            m_playerCtr = other.GetComponent<PlayerController>();
            float damage = CalculationDamage.NormalDamage(m_boss.GetStatus.damage * 2, m_playerCtr.pDefence); //��������
            m_playerCtr.GetDamage(damage);
            Debug.Log(damage);
        }
    }

    


}
