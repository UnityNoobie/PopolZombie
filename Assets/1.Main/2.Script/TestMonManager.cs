using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestMonManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_testMon;
    // Start is called before the first frame update
    GameObjectPool<TestMonster> m_mon = new GameObjectPool<TestMonster>();
    void Start()
    {
        m_mon = new GameObjectPool<TestMonster>(3, () =>
        {
            var obj = Instantiate(m_testMon);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.SetActive(false);
            var mon = obj.GetComponent<TestMonster>();
            return mon;        
        });
    }

    void Createmon()
    {
        for(int i = 0; i < 3; i++)
        {
            var mon = m_mon.Get();
            mon.transform.position = transform.position;
            mon.gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) Createmon();
    }
}
