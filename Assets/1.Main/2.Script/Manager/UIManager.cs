using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static GunManager;

public class UIManager : SingletonDontDestroy<UIManager>
{
    [SerializeField]
    public UILabel m_weaponInfoUI;
    [SerializeField]
    TextMeshProUGUI m_systemMessage;
    [SerializeField]
    UILabel waveUI;
    [SerializeField]
    UISlider m_hpUI;
    [SerializeField]
    UILabel m_hplable;
    [SerializeField]
    UILabel m_enemyReamain;
    [SerializeField]
    TweenAlpha m_damagedUI;
    [SerializeField]
    TweenAlpha m_DieUI;
    [SerializeField]
    TweenAlpha m_healUI;
    [SerializeField]
    TweenAlpha m_ReviveUI;
    [SerializeField]
    TweenAlpha m_levelUpUI;
    [SerializeField]
    UITexture m_weaponImage;
    [SerializeField]
    Inventory m_inven;
    [SerializeField]
    UILabel m_Score;
    [SerializeField] 
    UILabel m_Money;
    [SerializeField]
    QuickSlot m_quickSlot;
    [SerializeField]
    StoreUI m_store;


    IEnumerator SystemMessage(string message)
    {
        m_systemMessage.text = message;
        yield return new WaitForSeconds(1);
        m_systemMessage.text = null;
    }
    public void CloseTabs()
    {
        m_store.CloseAllTabs();
    }
    public void ScoreChange(float score)
    {
        PlayerController.Score += score;
        m_Score.text = "Score : " +Mathf.CeilToInt(PlayerController.Score).ToString();
    }
    public void MoneyChange(float money)
    {
        PlayerController.Money +=  money;
        m_Money.text = Mathf.CeilToInt(PlayerController.Money).ToString();
    }
    public void RoundInfo(int thisRound) //라운드 정보 UI
    {
        waveUI.text = (thisRound + " 라운드" );
    }
    public void WeaponImage(string name)  //무기별 이미지 교체.
    {
        Texture wptex = ImageLoader.Instance.GetImage(name).texture;
        m_weaponImage.mainTexture = wptex;
        m_quickSlot.SetItem(0,name);
        m_inven.WeaponImage(name);
    }
    public void HPBar(float hp, float max)
    {
        m_hplable.text = (hp + " / " + max);
        m_hpUI.value = hp / max;
    }
    public void DamagedUI()
    {
        if (!m_damagedUI.gameObject.activeSelf)
        {
            m_damagedUI.gameObject.SetActive(true);
        }
        if (m_hpUI.value <= 0.2f)
        {
            m_damagedUI.to = 1f;
        }
        else if (m_hpUI.value <= 0.4f)
        {
            m_damagedUI.to = 0.7f;
        }
        else if (m_hpUI.value <= 0.6f)
        {
            m_damagedUI.to = 0.5f;
        }
        else if (m_hpUI.value <= 0.8f)
        {
            m_damagedUI.to = 0.3f;
        }
        else
        {
            m_damagedUI.to = 0.1f;
        }
        m_damagedUI.PlayForward();
        m_damagedUI.PlayReverse();
        m_damagedUI.ResetToBeginning();
    }
    public void DiedUI()
    {
        if (!m_DieUI.gameObject.activeSelf)
        {
            m_DieUI.gameObject.SetActive(true);
        }
        m_DieUI.ResetToBeginning();
        m_DieUI.duration = 1.5f;
        m_DieUI.PlayForward();
        m_DieUI.ResetToBeginning();
    }
    public void ReviveUI()
    {
        m_DieUI.duration = 0.3f;
        m_DieUI.gameObject.SetActive(false);
        m_DieUI.ResetToBeginning();
        if (!m_ReviveUI.gameObject.activeSelf)
        {
            m_ReviveUI.gameObject.SetActive(true);
            m_ReviveUI.PlayForward();
            m_ReviveUI.PlayReverse();
        }
        m_ReviveUI.ResetToBeginning();
        m_ReviveUI.PlayForward();
        m_ReviveUI.PlayReverse();
    }
    public void HealUI()
    {
        if (!m_healUI.gameObject.activeSelf)
        {
            m_healUI.gameObject.SetActive(true);
            m_healUI.PlayForward();
            m_healUI.PlayReverse();
        }
        m_healUI.ResetToBeginning();
        m_healUI.PlayForward();
        m_healUI.PlayReverse();
    }
    public void LevelUPUI()
    {
        if (!m_levelUpUI.gameObject.activeSelf)
        {
            m_levelUpUI.gameObject.SetActive(true);
            m_levelUpUI.PlayForward();
            m_levelUpUI.PlayReverse();
        }
        m_levelUpUI.ResetToBeginning();
        m_levelUpUI.PlayForward();
        m_levelUpUI.PlayReverse();

    }
    public void WeaponInfoUI(string Info)
    {   
        m_weaponInfoUI.text = Info;
        m_inven.SetWeaponInfo(Info);
    }
    public void EnemyLeft(int remain)
    {
        m_enemyReamain.text = remain.ToString();
    }
    public void SystemMessageItem(string name)
    {
        StartCoroutine(SystemMessage(name + " 의 수량이 부족합니다."));
    }
    public void SystemMessageCantUse(string name)
    {
        StartCoroutine(SystemMessage(name + " 을 사용할 수 없습니다. "));
    }
    public void SystemMessageCantOpen(string text)
    {
        StartCoroutine(SystemMessage(text));
    }
    private void Start()
    {
        ImageLoader.Instance.Load();
        Skilldata.Instance.Load();
    }

    
}

