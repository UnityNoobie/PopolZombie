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
    [SerializeField]
    TextMeshProUGUI m_text;
    [SerializeField]
    Button m_truebutton;
    [SerializeField]
    Button m_falsebutton;
    Slot m_slot;
    int m_price;
    string m_name;

    public void OnclickedTrue()
    {
        m_slot.BuyItem();
        UIManager.Instance.SystemMessageCantOpen(m_name + " ���ſ� �����Ͽ����ϴ�.");
        DeActiveUI();
    }
    public void OnclickedFalse()
    {
        UIManager.Instance.SystemMessageCantOpen("������ ������ ����Ͽ����ϴ�.");
        DeActiveUI();
    }
    public void DeActiveUI()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
    private void Init()
    {
        m_truebutton.onClick.AddListener(OnclickedTrue);
        m_falsebutton.onClick.AddListener(OnclickedFalse);
    }
    public void ActiveUI(Slot slot, int price, string name)
    {
        if(gameObject.activeSelf)
        {
            UIManager.Instance.SystemMessageCantOpen("����â�� �̹� �����ֽ��ϴ�. ���� �۾��� ������ �� �۾����ּ���."); //�̹� �������� �� �޼��� ȣ�� �� ����
            return;
        }
        m_slot = slot;
        m_price = price;
        m_name = name;
        m_text.text = (m_name + " �� �����Ͻðڽ��ϱ�?\n���� : " + price);
        Init();
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
       
    }
}
