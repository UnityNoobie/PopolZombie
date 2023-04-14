using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class UpdateManager : SingletonDontDestroy<UpdateManager> 
{

    GunManager gunManager;
    [SerializeField]
    Inventory m_inven;
    bool m_isactive = false;
    bool m_skillactive = false;
    public PlayerController[] m_players;
    public PlayerController[] m_playersSave;
    int m_playercount;
    int testweapon;
    public void SetPlayerController(PlayerController player)
    {
        if(m_players != null) //플레이어리스트가 비어있지 않다면
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
            m_isactive = !m_isactive; //불값으로 액티브 변경. 
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
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (m_skillactive)
            {
                m_skillactive = false;
            }
            else
            {
                m_skillactive = true;
            }
            UIManager.Instance.SkillUIChange(m_skillactive);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.Instance.SystemMessageCantOpen("무기교체 기능 잠금되었습니다. GunManager스크립트와 UpdateManager 확인 바람.");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIManager.Instance.SystemMessageCantOpen("무기교체 기능 잠금되었습니다. GunManager스크립트와 UpdateManager 확인 바람.");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            UIManager.Instance.MoneyChange(10000);
            UIManager.Instance.SystemMessageCantOpen("돈치트 실행 돈 + 10000");
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
