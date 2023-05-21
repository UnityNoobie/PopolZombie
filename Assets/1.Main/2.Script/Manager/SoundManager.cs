using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SoundManager : SingletonDontDestroy<SoundManager>
{
    public enum AudioType
    {
        BGM,
        SFX,
        MAX
    }

    AudioSource[] m_audio;
    AudioSource m_bgm;
    [SerializeField]
    AudioClip[] m_bgmClips;
    [SerializeField]
    AudioClip[] m_sfxClips;
    const int MaxVolumLevel = 10;
    const int MaxSfxPlayCount = 5;
    public Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();
    Dictionary<AudioClip,int> m_sfxPlayList = new Dictionary<AudioClip,int>();

    IEnumerator Couroutine_CheckPlayEnded(AudioClip sfx, float length)
    {
        yield return new WaitForSeconds(length);
        m_sfxPlayList[sfx]--;
        if (m_sfxPlayList[sfx] <= 0)
        {
            m_sfxPlayList.Remove(sfx);
        }
    }
    protected override void OnStart()
    {
        m_bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");
        m_sfxClips = Resources.LoadAll<AudioClip>("Audio/SFX");
        m_bgm = Utill.GetChildObject(gameObject,"BGM").GetComponent<AudioSource>();
        m_bgm.loop = true;
        m_bgm.rolloffMode = AudioRolloffMode.Linear;
        m_bgm.playOnAwake = false;
    }
    public void PlayBGM(string bgm) //BGM플레이
    {
        m_bgm.clip = m_audioClips[bgm];
        m_bgm.Play();
    }
    public void PlaySFX(string name, AudioSource source) // SFX 플레이
    {
        int count = 0;
        AudioClip sfx = m_audioClips[name];
        if (m_sfxPlayList.TryGetValue(sfx, out count))
        {
            if (count >= MaxSfxPlayCount)
            {
                return;
            }
            else
            {
                m_sfxPlayList[sfx]++;
                count++;
            }
        }
        else
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
}
