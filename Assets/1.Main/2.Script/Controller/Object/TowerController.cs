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
public class TowerController : BuildableObject //������ ������Ʈ ��ӹ޴� ��ž ������Ʈ
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
    List<MonsterController> m_targetList = new List<MonsterController>(); //���� ������ Ÿ�� ����Ʈ
    GameObject m_target;
    float lastFireTime;
    const string m_fireSound = "SFX_MGShot";

    int m_machineLearning = 0;
    TowerState m_state { get; set; }

    #endregion
    IEnumerator ShootEffect(Vector3 hitPos) //���� ����Ʈ ���
    {
        SoundManager.Instance.PlaySFX(m_fireSound, m_audio);
        m_renderer.SetPosition(0, m_firePos.position);  //���� ������
        m_renderer.SetPosition(1, hitPos); //���� ������
        m_renderer.enabled = true;
        //0.03�ʰ� ���η������� �ִٲ� �Ѿ����� �����.
        yield return new WaitForSeconds(0.03f);
        m_renderer.enabled = false;
    }
    #region Methods
    protected override void Destroyed() //ü���� ������ �ı��Ǿ��� ���
    {
        base.Destroyed(); 
        m_state = TowerState.Destroyed;
        StopAllCoroutines();
        Invoke("DestroyGameObject", 1f); // ������Ʈ Ǯ�����ֱ�
    }
    protected override void DestroyGameObject() //Ȱ��ȭ ���� �� Ǯ�� �ٽ� �־��ֱ�
    {
        base.DestroyGameObject();
        ObjectManager.Instance.SetGunTower(this); //Ǯ�� �ֱ�
    }
    public void BuildTurretObject(Vector3 buildPos,TableSkillStat skill,ObjectStat stat) //������Ʈ ��ġ
    {
        transform.position = buildPos;
        gameObject.SetActive(true);
        GameManager.Instance.SetGameObject(gameObject);
        InitStatus(skill,stat);
    }
    public override void InitStatus(TableSkillStat skill, ObjectStat stat) //���� �������ֱ�
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
    void SetBarrelSpeed(float fireRate) //�ѿ� ȸ�� �ӵ� �������ֱ�
    {
        m_barrelSpeed = m_stat.FireRate * 100;
    }
    bool HasTarget()//���� ������ Ÿ�� ����Ʈ�� �ִ��� Ȯ�����ִ� �޼ҵ�.
    {
        if (m_targetList.Count > 0)
        {
            List<MonsterController> m_activeTargets = new List<MonsterController>();
            foreach (MonsterController go in m_targetList)//����Ʈ ���� ǥ���� �׾��� ��츦 �����Ͽ� üũ����.
            {
                if (go.IsAliveObject()) //Ÿ���� ��� �ִٸ� activeTargetList�� �߰����־� targetList�� ����.
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
    void FindNearTarget() // Ÿ�� ����Ʈ�� �ִ� ���� �� ���� ����� Ÿ�� Ž���Ͽ� ����.
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
    void OnDrawGizmosSelected() //�����Ÿ� üũ�� �����
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
        if (other.gameObject.CompareTag("Zombie")) //�����Ÿ� ������ ������ ����Ʈ���� ����
        {
            m_targetList.Remove(other.GetComponent<MonsterController>());
        }
    }*/ //AreaChecker�� ��� �̵�
    public void AddTargetList(MonsterController mon) //Ÿ�� ����Ʈ �߰�
    {
        m_targetList.Add(mon);
    }
    public void RemoveTargetList(MonsterController mon) //Ÿ�� ����Ʈ ����
    {
        m_targetList.Remove(mon);
    }
    public void MachineLearning() //Ư�� ȿ�� ����
    {
        m_machineLearning++;
        if(m_machineLearning >= 50)
        {
            m_machineLearning = 50;
        }
    }
   
    public void AimAndFire() // ���ذ� �߻� ���
    {
        if (m_state != TowerState.Alive) return; //���� ���°� ������ �ƴ� ���� ����
       // �ѿ� ȸ���ӵ�
        m_barrel.transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime);

        if (HasTarget()) //Ÿ���� �����Ÿ� ���� �ִٸ�
        {
             
             m_rotationSpeed = m_barrelSpeed; //ȸ�� , ���� ����

             Vector3 baseTargetPostition = new Vector3(m_target.transform.position.x, transform.position.y, m_target.transform.position.z);
             Vector3 gunBodyTargetPostition = new Vector3(m_target.transform.position.x, m_target.transform.position.y, m_target.transform.position.z);

             m_baseRoation.transform.LookAt(baseTargetPostition);
             m_gunBody.transform.LookAt(gunBodyTargetPostition);

             if(Time.time >= lastFireTime + (1 / m_stat.FireRate))
             {
                FindNearTarget(); //����� ǥ���� �������ְ�
                lastFireTime = Time.time;
                RaycastHit hit;
                Vector3 hitPos = Vector3.zero;
                Vector3 shotFire = m_firePos.transform.forward;
                var layer = (1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Monster")); //������ �浹�� ���, ���ͷ� ����
                if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_stat.Range,layer)) //��������, ����, �浹����, �����Ÿ� 
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
    Vector3 AttackProcess(RaycastHit hit) //���� �� ������� ��ȣ�ۿ��ϱ����� ���μ��� ����
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
