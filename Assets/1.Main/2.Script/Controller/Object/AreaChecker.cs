using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChecker : MonoBehaviour //포탑의 공격 범위를 확인하고 대상을 넣어주는 클래스
{
    TowerController m_tower;
    public void SetTower(TowerController tower) //정보를 전달해줄 포탑 설정.
    {
        m_tower = tower;
    }
    void OnTriggerEnter(Collider other) //범위안에 Zombie 태그를 가지고 있는 오브젝트가 있다면 추가
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            m_tower.AddTargetList(other.GetComponent<MonsterController>());
        }
    }
    // Stop firing
    void OnTriggerExit(Collider other) //밖으로 나가면 제거
    {
        if (other.gameObject.CompareTag("Zombie")) //사정거리 밖으로 나가면 리스트에서 제거
        {
            m_tower.RemoveTargetList(other.GetComponent<MonsterController>());
        }
    }
}
