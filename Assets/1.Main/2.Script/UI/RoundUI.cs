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
    IEnumerator DayStart()
    {
        m_group.alpha = 0.01f;
        m_group.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        m_group.DOFade(0f, 2f);
        yield return new WaitForSeconds(2f);
        m_text.text = null;
        gameObject.SetActive(false);
    }
    #endregion

    #region Method
    public void ChangeRound()
    {
        gameObject.SetActive(true);
        if (GameManager.Instance.GetDayInfo().Equals(DaynNight.Day))
        {
            m_text.text = "<size=100>"+ GameManager.Instance.GetRoundInfo() + "일차 낮</size>\n<size=40>밤을 준비하십시오.</size>";
        }
        else
        {
            m_text.text = "<size=100>"+ GameManager.Instance.GetRoundInfo() + "일차 밤</size>\n<size=40>몰려오는 좀비로부터 생존하십시오.</size>";
        }
        StartCoroutine(DayStart());
    }
    public void SetUI()
    {
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_group = GetComponent<CanvasGroup>();
    }
    private void Awake()
    {
        SetUI();
    }
    #endregion

}
