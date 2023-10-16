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
    IEnumerator SystemMessage(string message) //ȭ�鿡 ǥ�����ִ� �޼���.
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
        StartCoroutine(SystemMessage(name + " �� ������ �����մϴ�."));
    }
    public void SystemMessageCantUse(string name)
    {
        StartCoroutine(SystemMessage(name + " �� ����� �� �����ϴ�. "));
    }
    public void SystemMessageSendMessage(string text) //�ý��� �޼���
    {
        StopAllCoroutines(); //�ʱ�ȭ����
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
    public void SetPlayer(PlayerController player) //�÷��̾� ����.
    {
        m_statusUI.SetPlayer(player);
        m_player = player;
    }
    public void PlayClickSFX() //Ŭ�� ȿ���� ����� SFX
    {
        SoundManager.Instance.PlaySFX("SFX_LobbyMouseEnter", m_source);
    }
    public void CloseAllTabs() //���� �����ִ� UI�ǵ��� ���ִ� ���.
    {
        m_systemMessage.text = null;
        m_storeUI.CloseStore();
        m_statusUI.SetActive(false);
        m_skillUI.DeActiveSkill();
        m_volumeUI.CancleMenu();
        m_tipsUI.gameObject.SetActive(false);
    }
    public void GameStart() //�� ���� UI ȣ��.
    {
        m_lobbyUI.gameObject.SetActive(false);
        LoadGameScene();
        GameManager.Instance.LoadScene(Scene.GameScene);
    }
    public void InputNickName() //�÷��̾��� �г����� �������ִ� UI
    {
        m_input.ActiveUI();
    }
    public void LoadGameScene() // �ε���UI�� ���� ���� ���
    {
        m_loadingScene.StartGameScene();
    }
    public void LoadLobbyScene() // �ε���UI�� �κ�� �ҷ�����
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
    