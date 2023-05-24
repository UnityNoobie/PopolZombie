using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class SoundManager : SingletonDontDestroy<SoundManager>
{
    public enum SoundType
    {
        BGM,
        SFX,
        MAX
    }
    TableSound m_info = new TableSound();
    AudioSource[] m_audio;
    AudioSource m_bgm;
    [SerializeField]
    AudioClip[] m_bgmClips;
    [SerializeField]
    AudioClip[] m_sfxClips;
    const int MaxVolumLevel = 10;
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
    public void PlayBGM(string bgm) //BGM�÷���
    {
        m_bgm.clip = m_audioClips[bgm];
        m_bgm.Play();
    }
    public void PlaySFX(string name, AudioSource source) // SFX �÷���
    {
        int count = 0;
      
        TableSound sound = m_info.GetSound(name);
        // ���� ������ ���带 ������ �ִ� ���尡 �ֱ� ������ ����Ʈ���� �Ѱ� �̾ƿ���
        Debug.Log(m_audioClips.Count + "���⼱ �� 0�ϱ�?");
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
    private void Awake()
    {
        TableSoundInfo.Instance.Load();
        m_bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");
        m_sfxClips = Resources.LoadAll<AudioClip>("Audio/SFX");
        for (int i = 0; i < m_sfxClips.Length; i++)
        {
            m_audioClips.Add(m_sfxClips[i].name, m_sfxClips[i]);
            Debug.Log(m_audioClips.Count);
        }
        m_bgm = Utill.GetChildObject(gameObject, "BGM").GetComponent<AudioSource>();
        m_bgm.loop = true;
        m_bgm.rolloffMode = AudioRolloffMode.Linear;
        m_bgm.playOnAwake = false;
    }
}
