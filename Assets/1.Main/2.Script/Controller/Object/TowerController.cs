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

    protected List<GameObject> m_targetList = new List<GameObject>(); //���� ������ Ÿ�� ����Ʈ
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
    protected void FindNearTarget() //���� ����� Ÿ�� Ž���ؼ� �����ִ� ����
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
            foreach(GameObject go in m_targetList)//����Ʈ ���� ǥ���� �׾��� ��츦 �����Ͽ� üũ
            {
                if (go.gameObject.activeSelf)
                {
                    m_activeTargets.Add(go);
                }
            }
            m_targetList = m_activeTargets;
        }

        if (m_targetList.Count > 0)//����Ʈ�� ���� ���� ������ Ʈ��
        {
            m_canFire = true;
            return true;
        }
        m_canFire = false;
        return false; //�ƴϸ� false
    }
    void OnDrawGizmosSelected() //�����Ÿ� üũ�� �����
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
        if (other.gameObject.CompareTag("Zombie")) //�����Ÿ� ������ ������ ����Ʈ���� ����
        {
            m_targetList.Remove(other.gameObject);
        }
    }
    protected void AimAndFire()
    {
        // Gun barrel rotation
        m_barrel.transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime);

        // if can fire turret activates
        if (m_canFire) //���� ������ �����̰�
        {
            if (HasTarget()) //Ÿ���ִ°� �´ٸ�
            {
                FindNearTarget(); //����� ǥ���� �������ְ�

                m_rotationSpeed = m_barrelSpeed; //ȸ�� , ���� ����

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
