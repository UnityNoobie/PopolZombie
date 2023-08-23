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
    public void ScoreChange(float score) //ȭ�鿡 ǥ��Ǵ� ���� ǥ��
    {
       m_Score.text = "Score : " +score;
    }
    public void MoneyUI(int money) //ȭ�鿡 ǥ��Ǵ� �����ݾ� ǥ��
    {
        m_Money.text = money.ToString();
    }
    public void RoundInfo(int thisRound) //���� ���� UI
    {
        waveUI.text = (thisRound + " ����" );
    }
    public void WeaponImage(string name)  //���⺰ �̹��� ��ü.
    {
        Texture wptex = ImageLoader.Instance.GetImage(name).texture;
        m_weaponImage.mainTexture = wptex;
        m_quickSlot.SetItem(0,name);
    }
    public void HPBar(float hp, float max) //��ũ�� ü�¹� ����
    {
        m_hplable.text = (hp + " / " + max);
        m_hpUI.value = hp / max;
    }
    public void EXPUI(float exp,float max) //��ũ�� ����ġ�� ����
    {
        m_EXP.text = (exp+" / "+max);
        m_EXPBar.value = exp / max;
    } 
    public void DamagedUI() //���� ���� �� UITweenȰ���Ͽ� ��ũ�� ���� ȿ��( + ü�¹��� ������ ���� ���İ� ����)
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
    public void DiedUI() //��� �� ȭ�鿡 ǥ���� UI (UITweenȰ��)
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
    public void ReviveUI() //��Ȱ �� ��ũ�� ǥ�� UI
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
    public void HealUI() //��ũ�� ȸ�� ȿ��
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
    public void LevelUPUI(int level) //��ũ�� ������ ȿ��
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
    public void WeaponInfoUI(string Info) //���� ���� ȭ�� ǥ��
    {   
        m_weaponInfoUI.text = Info;
    }
    public void EnemyLeft(int remain) //���� �� ǥ��
    {
        m_enemyReamain.text = "������ : "+remain;
    }
    public void TimeLeft(int remain) //���� ������� ���� �ð� ǥ��
    {
        m_enemyReamain.text = "����� : " + remain;
    }
    public void GameDuration(int time)  //���� ����ð� ǥ��
    {
        m_gameDuration.text = "����ð� : "+ time/60 +":" + time%60;
    }
    #endregion
    public Light GetLight()
    {
        return m_dirLight;
    }

}

