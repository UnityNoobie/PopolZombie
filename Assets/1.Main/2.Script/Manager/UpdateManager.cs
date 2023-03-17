using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{

    GunManager gunManager;
    [SerializeField]
    Inventory m_inven;
    bool m_isactive = false;


    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_isactive = !m_isactive; //불값으로 액티브 변경. 
            m_inven.gameObject.SetActive(m_isactive);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_isactive)
            {
                m_isactive = false;
                m_inven.gameObject.SetActive(m_isactive);   
            }
            UIManager.Instance.CloseTabs();
        }
    }
    private void Start()
    {
          m_inven.gameObject.SetActive(m_isactive);
    }
}
