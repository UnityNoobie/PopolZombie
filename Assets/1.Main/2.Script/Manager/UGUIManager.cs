using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UGUIManager : SingletonDontDestroy<UGUIManager> 
{
    #region Constants and Fields
    TextMeshProUGUI m_systemMessage;
    StoreUI m_storeUI;
    StatusUI m_statusUI;
    SkillUI m_skillUI;
    RoundUI m_roundUI;
    Lobby_UI m_lobbyUI;
    ScoreUI m_scoreUI;
    TipsUI m_tipsUI;
    NickInput m_input;
    LoadingScene m_loadingScene;
    AudioSource m_source;
    ExitMenu m_exit;
    Canvas m_canvas;
    Camera m_uiCam;
    VolumeController m_volumeUI;
    GameMenuUI m_menuUI;
    PlayerController m_player;
    #endregion

    #region Coroutine
    IEnumerator SystemMessage(string message)
    {
        m_systemMessage.gameObject.SetActive(true);
        m_systemMessage.text = message;
        yield return new WaitForSeconds(1.5f);
        m_systemMessage.text = null;
        m_systemMessage.gameObject.SetActive(false);
    }
    #endregion

    #region SystemMessange
    public void SystemMessageItem(string name)
    {
        StartCoroutine(SystemMessage(name + " 의 수량이 부족합니다."));
    }
    public void SystemMessageCantUse(string name)
    {
        StartCoroutine(SystemMessage(name + " 을 사용할 수 없습니다. "));
    }
    public void SystemMessageSendMessage(string text)
    {
        StartCoroutine(SystemMessage(text));
    }
    #endregion

    #region Method
   
    public StoreUI GetStoreUI()
    {
        return m_storeUI;
    }
    public StatusUI GetStatusUI()
    {
        return m_statusUI;
    }
    public SkillUI GetSkillUI()
    {
        return m_skillUI;
    }
    public RoundUI GetRoundUI()
    {
        return m_roundUI;
    }
    public Camera GetUICam()
    {
        return m_uiCam;
    }
    public void SetPlayer(PlayerController player)
    {
        m_statusUI.SetPlayer(player);
        m_player = player;
    }
    public void PlayClickSFX()
    {
        SoundManager.Instance.PlaySFX("SFX_LobbyMouseEnter", m_source);
    }
    public void CloseAllTabs()
    {
        m_systemMessage.text = null;
        m_storeUI.CloseStore();
        m_statusUI.SetActive(false);
        m_skillUI.DeActiveSkill();
        m_volumeUI.CancleMenu();
        m_tipsUI.gameObject.SetActive(false);
    }
    public void OpenMenu()
    {
        if(m_menuUI.gameObject.activeSelf)
        {
            m_menuUI.DeactiveUI();
        }
        else
        {
            m_menuUI.ActiveUI();
        }
       
    }
    public void SkillUIChange(bool aa, PlayerSkillController skill)
    {
        if (aa)
        {
            m_skillUI.ActiveSkill(skill);
        }
        else
        {
            m_skillUI.DeActiveSkill();
        }
    }
    public void StartRound()
    {
        m_roundUI.ChangeRound();
    }
    public void GameStart()
    {
        m_lobbyUI.gameObject.SetActive(false);
        LoadGameScene();
        GameManager.Instance.LoadScene(Scene.GameScene);
    }
    public void InputNickName()
    {
        m_input.ActiveUI();
    }
    public void LoadGameScene()
    {
        m_loadingScene.StartGameScene();
    }
    public void LoadLobbyScene()
    {
        m_loadingScene.gameObject.SetActive(true);
        m_loadingScene.LoadLobbyScene();
    }
    public void ActiveLobbyUI(bool isActive)
    {
        m_lobbyUI.SetActiveUI(isActive);
    }
    public void LayerChanger(int layer)
    {
        m_canvas.sortingOrder = layer;
    }
    public void OpenExitMenu()
    {
        if (m_exit.gameObject.activeSelf)
        {
            m_exit.DeActiveUi();
        }
        else
        {
            m_exit.ActiveUI();
        } 
    }
    public void ActiveVolumeControll()
    {
        m_volumeUI.ActiveUI();
    }
    public void SaveData(int gameduration,int round)
    {
        m_scoreUI.SaveGameData(m_player, gameduration,round);
    }
    public void ActiveScoreUI()
    {
        m_scoreUI.ActiveUI();
    }
    public void ActiveTipMenu() 
    {
        m_tipsUI.ActiveUI();
    }
    void SetTransform()
    {
        m_canvas = GetComponent<Canvas>();
        m_source = GetComponentInChildren<AudioSource>();
        m_exit = GetComponentInChildren<ExitMenu>(true);
        m_systemMessage = Utill.GetChildObject(gameObject, "SystemMessage").GetComponent<TextMeshProUGUI>();
        m_statusUI = GetComponentInChildren<StatusUI>(true);
        m_skillUI = GetComponentInChildren<SkillUI>(true);
        m_storeUI = GetComponentInChildren<StoreUI>(true);
        m_roundUI = GetComponentInChildren<RoundUI>(true);
        m_volumeUI = GetComponentInChildren<VolumeController>(true);
        m_lobbyUI = GetComponentInChildren<Lobby_UI>(true);
        m_menuUI = GetComponentInChildren<GameMenuUI>(true);
        m_scoreUI = GetComponentInChildren<ScoreUI>(true);
        m_loadingScene = GetComponentInChildren<LoadingScene>(true);
        m_input = GetComponentInChildren<NickInput>(true);
        m_tipsUI = GetComponentInChildren<TipsUI>(true);
        m_uiCam = GetComponentInChildren<Camera>();
        m_tipsUI.SetTransform();
        m_input.SetTransform();
        m_scoreUI.SetTransform();
        m_volumeUI.SetTransform();
        m_menuUI.SetTransform();
        m_lobbyUI.SetTransform();
        m_skillUI.SetTransform();
       
     
    }
    protected override void OnStart()
    {
        SetTransform();    }
    #endregion
}
