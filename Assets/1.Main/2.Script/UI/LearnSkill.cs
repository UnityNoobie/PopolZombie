using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearnSkill : MonoBehaviour
{
    TextMeshProUGUI m_text;
    Button m_truebutton;
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
        if (gameObject.activeSelf)
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
    private void Start()
    {
        m_truebutton = Utill.GetChildObject(gameObject, "Button_true").GetComponent<Button>();
        m_falsebutton = Utill.GetChildObject(gameObject, "Button_false").GetComponent<Button>();
        m_text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
