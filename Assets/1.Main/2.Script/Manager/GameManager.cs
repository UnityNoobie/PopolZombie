using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum DaynNight
{
    Day,
    Night,
    MAX
}
public enum Scene
{
    LobbyScene,
    GameScene,
    Max
}
public class GameManager : SingletonDontDestroy<GameManager>
{
    #region Constants and Field
    [SerializeField]
    LightIntensityTween m_light;
    Scene m_scene;
    DaynNight roundTime;
    int m_round = 0;
    #endregion

    #region Coroutine
    IEnumerator Coroutine_DayTimeChecker()
    {
        for(int i = 20; i >= 0; i--)
        {
            UIManager.Instance.TimeLeft(i);
            yield return new WaitForSeconds(1);
        }
        StartNight();
    }
    IEnumerator Coroutine_GameStart()
    {
        yield return new WaitForSeconds(1);
        StartDay();
    }
    #endregion

    #region Methods
    void ResetRound()
    {
        m_round = 0;
    }
    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }
    public void StartDay()
    {
        if (m_light == null)
        {
            m_light = UIManager.Instance.GetLight().GetComponent<LightIntensityTween>();
        }
        StartCoroutine(Coroutine_DayTimeChecker());
        roundTime = DaynNight.Day;
        SetNextRound();
        SoundManager.Instance.DayStart();
        UGUIManager.Instance.StartRound();
        MonsterManager.Instance.ResetBossCount();
        m_light.StartDay();
    }
    public void StartNight()
    {
        roundTime = DaynNight.Night;
        SoundManager.Instance.NightStart();
        UGUIManager.Instance.StartRound();
        m_light.StartNight();
        MonsterManager.Instance.StartNight();
    }
    public void StartLobby()
    {
        ResetRound();
        SceneManager.LoadScene("LobbyScene");
        SoundManager.Instance.LobbyStart();
    }
    void SetNextRound()
    {
        m_round++;
        UIManager.Instance.RoundInfo(m_round);
        if (m_round == 15 || m_round == 30)
        {
            UGUIManager.Instance.GetStoreUI().SetItemListTable();
        }
    }
    public void LoadScene(Scene sin)
    {
        m_scene = sin;
        SceneManager.LoadScene(sin.ToString());
        if (m_scene.Equals(Scene.LobbyScene))
        {
            StartLobby();
        }
    }
    public void GameStart()
    {
        StartCoroutine(Coroutine_GameStart());
    }
    public int GetRoundInfo()
    {
        return m_round;
    }
    public DaynNight GetDayInfo()
    {
        return roundTime;
    }
    public void LoadLobbyScene()
    {
        UGUIManager.Instance.LoadLobbyScene();
        LoadScene(Scene.LobbyScene);
    }
    public void ExitGame() //게임 종료 기능
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    protected override void OnStart()
    {
        Application.targetFrameRate = 60; //타겟프레임
        if (m_scene.Equals(Scene.GameScene))   
        {
            StartDay();
        }
        else if(m_scene == Scene.LobbyScene) 
        { 
            StartLobby();
        }
    }

    #endregion
}
