using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObjectType
{
    Generator,
    Barricade,
    Turret,
    Max
}
public class ObjectManager : SingletonMonoBehaviour<ObjectManager> 
{
    Camera m_mainCam;
    Camera m_uiCam;
    [SerializeField]
    GameObject m_hudPrefab;
    [SerializeField]
    GameObject m_barricadePrefab;
    GameObject m_previewBarricade;

    GameObject preview;
    GameObjectPool<DamageAbleObjectHUD> m_hudPool = new GameObjectPool<DamageAbleObjectHUD>();
    GameObjectPool<Barricade> m_barricadePool = new GameObjectPool<Barricade>();

    float m_bariRotation = 0f;
    float m_hudRotation = 0f;

    
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
                preview.transform.localEulerAngles = new Vector3(0f,m_bariRotation,0f);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void StartPreviewBuild()
    {
        StartCoroutine(Coroutine_PreviewBuilding());
    }
    public void BuildBarricade()
    {
        StopBuilding();
        preview.SetActive(false);
        var obj = m_barricadePool.Get();
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            obj.BuildBarricade(200, 30, new Vector3(pointTolook.x, 0f, pointTolook.z), m_bariRotation, m_hudRotation);
        }
    }
    public void StopBuilding()
    {
        StopAllCoroutines();
    }
    public void RotationChanger()
    {
        m_bariRotation += 90f;
        m_hudRotation -= 90f;
    }

    public DamageAbleObjectHUD GetHud()
    {
        DamageAbleObjectHUD hud = m_hudPool.Get();
        return hud;
    }
    public void SetHud(DamageAbleObjectHUD hud)
    {
        m_hudPool.Set(hud);
    }
    public void SetBarricade(Barricade obj)
    {
        m_barricadePool.Set(obj);
    }
    public void SetTransform()
    {
        m_uiCam = UGUIManager.Instance.GetUICam();
        m_mainCam = Camera.main;
    }
    public void SetObjectPool()
    {
        m_hudPrefab = Resources.Load<GameObject>("Prefabs/HUDCanvas");
        m_barricadePrefab = Resources.Load<GameObject>("Prefabs/Barricade");
        m_previewBarricade = Resources.Load<GameObject>("Prefabs/Preview_Barricade");
        preview = Instantiate(m_previewBarricade);
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
            bar.SetTransform();
            return bar;
        });
    }
    protected override void OnStart()
    {
        SetTransform();
        SetObjectPool();
    }


}
