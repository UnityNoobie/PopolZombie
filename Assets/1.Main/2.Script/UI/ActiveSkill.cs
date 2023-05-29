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
        m_slot.ActiveSkill(); //스킬업 적용
        UGUIManager.Instance.SystemMessageSendMessage(m_name + " 습득에 성공하였습니다.");
        DeActiveUI();
    }
    public void OnclickedFalse()
    {
        UGUIManager.Instance.SystemMessageSendMessage("스킬 습득을 취소하였습니다.");
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
            UGUIManager.Instance.SystemMessageSendMessage("스킬 습득창이 이미 열려있습니다. 기존 작업을 마무리 후 작업해주세요."); //이미 열려있을 시 메세지 호출 후 리턴
            return;
        }
        m_name = name;
        m_slot = slot;
        Init();
        m_text.text = (m_name + " 를 습득하시겠습니까?\n소모포인트 : " + point);
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
