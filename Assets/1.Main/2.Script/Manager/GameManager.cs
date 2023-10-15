using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum DaynNight
{
    Day,
    Night,
    GameOver,
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
    List<GameObject> m_attackAbleObject = new List<GameObject>(); //공격 가능한 오브젝트 리스트
    PlayerController m_player;
    Scene m_scene; //현재 씬
    DaynNight roundTime; //현재 낮인지 밤인지 저장
    int m_round = 0;
    int gameDuration = 0;
    string playerNickname;
    bool m_isFirst = true;
    const int m_roundDelay = 30;
    const int m_reviveDelay = 10;
    const int m_gameFrame = 60;
    #endregion

    #region Coroutine
    IEnumerator Coroutine_DayTimeChecker() // 낮동안의 시간을 체크하고 제어하는 코루틴.
    {
        for(int i = m_roundDelay; i >= 0; i--)
        {
            UGUIManager.Instance.GetScreenHUD().TimeLeft(i);
           // UIManager.Instance.TimeLeft(i); //기존사용 NGUI
            yield return new WaitForSeconds(1);
        }
        StartNight();
    }
    IEnumerator Coroutine_GameStart() //게임 시작때 실행되는 코루틴.
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
            UGUIManager.Instance.GetScreenHUD().GameDuration(gameDuration);
            // UIManager.Instance.GameDuration(gameDuration); //기존 사용 NGUI
        }
    }
    IEnumerator Coroutine_RevivePlayer(PlayerController player) //플레이어 부활에 사용되는 코루틴.
    {
        for (int i = m_reviveDelay; i > 0; i--)
        {
            UGUIManager.Instance.SystemMessageSendMessage("부활까지 : " + i);
            yield return new WaitForSeconds(1);
            if (GetDayInfo().Equals(DaynNight.GameOver))
            {
                UGUIManager.Instance.SystemMessageSendMessage("부활 실패.");
                break;
            }
        }
        if (GetDayInfo() != DaynNight.GameOver)
        {
            player.Revive();
        }
    }
    #endregion

    #region Methods
    public GameObject GetTargetObject(Vector3 dir) //좀비등 AI에게 거리, 벨류값에 따라 공격 대상을 전달.
    {
        float targetValue = Mathf.Infinity;
        GameObject target = null;
        for(int i = 0; i < m_attackAbleObject.Count; i++)
        {
            if(m_attackAbleObject[i].CompareTag("Player"))
            {
                if(Vector3.Distance(m_attackAbleObject[i].transform.position,dir) < targetValue)
                {
                    targetValue = Vector3.Distance(m_attackAbleObject[i].transform.position, dir) ;
                    target = m_attackAbleObject[i]; 
                }
            }
            else if (m_attackAbleObject[i].CompareTag("Tower"))
            {
                if (Vector3.Distance(m_attackAbleObject[i].transform.position, dir) * 1.5f < targetValue)
                {
                    targetValue = Vector3.Distance(m_attackAbleObject[i].transform.position, dir) * 1.5f;
                    target = m_attackAbleObject[i];
                }
            }
            else if (m_attackAbleObject[i].CompareTag("Barricade"))
            {
                if (Vector3.Distance(m_attackAbleObject[i].transform.position, dir) * 2 < targetValue)
                {
                    targetValue = Vector3.Distance(m_attackAbleObject[i].transform.position, dir) * 2;
                    target = m_attackAbleObject[i];
                }  
            }
            else if (m_attackAbleObject[i].CompareTag("Generator"))
            {
                if (Vector3.Distance(m_attackAbleObject[i].transform.position, dir) * 3 + 10  < targetValue)
                {
                    targetValue = Vector3.Distance(m_attackAbleObject[i].transform.position, dir) * 3 + 10;
                    target = m_attackAbleObject[i];
                }
            }
        }
        return target;
    }
    public void SetGameObject(GameObject target) //오브젝트가 생성되었을 때 오브젝트를 리스트에 등록.
    {
        m_attackAbleObject.Add(target);
    }
    public void DestroyTarget(GameObject target) //오브젝트가 사망, 파괴 되었을 경우 공격 가능한 리스트에서 삭제
    {
        m_attackAbleObject.Remove(target);
    }
    public void PlayerDeath(PlayerController player) //플레이어가 사망하였을 때 발전기가 파괴되지 않았다면 10초뒤 부활 시켜주는 코루틴.
    {
        StartCoroutine(Coroutine_RevivePlayer(player));
    }
    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }
    void ResetRound() //씬전환 등의 상황에서 초기화되어야 하는 데이터
    {
        gameDuration = 0;
        m_round = 0;
        m_isFirst = true;
        m_attackAbleObject.Clear(); 
    }
    public void SetTimeScale(float time) // 게임내 효과, 메뉴UI의 호출 등에 사용되는 타임스케일 조절 효과.
    {
        Time.timeScale = time;
    }
    public void StartDay() // 밤이 지난 후 낮 시간을 호출하는 함수
    {
        if (m_light == null)
        {
            m_light = ObjectManager.Instance.GetLight().GetComponent<LightIntensityTween>();
        }
        StartCoroutine(Coroutine_DayTimeChecker());
        roundTime = DaynNight.Day;
        SetNextRound();
        SoundManager.Instance.DayStart();
        UGUIManager.Instance.StartRound();
        MonsterManager.Instance.ResetBossCount();
        m_light.StartDay();
    }
    public void StartNight() //낮 시간이 지난 후 밤 시작 호출
    {
        roundTime = DaynNight.Night;
        if(m_round % 5 == 0)
        {
            SoundManager.Instance.BossStart();
        }
        else
        {
            SoundManager.Instance.NightStart();
        }
       
        UGUIManager.Instance.StartRound();
        m_light.StartNight();
        MonsterManager.Instance.StartNight();
    }
    public void StartLobby() //로비로 씬변경
    {
        StopAllCoroutines();
        ResetRound();
        UGUIManager.Instance.GetStatusUI().ResetSlotList();
        SceneManager.LoadScene("LobbyScene");
        SoundManager.Instance.LobbyStart();
    }
    void SetNextRound() //라운드 정보 누적
    {
        m_round++;
        UGUIManager.Instance.GetScreenHUD().SetRoundText(m_round);
        // UIManager.Instance.RoundInfo(m_round); //기존 사용 NGUI
        if (m_round == 10 || m_round == 20)
        {
            UGUIManager.Instance.GetStoreUI().SetItemListTable();
        }
    }
    public void UsingCheatKey()
    {
        if(m_round < 10)
        {
            UGUIManager.Instance.GetStoreUI().SetItemListTable();
            m_round = 10;
            UGUIManager.Instance.GetStoreUI().SetItemListTable();
        }
        else if(m_round < 20)
        {
            m_round = 20;
            UGUIManager.Instance.GetStoreUI().SetItemListTable();
        }
        else
        {
            m_round += 10;
        }
        UGUIManager.Instance.GetScreenHUD().SetRoundText(m_round);
    }
    void SaveScore() //게임 데이터 저장기능
    {
        UGUIManager.Instance.SaveData(gameDuration, m_round);
    }
    public void LoadScene(Scene sin)// 씬바꿔주는 기능.
    {
        m_scene = sin;
        SceneManager.LoadScene(sin.ToString());
        if (m_scene.Equals(Scene.LobbyScene))
        {
            StartLobby();
        }
    }
    public void GameStart() //스타트 버튼 누르면 시작되는 함수. 데이터 누적등의 시작.
    {
        if(m_isFirst) //중복실행현상 발생해서 추가한 변수
        {
            StartCoroutine(Coroutine_GameStart());
            m_isFirst = false;
        }
           
    }
    public int GetRoundInfo() //현재 라운드의 정보를 전달.
    {
        return m_round;
    }
    public DaynNight GetDayInfo() //현재 라운드의 낮과밤 상태 전달
    {
        return roundTime;
    }
    public string GetNickname() //플레이어의 닉네임
    {
        return playerNickname;
    }
    public void LoadLobbyScene() //로비씬 로드 시작
    {
        SaveScore();
        UGUIManager.Instance.LoadLobbyScene();
        LoadScene(Scene.LobbyScene);
    }
    public void SetNickname(string str) //플레이어의 닉네임 설정.
    {
        playerNickname = str;
    }
    public void GameOver() // 발전기 파괴 시 호출되는 함수.
    {
        if (roundTime.Equals(DaynNight.GameOver)) return;
        UGUIManager.Instance.GetScreenHUD().Destroyed(m_player,gameDuration,m_round);
        StopAllCoroutines();
        roundTime = DaynNight.GameOver;
    }
    public void ExitGame() //게임 종료 기능
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    void LoadAlltable() // 시작될 떄 게임 내 사용되는 모든 데이터 테이블을 로드해줌 
    {
        TableEffect.Instance.Load();
        TableGunstat.Instance.Load();
        TableArmorStat.Instance.Load();
        TableItemData.Instance.Load();
        TableMonsterStat.Instance.Load();
        TableSoundInfo.Instance.Load();
        Skilldata.Instance.Load();
        ImageLoader.Instance.Load();
        TableObjectStat.Instance.Load();
    }
    protected override void OnAwake()
    {
        LoadAlltable();
    }
    protected override void OnStart()
    {
        Application.targetFrameRate = m_gameFrame; //타겟프레임
        SoundManager.Instance.LobbyStart();
    }

    #endregion
}
