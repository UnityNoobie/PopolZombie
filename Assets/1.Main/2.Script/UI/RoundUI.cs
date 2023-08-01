using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  
public class RoundUI : MonoBehaviour
{
    #region Constants and Fields
    TextMeshProUGUI m_text;
    CanvasGroup m_group;
    #endregion

    #region Coroutine
    IEnumerator ConvertRound() //Dotween�� Ȱ���� �㳷���� �ڷ�ƾ
    {
        m_group.alpha = 0.01f;
        m_group.DOFade(1f, 1.5f);
        yield return new WaitForSeconds(1.5f);
        m_group.DOFade(0f, 1.5f);
        yield return new WaitForSeconds(1.5f);
        m_text.text = null;
        gameObject.SetActive(false);
    }
    #endregion

    #region Method
    public void ChangeRound() //���� ���� UI
    {
        gameObject.SetActive(true);
        if (GameManager.Instance.GetDayInfo().Equals(DaynNight.Day))
        {
            m_text.text = "<size=100>"+ GameManager.Instance.GetRoundInfo() + "���� ��</size>\n<size=40>���� �غ��Ͻʽÿ�.</size>";
        }
        else
        {
            m_text.text = "<size=100>"+ GameManager.Instance.GetRoundInfo() + "���� ��</size>\n<size=40>�������� ����κ��� �����Ͻʽÿ�.</size>";
        }
        StartCoroutine(ConvertRound());
    }
    public void SetTransform() // ��ġ ����
    {
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_group = GetComponent<CanvasGroup>();
    }
    private void Awake()
    {
        SetTransform();
    }
    #endregion

}
