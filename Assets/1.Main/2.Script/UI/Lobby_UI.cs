using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class Lobby_UI : MonoBehaviour, IPointerClickHandler
{
    #region Constants and Fields
    GameObject m_panelButton;
    Button m_startButton;
    Button m_settingButton;
    Button m_recordButton;
    Button m_creditButton;
    Button m_exitButton;
    AudioSource m_source;
    #endregion

    IEnumerator Coroutine_StartLoad()
    {
        
        yield return new WaitForSeconds(0.1f);
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        UGUIManager.Instance.PlayClickSFX();
    }
    void StartButton()
    {
        
        UGUIManager.Instance.LoadGameScene();
        gameObject.SetActive(false);
        GameManager.Instance.LoadScene(Scene.GameScene);
        //StartCoroutine(Coroutine_StartLoad());     
    }
    void ExitButton()
    {
        GameManager.Instance.ExitGame();
    }
    void OnClick()
    {
        OnPointerClick(null);
    }
    void FindTransform()
    {
        m_panelButton = Utill.GetChildObject(gameObject, "Panel_Button").gameObject;
        m_startButton = Utill.GetChildObject(gameObject,"Button_GameStart").GetComponent<Button>();
        m_settingButton = Utill.GetChildObject(gameObject, "Button_Setting").GetComponent<Button>();
        m_recordButton = Utill.GetChildObject(gameObject, "Button_GameRecord").GetComponent<Button>();
        m_creditButton = Utill.GetChildObject(gameObject, "Button_GameMaker").GetComponent<Button>();
        m_exitButton = Utill.GetChildObject(gameObject, "Button_GameExit").GetComponent<Button>();
        SetListoner();
    }
    void SetListoner()
    {
        m_startButton.onClick.AddListener(StartButton);
        m_exitButton.onClick.AddListener(ExitButton);
        m_settingButton.onClick.AddListener(UGUIManager.Instance.ActiveVolumeControll);
        m_startButton.onClick.AddListener(OnClick);
        m_settingButton.onClick.AddListener(OnClick);
        m_recordButton.onClick.AddListener(OnClick);
        m_creditButton.onClick.AddListener(OnClick);
        m_exitButton.onClick.AddListener(OnClick);
    }
    private void Start()
    {
        m_source = Utill.GetChildObject(gameObject,"Camera").GetComponent<AudioSource>();
        FindTransform();
    }


}
