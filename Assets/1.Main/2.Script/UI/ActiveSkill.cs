using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    #region Constants and Fields
    TextMeshProUGUI m_text;
    Button m_truebutton;
    Button m_falsebutton;
    SkillSlot m_slot;
    int m_point;
    string m_name;
    #endregion

    #region Methods
    public void OnclickedTrue()
    {
        m_slot.ActiveSkill(); //��ų�� ����
        UGUIManager.Instance.SystemMessageSendMessage(m_name + " ���濡 �����Ͽ����ϴ�.");
        DeActiveUI();
    }
    public void OnclickedFalse()
    {
        UGUIManager.Instance.SystemMessageSendMessage("��ų ������ ����Ͽ����ϴ�.");
        DeActiveUI();
    }
    public void DeActiveUI()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
    void Init()
    {
        m_truebutton = Utill.GetChildObject(gameObject, "Button_true").GetComponent<Button>();
        m_falsebutton = Utill.GetChildObject(gameObject, "Button_false").GetComponent<Button>();
        m_text = GetComponentInChildren<TextMeshProUGUI>();
       
    }
    public void ActiveUI(SkillSlot slot, int point, string name)
    {
        if (gameObject.activeSelf)
        {
            UGUIManager.Instance.SystemMessageSendMessage("��ų ����â�� �̹� �����ֽ��ϴ�. ���� �۾��� ������ �� �۾����ּ���."); //�̹� �������� �� �޼��� ȣ�� �� ����
            return;
        }
        m_name = name;
        m_slot = slot;
        Init();
        m_text.text = (m_name + " �� �����Ͻðڽ��ϱ�?\n�Ҹ�����Ʈ : " + point);
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
    private void Start()
    {
        m_truebutton.onClick.AddListener(OnclickedTrue);
        m_falsebutton.onClick.AddListener(OnclickedFalse);
    }
    #endregion
}
