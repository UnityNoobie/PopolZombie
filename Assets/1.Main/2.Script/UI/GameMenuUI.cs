using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour
{
    #region Constants and Fields
    Button m_play;
    Button m_sound;
    Button m_vidio;
    Button m_control;
    Button m_lobby;
    Button m_exit;
    #endregion
    void AddListoner()
    {
        m_play.onClick.AddListener(DeactiveUI); //시간 재개 게임오브젝트 종료
        m_sound.onClick.AddListener(UGUIManager.Instance.ActiveVolumeControll); //
        m_lobby.onClick.AddListener(GameManager.Instance.LoadLobbyScene); //로비 불러오기
        m_lobby.onClick.AddListener(DeactiveUI);
        m_control.onClick.AddListener(UGUIManager.Instance.ActiveTipMenu);
        m_exit.onClick.AddListener(UGUIManager.Instance.OpenExitMenu);//게임종료
        m_play.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_sound.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_vidio.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_control.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_lobby.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_exit.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
    }
    public void DeactiveUI()
    {
        GameManager.Instance.SetTimeScale(1);
        gameObject.SetActive(false);
    }
    public void ActiveUI()
    {
        GameManager.Instance.SetTimeScale(0);
        gameObject.SetActive(true);
    }
    public void SetTransform()
    {
        m_play = Utill.GetChildObject(gameObject, "Button_Play").GetComponent<Button>();
        m_sound = Utill.GetChildObject(gameObject, "Button_Sound").GetComponent<Button>();
        m_vidio = Utill.GetChildObject(gameObject, "Button_Vidio").GetComponent<Button>();
        m_control = Utill.GetChildObject(gameObject, "Button_Controll").GetComponent<Button>();
        m_lobby = Utill.GetChildObject(gameObject, "Button_Lobby").GetComponent<Button>();
        m_exit = Utill.GetChildObject(gameObject, "Button_Exit").GetComponent<Button>();
        AddListoner();
    }
}
