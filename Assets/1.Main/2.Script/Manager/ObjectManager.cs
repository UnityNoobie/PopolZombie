using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ObjectManager : SingletonMonoBehaviour<ObjectManager> 
{

    [SerializeField]
    GameObject m_hudPrefab;
    [SerializeField]
    GameObject m_barricadePrefab;
    [SerializeField]
    GameObject m_gunTurretPrefab;
    GameObject m_previewBarricade;
    GameObject m_previewGunTurret;

    [SerializeField]
    GameObject preview;

    GameObjectPool<DamageAbleObjectHUD> m_hudPool = new GameObjectPool<DamageAbleObjectHUD>();
    GameObjectPool<Barricade> m_barricadePool = new GameObjectPool<Barricade>();
    GameObjectPool<TowerController> m_towerPool = new GameObjectPool<TowerController>();
    PlayerSkillController m_player;
    PlayerObjectController m_playerObject;

    float m_bariRotation = 0f;
    float m_hudRotation = 0f;
    int m_id;

    ObjectStat m_obejctStat = new ObjectStat(); 

    IEnumerator Coroutine_PreviewBuilding()
    {
        preview.SetActive(true);
        while (true)
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;
            if (GroupPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointTolook = cameraRay.GetPoint(rayLength);
                preview.transform.position = new Vector3(pointTolook.x, 0f, pointTolook.z);
                if (m_id == 3)
                {
                    preview.transform.localEulerAngles = new Vector3(0f, m_bariRotation, 0f);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public ObjectStat GetObjectStat(ObjectType type)
    {
        var stat = m_obejctStat.GetObjectStatus(type);
        return stat;
    }
    public void StartPreviewBuild()
    {
        StartCoroutine(Coroutine_PreviewBuilding());
    }
    public void BuildObject()
    {
        if (m_id == 3) BuildBarricade();
        else if (m_id == 4) BuildTurret();
    }
    public void BuildBarricade()
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
        }
    }
    public void BuildTurret()
    {
        StopBuilding();
        var obj = m_towerPool.Get();
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            obj.BuildTurretObject(new Vector3(pointTolook.x, 0f, pointTolook.z),m_player.GetPlayerSkillData(),GetObjectStat(ObjectType.Turret));
        }

    }
    public void StopBuilding()
    {
        preview.SetActive(false);
        StopAllCoroutines();
    }
    public void SetPreviewObject(int id)
    {
        m_id = id;
        if (m_id == 3)
        {
            preview = null;
            preview = Instantiate(m_previewBarricade);
        }
        else if(m_id == 4)
        {
            preview = null;
            preview = Instantiate(m_previewGunTurret);
        }
    }
    public void RotationChanger()
    {
        m_bariRotation += 90f;
        m_hudRotation -= 90f;
    }

    public DamageAbleObjectHUD GetHud()
    {
        if(m_hudPool.Count == 0)
        {
            SetObjectPool();
        }
        DamageAbleObjectHUD hud = m_hudPool.Get();
        return hud;
    }
    public void SetHud(DamageAbleObjectHUD hud)
    {
        m_hudPool.Set(hud);
    }
    public void SetBarricade(Barricade obj)
    {
        m_playerObject.DestroyedBarricade(obj);
        m_barricadePool.Set(obj);
    }
    public void SetGunTower(TowerController obj)
    {
        m_playerObject.DestroyedTurret(obj);
        m_towerPool.Set(obj);
    }
    public void SetPlayer(PlayerSkillController player)
    {
        m_player = player;
        m_playerObject = m_player.GetComponent<PlayerObjectController>();
    }
    public void SetObjectPool()
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
            //top.SetTransform();
            return top;
        });
    }
    protected override void OnStart()
    {
        SetObjectPool();
    }


}
