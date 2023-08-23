using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MonsterManager : SingletonMonoBehaviour<MonsterManager> 
{
    #region Constants and Fields
    SpawnPos[] m_spawnPoint; //���� ������ġ ���İ���
    [SerializeField]
    PlayerController m_player; //�÷��̾� �޾ƿ�
    [SerializeField]
    UIPanel m_panel;
    [SerializeField]
    GameObject[] m_monPrefabs; //�ӽ÷� ������ ������ �޾ƿ���
    [SerializeField]
    GameObject m_hudPrefab;
    [SerializeField]
    GameObject m_hudmanager;
    [SerializeField]
    StoreUI m_storeUI;
    Camera m_mainCam;
    Camera m_uiCam;
    MonsterController m_monctr;
    int currentBossCound = 0;
    Dictionary<MonsterType, GameObjectPool<MonsterController>> m_monsterPools = new Dictionary<MonsterType, GameObjectPool<MonsterController>>(); //����Ǯ �����̿�
    public List<MonsterController> m_monsterList = new List<MonsterController>();
    GameObjectPool<HUDController> m_hudPool = new GameObjectPool<HUDController>();
    bool isSpawning = false;
    const int m_maxSpawnMonster = 20;
    #endregion

    #region Coroutine
    IEnumerator Coroutine_SpawnMonsters() //���� ���� �� üũ
    {
        isSpawning = true;
        yield return new WaitForSeconds(5);
        for (int i = 0; i < 3; i++)
        {
            CreateMonster();
            yield return new WaitForSeconds(5);
        }
        isSpawning=false;
        StartCoroutine(CorouTine_CheckRoundEnd());
    }
    IEnumerator CorouTine_CheckRoundEnd() //���Ͱ� ��� ����ߴ��� Ȯ���ϴ� �޼���
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            if(m_monsterList.Count <= 0)
            {
                break;
            }
        }
        GameManager.Instance.StartDay();
    }
    #endregion

    #region Methods
    public void ResetMonster(MonsterController mon, HUDController hud)
    {
        mon.gameObject.SetActive(false); // ���͸� ��Ȱ��ȭ���ֱ�
        m_monsterList.Remove(mon); //����� ����Ʈ���� ���� �������ֱ�.
        hud.gameObject.SetActive(false); // hud��Ȱ��ȭ���ֱ�
        m_monsterPools[mon.Type].Set(mon); // Ǯ�� �־��ֱ�
        m_hudPool.Set(hud); // Ǯ�� �־��ֱ�
        UIManager.Instance.EnemyLeft(m_monsterList.Count);
    }
    public void CreateMonster() //���͸� ����Ȯ���� ��ȯ
    {
        int a = GameManager.Instance.GetRoundInfo();
        int Count = 5 + (a / 2);
        if(Count>=m_maxSpawnMonster) //�ִ� ���� ���� ����.
            Count= m_maxSpawnMonster;
        for (int i = 0; i < Count; i++)
        {
                var mon = m_monsterPools[(MonsterType)Random.Range(0, (int)MonsterType.Boss)].Get();
                var hud = m_hudPool.Get();
                MonsterType type = mon.Type;
                mon.transform.position = m_spawnPoint[Random.Range(0, m_spawnPoint.Length)].transform.position;
                mon.gameObject.SetActive(true);
                mon.SetStatus(type, StatScale());
                hud.SetHUD(mon.DummyHud.transform, mon.GetStatus.name);
                mon.SetMonster(m_player,hud);
                mon.tag = "Zombie"; //�ٽ� �±׸� ����� �����Ͽ� ���� �� �ְԲ�.
               // mon.GetComponent<MonsterAnimController>().SetFloat("Speed",StatScale(thisRound));
                hud.gameObject.SetActive(true); //������ �������� �ٶ� hudcontroller���� Show()�� Ű�ϱ� ����  �۵��� �� �ȵ�..
                m_monsterList.Add(mon);
        }
        if(GameManager.Instance.GetRoundInfo() % 5 == 0) //10���帶�� ������ ���� ��������
        {
            if(currentBossCound < MaxBossCount())
            {
                var hud = m_hudPool.Get();
                var mon = m_monsterPools[MonsterType.Boss].Get();
                mon.tag = "Zombie";
                MonsterType type = mon.Type;
                mon.transform.position = m_spawnPoint[Random.Range(0, m_spawnPoint.Length)].transform.position;
                mon.gameObject.SetActive(true);
                mon.SetStatus(type, StatScale());
                hud.SetHUD(mon.DummyHud.transform, mon.GetStatus.name);
                mon.SetMonster(m_player, hud);
                m_monsterList.Add(mon);
                currentBossCound++;
            }        
        }
        UIManager.Instance.EnemyLeft(m_monsterList.Count);
    }
    public void StartNight()
    {
        StartCoroutine(Coroutine_SpawnMonsters());
    }
    int MaxBossCount()
    {
        return 1 + GameManager.Instance.GetRoundInfo() / 10;
    }
    float StatScale()
    {
        float StatScale = 1 + (GameManager.Instance.GetRoundInfo() * 0.1f);
        return StatScale;
    }
    public void ResetBossCount()
    {
        currentBossCound = 0;
    }
    protected override void OnStart()
    {
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
        m_spawnPoint = GetComponentsInChildren<SpawnPos>();
    }
    #endregion
}
