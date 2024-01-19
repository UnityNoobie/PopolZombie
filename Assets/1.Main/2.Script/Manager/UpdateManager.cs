using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : SingletonMonoBehaviour<UpdateManager> //업데이트를 한곳에서 실행하기 위해 만든 클래스
{

    #region Constant and Field
    /*
    bool m_isactive = false;
    bool m_isopen = false;
    public PlayerController[] m_players;
    public PlayerGetItem[] m_playerItem;
    public PlayerController[] m_playersSave;
    int m_playercount;*/
    PlayerController m_player;
    #endregion

    #region Method
    /*
    public void SetPlayerController(PlayerController player)   
    {
        if (m_players != null) //플레이어리스트가 비어있지 않다면
        {
            m_playersSave = m_players; //기존 플레이어리스트를 받아와줌.
        }
        m_players = new PlayerController[m_playercount + 1]; //현재 들어와있는 플레이어의 수만큼 초기화
        if(m_playersSave != null) // m_players가 리셋되었으므로 m_players[i]에 기존 세이브해두었던 배열 넣어줌.
        {
            for(int i = 0; i < m_playersSave.Length; i++)
            {
                m_players[i] = m_playersSave[i];
            }
        }
        var obj = player.GetComponent<PlayerController>();
        m_players[m_playercount] = obj; //새로 들어온 플레이어컨트롤러 넣어주고 플레이어 수 추가.
        m_playerItem = new PlayerGetItem[m_players.Length];
        for(int i = 0; i < m_players.Length ; i++)
        {
            m_playerItem[i] = m_players[i].GetComponent<PlayerGetItem>();
        }
        m_playercount++;
    }*/ //처음 멀티플레이로 기획했을 때 생각 한 방식이나 현재 싱글플레이만 지원하기로 하여 폐기.
    public void SetPlayer(PlayerController player) //플레이어 지정
    {
        m_player = player;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            UGUIManager.Instance.CloseAllTabs();
            UGUIManager.Instance.GetMenuUI().ActiveUI();
        }
        if (GameManager.Instance.GetDayInfo() == DaynNight.GameOver)
            return;

        m_player.BehaviorProcess();

        if(Input.GetKeyDown(KeyCode.N)) GameManager.Instance.IncreaseWaitTime();
        
        for (int i = 0; i < MonsterManager.Instance.m_monsterList.Count; i++) //소환되어있는 몬스터에 적용
        {
            MonsterManager.Instance.m_monsterList[i].BehaviourProcess(); //각각 호출하는거보다 하나의 업데이트에서 사용하면 더 효율적
        }
        for(int i = 0; i < ObjectManager.Instance.m_towerList.Count; i++) // 포탑의 조준,발사 메커니즘
        {
            ObjectManager.Instance.m_towerList[i].AimAndFire();
        }
    }
    #endregion
}
