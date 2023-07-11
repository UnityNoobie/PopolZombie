using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    #region Constants and Fields
    protected Transform m_target;
    protected Transform m_baseRoation;
    protected Transform m_gunBody;
    protected Transform m_barrel;
    protected ParticleSystem m_effect;
    protected LineRenderer m_renderer;

    protected List<GameObject> m_targetList = new List<GameObject>(); //공격 가능한 타겟 리스트
    protected List<GameObject> m_activeTargets = new List<GameObject>();
    protected float m_rotationSpeed;
    protected float m_barrelSpeed;
    protected float m_fireRange;
    protected bool m_canFire;

    #endregion

    #region Methods
    public void SetTower(float roatation, float barrel, float range)
    {
        m_rotationSpeed = roatation;
        m_barrelSpeed = barrel;
        m_fireRange = range;


        this.GetComponent<SphereCollider>().radius = m_fireRange;
    }
    protected void FindNearTarget() //가장 가까운 타겟 탐색해서 정해주는 로직
    {
        if (HasTarget())
        {
            m_target = m_targetList[0].transform;
            foreach (GameObject target in m_targetList) //
            {
                if (Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(m_target.transform.position, transform.position))
                {
                    m_target = target.transform;
                }
            }
        }
    }
    protected bool HasTarget()
    {
        m_activeTargets.Clear();
        if(m_targetList.Count > 0)
        {
            foreach(GameObject go in m_targetList)//리스트 안의 표적이 죽었을 경우를 생각하여 체크
            {
                if (go.gameObject.activeSelf)
                {
                    m_activeTargets.Add(go);
                }
            }
            m_targetList = m_activeTargets;
        }

        if (m_targetList.Count > 0)//리스트에 적이 남아 있으면 트루
        {
            m_canFire = true;
            return true;
        }
        m_canFire = false;
        return false; //아니면 false
    }
    void OnDrawGizmosSelected() //사정거리 체크용 기즈모
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_fireRange);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            m_targetList.Add(other.gameObject);
        }
    }
    // Stop firing
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie")) //사정거리 밖으로 나가면 리스트에서 제거
        {
            m_targetList.Remove(other.gameObject);
        }
    }
    protected void AimAndFire()
    {
        // Gun barrel rotation
        m_barrel.transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime);

        // if can fire turret activates
        if (m_canFire) //공격 가능한 상태이고
        {
            if (HasTarget()) //타겟있는게 맞다면
            {
                FindNearTarget(); //가까운 표적을 지정해주고

                m_rotationSpeed = m_barrelSpeed; //회전 , 조준 시작

                Vector3 baseTargetPostition = new Vector3(m_target.position.x, this.transform.position.y, m_target.position.z);
                Vector3 gunBodyTargetPostition = new Vector3(m_target.position.x, m_target.position.y, m_target.position.z);

                m_baseRoation.transform.LookAt(baseTargetPostition);
                m_gunBody.transform.LookAt(gunBodyTargetPostition);

                // start particle system 
                if (!m_effect.isPlaying)
                {
                    m_effect.Play();
                }
            }
            
        }
        else
        {
            m_rotationSpeed = Mathf.Lerp(m_rotationSpeed, 0, 10 * Time.deltaTime);

            if (m_effect.isPlaying)
            {
                m_effect.Stop();
            }
        }
    }
    protected void Update()
    {
        AimAndFire();
    }

    #endregion
}
