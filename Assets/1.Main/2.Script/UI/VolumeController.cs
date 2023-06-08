using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    #region Constants and Fields
    Transform m_totalPos;
    Transform m_bgmPos;
    Transform m_sfxPos;
    Transform m_mutePos;
    Slider m_totalVolume;
    Slider m_sfxVolume;
    Slider m_bgmVolume;
    TextMeshProUGUI m_totalValue;
    TextMeshProUGUI m_sfxValue;
    TextMeshProUGUI m_bgmValue;
    Button m_muteButton;
    Button m_accaptButton;
    Button m_cancleButton;
    Image m_muteImage;
    bool isMute = false;

    #endregion
    #region Methods
    public void SetTransform() //자식오브젝트에서 가져와주어 세팅
    {
        m_totalPos = Utill.GetChildObject(gameObject,"TotalVolume");
        m_bgmPos = Utill.GetChildObject(gameObject, "BGMVolume");
        m_sfxPos = Utill.GetChildObject(gameObject, "SFXVolume");
        m_mutePos = Utill.GetChildObject(gameObject, "Mute");
        m_totalVolume = m_totalPos.GetComponentInChildren<Slider>();
        m_sfxVolume = m_sfxPos.GetComponentInChildren<Slider>();
        m_bgmVolume = m_bgmPos.GetComponentInChildren<Slider>();
        m_totalValue = Utill.GetChildObject(m_totalVolume.gameObject,"Value").GetComponent<TextMeshProUGUI>();
        m_bgmValue = Utill.GetChildObject(m_bgmVolume.gameObject, "Value").GetComponent<TextMeshProUGUI>();
        m_sfxValue = Utill.GetChildObject(m_sfxVolume.gameObject, "Value").GetComponent<TextMeshProUGUI>();
        m_muteButton = m_mutePos.GetComponentInChildren<Button>();
        m_muteImage = m_muteButton.GetComponent<Image>();
        LoadImage();
        m_accaptButton = Utill.GetChildObject(gameObject, "Button_Accapt").GetComponent<Button>();
        m_cancleButton = Utill.GetChildObject(gameObject,"Button_Cancle").GetComponent<Button>();
        m_muteButton.onClick.AddListener(SetMute);
        m_muteButton.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_accaptButton.onClick.AddListener(SetValue);
        m_accaptButton.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_cancleButton.onClick.AddListener(CancleMenu);
        m_cancleButton.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
    }
    public void SetValueControl() //사운드 벨류값 표시
    {
        m_totalValue.text = m_totalVolume.value.ToString();
        m_bgmValue.text = m_bgmVolume.value.ToString();
        m_sfxValue.text = m_sfxVolume.value.ToString();
    }
    void LoadImage()
    {
        if (isMute)
        {
            m_muteImage.sprite = Resources.Load<Sprite>("Image/MuteIcon");
        }
        else
        {
            m_muteImage.sprite = Resources.Load<Sprite>("Image/SoundIcon");
        }
    }
    void SetMute() //음소거 
    {
        isMute = !isMute;
        LoadImage();
    }
    void GetValue()
    {
        m_totalVolume.value = SoundManager.Instance.GetTotalVolume();
        m_bgmVolume.value = SoundManager.Instance.GetBGMVolume();
        m_sfxVolume.value = SoundManager.Instance.GetSFXVolume();
        SetValueControl();
        isMute = SoundManager.Instance.IsMute();
    }
    void SetValue()
    {
        SoundManager.Instance.SetBgmVolume(m_bgmVolume.value);
        SoundManager.Instance.SetSfxVolume(m_sfxVolume.value);
        SoundManager.Instance.SetVolumeLevel(m_totalVolume.value);
        SoundManager.Instance.SetMute(isMute);
    }
    public void CancleMenu()
    {
        UGUIManager.Instance.LayerChanger(0);
        UGUIManager.Instance.PlayClickSFX();
        gameObject.SetActive(false);
    }
    public void ActiveUI()
    {
        UGUIManager.Instance.PlayClickSFX();
        gameObject.SetActive(true); 
        GetValue();
    }

    #endregion

}
