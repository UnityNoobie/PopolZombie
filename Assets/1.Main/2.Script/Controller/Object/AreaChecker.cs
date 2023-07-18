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
        if (other.gameObject.CompareTag("Zombie")) //사정거리 밖으로 나가면 리스트에서 제거
        {
            m_tower.RemoveTargetList(other.GetComponent<MonsterController>());
        }
    }
}
