using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour  //게임 종료 확인 UI
{
    Button m_exit;
    Button m_cancle;
    bool isfirst = true;

    void SetTransform() //좌표 지정
    {
        m_exit = Utill.GetChildObject(gameObject, "ExitGame").GetComponent<Button>();
        m_cancle = Utill.GetChildObject(gameObject, "Cancle").GetComponent<Button>();
    }
    void SetAdlistoner() //버튼 기능 추가
    {
        m_exit.onClick.AddListener(GameManager.Instance.ExitGame);
        m_cancle.onClick.AddListener(DeActiveUi);
    }
    public void ActiveUI() //첫 실행일 시 필요기능 지정
    {
        if(isfirst)
        {
            SetTransform();
            SetAdlistoner();
        }
        if(gameObject.activeSelf) { DeActiveUi(); }
        gameObject.SetActive(true);
    }
    void DeActiveUi()
    {
        gameObject.SetActive(false);
    }
}
