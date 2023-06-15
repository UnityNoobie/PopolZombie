using System.Collections;
using System.Collections.Generic;
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
    LightIntensityTween m_light;
    Scene m_scene;
    DaynNight roundTime;
    int m_round = 0;
    int gameDuration = 0;
    string playerNickname;
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
        StartCoroutine(Coroutine_GameDuration()); //데이터 저장을 위한 게임 시간 누적용
    }
    IEnumerator Coroutine_GameDuration() //게임 진행시간 누적
    {
        while(m_scene == Scene.GameScene)
        {
            yield return new WaitForSeconds(1);
            gameDuration++;
            UIManager.Instance.GameDuration(gameDuration);
        }
    }
    #endregion

    #region Methods
    void ResetRound()
    {
        gameDuration = 0;
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
        StopAllCoroutines();
        ResetRound();
        SceneManager.LoadScene("LobbyScene");
        SoundManager.Instance.LobbyStart();
    }
    void SetNextRound()
    {
        m_round++;
        UIManager.Instance.RoundInfo(m_round);
        if (m_round == 10 || m_round == 20)
        {
            UGUIManager.Instance.GetStoreUI().SetItemListTable();
        }
    }
    void SaveScore() //게임 데이터 저장기능
    {
        UGUIManager.Instance.SaveData(gameDuration, m_round);
    }
    public void LoadScene(Scene sin)// 씬바꿔주기
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
    public string GetNickname()
    {
        return playerNickname;
    }
    public void LoadLobbyScene()
    {
        SaveScore();
        UGUIManager.Instance.LoadLobbyScene();
        LoadScene(Scene.LobbyScene);
    }
    public void SetNickname(string str)
    {
        playerNickname = str;
    }
    public void ExitGame() //게임 종료 기능
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    void LoadAlltable()
    {
        TableEffect.Instance.Load();
        TableGunstat.Instance.Load();
        TableArmorStat.Instance.Load();
        TableItemData.Instance.Load();
        TableMonsterStat.Instance.Load();
        TableSoundInfo.Instance.Load();
        Skilldata.Instance.Load();
        ImageLoader.Instance.Load();
    }
    protected override void OnAwake()
    {
        LoadAlltable();
    }
    protected override void OnStart()
    {
        Application.targetFrameRate = 60; //타겟프레임
        SoundManager.Instance.LobbyStart();
    }

    #endregion
}
