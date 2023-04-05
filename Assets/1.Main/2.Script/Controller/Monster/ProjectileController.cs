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
    IEnumerator Coroutine_FollowTarget(GameObject target)
    {
        while(target.activeSelf)
        {
            gameObject.transform.position = target.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false); 
    }
    public void SetProjectile(MonsterController mon, float value)
    {
        m_atkMon = mon;
        damageValue = value;
    }
    public void SetProjectileWithChild(MonsterController mon, float value) //스킬패턴때문에 추가
    {
        m_atkMon = mon;
        damageValue= value;
        for(int i = 0; i < m_child.Length; i++)
        {
            m_child[i].gameObject.GetComponent<ProjectileController>().SetProjectile(mon,value);
        }
    }
    public void SetFollowProjectile(MonsterController mon, float value)
    {
        m_atkMon = mon;
        damageValue = value;
        StartCoroutine(Coroutine_FollowTarget(m_atkMon.gameObject));
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            m_hitPlayer = other.GetComponent<PlayerController>();
            float damage = CalculationDamage.NormalDamage(m_atkMon.GetStatus.damage * damageValue, m_hitPlayer.pDefence, 0f); //성공적★
            m_hitPlayer.GetDamage(damage);
           // Debug.Log("투사체 명중!" + damage + "데미지!");
        }
    }
}
