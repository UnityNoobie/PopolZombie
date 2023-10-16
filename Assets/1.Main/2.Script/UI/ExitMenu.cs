using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour  //���� ���� Ȯ�� UI
{
    Button m_exit;
    Button m_cancle;
    bool isfirst = true;

    void SetTransform() //��ǥ ����
    {
        m_exit = Utill.GetChildObject(gameObject, "ExitGame").GetComponent<Button>();
        m_cancle = Utill.GetChildObject(gameObject, "Cancle").GetComponent<Button>();
    }
    void SetAdlistoner() //��ư ��� �߰�
    {
        m_exit.onClick.AddListener(GameManager.Instance.ExitGame);
        m_cancle.onClick.AddListener(DeActiveUi);
    }
    public void ActiveUI() //ù ������ �� �ʿ��� ����
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
