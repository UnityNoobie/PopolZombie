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
    public void OnclickedTrue()
    {
        UGUIManager.Instance.PlayClickSFX();
        m_slot.BuyItem();
        UGUIManager.Instance.SystemMessageSendMessage(m_name + " 구매에 성공하였습니다.");
        DeActiveUI();
    }
    public void OnclickedFalse()
    {
        UGUIManager.Instance.PlayClickSFX();
        UGUIManager.Instance.SystemMessageSendMessage("아이템 구입을 취소하였습니다.");
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
            UGUIManager.Instance.SystemMessageSendMessage("구매창이 이미 열려있습니다. 기존 작업을 마무리 후 작업해주세요."); //이미 열려있을 시 메세지 호출 후 리턴
            return;
        }
        m_slot = slot;
        m_price = price;
        m_name = name;
        m_text.text = (m_name + " 를 구입하시겠습니까?\n가격 : " + price);
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
       
    }
    private void Start()
    {
        Init();
    }
    #endregion
}
