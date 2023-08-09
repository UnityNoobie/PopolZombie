using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum ObjectType
{
    Player,
    Generator,
    Barricade,
    Turret,
    Max
}

public class ObjectManager : SingletonMonoBehaviour<ObjectManager> 
{

    #region Constants and Fields
    [SerializeField]
    GameObject m_hudPrefab;
    [SerializeField]
    GameObject m_barricadePrefab;
    [SerializeField]
    GameObject m_gunTurretPrefab;
    GameObject m_previewBarricade;
    GameObject m_previewGunTurret;
    Generator m_generator;
    [SerializeField]
    PreviewObject preview;

    GameObjectPool<DamageAbleObjectHUD> m_hudPool = new GameObjectPool<DamageAbleObjectHUD>();
    GameObjectPool<Barricade> m_barricadePool = new GameObjectPool<Barricade>();
    GameObjectPool<TowerController> m_towerPool = new GameObjectPool<TowerController>();
    public List<TowerController> m_towerList = new List<TowerController>();
    PlayerSkillController m_player;
    PlayerObjectController m_playerObject;
    PlayerController m_playerC;

    float m_bariRotation = 0f; //바리케이드 설치 시 사용할 Rotation값.
    float m_hudRotation = 0f;
    int m_id;


    ObjectStat m_obejctStat = new ObjectStat();
    #endregion

