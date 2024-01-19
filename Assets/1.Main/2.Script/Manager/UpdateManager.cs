using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : SingletonMonoBehaviour<UpdateManager> //������Ʈ�� �Ѱ����� �����ϱ� ���� ���� Ŭ����
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
        if (m_players != null) //�÷��̾��Ʈ�� ������� �ʴٸ�
        {
            m_playersSave = m_players; //���� �÷��̾��Ʈ�� �޾ƿ���.
        }
        m_players = new PlayerController[m_playercount + 1]; //���� �����ִ� �÷��̾��� ����ŭ �ʱ�ȭ
        if(m_playersSave != null) // m_players�� ���µǾ����Ƿ� m_players[i]�� ���� ���̺��صξ��� �迭 �־���.
        {
            for(int i = 0; i < m_playersSave.Length; i++)
            {
                m_players[i] = m_playersSave[i];
            }
        }
        var obj = player.GetComponent<PlayerController>();
        m_players[m_playercount] = obj; //���� ���� �÷��̾���Ʈ�ѷ� �־��ְ� �÷��̾� �� �߰�.
        m_playerItem = new PlayerGetItem[m_players.Length];
        for(int i = 0; i < m_players.Length ; i++)
        {
            m_playerItem[i] = m_players[i].GetComponent<PlayerGetItem>();
        }
        m_playercount++;
    }*/ //ó�� ��Ƽ�÷��̷� ��ȹ���� �� ���� �� ����̳� ���� �̱��÷��̸� �����ϱ�� �Ͽ� ���.
    public void SetPlayer(PlayerController player) //�÷��̾� ����
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
        
        for (int i = 0; i < MonsterManager.Instance.m_monsterList.Count; i++) //��ȯ�Ǿ��ִ� ���Ϳ� ����
        {
            MonsterManager.Instance.m_monsterList[i].BehaviourProcess(); //���� ȣ���ϴ°ź��� �ϳ��� ������Ʈ���� ����ϸ� �� ȿ����
        }
        for(int i = 0; i < ObjectManager.Instance.m_towerList.Count; i++) // ��ž�� ����,�߻� ��Ŀ����
        {
            ObjectManager.Instance.m_towerList[i].AimAndFire();
        }
    }
    #endregion
}
