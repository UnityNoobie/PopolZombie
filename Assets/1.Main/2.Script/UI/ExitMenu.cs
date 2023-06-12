using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour
{
    Button m_exit;
    Button m_cancle;
    bool isfirst = true;

    void SetTransform()
    {
        m_exit = Utill.GetChildObject(gameObject, "ExitGame").GetComponent<Button>();
        m_cancle = Utill.GetChildObject(gameObject, "Cancle").GetComponent<Button>();
    }
    void SetAdlistoner()
    {
        m_exit.onClick.AddListener(GameManager.Instance.ExitGame);
        m_cancle.onClick.AddListener(DeActiveUi);
    }
    public void ActiveUI()
    {
        if(isfirst)
        {
            SetTransform();
            SetAdlistoner();
        }
        gameObject.SetActive(true);
    }
    public void DeActiveUi()
    {
        gameObject.SetActive(false);
    }
}
