using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static InvBaseItem;

public class BuyItems : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    TextMeshProUGUI m_text;
    [SerializeField]
    Button m_truebutton;
    [SerializeField]
    Button m_falsebutton;
    Slot m_slot;
    int m_price;
    string m_name;
    #endregion

    #region Methods
    public void OnclickedTrue() // True��ư�� ������ �� ������ ���� ����.
    {
        UGUIManager.Instance.PlayClickSFX();
        m_slot.BuyItem();
        UGUIManager.Instance.SystemMessageSendMessage(m_name + " ���ſ� �����Ͽ����ϴ�.");
        DeActiveUI();
    }
    public void OnclickedFalse() // False��ư�� ������ �� ������ ���� ���.
    {
        UGUIManager.Instance.PlayClickSFX();
        UGUIManager.Instance.SystemMessageSendMessage("������ ������ ����Ͽ����ϴ�.");
        DeActiveUI();
    }
    public void DeActiveUI() //���ֱ�
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
    private void ButtonAddListener() //Ŭ�� �� ������ �̺�Ʈ ����
    {
        m_truebutton.onClick.AddListener(OnclickedTrue);
        m_falsebutton.onClick.AddListener(OnclickedFalse);
    }

    public void ActiveUI(Slot slot, int price, string name) //������ �������� ������ �޾ƿ��� UI���ֱ�
    {
        if(gameObject.activeSelf)
        {
            UGUIManager.Instance.SystemMessageSendMessage("����â�� �̹� �����ֽ��ϴ�. ���� �۾��� ������ �� �۾����ּ���."); //�̹� �������� �� �޼��� ȣ�� �� ����
            return;
        }
        m_slot = slot;
        m_price = price;
        m_name = name;
        m_text.text = (m_name + " �� �����Ͻðڽ��ϱ�?\n���� : " + price);
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
       
    }
    private void Start()
    {
        ButtonAddListener();
    }
    #endregion
}