    #region Coroutine
    IEnumerator Coroutine_PreviewBuilding() //Update 대신 사용하는 Coroutine. 마우스의 위치에 따라 화면에 Preview Object가 따라가도록 배치하여 스타크래프트 등의 건설 효과 주려 노력했음.
    {
        preview.ActiveOBJ();
        while (true)
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;
            if (GroupPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointTolook = cameraRay.GetPoint(rayLength);
                preview.gameObject.transform.position = new Vector3(pointTolook.x, 0f, pointTolook.z);
                if (m_id == 3)
                {
                    preview.gameObject.transform.localEulerAngles = new Vector3(0f, m_bariRotation, 0f);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region Methods
    public ObjectStat GetObjectStat(ObjectType type) //Object의 설치 등에 사용하는 ObjectStat을 Table에서 가져오는 메소드. ObjectType별로 Key값을 주어 가져옴.
    {
        var stat = m_obejctStat.GetObjectStatus(type);
        return stat;
    }
    public void SetGenerator(Generator generator) // 발전기와의 상호작용을 위해 설치한 발전기를 클래스를 가져와주어 보관.
    {
        m_generator = generator;
    }
    public Generator GetGenerator() //발전기를 필요로 하는 스크립트에 보내줌.
    {
        return m_generator;
    }
    public void StartPreviewBuild() //플레이어가 건설 모드에 들어갔을 때 Coroutine 실행하여 Preview Object를 활성화해줌.
    {
        StartCoroutine(Coroutine_PreviewBuilding());
    }
    public void BuildObject() //해당 장소에 다른 오브젝트, 배경 등이 있는지 확인하여 건설을 확정하는 메소드
    {
        if (preview.IsCanBuild())
        {
            if (m_id == 3) BuildBarricade();
            else if (m_id == 4) BuildTurret();
            m_playerC.BuildingConvert();
        }
        else
        {
            UGUIManager.Instance.SystemMessageSendMessage("적합하지 않은 장소입니다. 비어있는 위치에 다시 건설해 주세요.");
        }
        
    }
    public void BuildBarricade() //해당 장소에 바리케이드 설치. 바리케이드는 직사각형 오브젝트로서 회전시켜 설치하는 기능이 더 붙어있음.
    {
        StopBuilding();
        var obj = m_barricadePool.Get();
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            obj.BuildBarricade(new Vector3(pointTolook.x, 0f, pointTolook.z), m_bariRotation, m_hudRotation, m_player.GetPlayerSkillData(), GetObjectStat(ObjectType.Barricade));
            m_playerObject.BuildBarricade(obj);
            m_playerC.ObjcetBuildSuccesed(2);
        }
    }
    public void BuildTurret() //포탑 설치.
    {
        StopBuilding();
        var obj = m_towerPool.Get();
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            m_playerC.ObjcetBuildSuccesed(3);
            obj.BuildTurretObject(new Vector3(pointTolook.x, 0f, pointTolook.z),m_player.GetPlayerSkillData(),GetObjectStat(ObjectType.Turret));
            m_playerObject.BuildTurret(obj);
            m_towerList.Add(obj);
        }

    }
    public void StopBuilding() // Preview Object를 비활성화 하고 코루틴 종료.
    {
        preview.DeActive();
        StopAllCoroutines();
    }
    public void SetPreviewObject(int id) //들어오는 id값에 따라 Preview Object를 변경.
    {
        m_id = id;
        if (m_id == 3)
        {
            preview = null;
            preview = Instantiate(m_previewBarricade).GetComponent<PreviewObject>();
        }
        else if(m_id == 4)
        {
            preview = null;
            preview = Instantiate(m_previewGunTurret).GetComponent<PreviewObject>();
        }
    }
    public void RotationChanger() //바리케이드 Rotation 바꿔주는 기능. HUD도 함께 들어있어 반대로 방향 같이 돌려줌.
    {
        m_bariRotation += 90f;
        m_hudRotation -= 90f;
    }

    public DamageAbleObjectHUD GetHudObject() //오브젝트 풀링을 해놓은 HUD를 전달해주는 메소드.
    {
        if(m_hudPool.Count == 0)
        {
            SetObjectPool();
        }
        DamageAbleObjectHUD hud = m_hudPool.Get();
        return hud;
    }
    public void SetHud(DamageAbleObjectHUD hud) //사용을 끝낸 HUD를 풀에 넣어주는 기능.
    {
        m_hudPool.Set(hud);
    }
    public void SetBarricade(Barricade obj) //사용을 끝낸 Barricade Object를 풀에 넣어줌.
    {
        m_playerObject.DestroyedBarricade(obj);
        m_barricadePool.Set(obj);
    }
    public void SetGunTower(TowerController obj) //사용을 끝낸 Turret Object를 풀에 넣어줌.
    {
        m_towerList.Remove(obj);
        m_playerObject.DestroyedTurret(obj);
        m_towerPool.Set(obj);

    }
    public void SetPlayer(PlayerSkillController player) //플레이어 설정.
    {
        m_player = player;
        m_playerC = m_player.GetComponent<PlayerController>();
        m_playerObject = m_player.GetComponent<PlayerObjectController>();
    }
    public void SetObjectPool() // 게임 내 사용되는 HUD, 바리케이드, 포탑 등의 오브젝트를 풀링해줌.
    {
        m_hudPrefab = Resources.Load<GameObject>("Prefabs/HUDCanvas");
        m_barricadePrefab = Resources.Load<GameObject>("Prefabs/Barricade");
        m_gunTurretPrefab = Resources.Load<GameObject>("Prefabs/GunTower");
        m_previewBarricade = Resources.Load<GameObject>("Prefabs/Preview_Barricade");
        m_previewGunTurret = Resources.Load<GameObject>("Prefabs/PreviewGunTower");
        SetPreviewObject(3);
        preview.transform.localScale = Vector3.one;
        preview.gameObject.SetActive(false);
        
        m_hudPool = new GameObjectPool<DamageAbleObjectHUD>(6, () =>
        {
            var obj = Instantiate(m_hudPrefab);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            var hud = obj.GetComponent<DamageAbleObjectHUD>();
            hud.SetTransform(transform);
            hud.SetHUD();
            obj.gameObject.SetActive(false);
            return hud;
        });
        m_barricadePool = new GameObjectPool<Barricade>(5, () =>
        {
            var obj = Instantiate(m_barricadePrefab);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(false);
            var bar = obj.GetComponent<Barricade>();
            //bar.SetTransform();
            return bar;
        });
        m_towerPool = new GameObjectPool<TowerController>(5, () =>
        {
            var obj = Instantiate(m_gunTurretPrefab);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(false);
            var top = obj.GetComponent<TowerController>();
            return top;
        });
    }
    protected override void OnStart() //시작할때 풀링 실행
    {
        SetObjectPool();
    }

    #endregion
}
