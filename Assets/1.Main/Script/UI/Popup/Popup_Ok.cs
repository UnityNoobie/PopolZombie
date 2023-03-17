using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Ok : MonoBehaviour
{
    [SerializeField]
    UILabel m_titleLable;
    [SerializeField]
    UILabel m_bodyLable;
    [SerializeField]
    UILabel m_okBtnLable;    
    Action m_okDel;    
    public void SetUI(string title, string body, Action okDel, string okBtnText = "Ok")
    {
        m_titleLable.text = title;
        m_bodyLable.text = body;
        m_okBtnLable.text = okBtnText;        
        m_okDel = okDel;        
    }
    public void OnPressButtonOk()
    {
        if (m_okDel != null)
            m_okDel();
        else
            PopupManager.Instance.PopupClose();
    }    
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
