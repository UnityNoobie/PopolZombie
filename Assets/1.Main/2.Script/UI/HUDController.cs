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
    public void SetHUD(Transform target, string name) // ������ ��� Transform�� �޾ƿ� NGUI�� ����� UI FollowTarget�� �̿��� ����.
    {
        m_uiFollow.target = target;
        m_name.text = name;
        m_hpBar.value = 1f;
        Hide();
    }
    public void InitHUD(Camera gameCam, Camera uiCam ) //���� ȭ���� ǥ���ϴ� ī�޶�� UI�� ǥ���ϴ� ī�޶� ����
    {
        m_uiFollow.gameCamera = gameCam;
        m_uiFollow.uiCamera = uiCam;
    }
    public void DisplayDamage(AttackType type, float damage, float normalizedHP)  //ȭ�鿡 HUD ǥ���ϴ� ��� Invoke�� ���� �����ð� ǥ�� �� ���ӿ�����Ʈ ���־� ȭ�� �����ϰ�
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
    void Show() // UIȭ�鿡 ����ϱ� ���� Ȱ��ȭ
    {
        gameObject.SetActive(true);
    }
    void Hide() // ��Ȱ��ȭ.
    {
        gameObject.SetActive(false);
    }
    void Start() // ��ü ���� �� HUD ���־� ȭ�� �����ϰ�.
    {
        Hide();
    }
    #endregion
}
