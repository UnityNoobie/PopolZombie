using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : SingletonDontDestroy<UpdateManager> 
{

    GunManager gunManager;
    [SerializeField]
    Inventory m_inven;
    bool m_isactive = false;
    public PlayerController[] m_players;
    public PlayerController[] m_playersSave;
    int m_playercount;
    public void SetPlayerController(PlayerController player)
    {
        if(m_players != null) //�÷��̾��Ʈ�� ������� �ʴٸ�
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
        m_players[m_playercount] = player; //���� ���� �÷��̾���Ʈ�ѷ� �־��ְ� �÷��̾� �� �߰�.
        m_playercount++;
    }
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < m_players.Length; i++)
        {
            m_players[i].BehaviorProcess();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_isactive = !m_isactive; //�Ұ����� ��Ƽ�� ����. 
            m_inven.gameObject.SetActive(m_isactive);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_isactive)
            {
                m_isactive = false;
                m_inven.gameObject.SetActive(m_isactive);   
            }
            UIManager.Instance.CloseTabs();
        }
    }
    private void Awake()
    {
        m_playercount = 0;
        m_players = new PlayerController[m_players.Length];
    }
    private void Start()
    {
          m_inven.gameObject.SetActive(m_isactive);
    }
}
