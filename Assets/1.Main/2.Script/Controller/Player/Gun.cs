using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static SoundManager;
public enum GunState
{
    Ready,
    Empty,
    Reload,
    Dead,
    Idle
}

public class Gun : MonoBehaviour
{

    #region Constants and Fields
   
    
    [SerializeField]
    ParticleSystem m_flashEffect;
    [SerializeField]
    ParticleSystem m_ShellEffect;
    LineRenderer[] m_bulletRender;
    AudioSource m_gunAudio;
    [SerializeField]
    PlayerController m_player;
    Transform m_firePos;
    WeaponData m_data;
    public WeaponData GetStatus = new WeaponData();
    int ammoRemain;
    float lastFireTime;
    public bool isfirst = true;
    bool lastfire;
    bool pierce;
    bool burn;
    bool boom;
    public GunState gunstate;
    #endregion

    #region Property
    public WeaponType m_type { get; set; }
    
    #endregion

    #region Coroutine
    IEnumerator ShootEffect(Vector3 hitPos)
    {     
        m_flashEffect.Play();  //ȭ������Ʈ
        m_ShellEffect.Play(); //ź�� ����Ʈ
        for(int i = 0; i < m_bulletRender.Length; i++)
        {
            if (!m_bulletRender[i].enabled)
            {
                m_bulletRender[i].SetPosition(0, m_firePos.position);  //���� ������
                m_bulletRender[i].SetPosition(1, hitPos); //���� ������
                m_bulletRender[i].enabled = true;
                //0.03�ʰ� ���η������� �ִٲ� �Ѿ����� �����.
                yield return new WaitForSeconds(0.05f);
                m_bulletRender[i].enabled = false;
                break;
            }
        } 
    }
    IEnumerator ShotGunReload() //���� ���� ��ƾ
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
            SoundManager.Instance.PlaySFX(m_data.ReloadSound, m_gunAudio);
            ammoRemain++;
            ammoCheck(); //�Ѿ� üũ
        }
    }
    IEnumerator ReloadRoutine() //���ε� ��ƾ 
    {
        float reloadTime = m_player.GetStatus.reloadTime;
        gunstate = GunState.Reload;
        if(m_type == WeaponType.ShotGun) //���� ������ ������ ���
        {
            StartCoroutine(ShotGunReload());
        }
        else //�ٸ� ������� ���
        {
            SoundManager.Instance.PlaySFX(m_data.ReloadSound, m_gunAudio);
            float speed = 2 / reloadTime;
            GunManager.m_animCtr.SetFloat("ReloadTime", speed);
            GunManager.m_animCtr.Play("Reload");
            yield return new WaitForSeconds(reloadTime);
            ammoRemain = m_player.GetStatus.maxammo;
            ammoCheck();
            gunstate = GunState.Ready;
        }
    }
    #endregion

    #region Methods
    public void SetGun(WeaponData data)
    {
        m_data = new WeaponData();
        m_data = data;
        m_type = data.weaponType;
    }
    public void ResetBoolin() //���� ��ü, ��ų �� ���� ��Ȳ���� �ʱ�ȭ
    {
        isfirst = false;
        lastfire = false;
        pierce = false;
        burn = false;
        boom = false;
    }
    public void CheckBoolin()
    {
        Status status = m_player.GetStatus;
        if(status.LastFire != 0)
        {
            lastfire = true;
        }
        if(status.Pierce != 0)
        {
            pierce=true;
        }
        if(status.Burn != 0)
        {
            burn = true;
        }
        if(status.Boom != 0)
        {
            boom = true;
        }
    }
    public void ammoCheck() //�����Ѿ��� �ǽð����� Ȯ��.
    {
        if(m_player.GetStatus.maxammo > 1000) //����� Ư�� ǥ���.
        {
            UGUIManager.Instance.GetScreenHUD().SetWeaponInfo(m_type + ".LV" + m_data.Grade + "\n�� / ��");
            //UIManager.Instance.WeaponInfoUI(m_type + ".LV" + m_data.Grade + "\n�� / ��"); //������
        }
        else
        {
            UGUIManager.Instance.GetScreenHUD().SetWeaponInfo(m_type + ".LV" + m_data.Grade + "\n" + ammoRemain + " / " + m_player.GetStatus.maxammo);
          //  UIManager.Instance.WeaponInfoUI(m_type + ".LV" + m_data.Grade + "\n" + ammoRemain + " / " + m_player.GetStatus.maxammo); //������
        }
    }
    void Start()
    {
        m_gunAudio = GetComponent<AudioSource>();
        m_bulletRender = GetComponentsInChildren<LineRenderer>();
        for(int i = 0; i < m_bulletRender.Length; i++)
        {
            m_bulletRender[i].positionCount = 2;
            m_bulletRender[i].enabled = false;
        } 
        m_firePos = Utill.GetChildObject(gameObject, "Dummy_Firepos");
    }
  
    void OnEnable() // ������� �Ѿ��� �ִ�ġ�� �ϰ� ���� ������ �ҷ���
    {
        ammoRemain = m_player.GetStatus.maxammo; //��ó�� �õ��� �Ͽ������� ������ ����� �������� ����Ұ��̰� ��ü�ϴ� ������ ��� ��������, �ٴڿ����ݱ� �� ���ο� �� ��� ���� ������
        gunstate = GunState.Ready; //�׳� �⺻��� ��������.
        ammoCheck();
        lastFireTime = 0;
    }
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
        var layer = (1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Monster"));
        shotFire = m_player.transform.forward;
        float verti = Random.Range(-0.05f, 0.05f); //�ѱ� ź���� ǥ����
        if (m_data.weaponType == WeaponType.ShotGun) //������ ��� Ư���� �۵������ ������ ������ �̷��� �սô�.
        {
            for (int i = 0; i < m_player.GetStatus.ShotGun; i++)
            {
                shotFire = m_player.transform.forward;
                verti = Random.Range(-0.4f, 0.4f);
                shotFire.x += verti;
                if (pierce)
                {
                    RaycastHit[] hits = Physics.RaycastAll(m_firePos.position, shotFire, m_player.GetStatus.AtkDist, layer);
                    for (int j = 0; j < hits.Length; j++)
                    {
                        if (hits[j].collider.CompareTag("Zombie"))
                        {
                            AttackProcess(hits[j]);
                        }
                        hitPos = m_firePos.position + shotFire * m_player.GetStatus.AtkDist;
                        StartCoroutine(ShootEffect(hitPos));
                    }
                }
                else
                {
                    if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_player.GetStatus.AtkDist, layer)) //��������, ����, �浹����, �����Ÿ� 
                    {
                        if (hit.collider.CompareTag("Background"))
                        {
                            hitPos = hit.point;
                        }
                        else if (hit.collider.CompareTag("Zombie"))
                        {
                            hitPos = AttackProcess(hit);
                        }  
                    }
                    else
                    {
                        hitPos = m_firePos.position + shotFire * m_player.GetStatus.AtkDist;
                    }
                    StartCoroutine(ShootEffect(hitPos));

                }
            }
            m_flashEffect.Play();  //ȭ������Ʈ
            m_ShellEffect.Play(); //ź�� ����Ʈ
            SoundManager.Instance.PlaySFX(m_data.ShotSound, m_gunAudio);
        }
        else //���ǿ��� ����
        {
            shotFire = m_player.transform.forward;
            shotFire.x += verti;
            if (pierce)
            {
                RaycastHit[] hits = Physics.RaycastAll(m_firePos.position, shotFire, m_player.GetStatus.AtkDist, layer);
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
                if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_player.GetStatus.AtkDist, layer)) //��������, ����, �浹����, �����Ÿ� 
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
            SoundManager.Instance.PlaySFX(m_data.ShotSound, m_gunAudio);
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
        mon.PlayHitSound(m_data.AtkSound); //���ػ�� �����ϸ� �Ҹ�������� �õ�
        if (lastfire && ammoRemain == 1) //������ �ѹ�Ư���� Ȱ��ȭ �Ǿ��ְ� źȯ�� �ѹ��϶� ����� �õ��ϸ� 6.66���� ������
        {
            mon.SetDamage(type, damage * m_player.GetStatus.LastFire, m_player, burn,m_player);
        }
        else
        {
            mon.SetDamage(type, damage, m_player, burn,m_player);
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
