using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : SingletonDontDestroy<SoundManager>
{
    #region Constants and Fields
    TableSound m_info = new TableSound();
    AudioSource[] m_audio;
    AudioSource m_bgm;
    AudioSource m_sfx;
    [SerializeField]
    AudioClip[] m_bgmClips;
    [SerializeField]
    AudioClip[] m_sfxClips;
    const int MaxVolumLevel = 10;
    public Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();
    Dictionary<AudioClip,int> m_sfxPlayList = new Dictionary<AudioClip,int>();
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
    public void DayStart()
    {
        PlayBGM("BGM_Day");
        PlaySFX("SFX_DayStart", m_sfx);
    }
    public void NightStart()
    {
        PlayBGM("BGM_Night");
        PlaySFX("SFX_NightStart", m_sfx);
    }
    public void BossStart()
    {
        PlayBGM("BGM_Boss");
        PlaySFX("SFX_BossStart", m_sfx);
    }
    public void PlayBGM(string bgm) //BGM�÷���
    {
        TableSound sound = m_info.GetSound(bgm);
        AudioClip sfx = m_audioClips[sound.GetSound(bgm).soundList[Random.Range(0, sound.soundList.Length)]];
        m_bgm.clip = sfx;
        m_bgm.Play();
    }
    public void PlaySFX(string name, AudioSource source) // SFX �÷���
    {
        int count = 0;
        TableSound sound = m_info.GetSound(name);
        // ���� ������ ���带 ������ �ִ� ���尡 �ֱ� ������ ����Ʈ���� �Ѱ� �̾ƿ���
        AudioClip sfx = m_audioClips[sound.GetSound(name).soundList[Random.Range(0,sound.soundList.Length)]];
        if (m_sfxPlayList.TryGetValue(sfx, out count))
        {
            
            if (count >= m_info.GetSound(name).maxPlay) //������ ������ �ִ� ���Ƚ������ ũ�ٸ� �����ؼ� �ò��������� �ʰ�.
            {
                return;
            }
            else //�ִ� Ƚ������ ���ڸ� ���.
            {
                m_sfxPlayList[sfx]++;
                count++;
            }
        }
        else //����Ʈ�� ������ �߰����ֱ�
        {
            m_sfxPlayList.Add(sfx, 1);
        }
        source.PlayOneShot(sfx);
        StartCoroutine(Couroutine_CheckPlayEnded(sfx, sfx.length));
    }
    public void SetBgmVolume(int level)
    {
        if (level > MaxVolumLevel)
        {
            level = MaxVolumLevel;
            m_bgm.volume = (float)level / MaxVolumLevel;
        }
        else
        {
            m_bgm.volume = (float)level / MaxVolumLevel;
        }
    }
    public void SetMute(bool isActive)
    {
        for (int i = 0; i < (int)m_audio.Length; i++)
        {
            m_audio[i].mute = isActive;
        }
    }
    protected override void OnStart()
    {
        TableSoundInfo.Instance.Load();
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
