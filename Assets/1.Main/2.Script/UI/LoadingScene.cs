using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScene : MonoBehaviour //씬 로딩 UI
{
    #region Constants and Fields
    Slider m_loadingslider;
    Button m_startButton;
    CanvasGroup m_group;
    bool isFirst = true;
    #endregion

    #region Coroutine
    IEnumerator ChangeSliderValue() //로딩씬 하단에 Slider의 value조절 기능
    {
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
        LoadFinished();
    }
    IEnumerator GameStart() //게임 시작 시 DoTween의 DoFade 기능 활용하여 로비씬 투명화하며 제거
    {
        m_group.DOFade(0f,0.7f);
        yield return new WaitForSeconds(0.2f);
        UGUIManager.Instance.GetScreenHUD().SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    IEnumerator Coroutine_ToLobbyScene() //로비씬으로 복귀하는 코루틴
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
    void StartButton() //스타트 버튼 Onclick() 효과에 적용할 효과
    {
        UGUIManager.Instance.PlayClickSFX();
        StartCoroutine(GameStart());
        GameManager.Instance.GameStart();
    }
    void LoadFinished() //로딩이 완료되었을 때 스타트버튼 On 슬라이드 삭제
    {
        m_startButton.gameObject.SetActive(true);
        m_loadingslider.gameObject.SetActive(false);
    }
    void LoadLobby() //로비 로딩
    {
        gameObject.SetActive(false);
        UGUIManager.Instance.GetLobbyUI().SetActiveUI(true);
    }
    void LoadStart() //로딩이 시작될 때 값을 초기화 해주고 해당 코루틴 실행
    {
        m_group.alpha = 1.0f;
        m_loadingslider.value = 0;
        m_loadingslider.gameObject.SetActive(true);
        m_startButton.gameObject.SetActive(false);
        StartCoroutine(ChangeSliderValue());
    }
    void LoadLoading() //값 초기화하고 스타트버튼 제거, 로딩 슬라이더 시작
    {
        m_startButton.gameObject.SetActive(false);
        m_group.alpha = 1.0f;
        m_loadingslider.value = 0;
        m_loadingslider.gameObject.SetActive(true);
    }
    public void StartGameScene() // 게임 씬 시작 시 호출. 처음 실행 시 좌표지정 같이해줌
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
        UGUIManager.Instance.GetLobbyUI().SetActiveUI(false);
        LoadStart();
    }
    public void LoadLobbyScene() // 로비씬 이동 시 실행. 코루틴 호출
    {
        StartCoroutine(Coroutine_ToLobbyScene());
    }
    #endregion 
}
