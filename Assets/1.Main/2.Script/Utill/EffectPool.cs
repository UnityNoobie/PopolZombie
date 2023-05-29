using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : SingletonMonoBehaviour<EffectPool> 
{
    #region Constants and Fields
    [SerializeField]
    int m_presetSize = 1;
    List<string> m_listEffectNames = new List<string>(); //이펙트의 이름으로 리스트작성
    Dictionary<string, GameObjectPool<EffectPoolUnit>> m_effectPool = new Dictionary<string, GameObjectPool<EffectPoolUnit>>(); //EffectPoolUnit을 상속받는 게임오브젝트풀을 딕셔너리로 만듬
    Dictionary<string, GameObject> m_dicPrefabList = new Dictionary<string, GameObject>(); //딕셔너리 이용하여 프리펩리스트 저장
    #endregion

    #region Methods
    IEnumerator CorouTine_SetActive(GameObject obj, bool isActive) //모종의 이유로 코드가 작동하지않아 임시로 사용하였으나 수정후 사용X 
    {
        yield return new WaitForEndOfFrame(); //한프레임동안 null해서 다음프레임에 다음코드 실행하기위함.
        obj.SetActive(false);
    }
    void LoadEffect() //이펙트를 불러오는 메소드
    {
        TableEffect.Instance.Load(); //
        EffectPoolUnit unit = null; //
        VFXAutoDestroy autoDestroy;
        foreach (KeyValuePair<int, TableEffect.Data> pair in TableEffect.Instance.m_tableData)
        {
            for (int i = 0; i < pair.Value.Prefab.Length; i++)
            {
                if (!m_listEffectNames.Contains(pair.Value.Prefab[i]))
                {
                    m_listEffectNames.Add(pair.Value.Prefab[i]);
                }
            }
        }
        for (int i = 0; i < m_listEffectNames.Count; i++)
        {
            string effectName = m_listEffectNames[i];
            var prefab = Resources.Load<GameObject>("Prefabs/Effect/" + effectName);
            m_dicPrefabList.Add(effectName, prefab);
            GameObjectPool<EffectPoolUnit> pool = new GameObjectPool<EffectPoolUnit>();
            m_effectPool.Add(effectName, pool);
            pool.MakePool(m_presetSize, () =>
            {
                prefab = m_dicPrefabList[effectName];
                if (prefab != null)
                {
                    var obj = Instantiate(prefab);
                    unit = obj.GetComponent<EffectPoolUnit>();
                    if (unit == null)
                        unit = obj.AddComponent<EffectPoolUnit>();

                    unit.SetObjectPool(effectName);
                    autoDestroy = obj.GetComponent<VFXAutoDestroy>();
                    if (autoDestroy == null)
                        autoDestroy = obj.AddComponent<VFXAutoDestroy>();

                    //StartCoroutine(CorouTine_SetActive(obj,false));
                    obj.SetActive(false);
                }
                return unit;
            });

        }
    }
    public void AddPool(string effectName, EffectPoolUnit unit)
    {
        var pool = m_effectPool[effectName];
        if (pool == null)
        {
            return;
        }
        pool.Set(unit);

    }

    public GameObject Create(string effectName, Vector3 position, Quaternion rotation)
    {
        EffectPoolUnit unit = null;
        GameObjectPool<EffectPoolUnit> pool;
        m_effectPool.TryGetValue(effectName, out pool);
        if (pool == null) return null;
        for (int i = 0; i < pool.Count; i++)
        {
            unit = pool.Get();

            if (!unit.IsReady)
            {
                pool.Set(unit);
                unit = null;
            }
            else
            {
                break;
            }
        }
        if (unit == null)
        {
            unit = pool.New();
        }
        unit.transform.position = position;
        unit.transform.rotation = rotation;
        unit.gameObject.SetActive(true);

        return unit.gameObject;
    }
    public GameObject Create(string effectName)
    {
        return Create(effectName, Vector3.zero, Quaternion.identity);
    }
    protected override void OnStart()
    {
        LoadEffect();
    }
    #endregion
}
