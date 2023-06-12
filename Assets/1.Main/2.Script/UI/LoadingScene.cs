using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    #region Constants and Fields
    Slider m_loadingslider;
    Button m_startButton;
    CanvasGroup m_group;
    bool isFirst = true;
    #endregion

    #region Coroutine
    IEnumerator ChangeSliderValue()
    {
        float initialValue = m_loadingslider.value;
        float timer = 0f;
        while (timer < 6)
        {
            timer += Time.deltaTime;
            float t = timer / 6;
            m_loadingslider.value = Mathf.Lerp(initialValue, 1F, t);
            yield return null;
        }
        m_loadingslider.value = 1F;
        LoadFinished();
    }
    IEnumerator GameStart()
    {
        m_group.DOFade(0f,0.7f);
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }
    IEnumerator Coroutine_ToLobbyScene()
    {
        LoadLoading();
        float initialValue = m_loadingslider.value;
        float timer = 0f;
        while (timer < 3)
        {
            timer += Time.deltaTime;
            float t = timer / 3;
            m_loadingslider.value = Mathf.Lerp(initialValue, 1F, t);
            yield return null;
        }
        m_loadingslider.value = 1F;
        LoadLobby();
    }
    #endregion

    #region Methods
    void StartButton()
    {
        UGUIManager.Instance.PlayClickSFX();
        StartCoroutine(GameStart());
        GameManager.Instance.GameStart();
    }
    void LoadFinished()
    {
        m_startButton.gameObject.SetActive(true);
        m_loadingslider.gameObject.SetActive(false);
    }
    void LoadLobby()
    {
        gameObject.SetActive(false);
        UGUIManager.Instance.ActiveLobbyUI(true);
    }
    void LoadStart()
    {
        m_group.alpha = 1.0f;
        m_loadingslider.value = 0;
        m_loadingslider.gameObject.SetActive(true);
        m_startButton.gameObject.SetActive(false);
        StartCoroutine(ChangeSliderValue());
    }
    void LoadLoading()
    {
        m_startButton.gameObject.SetActive(false);
        m_group.alpha = 1.0f;
        m_loadingslider.value = 0;
        m_loadingslider.gameObject.SetActive(true);
    }
    public void StartGameScene()
    {
        if(isFirst)
        {
            isFirst = false;
            m_group = GetComponent<CanvasGroup>();
            m_loadingslider = GetComponentInChildren<Slider>(true);
            m_startButton = GetComponentInChildren<Button>(true);
            m_startButton.onClick.AddListener(StartButton);
        }
        gameObject.SetActive(true);
        UGUIManager.Instance.ActiveLobbyUI(false);
        LoadStart();
    }
    public void LoadLobbyScene()
    {
        StartCoroutine(Coroutine_ToLobbyScene());
    }
    #endregion 
}
