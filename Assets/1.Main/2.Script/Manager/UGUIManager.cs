using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UGUIManager : SingletonMonoBehaviour<UGUIManager> 
{
    #region Constants and Fields
    TextMeshProUGUI m_systemMessage;
    StoreUI m_storeUI;
    StatusUI m_statusUI;
    SkillUI m_skillUI;
    RoundUI m_roundUI;
    #endregion

    #region Coroutine
    IEnumerator SystemMessage(string message)
    {
        m_systemMessage.text = message;
        yield return new WaitForSeconds(1.5f);
        m_systemMessage.text = null;
    }
    #endregion

    #region SystemMessange
    public void SystemMessageItem(string name)
    {
        StartCoroutine(SystemMessage(name + " 의 수량이 부족합니다."));
    }
    public void SystemMessageCantUse(string name)
    {
        StartCoroutine(SystemMessage(name + " 을 사용할 수 없습니다. "));
    }
    public void SystemMessageSendMessage(string text)
    {
        StartCoroutine(SystemMessage(text));
    }
    #endregion

    #region Method
    public void SetPlayer(PlayerController player)
    {
        m_statusUI.SetPlayer(player);
    }
    public StoreUI GetStoreUI()
    {
        return m_storeUI;
    }
    public StatusUI GetStatusUI()
    {
        return m_statusUI;
    }
    public SkillUI GetSkillUI()
    {
        return m_skillUI;
    }
    public RoundUI GetRoundUI()
    {
        return m_roundUI;
    }
    public void CloseAllTabs()
    {
        m_systemMessage.text = null;
        m_storeUI.CloseStore();
        m_statusUI.SetActive(false);
        m_skillUI.DeActiveSkill();
    }
    public void SkillUIChange(bool aa, PlayerSkillController skill)
    {
        if (aa)
        {
            m_skillUI.ActiveSkill(skill);
        }
        else
        {
            m_skillUI.DeActiveSkill();
        }
    }
    public void StartRound()
    {
        m_roundUI.ChangeRound();
    }
    protected override void OnAwake()
    {
        m_systemMessage = Utill.GetChildObject(gameObject, "SystemMessage").GetComponent<TextMeshProUGUI>();
        m_statusUI = Utill.GetChildObject(gameObject, "StatusUI").GetComponent<StatusUI>();
        m_skillUI = Utill.GetChildObject(gameObject, "SkillUI").GetComponent<SkillUI>();
        m_storeUI = Utill.GetChildObject(gameObject, "StoreUI").GetComponent<StoreUI>();
        m_roundUI = Utill.GetChildObject(gameObject, "RoundUI").GetComponent<RoundUI>();
    }
    protected override void OnStart()
    {
        GameManager.Instance.StartDay();
    }
    #endregion
}
