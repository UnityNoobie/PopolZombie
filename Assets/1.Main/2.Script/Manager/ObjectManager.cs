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

    GameObjectPool<DamageAbleObjectHUD> m_hudPool = new GameObjectPool<DamageAbleObjectHUD>();
    GameObjectPool<Barricade> m_barricadePool = new GameObjectPool<Barricade>();
    public DamageAbleObjectHUD GetHud()
    {
        return m_hudPool.Get();
    }
    public void SetHud(DamageAbleObjectHUD hud)
    {
        m_hudPool.Set(hud);
    }
    public void SetTransform()
    {
        m_uiCam = UGUIManager.Instance.GetUICam();
        m_mainCam = Camera.main;
    }
    public void SetObjectPool()
    {
        m_hudPrefab = Resources.Load<GameObject>("Prefabs/ObjectHUD");
        m_barricadePrefab = Resources.Load<GameObject>("Prefabs/Barricade");
        m_hudPool = new GameObjectPool<DamageAbleObjectHUD>(6, () =>
        {
            var obj = Instantiate(m_hudPrefab);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            var hud = obj.GetComponent<DamageAbleObjectHUD>();
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
            return bar;
        });
    }
    protected override void OnStart()
    {
        SetObjectPool();
        SetTransform();
    }


}
