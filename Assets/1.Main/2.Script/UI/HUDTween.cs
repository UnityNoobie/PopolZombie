using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Rendering;

public class HUDTween : MonoBehaviour
{
    TextMeshProUGUI m_text;
    const float m_start = 0f;
    float m_to;
    float m_delay;
    float m_show;
    bool m_isPlaying;
    CanvasGroup m_group;
    Button m_backToLobby;

    IEnumerator Coroutine_FadeEffect()
    {
        m_isPlaying = true;
        m_group.alpha = 0.01f;
        m_group.DOFade(m_to, m_delay);
        yield return new WaitForSeconds(m_delay);
        yield return new WaitForSeconds(m_delay + m_show);
        m_group.DOFade(0f, m_delay);
        yield return new WaitForSeconds(m_delay);
        m_isPlaying = false;
        gameObject.SetActive(false);
    }
    public void SetEffect(float to,float show = 0f, float delay = 0.3f)
    {
        if (m_isPlaying) return;
        m_to = to;
        m_delay = delay;
        m_show = show;
        gameObject.SetActive(true);
        StartCoroutine(Coroutine_FadeEffect());
    }
    public void CancleEffect()
    {
        StopAllCoroutines();
        m_isPlaying = false;
        gameObject.SetActive(false);
    }
    public void GeneratorDestroyed(string text)
    {
        gameObject.SetActive(true);
        m_text.text = text;
    }
    void BackToLobby()
    {
        GameManager.Instance.LoadLobbyScene();
        gameObject.SetActive(false);
    }
    public void SetTransform()
    {
        m_group = GetComponent<CanvasGroup>();
        m_backToLobby = GetComponentInChildren<Button>();
        
        if(m_backToLobby != null)
        {
            m_backToLobby.onClick.AddListener(BackToLobby);
            m_text = Utill.GetChildObject(gameObject,"DeadText").GetComponent<TextMeshProUGUI>();   
        }
    }
}
