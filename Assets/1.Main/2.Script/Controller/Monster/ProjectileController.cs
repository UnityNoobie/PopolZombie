using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    MonsterController m_atkMon; 
    PlayerController m_hitPlayer;
    

    [SerializeField]
    ProjectileController[] m_child;
    float damageValue;
    IEnumerator Coroutine_FollowTarget(GameObject target) //대상의 위치를 추적하여 계속해서 위치 이동해서 실행하는 코루틴
    {
        while(target.activeSelf)
        {
            gameObject.transform.position = target.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false); 
    }
    public void SetProjectile(MonsterController mon, float value)  // 투사체를 생성하며 공격자, 데미지의 벨류(스킬별 다단히트수가 다르기 때문에) 지정
    {
        m_atkMon = mon;
        damageValue = value;
    }
    public void SetProjectileWithChild(MonsterController mon, float value) //스킬패턴때문에 추가 ㅇ여러개의 파티클시스템을 자식오브젝트 형태로 가져와 사용
    {
        m_atkMon = mon;
        damageValue= value;
        for(int i = 0; i < m_child.Length; i++)
        {
            m_child[i].gameObject.GetComponent<ProjectileController>().SetProjectile(mon,value);
        }
    }
    public void SetFollowProjectile(MonsterController mon, float value) //보스몬스터 패턴용 메소드 코루틴으로 대상의 위치를 따라가면서 실행
    {
        m_atkMon = mon;
        damageValue = value;
        StartCoroutine(Coroutine_FollowTarget(m_atkMon.gameObject));
    }
    private void OnParticleCollision(GameObject other) //투사체가 명중했을 시 실행
    {
        if (other.CompareTag("Player"))
        {
            m_hitPlayer = other.GetComponent<PlayerController>();
            m_hitPlayer.GetDamage(m_atkMon.GetStatus.damage * damageValue);
        }
        else
        {
            if (other.GetComponent<IDamageAbleObject>() != null)
            {
                IDamageAbleObject target = other.GetComponent<IDamageAbleObject>();
                target.SetDamage(m_atkMon.GetStatus.damage * damageValue);
            }
        }
       
    }
}
