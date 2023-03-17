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
        m_flashEffect.Play();  //화염이펙트
        m_ShellEffect.Play(); //탄피 이펙트
        m_gunAudio.PlayOneShot(m_shootSound); //총소리
        m_bulletRender.SetPosition(0, m_firePos.position);  //레이 시작점
        m_bulletRender.SetPosition(1, hitPos); //레이 도착점
        m_bulletRender.enabled = true;
        //0.03초간 라인렌더러를 켯다꺼 총알흔적 남기기.
        yield return new WaitForSeconds(0.03f);
        m_bulletRender.enabled = false;
    }
    IEnumerator ShotGunReload()
    {
        while (gunstate == GunState.Reload) //총의 상태가 장전중 일경우 계속 실행
        {
            if (ammoRemain > 0 && ammoRemain >= m_player.GetStatus.maxammo || ammoRemain > 0 && Input.GetMouseButton(0)) // 최대 총알에 도달했거나 마우스 클릭을 했을 경우 준비상태로 변경
            {
                GunManager.m_animCtr.Play("Idle");
                yield return new WaitForSeconds(0.1f);
                gunstate = GunState.Ready;
                yield break;
            }
            GunManager.m_animCtr.Play("ShotReload"); //샷건장전 애니메이션 실행
            yield return new WaitForSeconds(m_player.GetStatus.reloadTime); //샷건 장전시간동안 대기
            m_gunAudio.PlayOneShot(m_reloadSound);
            ammoRemain++;
            ammoCheck();
        }
    }
    IEnumerator ReloadRoutine() //리로드 루틴 
    {
        float reloadTime = m_player.GetStatus.reloadTime;
        isReload = true;
        gunstate = GunState.Reload;
        if(m_type == WeaponType.ShotGun) //무기 종류가 샷건일 경우
        {
            StartCoroutine(ShotGunReload());
        }
        else
        {
            m_gunAudio.PlayOneShot(m_reloadSound);   //총의 상태를 Reload로 바꾸어 발사를 막고 시간이 지난 후 총알을 체워주고 준비상태로 변경
            float speed = 2 / reloadTime;
            GunManager.m_animCtr.SetFloat("ReloadTime", speed);
            GunManager.m_animCtr.Play("Reload");
            yield return new WaitForSeconds(reloadTime);
            ammoRemain = m_player.GetStatus.maxammo;
            ammoCheck();
            gunstate = GunState.Ready;
        }
      
    }

    public void ammoCheck() //남은총알을 실시간으로 확인.
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
  
    private void OnEnable() // 켜질경우 총알을 최대치로 하고 총의 정보를 불러옴
    {
        /*
        if (isfirst) //최초로 실행시에만 무기 변경 시 장탄량 최대로 해보자구?
        {
            ammoRemain = m_player.GetStatus.maxammo;
            isfirst= false;
        }*/
        ammoRemain = m_player.GetStatus.maxammo; //위처럼 시도를 하였었으나 어차피 무기는 한종류만 사용할것이고 교체하는 무기의 경우 상점구입, 바닥에서줍기 등 새로운 총 얻는 경우기 때문에
        gunstate = GunState.Ready; //그냥 기본대로 적용했음.
        ammoCheck();
        lastFireTime = 0;
    }

    public void Fire()
    {
        if (gunstate == GunState.Ready && Time.time >= lastFireTime + 1/ m_player.GetStatus.atkSpeed) //총의 상태가 Ready이고 발사속도가 준비되었을 경우.
        { 
            lastFireTime= Time.time;
            Shot(); //쏴라!!!!
        }
    }
    void Shot()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        Vector3 shotFire = Vector3.zero;
        float damage = 0f;
        shotFire = m_player.transform.forward;
        if (m_type == WeaponType.ShotGun) //샷건의 경우 특수한 작동방식을 가지기 때문에 이렇게 합시다.
        {
            for (int i = 0; i < m_player.GetStatus.ShotGun; i++)
            {
                shotFire = m_player.transform.forward;
                  float verti = Random.Range(-0.3f, 0.3f);
                  shotFire.x += verti;
                
                if (Physics.Raycast(m_firePos.position,shotFire, out hit, m_player.GetStatus.AtkDist)) //시작지점, 방향, 충돌정보, 사정거리 
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
            m_flashEffect.Play();  //화염이펙트
            m_ShellEffect.Play(); //탄피 이펙트
            m_gunAudio.PlayOneShot(m_shootSound); //총소리 
        }
        else //샷건외의 총은
        {
           
            if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_player.GetStatus.AtkDist)) //시작지점, 방향, 충돌정보, 사정거리 
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
    public bool Reload() //재장전
    {
        if (gunstate == GunState.Reload || ammoRemain >= m_player.GetStatus.maxammo)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

   
  
}
