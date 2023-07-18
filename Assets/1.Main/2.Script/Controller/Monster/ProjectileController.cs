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
    IEnumerator Coroutine_FollowTarget(GameObject target) //����� ��ġ�� �����Ͽ� ����ؼ� ��ġ �̵��ؼ� �����ϴ� �ڷ�ƾ
    {
        while(target.activeSelf)
        {
            gameObject.transform.position = target.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false); 
    }
    public void SetProjectile(MonsterController mon, float value)  // ����ü�� �����ϸ� ������, �������� ����(��ų�� �ٴ���Ʈ���� �ٸ��� ������) ����
    {
        m_atkMon = mon;
        damageValue = value;
    }
    public void SetProjectileWithChild(MonsterController mon, float value) //��ų���϶����� �߰� ���������� ��ƼŬ�ý����� �ڽĿ�����Ʈ ���·� ������ ���
    {
        m_atkMon = mon;
        damageValue= value;
        for(int i = 0; i < m_child.Length; i++)
        {
            m_child[i].gameObject.GetComponent<ProjectileController>().SetProjectile(mon,value);
        }
    }
    public void SetFollowProjectile(MonsterController mon, float value) //�������� ���Ͽ� �޼ҵ� �ڷ�ƾ���� ����� ��ġ�� ���󰡸鼭 ����
    {
        m_atkMon = mon;
        damageValue = value;
        StartCoroutine(Coroutine_FollowTarget(m_atkMon.gameObject));
    }
    private void OnParticleCollision(GameObject other) //����ü�� �������� �� ����
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
