using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lobby_UI : MonoBehaviour 
{
    #region Constants and Fields
    GameObject m_panelButton;
    Button m_startButton;
    Button m_settingButton;
    Button m_recordButton;
    Button m_creditButton;
    Button m_exitButton;
    #endregion
    public void ClickSound()
    {
        SoundManager.Instance.PlaySFX("SFX_LobbyClick", Camera.main.GetComponent<AudioSource>());
    }
    void FindTransform()
    {
        m_panelButton = Utill.GetChildObject(gameObject, "Panel_Button").gameObject;
        m_startButton = Utill.GetChildObject(gameObject,"Button_GameStart").GetComponent<Button>();
        m_settingButton = Utill.GetChildObject(gameObject, "Button_GameStart").GetComponent<Button>();
        m_recordButton = Utill.GetChildObject(gameObject, "Button_GameStart").GetComponent<Button>();
        m_creditButton = Utill.GetChildObject(gameObject, "Button_GameStart").GetComponent<Button>();
        m_exitButton = Utill.GetChildObject(gameObject, "Button_GameStart").GetComponent<Button>();
    }
    void SetListoner()
    {
        m_startButton.onClick.AddListener(GameManager.Instance.GameStart);

    }
    private void Start()
    {
        FindTransform();
    }


}
