using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    UIFollowTarget m_uiFollow;
    [SerializeField]
    UILabel m_name;
    [SerializeField]
    HUDText[] m_hudText; 
    [SerializeField]
    UIProgressBar m_hpBar;
    const int m_hideRate = 5;
    #endregion
    #region Methods
    public void SetHUD(Transform target, string name) // 추적할 대상 Transform을 받아와 NGUI의 기능인 UI FollowTarget을 이용해 추적.
    {
        m_uiFollow.target = target;
        m_name.text = name;
        m_hpBar.value = 1f;
        Hide();
    }
    public void InitHUD(Camera gameCam, Camera uiCam ) //게임 화면을 표시하는 카메라와 UI를 표시하는 카메라 지정
    {
        m_uiFollow.gameCamera = gameCam;
        m_uiFollow.uiCamera = uiCam;
    }
    public void DisplayDamage(AttackType type, float damage, float normalizedHP)  //화면에 HUD 표시하는 기능 Invoke를 통해 일정시간 표기 후 게임오브젝트 꺼주어 화면 깨끗하게
    {
        Show();
        if (IsInvoking("Hide"))
        {
            CancelInvoke("Hide");
        }
        Invoke("Hide", m_hideRate);
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
    void Show() // UI화면에 출력하기 위해 활성화
    {
        gameObject.SetActive(true);
    }
    void Hide() // 비활성화.
    {
        gameObject.SetActive(false);
    }
    void Start() // 객체 생성 시 HUD 꺼주어 화면 깨끗하게.
    {
        Hide();
    }
    #endregion
}
