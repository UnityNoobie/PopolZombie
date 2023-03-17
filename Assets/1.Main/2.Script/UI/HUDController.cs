using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    UIFollowTarget m_uiFollow;
    [SerializeField]
    UILabel m_name;
    [SerializeField]
    HUDText[] m_hudText; 
    [SerializeField]
    UIProgressBar m_hpBar;
    
    public void SetHUD(Transform target, string name)
    {
        m_uiFollow.target = target;
        m_name.text = name;
        m_hpBar.value = 1f;
    }
    public void InitHUD(Camera gameCam, Camera uiCam )
    {
        m_uiFollow.gameCamera = gameCam;
        m_uiFollow.uiCamera = uiCam;
    }
    public void DisplayDamage(AttackType type, float damage, float normalizedHP)
    {
        Show();
        if (IsInvoking("Hide"))
        {
            CancelInvoke("Hide");
        }
        Invoke("Hide", 5f);
        if (type == AttackType.Normal)
        {
            m_hudText[0].Add(-damage, Color.red, 0f);
        }
        if (type == AttackType.Critical)
        {
            m_hudText[1].Add(-damage, Color.yellow, 0f);
            m_hudText[2].Add("CritiCal!", Color.red, 0f);
        }
        m_hpBar.value = normalizedHP;
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
    void Start()
    {
        Hide();
    }
}
