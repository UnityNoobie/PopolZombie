using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Gun : MonoBehaviour
{

    public enum GunState
    {
        Ready,
        Empty,
        Reload,
        Dead,
        Idle
    }
    public GunState gunstate { get; set; }
    
    [SerializeField]
    ParticleSystem m_flashEffect;
    [SerializeField]
    ParticleSystem m_ShellEffect;
    LineRenderer m_bulletRender;
    AudioSource m_gunAudio;
    [SerializeField]
    AudioClip m_shootSound;
    [SerializeField]
    AudioClip m_reloadSound;
    [SerializeField]
    PlayerController m_player;
    Transform m_firePos;
    public int grade;
    public WeaponType m_type { get; set; }
    public int ammoRemain;
    public bool isReload = false;
    float lastFireTime;
    public WeaponData GetStatus { get; set; }
    bool isfirst = true;
    IEnumerator ShootEffect(Vector3 hitPos)
    {     
        m_flashEffect.Play();  //ȭ������Ʈ
        m_ShellEffect.Play(); //ź�� ����Ʈ
        m_gunAudio.PlayOneShot(m_shootSound); //�ѼҸ�
        m_bulletRender.SetPosition(0, m_firePos.position);  //���� ������
        m_bulletRender.SetPosition(1, hitPos); //���� ������
        m_bulletRender.enabled = true;
        //0.03�ʰ� ���η������� �ִٲ� �Ѿ����� �����.
        yield return new WaitForSeconds(0.03f);
        m_bulletRender.enabled = false;
    }
    IEnumerator ShotGunReload()
    {
        while (gunstate == GunState.Reload) //���� ���°� ������ �ϰ�� ��� ����
        {
            if (ammoRemain > 0 && ammoRemain >= m_player.GetStatus.maxammo || ammoRemain > 0 && Input.GetMouseButton(0)) // �ִ� �Ѿ˿� �����߰ų� ���콺 Ŭ���� ���� ��� �غ���·� ����
            {
                GunManager.m_animCtr.Play("Idle");
                yield return new WaitForSeconds(0.1f);
                gunstate = GunState.Ready;
                yield break;
            }
            GunManager.m_animCtr.Play("ShotReload"); //�������� �ִϸ��̼� ����
            yield return new WaitForSeconds(m_player.GetStatus.reloadTime); //���� �����ð����� ���
            m_gunAudio.PlayOneShot(m_reloadSound);
            ammoRemain++;
            ammoCheck();
        }
    }
    IEnumerator ReloadRoutine() //���ε� ��ƾ 
    {
        float reloadTime = m_player.GetStatus.reloadTime;
        isReload = true;
        gunstate = GunState.Reload;
        if(m_type == WeaponType.ShotGun) //���� ������ ������ ���
        {
            StartCoroutine(ShotGunReload());
        }
        else
        {
            m_gunAudio.PlayOneShot(m_reloadSound);   //���� ���¸� Reload�� �ٲپ� �߻縦 ���� �ð��� ���� �� �Ѿ��� ü���ְ� �غ���·� ����
            float speed = 2 / reloadTime;
            GunManager.m_animCtr.SetFloat("ReloadTime", speed);
            GunManager.m_animCtr.Play("Reload");
            yield return new WaitForSeconds(reloadTime);
            ammoRemain = m_player.GetStatus.maxammo;
            ammoCheck();
            gunstate = GunState.Ready;
        }
      
    }

    public void ammoCheck() //�����Ѿ��� �ǽð����� Ȯ��.
    {
        UIManager.Instance.WeaponInfoUI(m_type + ".LV" + grade + "\n" + ammoRemain + " / " + m_player.GetStatus.maxammo);
    }
    void Start()
    {
        m_gunAudio = GetComponent<AudioSource>();
        m_bulletRender = GetComponent<LineRenderer>();
        m_bulletRender.positionCount = 2;
        m_bulletRender.enabled = false;
        m_firePos = Utill.GetChildObject(gameObject, "Dummy_Firepos");
    }
  
    private void OnEnable() // ������� �Ѿ��� �ִ�ġ�� �ϰ� ���� ������ �ҷ���
    {
        /*
        if (isfirst) //���ʷ� ����ÿ��� ���� ���� �� ��ź�� �ִ�� �غ��ڱ�?
        {
            ammoRemain = m_player.GetStatus.maxammo;
            isfirst= false;
        }*/
        ammoRemain = m_player.GetStatus.maxammo; //��ó�� �õ��� �Ͽ������� ������ ����� �������� ����Ұ��̰� ��ü�ϴ� ������ ��� ��������, �ٴڿ����ݱ� �� ���ο� �� ��� ���� ������
        gunstate = GunState.Ready; //�׳� �⺻��� ��������.
        ammoCheck();
        lastFireTime = 0;
    }

    public void Fire()
    {
        if (gunstate == GunState.Ready && Time.time >= lastFireTime + 1/ m_player.GetStatus.atkSpeed) //���� ���°� Ready�̰� �߻�ӵ��� �غ�Ǿ��� ���.
        { 
            lastFireTime= Time.time;
            Shot(); //����!!!!
        }
    }
    void Shot()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        Vector3 shotFire = Vector3.zero;
        float damage = 0f;
        shotFire = m_player.transform.forward;
        if (m_type == WeaponType.ShotGun) //������ ��� Ư���� �۵������ ������ ������ �̷��� �սô�.
        {
            for (int i = 0; i < m_player.GetStatus.ShotGun; i++)
            {
                shotFire = m_player.transform.forward;
                  float verti = Random.Range(-0.3f, 0.3f);
                  shotFire.x += verti;
                
                if (Physics.Raycast(m_firePos.position,shotFire, out hit, m_player.GetStatus.AtkDist)) //��������, ����, �浹����, �����Ÿ� 
                {
                    if (hit.collider.CompareTag("Background"))
                    {
                        hitPos = hit.point;
                    }
                    else if (hit.collider.CompareTag("Zombie"))
                    {
                        var mon = hit.collider.GetComponent<MonsterController>();
                        var type = GunManager.AttackProcess(mon, m_player.GetStatus.damage,m_player.GetStatus.criRate,m_player.GetStatus.criAttack, out damage);
                        mon.SetDamage(type, damage,m_player);
                        hitPos = hit.point;
                        var hiteffect = TableEffect.Instance.m_tableData[4].Prefab[2];
                        var effect = EffectPool.Instance.Create(hiteffect);
                        effect.transform.position = hitPos;
                        effect.SetActive(true);
                    }
                    else
                    {
                        hitPos = m_firePos.position + shotFire * m_player.GetStatus.AtkDist;
                    }
                }
                if (hit.collider == null)
                {
                    hitPos = m_firePos.position + shotFire * m_player.GetStatus.AtkDist;
                }
               
                Debug.DrawRay(m_firePos.position, shotFire * m_player.GetStatus.AtkDist, Color.yellow, 0.1f);
            }
            m_flashEffect.Play();  //ȭ������Ʈ
            m_ShellEffect.Play(); //ź�� ����Ʈ
            m_gunAudio.PlayOneShot(m_shootSound); //�ѼҸ� 
        }
        else //���ǿ��� ����
        {
           
            if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_player.GetStatus.AtkDist)) //��������, ����, �浹����, �����Ÿ� 
            {
                if (hit.collider.CompareTag("Background"))
                {
                    hitPos = hit.point;
                }
                else if (hit.collider.CompareTag("Zombie"))
                {
                    var mon = hit.collider.GetComponent<MonsterController>();                                                        
                    var type = GunManager.AttackProcess(mon, m_player.GetStatus.damage, m_player.GetStatus.criRate, m_player.GetStatus.criAttack, out damage);
                    mon.SetDamage(type, damage,m_player);
                    hitPos = hit.point;
                    var hiteffect = TableEffect.Instance.m_tableData[4].Prefab[2];
                    var effect = EffectPool.Instance.Create(hiteffect);
                    effect.transform.position = hitPos;
                    effect.SetActive(true);
                }
                else
                {
                    hitPos = m_firePos.position + shotFire * m_player.GetStatus.AtkDist;
                }
            }
            if (hit.collider == null)
            {
                hitPos = m_firePos.position + shotFire * m_player.GetStatus.AtkDist;
            }
            StartCoroutine(ShootEffect(hitPos));
        }
        ammoRemain--;
        ammoCheck();
        if (ammoRemain <= 0)
        {
            gunstate = GunState.Empty;
            Reload();
        }
    }
    public bool Reload() //������
    {
        if (gunstate == GunState.Reload || ammoRemain >= m_player.GetStatus.maxammo)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

   
  
}
