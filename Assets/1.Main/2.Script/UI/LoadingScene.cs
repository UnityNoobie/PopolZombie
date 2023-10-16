using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScene : MonoBehaviour //�� �ε� UI
{
    #region Constants and Fields
    Slider m_loadingslider;
    Button m_startButton;
    CanvasGroup m_group;
    bool isFirst = true;
    #endregion

    #region Coroutine
    IEnumerator ChangeSliderValue() //�ε��� �ϴܿ� Slider�� value���� ���
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
    IEnumerator GameStart() //���� ���� �� DoTween�� DoFade ��� Ȱ���Ͽ� �κ�� ����ȭ�ϸ� ����
    {
        m_group.DOFade(0f,0.7f);
        yield return new WaitForSeconds(0.2f);
        UGUIManager.Instance.GetScreenHUD().SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    IEnumerator Coroutine_ToLobbyScene() //�κ������ �����ϴ� �ڷ�ƾ
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
    void StartButton() //��ŸƮ ��ư Onclick() ȿ���� ������ ȿ��
    {
        UGUIManager.Instance.PlayClickSFX();
        StartCoroutine(GameStart());
        GameManager.Instance.GameStart();
    }
    void LoadFinished() //�ε��� �Ϸ�Ǿ��� �� ��ŸƮ��ư On �����̵� ����
    {
        m_startButton.gameObject.SetActive(true);
        m_loadingslider.gameObject.SetActive(false);
    }
    void LoadLobby() //�κ� �ε�
    {
        gameObject.SetActive(false);
        UGUIManager.Instance.GetLobbyUI().SetActiveUI(true);
    }
    void LoadStart() //�ε��� ���۵� �� ���� �ʱ�ȭ ���ְ� �ش� �ڷ�ƾ ����
    {
        m_group.alpha = 1.0f;
        m_loadingslider.value = 0;
        m_loadingslider.gameObject.SetActive(true);
        m_startButton.gameObject.SetActive(false);
        StartCoroutine(ChangeSliderValue());
    }
    void LoadLoading() //�� �ʱ�ȭ�ϰ� ��ŸƮ��ư ����, �ε� �����̴� ����
    {
        m_startButton.gameObject.SetActive(false);
        m_group.alpha = 1.0f;
        m_loadingslider.value = 0;
        m_loadingslider.gameObject.SetActive(true);
    }
    public void StartGameScene() // ���� �� ���� �� ȣ��. ó�� ���� �� ��ǥ���� ��������
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
    public void LoadLobbyScene() // �κ�� �̵� �� ����. �ڷ�ƾ ȣ��
    {
        StartCoroutine(Coroutine_ToLobbyScene());
    }
    #endregion 
}
