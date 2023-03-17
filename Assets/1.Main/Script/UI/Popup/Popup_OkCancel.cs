using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_OkCancel : MonoBehaviour
{
    [SerializeField]
    UILabel m_titleLable;
    [SerializeField]
    UILabel m_bodyLable;
    [SerializeField]
    UILabel m_okBtnLable;
    [SerializeField]
    UILabel m_cancelBtnLable;
    Action m_okDel;
    Action m_cancelDel;
    public void SetUI(string title, string body, Action okDel, Action cancelDel, string okBtnText = "Ok", string cancelBtnText = "Cancel" )
    {
        m_titleLable.text = title;
        m_bodyLable.text = body;
        m_okBtnLable.text = okBtnText;
        m_cancelBtnLable.text = cancelBtnText;
        m_okDel = okDel;
        m_cancelDel = cancelDel;
    }
    public void OnPressButtonOk()
    {
        if (m_okDel != null)
            m_okDel();
        else
            PopupManager.Instance.PopupClose();
    }
    public void OnPressButtonCancel()
    {
        if (m_cancelDel != null)
            m_cancelDel();
        else
            PopupManager.Instance.PopupClose();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
