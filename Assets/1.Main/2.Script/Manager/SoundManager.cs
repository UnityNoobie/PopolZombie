using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : SingletonDontDestroy<SoundManager>
{
    #region Constants and Fields
    TableSound m_info = new TableSound();
    AudioSource m_bgm;
    AudioSource m_sfx;
    AudioClip[] m_bgmClips;
    AudioClip[] m_sfxClips;
    public Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();
    Dictionary<AudioClip,int> m_sfxPlayList = new Dictionary<AudioClip,int>();
    const float MaxVolumLevel = 10;
    const float MinVolumLevel = 0;
    bool ismute = false;
    float totalVolume = 5;
    float bgmVolume = 10;
    float sfxVolume = 10;
    const string m_dayBGM = "BGM_Day";
    const string m_nightBGM = "BGM_Night";
    const string m_dayStart = "SFX_DayStart";
    const string m_nightStart = "SFX_NightStart";
    const string m_bossBGM = "BGM_Boss";
    const string m_bossSart = "SFX_BossStart";
    #endregion

    #region Coroutine
    IEnumerator Couroutine_CheckPlayEnded(AudioClip sfx, float length)
    {
        yield return new WaitForSeconds(length);
        m_sfxPlayList[sfx]--;
        if (m_sfxPlayList[sfx] <= 0)
        {
            m_sfxPlayList.Remove(sfx);
        }
    }
    #endregion

    #region Methods
    public void PlaySFX(string name, AudioSource source) // SFX 플레이
    {
        int count = 0;
        TableSound sound = m_info.GetSound(name);
        // 여러 종류의 사운드를 가지고 있는 사운드가 있기 때문에 리스트에서 한개 뽑아오기
        AudioClip sfx = m_audioClips[sound.GetSound(name).soundList[Random.Range(0, sound.soundList.Length)]];
        if (m_sfxPlayList.TryGetValue(sfx, out count))
        {
            if (count >= m_info.GetSound(name).maxPlay) //지정된 사운드의 최대 재생횟수보다 크다면 리턴해서 시끄러워지지 않게.
            {
                return;
            }
            else //최대 횟수보다 작자면 재생.
            {
                m_sfxPlayList[sfx]++;
                count++;
            }
        }
        else //리스트에 없으면 추가해주기
        {
            m_sfxPlayList.Add(sfx, 1);
        }
        if (ismute) //음소거 기능이 활성화 되어있을 경우
            source.volume = 0;
        else //음소거상태가 아니라면
        {
            source.volume = (totalVolume * sfxVolume) / 100;
        }
        source.PlayOneShot(sfx);
        StartCoroutine(Couroutine_CheckPlayEnded(sfx, sfx.length));
    }
    public void PlayBGM(string bgm) //BGM플레이
    {
        TableSound sound = m_info.GetSound(bgm);
        AudioClip sfx = m_audioClips[sound.GetSound(bgm).soundList[Random.Range(0, sound.soundList.Length)]];
        if(ismute) //음소거 기능이 활성화 되어있을 경우
            m_bgm.volume = 0;
        else //음소거상태가 아니라면
        {
            m_bgm.volume = (totalVolume * bgmVolume) / 100; 
        }
        m_bgm.clip = sfx;
        m_bgm.Play();
    }
  
    public void SetBgmVolume(float level) //BGM 볼륨 설정
    {
        if(level >= MaxVolumLevel)  //볼륨이 최대치보다 클 경우
        {
            bgmVolume = MaxVolumLevel;
            return;
        }
        else if (level <= MinVolumLevel)
        {
            bgmVolume = MinVolumLevel;
        }
        bgmVolume = level;
        m_bgm.volume = m_bgm.volume = (totalVolume * bgmVolume) / 100;
    }
    public void SetSfxVolume(float level) //SFX 볼륨 설정
    {
        if (level >= MaxVolumLevel)  //볼륨이 최대치보다 클 경우
        {
            sfxVolume = MaxVolumLevel;
            return;
        }
        else if (level <= MinVolumLevel)
        {
            sfxVolume = MinVolumLevel;
        }
        sfxVolume = level;
    }
    public void SetVolumeLevel(float level) //마스터 볼륨 설정
    {
        if (level >= MaxVolumLevel)  //볼륨이 최대치보다 클 경우
        {
            totalVolume = MaxVolumLevel;
            return;
        }
        else if (level <= MinVolumLevel)
        {
            totalVolume = MinVolumLevel;
        }
        totalVolume = level;
    }
    public void SetMute(bool isActive) //음소거 설정
    {
        ismute = isActive;
        if(ismute)
        {
            m_bgm.volume = 0;
        }
        else
        {
            m_bgm.volume = (totalVolume * bgmVolume) / 100;
        }
    }
    public void DayStart() //낮시간 시작 시 재생
    {
        PlayBGM(m_dayBGM);
        PlaySFX(m_dayStart, m_sfx);
    }
    public void NightStart() //밤 시작 시 재생
    {
        PlayBGM(m_nightBGM);
        PlaySFX(m_nightStart, m_sfx);
    }
    public void BossStart() //보스 라운드의 경우 재생
    {
        PlayBGM(m_bossBGM);
        PlaySFX(m_bossSart, m_sfx);
    }
    public void LobbyStart() //로비 로드 시 재생
    {
        PlayBGM("BGM_Lobby");
    }
    public float GetTotalVolume() //마스터 볼륨 반환
    {
        return totalVolume;
    }
    public float GetBGMVolume() //BGM 볼륨 반환
    {
        return bgmVolume;
    }
    public float GetSFXVolume() // SFX 볼륨 반환
    {
        return sfxVolume;
    }
    public bool IsMute() //음소거 여부 반환
    {
        return ismute;
    }
    protected override void OnStart() //좌표설정 + audioclip 리소스 가져오기
    {
        m_bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");
        m_sfxClips = Resources.LoadAll<AudioClip>("Audio/SFX");
        m_bgm = Utill.GetChildObject(gameObject, "BGM").GetComponent<AudioSource>();
        m_sfx = Utill.GetChildObject(gameObject, "SFX").GetComponent<AudioSource>();
        m_bgm.loop = true;
        m_bgm.rolloffMode = AudioRolloffMode.Linear;
        m_bgm.playOnAwake = false;
        for (int i = 0; i < m_sfxClips.Length; i++)
        {
            m_audioClips.Add(m_sfxClips[i].name, m_sfxClips[i]);
        }
        for(int i = 0; i < m_bgmClips.Length; i++)
        {
            m_audioClips.Add(m_bgmClips[i].name, m_bgmClips[i]);
        }
    }
    #endregion
}
