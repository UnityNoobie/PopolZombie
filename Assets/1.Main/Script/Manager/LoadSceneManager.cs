using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneState
{
    None = -1,
    Title,
    Lobby,
    Game,
    Max
}
public class LoadSceneManager : SingletonDontDestroy<LoadSceneManager>
{
    [SerializeField]
    GameObject m_loadingObj;
    [SerializeField]
    UIProgressBar m_loadingBar;
    [SerializeField]
    UILabel m_loadingProgress;
    AsyncOperation m_loadStateInfo;

    SceneState m_state = SceneState.Title;
    SceneState m_loadState = SceneState.None;
    public void LoadSceneAsync(SceneState scene)
    {
        if (m_loadState != SceneState.None)
            return;
        m_loadState = scene;
        m_loadingObj.SetActive(true);
        m_loadStateInfo = SceneManager.LoadSceneAsync((int)scene);
    }   
    void HideUI()
    {
        m_loadingObj.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_loadingObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PopupManager.Instance.IsOpen)
                PopupManager.Instance.PopupClose();
            else
            {
                switch(m_state)
                {
                    case SceneState.Title:
                        PopupManager.Instance.PopupOpen_OkCancel("안내", "정말로 게임을 종료하시겠습니까?", () =>
                        {
#if UNITY_EDITOR
                            EditorApplication.isPlaying = false;
#else
                            Application.Quit();
#endif
                        }, null, "예", "아니오");
                        break;
                    case SceneState.Lobby:
                        PopupManager.Instance.PopupOpen_OkCancel("안내", "타이틀 화면으로 돌아가시겠습니까?", () =>
                        {
                            LoadSceneAsync(SceneState.Title);
                            PopupManager.Instance.PopupClose();
                        }, null, "예", "아니오");
                        break;
                    case SceneState.Game:
                        PopupManager.Instance.PopupOpen_OkCancel("안내", "현재 게임을 종료하고 로비로 돌아가시겠습니까?\r\n저장하지 않은 정보는 모두 사라집니다.", ()=> 
                        {
                            LoadSceneAsync(SceneState.Lobby);
                            PopupManager.Instance.PopupClose();
                        }, null, "예", "아니오");
                        break;
                }
            }
        }
        if (m_loadStateInfo != null)
        {
            if (m_loadStateInfo.isDone)
            {
                m_loadStateInfo = null;
                m_state = m_loadState;
                m_loadState = SceneState.None;
                m_loadingBar.value = 1f;
                m_loadingProgress.text = "100%";
                Invoke("HideUI", 1f);
            }
            else
            {
                Debug.Log(m_loadStateInfo.progress);
                m_loadingBar.value = m_loadStateInfo.progress;
                m_loadingProgress.text = ((int)(m_loadStateInfo.progress * 100f)).ToString() + "%";
            }
        }
    }
}
