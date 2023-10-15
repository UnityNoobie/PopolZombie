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
    List<GameObject> m_attackAbleObject = new List<GameObject>(); //���� ������ ������Ʈ ����Ʈ
    PlayerController m_player;
    Scene m_scene; //���� ��
    DaynNight roundTime; //���� ������ ������ ����
    int m_round = 0;
    int gameDuration = 0;
    string playerNickname;
    bool m_isFirst = true;
    const int m_roundDelay = 30;
    const int m_reviveDelay = 10;
    const int m_gameFrame = 60;
    #endregion

    #region Coroutine
    IEnumerator Coroutine_DayTimeChecker() // �������� �ð��� üũ�ϰ� �����ϴ� �ڷ�ƾ.
    {
        for(int i = m_roundDelay; i >= 0; i--)
        {
            UGUIManager.Instance.GetScreenHUD().TimeLeft(i);
           // UIManager.Instance.TimeLeft(i); //������� NGUI
            yield return new WaitForSeconds(1);
        }
        StartNight();
    }
    IEnumerator Coroutine_GameStart() //���� ���۶� ����Ǵ� �ڷ�ƾ.
    {
        yield return new WaitForSeconds(1);
        StartDay();
        StartCoroutine(Coroutine_GameDuration()); //������ ������ ���� ���� �ð� ������
    }
    IEnumerator Coroutine_GameDuration() //���� ����ð� ����
    {
        while(m_scene == Scene.GameScene)
        {
            yield return new WaitForSeconds(1);
            gameDuration++;
            UGUIManager.Instance.GetScreenHUD().GameDuration(gameDuration);
            // UIManager.Instance.GameDuration(gameDuration); //���� ��� NGUI
        }
    }
    IEnumerator Coroutine_RevivePlayer(PlayerController player) //�÷��̾� ��Ȱ�� ���Ǵ� �ڷ�ƾ.
    {
        for (int i = m_reviveDelay; i > 0; i--)
        {
            UGUIManager.Instance.SystemMessageSendMessage("��Ȱ���� : " + i);
            yield return new WaitForSeconds(1);
            if (GetDayInfo().Equals(DaynNight.GameOver))
            {
                UGUIManager.Instance.SystemMessageSendMessage("��Ȱ ����.");
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
    public GameObject GetTargetObject(Vector3 dir) //����� AI���� �Ÿ�, �������� ���� ���� ����� ����.
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
    public void SetGameObject(GameObject target) //������Ʈ�� �����Ǿ��� �� ������Ʈ�� ����Ʈ�� ���.
    {
        m_attackAbleObject.Add(target);
    }
    public void DestroyTarget(GameObject target) //������Ʈ�� ���, �ı� �Ǿ��� ��� ���� ������ ����Ʈ���� ����
    {
        m_attackAbleObject.Remove(target);
    }
    public void PlayerDeath(PlayerController player) //�÷��̾ ����Ͽ��� �� �����Ⱑ �ı����� �ʾҴٸ� 10�ʵ� ��Ȱ �����ִ� �ڷ�ƾ.
    {
        StartCoroutine(Coroutine_RevivePlayer(player));
    }
    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }
    void ResetRound() //����ȯ ���� ��Ȳ���� �ʱ�ȭ�Ǿ�� �ϴ� ������
    {
        gameDuration = 0;
        m_round = 0;
        m_isFirst = true;
        m_attackAbleObject.Clear(); 
    }
    public void SetTimeScale(float time) // ���ӳ� ȿ��, �޴�UI�� ȣ�� � ���Ǵ� Ÿ�ӽ����� ���� ȿ��.
    {
        Time.timeScale = time;
    }
    public void StartDay() // ���� ���� �� �� �ð��� ȣ���ϴ� �Լ�
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
    public void StartNight() //�� �ð��� ���� �� �� ���� ȣ��
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
    public void StartLobby() //�κ�� ������
    {
        StopAllCoroutines();
        ResetRound();
        UGUIManager.Instance.GetStatusUI().ResetSlotList();
        SceneManager.LoadScene("LobbyScene");
        SoundManager.Instance.LobbyStart();
    }
    void SetNextRound() //���� ���� ����
    {
        m_round++;
        UGUIManager.Instance.GetScreenHUD().SetRoundText(m_round);
        // UIManager.Instance.RoundInfo(m_round); //���� ��� NGUI
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
    void SaveScore() //���� ������ ������
    {
        UGUIManager.Instance.SaveData(gameDuration, m_round);
    }
    public void LoadScene(Scene sin)// ���ٲ��ִ� ���.
    {
        m_scene = sin;
        SceneManager.LoadScene(sin.ToString());
        if (m_scene.Equals(Scene.LobbyScene))
        {
            StartLobby();
        }
    }
    public void GameStart() //��ŸƮ ��ư ������ ���۵Ǵ� �Լ�. ������ �������� ����.
    {
        if(m_isFirst) //�ߺ��������� �߻��ؼ� �߰��� ����
        {
            StartCoroutine(Coroutine_GameStart());
            m_isFirst = false;
        }
           
    }
    public int GetRoundInfo() //���� ������ ������ ����.
    {
        return m_round;
    }
    public DaynNight GetDayInfo() //���� ������ ������ ���� ����
    {
        return roundTime;
    }
    public string GetNickname() //�÷��̾��� �г���
    {
        return playerNickname;
    }
    public void LoadLobbyScene() //�κ�� �ε� ����
    {
        SaveScore();
        UGUIManager.Instance.LoadLobbyScene();
        LoadScene(Scene.LobbyScene);
    }
    public void SetNickname(string str) //�÷��̾��� �г��� ����.
    {
        playerNickname = str;
    }
    public void GameOver() // ������ �ı� �� ȣ��Ǵ� �Լ�.
    {
        if (roundTime.Equals(DaynNight.GameOver)) return;
        UGUIManager.Instance.GetScreenHUD().Destroyed(m_player,gameDuration,m_round);
        StopAllCoroutines();
        roundTime = DaynNight.GameOver;
    }
    public void ExitGame() //���� ���� ���
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
    void LoadAlltable() // ���۵� �� ���� �� ���Ǵ� ��� ������ ���̺��� �ε����� 
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
        Application.targetFrameRate = m_gameFrame; //Ÿ��������
        SoundManager.Instance.LobbyStart();
    }

    #endregion
}
