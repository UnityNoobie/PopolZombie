using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DaynNight
{
    Day,
    Night,
    MAX
}
public class GameManager : SingletonDontDestroy<GameManager>
{
    #region Constants and Field
    [SerializeField]
    LightIntensityTween Light;
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
    #endregion

    #region Methods
    public void StartDay()
    {
        StartCoroutine(Coroutine_DayTimeChecker());
        roundTime = DaynNight.Day;
        SetNextRound();
        SoundManager.Instance.DayStart();
        UGUIManager.Instance.StartRound();
        MonsterManager.Instance.ResetBossCount();
        Light.StartDay();
    }
    public void StartNight()
    {
        roundTime = DaynNight.Night;
        SoundManager.Instance.NightStart();
        UGUIManager.Instance.StartRound();
        Light.StartNight();
        MonsterManager.Instance.StartNight();
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
    public int GetRoundInfo()
    {
        return m_round;
    }
    public DaynNight GetDayInfo()
    {
        return roundTime;
    }
    protected override void OnStart()
    {
        Application.targetFrameRate = 60; //≈∏∞Ÿ«¡∑π¿”
        StartDay();
    }
    #endregion
}
