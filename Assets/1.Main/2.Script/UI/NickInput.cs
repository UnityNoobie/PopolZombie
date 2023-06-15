using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NickInput : MonoBehaviour
{
    // Start is called before the first frame update
    TMP_InputField m_input;
    Button m_return;
    Button accapt;
    Button cancle;
    GameObject m_confirm;
    TextMeshProUGUI m_text;
    void SetNickName(string str)
    {
        if(str.Length <= 1)
        {
            UGUIManager.Instance.SystemMessageSendMessage("�г����� 2���� �̻����� ������ �ּ���");
            return;
        }
        GameManager.Instance.SetNickname(str);
        ConfirmNick();
    }
    void ConfirmNick()
    {
        m_confirm.SetActive(true);
        m_text.text = GameManager.Instance.GetNickname() + "\n���� �����Ͻðڽ��ϱ�??";
    }
    void AccaptNick()
    {
        m_confirm.SetActive(false);
        UGUIManager.Instance.GameStart();
        DeActiveUI();
    }
    void CancleNick()
    {
        m_confirm.SetActive(false);
    }
    public void ActiveUI()
    {
        gameObject.SetActive(true);
    }
    public void DeActiveUI()
    {
        gameObject.SetActive(false);
    }
    public void SetTransform()
    {
        m_input = GetComponentInChildren<TMP_InputField>(true);
        m_return = Utill.GetChildObject(gameObject,"Return").GetComponent<Button>();
        m_confirm = Utill.GetChildObject(gameObject, "Confirm").gameObject;
        accapt = Utill.GetChildObject(m_confirm, "Accapt").GetComponent<Button>();
        cancle = Utill.GetChildObject(m_confirm, "Cancle").GetComponent<Button>();
        m_text = Utill.GetChildObject(m_confirm, "Text").GetComponent<TextMeshProUGUI>();
        m_return.onClick.AddListener(DeActiveUI);
        m_input.onSubmit.AddListener(SetNickName);
        accapt.onClick.AddListener(AccaptNick);
        cancle.onClick.AddListener(CancleNick);
    }
}
