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
        m_flashEffect.Play();  //화염이펙트
        m_ShellEffect.Play(); //탄피 이펙트
        for(int i = 0; i < m_bulletRender.Length; i++)
        {
            if (!m_bulletRender[i].enabled)
            {
                m_bulletRender[i].SetPosition(0, m_firePos.position);  //레이 시작점
                m_bulletRender[i].SetPosition(1, hitPos); //레이 도착점
                m_bulletRender[i].enabled = true;
                //0.03초간 라인렌더러를 켯다꺼 총알흔적 남기기.
                yield return new WaitForSeconds(0.05f);
                m_bulletRender[i].enabled = false;
                break;
            }
        } 
    }
    IEnumerator ShotGunReload() //샷건 장전 루틴
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
            SoundManager.Instance.PlaySFX(m_data.ReloadSound, m_gunAudio);
            ammoRemain++;
            ammoCheck(); //총알 체크
        }
    }
    IEnumerator ReloadRoutine() //리로드 루틴 
    {
        float reloadTime = m_player.GetStatus.reloadTime;
        gunstate = GunState.Reload;
        if(m_type == WeaponType.ShotGun) //무기 종류가 샷건일 경우
        {
            StartCoroutine(ShotGunReload());
        }
        else //다른 무기들일 경우
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
    public void ResetBoolin() //무기 교체, 스킬 업 등의 상황에서 초기화
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
    public void ammoCheck() //남은총알을 실시간으로 확인.
    {
        if(m_player.GetStatus.maxammo > 1000) //기관총 특성 표기용.
        {
            UGUIManager.Instance.GetScreenHUD().SetWeaponInfo(m_type + ".LV" + m_data.Grade + "\n∞ / ∞");
            //UIManager.Instance.WeaponInfoUI(m_type + ".LV" + m_data.Grade + "\n∞ / ∞"); //구버전
        }
        else
        {
            UGUIManager.Instance.GetScreenHUD().SetWeaponInfo(m_type + ".LV" + m_data.Grade + "\n" + ammoRemain + " / " + m_player.GetStatus.maxammo);
          //  UIManager.Instance.WeaponInfoUI(m_type + ".LV" + m_data.Grade + "\n" + ammoRemain + " / " + m_player.GetStatus.maxammo); //구버전
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
  
    void OnEnable() // 켜질경우 총알을 최대치로 하고 총의 정보를 불러옴
    {
        ammoRemain = m_player.GetStatus.maxammo; //위처럼 시도를 하였었으나 어차피 무기는 한종류만 사용할것이고 교체하는 무기의 경우 상점구입, 바닥에서줍기 등 새로운 총 얻는 경우기 때문에
        gunstate = GunState.Ready; //그냥 기본대로 적용했음.
        ammoCheck();
        lastFireTime = 0;
    }
    public void Fire()
    {
        if (gunstate == GunState.Ready && Time.time >= lastFireTime + 1 / m_player.GetStatus.atkSpeed) //총의 상태가 Ready이고 발사속도가 준비되었을 경우.
        {
            lastFireTime = Time.time;
            Shot(); //쏴라!!!!
        }
    }
    void Shot()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        Vector3 shotFire = Vector3.zero;
        var layer = (1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Monster"));
        shotFire = m_player.transform.forward;
        float verti = Random.Range(-0.05f, 0.05f); //총기 탄퍼짐 표현용
        if (m_data.weaponType == WeaponType.ShotGun) //샷건의 경우 특수한 작동방식을 가지기 때문에 이렇게 합시다.
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
                    if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_player.GetStatus.AtkDist, layer)) //시작지점, 방향, 충돌정보, 사정거리 
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
            m_flashEffect.Play();  //화염이펙트
            m_ShellEffect.Play(); //탄피 이펙트
            SoundManager.Instance.PlaySFX(m_data.ShotSound, m_gunAudio);
        }
        else //샷건외의 총은
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
                if (Physics.Raycast(m_firePos.position, shotFire, out hit, m_player.GetStatus.AtkDist, layer)) //시작지점, 방향, 충돌정보, 사정거리 
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
    public bool Reload() //재장전
    {
        if (gunstate == GunState.Reload || ammoRemain >= m_player.GetStatus.maxammo)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }
    Vector3 AttackProcess(RaycastHit hit) //공격 시 좀비와의 상호작용하기위한 프로세스 모음
    {
        float damage = 0f;
        Vector3 hitPos = Vector3.zero;
        var mon = hit.collider.GetComponent<MonsterController>();
        var type = GunManager.AttackProcess(mon, m_player.GetStatus.damage, m_player.GetStatus.criRate, m_player.GetStatus.criAttack,m_player.GetStatus.ArmorPierce, out damage);
        mon.PlayHitSound(m_data.AtkSound); //피해사실 전달하며 소리재생유도 시도
        if (lastfire && ammoRemain == 1) //마지막 한발특성이 활성화 되어있고 탄환이 한발일때 사격을 시도하면 6.66배의 데미지
        {
            mon.SetDamage(type, damage * m_player.GetStatus.LastFire, m_player, burn,m_player);
        }
        else
        {
            mon.SetDamage(type, damage, m_player, burn,m_player);
        }
        hitPos = hit.point;
        if(boom && ammoRemain % 5 == 0) //Boom이 활성화 되어있고 총알이 5의배수로 있으면(5번째 탄마다 실행)
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
