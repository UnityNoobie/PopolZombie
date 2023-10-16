using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

enum TowerState
{
    Alive,
    Destroyed,
    Max
}
public class TowerController : BuildableObject //빌더블 오브젝트 상속받는 포탑 오브젝트
{

    #region Constants and Fields

    [SerializeField]
    float m_barrelSpeed;
    [SerializeField]
    float m_rotationSpeed;
    [SerializeField]
    Transform m_baseRoation;
    [SerializeField]
    Transform m_gunBody;
    [SerializeField]
    Transform m_barrel;
    [SerializeField]
    ParticleSystem m_effect;
    [SerializeField]
    LineRenderer m_renderer;
    [SerializeField]
    Transform m_firePos;
    [SerializeField]
    List<MonsterController> m_targetList = new List<MonsterController>(); //공격 가능한 타겟 리스트
    GameObject m_target;
    float lastFireTime;
    const string m_fireSound = "SFX_MGShot";

    int m_machineLearning = 0;
    TowerState m_state { get; set; }

    #endregion
    IEnumerator ShootEffect(Vector3 hitPos) //공격 이펙트 재생
    {
        SoundManager.Instance.PlaySFX(m_fireSound, m_audio);
        m_renderer.SetPosition(0, m_firePos.position);  //레이 시작점
        m_renderer.SetPosition(1, hitPos); //레이 도착점
        m_renderer.enabled = true;
        //0.03초간 라인렌더러를 켯다꺼 총알흔적 남기기.
        yield return new WaitForSeconds(0.03f);
        m_renderer.enabled = false;
    }
    #region Methods
    protected override void Destroyed() //체력이 떨어져 파괴되었을 경우
    {
        base.Destroyed(); 
        m_state = TowerState.Destroyed;
        StopAllCoroutines();
        Invoke("DestroyGameObject", 1f); // 오브젝트 풀링해주기
    }
    protected override void DestroyGameObject() //활성화 종료 후 풀에 다시 넣어주기
    {
        base.DestroyGameObject();
        ObjectManager.Instance.SetGunTower(this); //풀에 넣기
    }
    public void BuildTurretObject(Vector3 buildPos,TableSkillStat skill,ObjectStat stat) //오브젝트 설치
    {
        transform.position = buildPos;
        gameObject.SetActive(true);
        GameManager.Instance.SetGameObject(gameObject);
        InitStatus(skill,stat);
    }
    public override void InitStatus(TableSkillStat skill, ObjectStat stat) //스탯 설정해주기
    {
        SetTransform();
        base.InitStatus(skill,stat);
        m_attackArea = GetComponentInChildren<AreaChecker>();
        m_attackArea.SetTower(this);
        m_attackArea.GetComponent<SphereCollider>().radius = m_stat.Range;
        m_state = TowerState.Alive;
        SetBarrelSpeed(m_stat.FireRate);
        m_renderer.positionCount = 2;
    }
    void SetBarrelSpeed(float fireRate) //총열 회전 속도 조절해주기
    {
        m_barrelSpeed = m_stat.FireRate * 100;
    }
    bool HasTarget()//공격 가능한 타겟 리스트가 있는지 확인해주는 메소드.
    {
        if (m_targetList.Count > 0)
        {
            List<MonsterController> m_activeTargets = new List<MonsterController>();
            foreach (MonsterController go in m_targetList)//리스트 안의 표적이 죽었을 경우를 생각하여 체크해줌.
            {
                if (go.IsAliveObject()) //타겟이 살아 있다면 activeTargetList에 추가해주어 targetList를 변경.
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
    void FindNearTarget() // 타겟 리스트에 있는 적들 중 가장 가까운 타겟 탐색하여 전달.
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
    void OnDrawGizmosSelected() //사정거리 체크용 기즈모
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_stat.Range);
    }
    /*
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
    }*/ //AreaChecker로 기능 이동
    public void AddTargetList(MonsterController mon) //타겟 리스트 추가
    {
        m_targetList.Add(mon);
    }
    public void RemoveTargetList(MonsterController mon) //타겟 리스트 삭제
    {
        m_targetList.Remove(mon);
    }
    public void MachineLearning() //특성 효과 적용
    {
        m_machineLearning++;
        if(m_machineLearning >= 50)
        {
            m_machineLearning = 50;
        }
    }
   
    public void AimAndFire() // 조준과 발사 담당
    {
        if (m_state != TowerState.Alive) return; //현재 상태가 생존이 아닐 때는 리턴
       // 총열 회전속도
        m_barrel.transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime);

        if (HasTarget()) //타겟이 사정거리 내에 있다면
        {
             
             m_rotationSpeed = m_barrelSpeed; //회전 , 조준 시작

             Vector3 baseTargetPostition = new Vector3(m_target.transform.position.x, transform.position.y, m_target.transform.position.z);
             Vector3 gunBodyTargetPostition = new Vector3(m_target.transform.position.x, m_target.transform.position.y, m_target.transform.position.z);

             m_baseRoation.transform.LookAt(baseTargetPostition);
             m_gunBody.transform.LookAt(gunBodyTargetPostition);

             if(Time.time >= lastFireTime + (1 / m_stat.FireRate))
             {
                FindNearTarget(); //가까운 표적을 지정해주고
                lastFireTime = Time.time;
                RaycastHit hit;
                Vector3 hitPos = Vector3.zero;
                Vector3 shotFire = m_firePos.transform.forward;
                var layer = (1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Monster")); //레이의 충돌은 배경, 몬스터로 제한
                if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_stat.Range,layer)) //시작지점, 방향, 충돌정보, 사정거리 
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
                    hitPos = m_firePos.position + shotFire * m_stat.Range;
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
        GunManager.AttackProcess(mon, m_stat.Damage + m_machineLearning , m_stat.CriRate, m_stat.CriRate, m_stat.ArmorPierce, out damage);
        mon.SetDamage(AttackType.Normal, damage, null, false, this);
        hitPos = hit.point;
        var hiteffect = TableEffect.Instance.m_tableData[4].Prefab[2];
        var effect = EffectPool.Instance.Create(hiteffect);
        effect.transform.position = hitPos;
        effect.SetActive(true);
        return hitPos;

    }
    #endregion
}
