using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MonsterManager : SingletonMonoBehaviour<MonsterManager> 
{
    [SerializeField]
    GameObject m_spawnPoint; //몬스터 생성위치 추후결정
    [SerializeField]
    PlayerController m_player; //플레이어 받아옴
    [SerializeField]
    UIPanel m_panel;
    [SerializeField]
    GameObject[] m_monPrefabs; //임시로 쓸몬스터 프리펩 받아오기
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
    Dictionary<MonsterType, GameObjectPool<MonsterController>> m_monsterPools = new Dictionary<MonsterType, GameObjectPool<MonsterController>>(); //몬스터풀 저장이요
    List<MonsterController> m_monsterList = new List<MonsterController>();
    GameObjectPool<HUDController> m_hudPool = new GameObjectPool<HUDController>();
    public void ResetMonster(MonsterController mon, HUDController hud)
    {
        mon.gameObject.SetActive(false); // 몬스터를 비활성화해주기
        m_monsterList.Remove(mon); //사망시 리스트에서 몬스터 제거해주기.
        hud.gameObject.SetActive(false); // hud비활성화해주기
        m_monsterPools[mon.Type].Set(mon); // 풀에 넣어주기
        m_hudPool.Set(hud); // 풀에 넣어주기
        UIManager.Instance.EnemyLeft(m_monsterList.Count);
    }
    public void CreateMonster() //몬스터를 랜덤확률로 소환하는기능
    {
        int Count = 7 + (thisRound / 2);
        if(Count>=30)
            Count= 30;
        if(thisRound % 10 != 0) //10라운드가 아닌 일반 라운드의 경우
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
                mon.tag = "Zombie"; //다시 태그를 좀비로 설정하여 맞을 수 있게끔.
               // mon.GetComponent<MonsterAnimController>().SetFloat("Speed",StatScale(thisRound));
                hud.gameObject.SetActive(true); //어차피 데미지르 ㄹ줄때 hudcontroller에서 Show()로 키니까 꺼봄  작동이 잘 안됨..
                m_monsterList.Add(mon);
            }
        }
        else //10라운드마다 나오는 보스 스테이지
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
        m_monPrefabs = Resources.LoadAll<GameObject>("Prefabs/Zombie"); //몬스터의 프리펩을 가져와
        foreach (var prefab in m_monPrefabs)
        {
            MonsterType type = (MonsterType)(int.Parse(prefab.name.Split('.')[0])); 
            GameObjectPool<MonsterController> monPool = new GameObjectPool<MonsterController>(1, () => //풀링해주기
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

        if (Input.GetKeyDown(KeyCode.Space)) //테스트용 몬스터 생성
        {
            CreateMonster(); 
        }
        if (Input.GetKeyDown(KeyCode.V)) //테스트용 라운드넘기기
        {
            SetNextRound();
        }
        if (Input.GetKeyDown(KeyCode.H)) //테스트용 부활기능.
        {
            if (m_player.gameObject.activeSelf) return;
            m_player.Revive();
        }
        for (int i = 0; i < m_monsterList.Count; i++)
        {
            m_monsterList[i].BehaviourProcess(); //각각 호출하는거보다 하나의 업데이트에서 사용하면 더 효율적임.
        }
    }

}
