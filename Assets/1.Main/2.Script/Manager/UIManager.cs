using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static GunManager;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    #region Constants and Fields
    [SerializeField]
    Light m_dirLight;
    [SerializeField]
    public UILabel m_weaponInfoUI;
    [SerializeField]
    UILabel waveUI;
    [SerializeField]
    UISlider m_hpUI;
    [SerializeField]
    UISlider m_EXPBar;
    [SerializeField]
    UILabel m_hplable;
    [SerializeField]
    UILabel m_enemyReamain;
    [SerializeField]
    UILabel m_gameDuration;
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
    UILabel m_Score;
    [SerializeField] 
    UILabel m_Money;
    [SerializeField]
    UILabel m_EXP;
    [SerializeField]
    UILabel m_screenLV;
    [SerializeField]
    QuickSlot m_quickSlot;
    #endregion

    #region ScreenUI
    public void ScoreChange(float score) //화면에 표기되는 점수 표기
    {
       m_Score.text = "Score : " +score;
    }
    public void MoneyUI(int money) //화면에 표기되는 보유금액 표기
    {
        m_Money.text = money.ToString();
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
    }
    public void HPBar(float hp, float max) //스크린 체력바 조절
    {
        m_hplable.text = (hp + " / " + max);
        m_hpUI.value = hp / max;
    }
    public void EXPUI(float exp,float max) //스크린 경험치바 조절
    {
        m_EXP.text = (exp+" / "+max);
        m_EXPBar.value = exp / max;
    } 
    public void DamagedUI() //피해 입을 시 UITween활용하여 스크린 피해 효과( + 체력바의 벨류에 따라 알파값 조정)
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
    public void DiedUI() //사망 시 화면에 표기할 UI (UITween활용)
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
    public void ReviveUI() //부활 시 스크린 표기 UI
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
    public void HealUI() //스크린 회복 효과
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
    public void LevelUPUI(int level) //스크린 레벨업 효과
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
        m_screenLV.text = "LV" + level;
    }
    public void WeaponInfoUI(string Info) //무기 정보 화면 표시
    {   
        m_weaponInfoUI.text = Info;
    }
    public void EnemyLeft(int remain) //남은 적 표기
    {
        m_enemyReamain.text = "남은적 : "+remain;
    }
    public void TimeLeft(int remain) //다음 라운드까지 남은 시간 표기
    {
        m_enemyReamain.text = "밤까지 : " + remain;
    }
    public void GameDuration(int time)  //게임 진행시간 표기
    {
        m_gameDuration.text = "진행시간 : "+ time/60 +":" + time%60;
    }
    #endregion
    public Light GetLight()
    {
        return m_dirLight;
    }

}

