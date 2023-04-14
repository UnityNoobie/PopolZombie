using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
   // public WeaponData GetStatus { get; set; }
    public bool isfirst = true; 
    bool lastfire;
    bool pierce;
    bool burn;
    bool boom;
    //bool remove;

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
    public void ResetBoolin() //���� ��ü, ��ų �� ���� ��Ȳ���� �ʱ�ȭ
    {
        isfirst = false;
        lastfire = false;
        pierce = false;
        burn = false;
        boom = false;
        //remove = false;
        //crush = false;
        //armorpierce = false;
    }
    public void CheckBoolin()
    {
        if(m_player.GetStatus.LastFire != 0)
        {
            lastfire = true;
        }
        if(m_player.GetStatus.Pierce != 0)
        {
            pierce=true;
        }
        if(m_player.GetStatus.Burn != 0)
        {
            burn = true;
        }
       
        if(m_player.GetStatus.Boom != 0)
        {
            boom = true;
        }
        /*
        if(m_player.GetStatus.Remove != 0)
        {
            remove = true;
        }
        
        if(m_player.GetStatus.Crush != 0)
        {
           // crush = true;
        }
        if (m_player.GetStatus.ArmorPierce != 0)
        {
            // armorpierce=true;
        }*/
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
  
    void OnEnable() // ������� �Ѿ��� �ִ�ġ�� �ϰ� ���� ������ �ҷ���
    {
        ammoRemain = m_player.GetStatus.maxammo; //��ó�� �õ��� �Ͽ������� ������ ����� �������� ����Ұ��̰� ��ü�ϴ� ������ ��� ��������, �ٴڿ����ݱ� �� ���ο� �� ��� ���� ������
        gunstate = GunState.Ready; //�׳� �⺻��� ��������.
        ammoCheck();
        lastFireTime = 0;
    }
    #region Gun_Fire_Sequance

    public void Fire()
    {
        if (gunstate == GunState.Ready && Time.time >= lastFireTime + 1 / m_player.GetStatus.atkSpeed) //���� ���°� Ready�̰� �߻�ӵ��� �غ�Ǿ��� ���.
        {
            lastFireTime = Time.time;
            Shot(); //����!!!!
        }
    }
    void Shot()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        Vector3 shotFire = Vector3.zero;
        shotFire = m_player.transform.forward;
        if (m_type == WeaponType.ShotGun) //������ ��� Ư���� �۵������ ������ ������ �̷��� �սô�.
        {
            for (int i = 0; i < m_player.GetStatus.ShotGun; i++)
            {
                shotFire = m_player.transform.forward;
                float verti = Random.Range(-0.3f, 0.3f);
                shotFire.x += verti;
                if (pierce)
                {
                    RaycastHit[] hits = Physics.RaycastAll(m_firePos.position, shotFire, m_player.GetStatus.AtkDist);
                    for (int j = 0; j < hits.Length; j++)
                    {
                        if (hits[j].collider.CompareTag("Zombie"))
                        {
                            AttackProcess(hits[j]);
                        }
                    }
                }
                else
                {
                    if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_player.GetStatus.AtkDist)) //��������, ����, �浹����, �����Ÿ� 
                    {
                        if (hit.collider.CompareTag("Zombie"))
                        {
                            AttackProcess(hit);
                        }
                    }
                   
                }

                Debug.DrawRay(m_firePos.position, shotFire * m_player.GetStatus.AtkDist, Color.yellow, 0.1f);
            }
            m_flashEffect.Play();  //ȭ������Ʈ
            m_ShellEffect.Play(); //ź�� ����Ʈ
            m_gunAudio.PlayOneShot(m_shootSound); //�ѼҸ� 
        }
        else //���ǿ��� ����
        {

            if (pierce)
            {
                RaycastHit[] hits = Physics.RaycastAll(m_firePos.position, shotFire, m_player.GetStatus.AtkDist);
                for (int j = 0; j < hits.Length; j++)
                {
                    if (hits[j].collider.CompareTag("Zombie"))
                    {
                        AttackProcess(hits[j]);
                    }
                }
                hitPos = m_firePos.position + shotFire * m_player.GetStatus.AtkDist;
            }
            else
            {
                if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_player.GetStatus.AtkDist)) //��������, ����, �浹����, �����Ÿ� 
                {
                    if (hit.collider.CompareTag("Background"))
                    {
                        hitPos = hit.point;
                    }
                    else if (hit.collider.CompareTag("Zombie"))
                    {
                        hitPos = AttackProcess(hit);
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
    Vector3 AttackProcess(RaycastHit hit) //���� �� ������� ��ȣ�ۿ��ϱ����� ���μ��� ����
    {
        float damage = 0f;
        Vector3 hitPos = Vector3.zero;
        var mon = hit.collider.GetComponent<MonsterController>();
        var type = GunManager.AttackProcess(mon, m_player.GetStatus.damage, m_player.GetStatus.criRate, m_player.GetStatus.criAttack,m_player.GetStatus.ArmorPierce, out damage);
        if (burn) //ȭ�� ȿ�� Ȱ��ȭ ���ִٸ�. ȭ��� ���� ��ȣ�� �߰��� ��.
        {
           
            mon.SetDamage(type, damage, m_player,true);
        }
        if (lastfire && ammoRemain == 1) //������ �ѹ�Ư���� Ȱ��ȭ �Ǿ��ְ� źȯ�� �ѹ��϶� ����� �õ��ϸ� 5���� ������
        {
          //  Debug.Log("LastFire ����!  ������ : " + damage*5);
            mon.SetDamage(type, damage * 5, m_player, false);
        }
        else
        {
          //  Debug.Log("�Ϲ� ��� ����!  ������ : " + damage);
            mon.SetDamage(type, damage, m_player, false);
        }
        hitPos = hit.point;
        if(boom && ammoRemain % 5 == 0) //Boom�� Ȱ��ȭ �Ǿ��ְ� �Ѿ��� 5�ǹ���� ������(5��° ź���� ����)
        {
            var hiteffect = TableEffect.Instance.m_tableData[7].Prefab[2];
            var effect = EffectPool.Instance.Create(hiteffect);
            Vector3 effectPos = new Vector3(hitPos.x, hitPos.y+0.1f, hitPos.z - 1f);
            effect.GetComponent<PlayerProjectileCtr>().SetPlayerProjectile(m_player);
            effect.transform.position = effectPos;
            effect.SetActive(true);  
        }
        else
        {
            var hiteffect = TableEffect.Instance.m_tableData[4].Prefab[2];
            var effect = EffectPool.Instance.Create(hiteffect);
            effect.transform.position = hitPos;
            effect.SetActive(true);
        }
        return hitPos;

    }

    #endregion



}
