using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    MonsterController m_monCtr ;
    PlayerController m_playerCtr;
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
    public void SetProjectile(MonsterController mon,float value)
    {
        m_monCtr = mon;
        damageValue= value;
    }
    public void SetProjectileWithChild(MonsterController mon, float value) //스킬패턴때문에 추가
    {
        m_monCtr = mon;
        damageValue= value;
        for(int i = 0; i < m_child.Length; i++)
        {
            m_child[i].gameObject.GetComponent<ProjectileController>().SetProjectile(mon,value);
        }
    }
    public void SetFollowProjectile(MonsterController mon, float value)
    {
        m_monCtr = mon;
        damageValue = value;
        StartCoroutine(Coroutine_FollowTarget(m_monCtr.gameObject));
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            m_playerCtr = other.GetComponent<PlayerController>();
            float damage = CalculationDamage.NormalDamage(m_monCtr.GetStatus.damage * damageValue, m_playerCtr.pDefence); //성공적★
            m_playerCtr.GetDamage(damage);
           // Debug.Log("투사체 명중!" + damage + "데미지!");
        }
    }
    private void Update()
    {
        
    }
}
