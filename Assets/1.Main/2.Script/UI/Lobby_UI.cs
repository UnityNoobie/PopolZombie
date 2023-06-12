using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

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

    #region Methods
    void StartButton()
    {
        UGUIManager.Instance.LoadGameScene();
        gameObject.SetActive(false);
        GameManager.Instance.LoadScene(Scene.GameScene);  
    }
    void SetListoner()
    {
        m_startButton.onClick.AddListener(StartButton);
        m_exitButton.onClick.AddListener(GameManager.Instance.ExitGame);
        m_settingButton.onClick.AddListener(UGUIManager.Instance.ActiveVolumeControll);
        m_startButton.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_settingButton.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_recordButton.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_creditButton.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_exitButton.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
    }
    public void SetTransform()
    {
        m_panelButton = Utill.GetChildObject(gameObject, "Panel_Button").gameObject;
        m_startButton = Utill.GetChildObject(m_panelButton, "Button_GameStart").GetComponent<Button>();
        m_settingButton = Utill.GetChildObject(m_panelButton, "Button_Setting").GetComponent<Button>();
        m_recordButton = Utill.GetChildObject(m_panelButton, "Button_GameRecord").GetComponent<Button>();
        m_creditButton = Utill.GetChildObject(m_panelButton, "Button_GameMaker").GetComponent<Button>();
        m_exitButton = Utill.GetChildObject(m_panelButton, "Button_GameExit").GetComponent<Button>();
        SetListoner();
    }
    public void SetActiveUI(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    
    
    #endregion

}
