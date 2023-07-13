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
    protected List<MonsterController> m_targetList = new List<MonsterController>(); //공격 가능한 타겟 리스트
    
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
        m_renderer.SetPosition(0, m_firePos.position);  //레이 시작점
        m_renderer.SetPosition(1, hitPos); //레이 도착점
        m_renderer.enabled = true;
        //0.03초간 라인렌더러를 켯다꺼 총알흔적 남기기.
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
   
    protected void FindNearTarget() //가장 가까운 타겟 탐색
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
            foreach (MonsterController go in m_targetList)//리스트 안의 표적이 죽었을 경우를 생각하여 체크
            {
                if (go.IsAliveObject())
                {
                    m_activeTargets.Add(go);
                }
            }
            m_targetList = m_activeTargets;
        }
        if (m_targetList.Count > 0)//리스트에 적이 남아 있으면 트루
        {
            return true;
        }
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
            m_targetList.Add(other.GetComponent<MonsterController>());
        }
    }
    // Stop firing
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie")) //사정거리 밖으로 나가면 리스트에서 제거
        {
            m_targetList.Remove(other.GetComponent<MonsterController>());
        }
    }
    protected void AimAndFire()
    {
       // 총열 회전속도
        m_barrel.transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime);

        if (HasTarget()) //타겟이 있다면
        {
             FindNearTarget(); //가까운 표적을 지정해주고
             m_rotationSpeed = m_barrelSpeed; //회전 , 조준 시작

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
                if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_fireRange)) //시작지점, 방향, 충돌정보, 사정거리 
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
    Vector3 AttackProcess(RaycastHit hit) //공격 시 좀비와의 상호작용하기위한 프로세스 모음
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
