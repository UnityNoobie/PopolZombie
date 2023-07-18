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

    TowerState m_state;

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


    private void Start()
    {
        SetTransform();
        SetTower(200,10,20,5,20,10,50,0);
        GameManager.Instance.SetGameObject(gameObject);
    }
    public override void SetTower(int hp, float damage,float defence, float fireRate, float range, float crirate, float cridam, float armorpierce)
    {
        base.SetTower(hp,damage,defence,fireRate,range,crirate,cridam,armorpierce);
        m_attackArea = GetComponentInChildren<AreaChecker>();
        m_attackArea.SetTower(this);
        m_attackArea.GetComponent<SphereCollider>().radius = m_fireRange;
        m_state = TowerState.Alive;
        SetBarrelSpeed(m_fireRate);
        m_renderer.positionCount = 2;
    }
    void SetBarrelSpeed(float fireRate)
    {
        m_barrelSpeed = m_fireRate * 100;
    }
    void OnDrawGizmosSelected() //�����Ÿ� üũ�� �����
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_fireRange);
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
    }*/
    public void AddTargetList(MonsterController mon)
    {
        m_targetList.Add(mon);
    }
    public void RemoveTargetList(MonsterController mon)
    {
        m_targetList.Remove(mon);
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

             if(Time.time >= lastFireTime + (1 / m_fireRate))
             {
                lastFireTime = Time.time;
                RaycastHit hit;
                Vector3 hitPos = Vector3.zero;
                Vector3 shotFire = m_firePos.transform.forward;
                var layer = (1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Zombie")); //������ �浹�� ���, ���ͷ� ����
                if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_fireRange,layer)) //��������, ����, �浹����, �����Ÿ� 
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
        if(m_state.Equals(TowerState.Alive)) //���� �����϶��� �۵�
            AimAndFire();
    }

    #endregion
}