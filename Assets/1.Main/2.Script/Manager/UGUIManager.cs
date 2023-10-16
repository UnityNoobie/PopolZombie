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
    Camera m_uiCam;
    VolumeController m_volumeUI;
    ScreenHUD m_hud;
    GameMenuUI m_menuUI;
    PlayerController m_player;
    #endregion

    #region Coroutine
    IEnumerator SystemMessage(string message) //화면에 표기해주는 메세지.
    {
        m_systemMessage.gameObject.SetActive(true);
        m_systemMessage.text = message;
        yield return new WaitForSeconds(1f);
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
    public void SystemMessageSendMessage(string text) //시스템 메세지
    {
        StopAllCoroutines(); //초기화진행
        StartCoroutine(SystemMessage(text));
    }
    #endregion

    #region Method

    #region returnMethod
    public ScreenHUD GetScreenHUD()
    {
        return m_hud;
    }
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
    public VolumeController GetVolumeController()
    {
        return m_volumeUI;
    }
    public ScoreUI GetScoreUI()
    {
        return m_scoreUI;
    }
    public TipsUI GetTipUI()
    {
        return m_tipsUI;
    }
    public GameMenuUI GetMenuUI()
    {
        return m_menuUI;
    }
    public Lobby_UI GetLobbyUI()
    {
        return m_lobbyUI;
    }
    public ExitMenu GetExitMenu()
    {
        return m_exit;
    }
    #endregion
    public void SetPlayer(PlayerController player) //플레이어 설정.
    {
        m_statusUI.SetPlayer(player);
        m_player = player;
    }
    public void PlayClickSFX() //클릭 효과에 실행될 SFX
    {
        SoundManager.Instance.PlaySFX("SFX_LobbyMouseEnter", m_source);
    }
    public void CloseAllTabs() //현재 열려있는 UI탭들을 꺼주는 기능.
    {
        m_systemMessage.text = null;
        m_storeUI.CloseStore();
        m_statusUI.SetActive(false);
        m_skillUI.DeActiveSkill();
        m_volumeUI.CancleMenu();
        m_tipsUI.gameObject.SetActive(false);
    }
    public void GameStart() //씬 변경 UI 호출.
    {
        m_lobbyUI.gameObject.SetActive(false);
        LoadGameScene();
        GameManager.Instance.LoadScene(Scene.GameScene);
    }
    public void InputNickName() //플레이어의 닉네임을 설정해주는 UI
    {
        m_input.ActiveUI();
    }
    public void LoadGameScene() // 로딩씬UI의 게임 시작 기능
    {
        m_loadingScene.StartGameScene();
    }
    public void LoadLobbyScene() // 로딩씬UI의 로비씬 불러오기
    {
        m_hud.SetActive(false);
        m_loadingScene.gameObject.SetActive(true);
        m_loadingScene.LoadLobbyScene();
    }
    void SetTransform()
    {
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
        m_hud = GetComponentInChildren<ScreenHUD>(true);
        m_hud.SetTransform();
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
        SetTransform();
    }

    #endregion
}
    