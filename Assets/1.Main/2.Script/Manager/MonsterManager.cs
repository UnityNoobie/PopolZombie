using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MonsterManager : SingletonMonoBehaviour<MonsterManager> 
{
    [SerializeField]
    GameObject m_spawnPoint; //���� ������ġ ���İ���
    [SerializeField]
    PlayerController m_player; //�÷��̾� �޾ƿ�
    [SerializeField]
    UIPanel m_panel;
    [SerializeField]
    GameObject[] m_monPrefabs; //�ӽ÷� ������ ������ �޾ƿ���
    [SerializeField]
    GameObject m_hudPrefab;
    [SerializeField]
    public static int thisRound = 0;
    [SerializeField]
    GameObject m_hudmanager;
    [SerializeField]
    StoreUI m_storeUI;
    Camera m_mainCam;
    Camera m_uiCam;
    MonsterController m_monctr;
    void SetNextRound()
    {
        thisRound++;
        UIManager.Instance.RoundInfo(thisRound);
        if(thisRound == 15 || thisRound == 30 )
        {
            m_storeUI.SetItemListTable();
        }
    }
    float StatScale(int ThisRound)
    {
        float StatScale = 1 + (ThisRound * 0.05f);
        return StatScale;
    }
    Dictionary<MonsterType, GameObjectPool<MonsterController>> m_monsterPools = new Dictionary<MonsterType, GameObjectPool<MonsterController>>(); //����Ǯ �����̿�
    List<MonsterController> m_monsterList = new List<MonsterController>();
    GameObjectPool<HUDController> m_hudPool = new GameObjectPool<HUDController>();
    public void ResetMonster(MonsterController mon, HUDController hud)
    {
        mon.gameObject.SetActive(false); // ���͸� ��Ȱ��ȭ���ֱ�
        m_monsterList.Remove(mon); //����� ����Ʈ���� ���� �������ֱ�.
        hud.gameObject.SetActive(false); // hud��Ȱ��ȭ���ֱ�
        m_monsterPools[mon.Type].Set(mon); // Ǯ�� �־��ֱ�
        m_hudPool.Set(hud); // Ǯ�� �־��ֱ�
        UIManager.Instance.EnemyLeft(m_monsterList.Count);
    }
    public void CreateMonster() //���͸� ����Ȯ���� ��ȯ�ϴ±��
    {
        int Count = 7 + (thisRound / 2);
        if(Count>=30)
            Count= 30;
        if(thisRound % 10 != 0) //10���尡 �ƴ� �Ϲ� ������ ���
        {
            for (int i = 0; i < Count; i++)
            {
                var mon = m_monsterPools[(MonsterType)Random.Range(0, (int)MonsterType.Boss)].Get();
                var hud = m_hudPool.Get();
                MonsterType type = mon.Type;
                mon.transform.position = transform.position;
                mon.gameObject.SetActive(true);
                mon.SetStatus(type, StatScale(thisRound));
                hud.SetHUD(mon.DummyHud.transform, mon.GetStatus.name);
                mon.SetMonster(m_player,hud);
                mon.tag = "Zombie"; //�ٽ� �±׸� ����� �����Ͽ� ���� �� �ְԲ�.
               // mon.GetComponent<MonsterAnimController>().SetFloat("Speed",StatScale(thisRound));
                hud.gameObject.SetActive(true); //������ �������� ���ٶ� hudcontroller���� Show()�� Ű�ϱ� ����  �۵��� �� �ȵ�..
                m_monsterList.Add(mon);
            }
        }
        else //10���帶�� ������ ���� ��������
        {
            var hud = m_hudPool.Get();
            var mon = m_monsterPools[MonsterType.Boss].Get();
            mon.tag = "Zombie";
            MonsterType type = mon.Type;
            mon.transform.position = transform.position;
            mon.gameObject.SetActive(true);
            mon.SetStatus(type, StatScale(thisRound));
            hud.SetHUD(mon.DummyHud.transform, mon.GetStatus.name);
         //   mon.GetComponent<MonsterAnimController>().SetFloat("Speed", StatScale(thisRound));
            mon.SetMonster(m_player, hud);
            m_monsterList.Add(mon);
        }
        UIManager.Instance.EnemyLeft(m_monsterList.Count);

    }
    protected override void OnStart()
    {
        thisRound = 0;
        m_mainCam = Camera.main;
        m_uiCam = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        m_monPrefabs = Resources.LoadAll<GameObject>("Prefabs/Zombie"); //������ �������� ������
        foreach (var prefab in m_monPrefabs)
        {
            MonsterType type = (MonsterType)(int.Parse(prefab.name.Split('.')[0])); 
            GameObjectPool<MonsterController> monPool = new GameObjectPool<MonsterController>(1, () => //Ǯ�����ֱ�
            {
                var obj = Instantiate(prefab);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                obj.SetActive(false);
                var mon = obj.GetComponent<MonsterController>();
                mon.InitMonster(type);
                return mon;
            });
            m_monsterPools.Add(type, monPool);
        }
        m_hudPool = new GameObjectPool<HUDController>(5, () =>
        {
            var obj = Instantiate(m_hudPrefab);
            obj.transform.SetParent(m_hudmanager.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);
            var hud = obj.GetComponent<HUDController>();
            hud.InitHUD(m_mainCam, m_uiCam);
            hud.gameObject.SetActive(false);
            return hud;
        });
        
    }




    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) //�׽�Ʈ�� ���� ����
        {
            CreateMonster(); 
        }
        if (Input.GetKeyDown(KeyCode.V)) //�׽�Ʈ�� ����ѱ��
        {
            SetNextRound();
        }
        if (Input.GetKeyDown(KeyCode.H)) //�׽�Ʈ�� ��Ȱ���.
        {
            if (m_player.gameObject.activeSelf) return;
            m_player.Revive();
        }
        for (int i = 0; i < m_monsterList.Count; i++)
        {
            m_monsterList[i].BehaviourProcess(); //���� ȣ���ϴ°ź��� �ϳ��� ������Ʈ���� ����ϸ� �� ȿ������.
        }
    }

}
