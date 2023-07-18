using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChecker : MonoBehaviour
{
    TowerController m_tower;
    public void SetTower(TowerController tower)
    {
        m_tower = tower;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            m_tower.AddTargetList(other.GetComponent<MonsterController>());
        }
    }
    // Stop firing
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie")) //�����Ÿ� ������ ������ ����Ʈ���� ����
        {
            m_tower.RemoveTargetList(other.GetComponent<MonsterController>());
        }
    }
}
