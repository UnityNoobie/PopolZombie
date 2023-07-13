using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TowerController : MonoBehaviour ,IDamageAbleObject
{
    #region Constants and Fields
    [SerializeField]
    protected GameObject m_target;
    [SerializeField]
    protected Transform m_baseRoation;
    [SerializeField]
    protected Transform m_gunBody;
    [SerializeField]
    protected Transform m_barrel;
    [SerializeField]
    protected ParticleSystem m_effect;
    [SerializeField]
    protected LineRenderer m_renderer;
    [SerializeField]
    protected Transform m_firePos;
    [SerializeField]
    protected List<MonsterController> m_targetList = new List<MonsterController>(); //���� ������ Ÿ�� ����Ʈ
    
    [SerializeField]
    protected float m_rotationSpeed;
    [SerializeField]
    protected float m_barrelSpeed;
    [SerializeField]
    protected float m_fireRange;
    [SerializeField]
    protected float m_fireRate;
    [SerializeField]
    protected float m_damage;
    float lastFireTime;
    protected int m_hp = 1000;
    protected int m_hpMax = 1000;
    protected float m_criRate = 10;
    protected float m_criDamage = 50;
    protected float m_armorPierce = 0;
    #endregion
    IEnumerator ShootEffect(Vector3 hitPos)
    {
        m_renderer.SetPosition(0, m_firePos.position);  //���� ������
        m_renderer.SetPosition(1, hitPos); //���� ������
        m_renderer.enabled = true;
        //0.03�ʰ� ���η������� �ִٲ� �Ѿ����� �����.
        yield return new WaitForSeconds(0.03f);
        m_renderer.enabled = false;
    }
    #region Methods

    public void SetDamage(float damage)
    {
        m_hp -= (int)damage;
    }
    private void Start()
    {
        SetTower(10,5,1000,20);
    }
    public void SetTower(float damage,float fireRate, float barrel, float range)
    {
        m_damage = damage;
        m_fireRate = fireRate;
        m_barrelSpeed = barrel;
        m_fireRange = range;
        m_renderer.positionCount = 2;

        this.GetComponent<SphereCollider>().radius = m_fireRange;
    }
   
    protected void FindNearTarget() //���� ����� Ÿ�� Ž��
    {
        MonsterController closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (MonsterController target in m_targetList)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }
        m_target = closestTarget.gameObject;
    }

    protected bool HasTarget()
    {
        if(m_targetList.Count > 0)
        {
            List<MonsterController> m_activeTargets = new List<MonsterController>();
            foreach (MonsterController go in m_targetList)//����Ʈ ���� ǥ���� �׾��� ��츦 �����Ͽ� üũ
            {
                if (go.IsAliveObject())
                {
                    m_activeTargets.Add(go);
                }
            }
            m_targetList = m_activeTargets;
        }
        if (m_targetList.Count > 0)//����Ʈ�� ���� ���� ������ Ʈ��
        {
            return true;
        }
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
            m_targetList.Add(other.GetComponent<MonsterController>());
        }
    }
    // Stop firing
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie")) //�����Ÿ� ������ ������ ����Ʈ���� ����
        {
            m_targetList.Remove(other.GetComponent<MonsterController>());
        }
    }
    protected void AimAndFire()
    {
       // �ѿ� ȸ���ӵ�
        m_barrel.transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime);

        if (HasTarget()) //Ÿ���� �ִٸ�
        {
             FindNearTarget(); //����� ǥ���� �������ְ�
             m_rotationSpeed = m_barrelSpeed; //ȸ�� , ���� ����

             Vector3 baseTargetPostition = new Vector3(m_target.transform.position.x, transform.position.y, m_target.transform.position.z);
             Vector3 gunBodyTargetPostition = new Vector3(m_target.transform.position.x, m_target.transform.position.y, m_target.transform.position.z);

             m_baseRoation.transform.LookAt(baseTargetPostition);
             m_gunBody.transform.LookAt(gunBodyTargetPostition);

             if(Time.time >= lastFireTime + (1 / m_fireRate))
             {
                lastFireTime = Time.time;
                RaycastHit hit;
                Vector3 hitPos = Vector3.zero;
                Vector3 shotFire = m_firePos.transform.forward;            
                if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_fireRange)) //��������, ����, �浹����, �����Ÿ� 
                {
                    if (hit.collider.CompareTag("Zombie"))
                    {
                       hitPos = AttackProcess(hit);
                    }
                    else
                    {
                        hitPos = hit.point;
                    }
                }
                if (hit.collider == null)
                {
                    hitPos = m_firePos.position + shotFire * m_fireRange;
                }
                StartCoroutine(ShootEffect(hitPos));
             }
             if (!m_effect.isPlaying)
             {
                 m_effect.Play();   
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
    Vector3 AttackProcess(RaycastHit hit) //���� �� ������� ��ȣ�ۿ��ϱ����� ���μ��� ����
    {
        float damage;
        Vector3 hitPos = Vector3.zero;
        var mon = hit.collider.GetComponent<MonsterController>();
     
        GunManager.AttackProcess(mon, m_damage, m_criRate, m_criDamage, m_armorPierce, out damage);
        mon.SetDamage(AttackType.Normal, damage, null, false);

        hitPos = hit.point;

        var hiteffect = TableEffect.Instance.m_tableData[4].Prefab[2];
        var effect = EffectPool.Instance.Create(hiteffect);
        effect.transform.position = hitPos;
        effect.SetActive(true);
        return hitPos;

    }
    protected void Update()
    {
       AimAndFire();
    }

    #endregion
}
