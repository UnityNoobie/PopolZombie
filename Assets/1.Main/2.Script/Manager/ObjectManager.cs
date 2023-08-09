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

    float m_bariRotation = 0f; //�ٸ����̵� ��ġ �� ����� Rotation��.
    float m_hudRotation = 0f;
    int m_id;


    ObjectStat m_obejctStat = new ObjectStat();
    #endregion

    #region Coroutine
    IEnumerator Coroutine_PreviewBuilding() //Update ��� ����ϴ� Coroutine. ���콺�� ��ġ�� ���� ȭ�鿡 Preview Object�� ���󰡵��� ��ġ�Ͽ� ��Ÿũ����Ʈ ���� �Ǽ� ȿ�� �ַ� �������.
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
    public ObjectStat GetObjectStat(ObjectType type) //Object�� ��ġ � ����ϴ� ObjectStat�� Table���� �������� �޼ҵ�. ObjectType���� Key���� �־� ������.
    {
        var stat = m_obejctStat.GetObjectStatus(type);
        return stat;
    }
    public void SetGenerator(Generator generator) // ��������� ��ȣ�ۿ��� ���� ��ġ�� �����⸦ Ŭ������ �������־� ����.
    {
        m_generator = generator;
    }
    public Generator GetGenerator() //�����⸦ �ʿ�� �ϴ� ��ũ��Ʈ�� ������.
    {
        return m_generator;
    }
    public void StartPreviewBuild() //�÷��̾ �Ǽ� ��忡 ���� �� Coroutine �����Ͽ� Preview Object�� Ȱ��ȭ����.
    {
        StartCoroutine(Coroutine_PreviewBuilding());
    }
    public void BuildObject() //�ش� ��ҿ� �ٸ� ������Ʈ, ��� ���� �ִ��� Ȯ���Ͽ� �Ǽ��� Ȯ���ϴ� �޼ҵ�
    {
        if (preview.IsCanBuild())
        {
            if (m_id == 3) BuildBarricade();
            else if (m_id == 4) BuildTurret();
            m_playerC.BuildingConvert();
        }
        else
        {
            UGUIManager.Instance.SystemMessageSendMessage("�������� ���� ����Դϴ�. ����ִ� ��ġ�� �ٽ� �Ǽ��� �ּ���.");
        }
        
    }
    public void BuildBarricade() //�ش� ��ҿ� �ٸ����̵� ��ġ. �ٸ����̵�� ���簢�� ������Ʈ�μ� ȸ������ ��ġ�ϴ� ����� �� �پ�����.
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
    public void BuildTurret() //��ž ��ġ.
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
    public void StopBuilding() // Preview Object�� ��Ȱ��ȭ �ϰ� �ڷ�ƾ ����.
    {
        preview.DeActive();
        StopAllCoroutines();
    }
    public void SetPreviewObject(int id) //������ id���� ���� Preview Object�� ����.
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
    public void RotationChanger() //�ٸ����̵� Rotation �ٲ��ִ� ���. HUD�� �Բ� ����־� �ݴ�� ���� ���� ������.
    {
        m_bariRotation += 90f;
        m_hudRotation -= 90f;
    }

    public DamageAbleObjectHUD GetHudObject() //������Ʈ Ǯ���� �س��� HUD�� �������ִ� �޼ҵ�.
    {
        if(m_hudPool.Count == 0)
        {
            SetObjectPool();
        }
        DamageAbleObjectHUD hud = m_hudPool.Get();
        return hud;
    }
    public void SetHud(DamageAbleObjectHUD hud) //����� ���� HUD�� Ǯ�� �־��ִ� ���.
    {
        m_hudPool.Set(hud);
    }
    public void SetBarricade(Barricade obj) //����� ���� Barricade Object�� Ǯ�� �־���.
    {
        m_playerObject.DestroyedBarricade(obj);
        m_barricadePool.Set(obj);
    }
    public void SetGunTower(TowerController obj) //����� ���� Turret Object�� Ǯ�� �־���.
    {
        m_towerList.Remove(obj);
        m_playerObject.DestroyedTurret(obj);
        m_towerPool.Set(obj);

    }
    public void SetPlayer(PlayerSkillController player) //�÷��̾� ����.
    {
        m_player = player;
        m_playerC = m_player.GetComponent<PlayerController>();
        m_playerObject = m_player.GetComponent<PlayerObjectController>();
    }
    public void SetObjectPool() // ���� �� ���Ǵ� HUD, �ٸ����̵�, ��ž ���� ������Ʈ�� Ǯ������.
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
    protected override void OnStart() //�����Ҷ� Ǯ�� ����
    {
        SetObjectPool();
    }

    #endregion
}
