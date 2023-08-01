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
public class TowerController : BuildableObject 
{
    #region Constants and Fields

    [SerializeField]
    protected float m_barrelSpeed;
    [SerializeField]
    protected float m_rotationSpeed;
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
    protected float lastFireTime;


    int m_machineLearning = 0;
    TowerState m_state;

    #endregion
    IEnumerator ShootEffect(Vector3 hitPos)
    {
        SoundManager.Instance.PlaySFX("SFX_MGShot", m_audio);
        m_renderer.SetPosition(0, m_firePos.position);  //���� ������
        m_renderer.SetPosition(1, hitPos); //���� ������
        m_renderer.enabled = true;
        //0.03�ʰ� ���η������� �ִٲ� �Ѿ����� �����.
        yield return new WaitForSeconds(0.03f);
        m_renderer.enabled = false;
    }
    #region Methods



    public void BuildTurretObject(Vector3 buildPos,TableSkillStat skill,ObjectStat stat)
    {
        transform.position = buildPos;
        gameObject.SetActive(true);
        GameManager.Instance.SetGameObject(gameObject);
        InitStatus(skill,stat);
    }
    public override void InitStatus(TableSkillStat skill, ObjectStat stat)
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
    void SetBarrelSpeed(float fireRate)
    {
        m_barrelSpeed = m_stat.FireRate * 100;
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
    public void AddTargetList(MonsterController mon)
    {
        m_targetList.Add(mon);
    }
    public void RemoveTargetList(MonsterController mon)
    {
        m_targetList.Remove(mon);
    }
    public void MachineLearning()
    {
        m_machineLearning++;
        if(m_machineLearning >= 50)
        {
            m_machineLearning = 50;
        }
    }
    protected override void Destroyed()
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

             if(Time.time >= lastFireTime + (1 / m_stat.FireRate))
             {
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
    protected void Update()
    {
        if(m_state.Equals(TowerState.Alive)) //���� �����϶��� �۵�
            AimAndFire();
    }

    #endregion
}
