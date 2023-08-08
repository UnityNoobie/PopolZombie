using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NickInput : MonoBehaviour //닉네임 입력 UI
{
    #region Constants and Fields
    TMP_InputField m_input;
    Button m_return;
    Button accapt;
    Button cancle;
    GameObject m_confirm;
    TextMeshProUGUI m_text;
    #endregion

    #region Methods
    void SetNickName(string str) //인풋 필드에 입력된 닉네임 받아옴.
    {
        if(str.Length <= 1)
        {
            UGUIManager.Instance.SystemMessageSendMessage("닉네임을 2글자 이상으로 설정해 주세요");
            return;
        }
        GameManager.Instance.SetNickname(str);
        ConfirmNick();
    }
    void ConfirmNick() //입력된 닉네임 확인창 호출.
    {
        m_confirm.SetActive(true);
        m_text.text = GameManager.Instance.GetNickname() + "\n으로 결정하시겠습니까??";
    }
    void AccaptNick() //닉네임 확정, 게임 시작
    {
        m_confirm.SetActive(false);
        UGUIManager.Instance.GameStart();
        DeActiveUI();
    }
    void CancleNick() //닉네임 결정 취소.
    {
        m_confirm.SetActive(false);
    }
    public void ActiveUI() //UI온
    {
        gameObject.SetActive(true);
    }
    public void DeActiveUI() // 오프
    {
        gameObject.SetActive(false);
    }
    public void SetTransform() //좌표 지정.
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
    #endregion
}
