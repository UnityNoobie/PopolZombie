using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ScreenHUD : MonoBehaviour
{

    Transform m_sliderPos;
    Transform m_textPos;
    Transform m_slotPos;
    Transform m_itemSlotPos;

    UISliderController m_hpSlider;
    UISliderController m_expSlider;

    TextMeshProUGUI m_roundText;
    TextMeshProUGUI m_subText;
    TextMeshProUGUI m_scoreText;
    TextMeshProUGUI m_moneyText;
    TextMeshProUGUI m_timeText;
    TextMeshProUGUI m_weaponText;

    RawImage m_mainWeapon;
    RawImage m_subWeapon;

    UIQuickSlot[] m_quickSlots;

    public void SetRoundText(int round)
    {
        m_roundText.text = round + " 라운드";
    }
    public void GameDuration(int time)
    {
        m_timeText.text = "진행시간 : " + time / 60 + ":" + time % 60;
    }
    public void TimeLeft(int remain)
    {
        m_subText.text = "밤까지 : " + remain;
    }
    public void EnemyLeft(int remain)
    {
        m_subText.text = "남은적 : " + remain;
    }
    public void SetScore(int score)
    {
        m_scoreText.text = "Score : "+ score;
    }
    public void SetActive(bool active)
    {
        if(!active)  ResetData();
        gameObject.SetActive(active);
    }
    public void SetHPValue(int max, int current)
    {
        m_hpSlider.SetSliderValue(max,current,true,2f);
    }
    public void SetEXPValue(int max, int current)
    {
        m_expSlider.SetSliderValue(max,current,true,1.5f);
    }
    public void SetMoney(int money)
    {
        m_moneyText.text = money + "골드";
    }
    public void SetMainImage(string image)
    {
        m_mainWeapon.texture = ImageLoader.Instance.GetImage(image).texture;
        if (!m_mainWeapon.enabled) m_mainWeapon.gameObject.SetActive(true);
        UpdateQuickSlotItem(0, 1, image, true);
    }
    public void UpdateQuickSlotItem(int slottype,int itemCount,string sprite,bool isweapon = false)
    {
        m_quickSlots[slottype].UpdateItemSlot(sprite, itemCount,isweapon);
    }
    public void SetWeaponInfo(string info)
    {
        m_weaponText.text = info;
    }
    void ResetData()
    {
        m_roundText.text = null;
        m_moneyText.text = null;
        m_timeText.text = null;
        m_weaponText.text = null;
        m_scoreText.text = null;
        m_mainWeapon.texture = null;
        m_subWeapon.texture = null;
    }
    public void SetTransform()
    {
        m_sliderPos = Utill.GetChildObject(gameObject, "HUD_Slider");
        m_textPos = Utill.GetChildObject(gameObject, "HUD_InfoText");
        m_slotPos = Utill.GetChildObject(gameObject, "HUD_Slots");
        m_itemSlotPos = Utill.GetChildObject(m_slotPos.gameObject, "ItemSlot");

        m_hpSlider = Utill.GetChildObject(m_sliderPos.gameObject,"HPSlider").GetComponent<UISliderController>();
        m_expSlider = Utill.GetChildObject(m_sliderPos.gameObject, "EXPSlider").GetComponent<UISliderController>();

        m_hpSlider.SetTransform();
        m_expSlider.SetTransform();

        m_roundText = Utill.GetChildObject(m_textPos.gameObject,"RoundText").GetComponent<TextMeshProUGUI>();
        m_subText = Utill.GetChildObject(m_textPos.gameObject, "SubInfoText").GetComponent<TextMeshProUGUI>();
        m_scoreText = Utill.GetChildObject(m_textPos.gameObject, "ScoreText").GetComponent<TextMeshProUGUI>();
        m_moneyText = Utill.GetChildObject(m_textPos.gameObject, "MoneyText").GetComponent<TextMeshProUGUI>();
        m_timeText = Utill.GetChildObject(m_textPos.gameObject, "TimeText").GetComponent<TextMeshProUGUI>();
        m_weaponText = Utill.GetChildObject(m_textPos.gameObject, "WeaponInfoText").GetComponent<TextMeshProUGUI>();

        m_mainWeapon = Utill.GetChildObject(m_slotPos.gameObject,"MainWeapon").GetComponent<RawImage>();
        m_subWeapon = Utill.GetChildObject(m_slotPos.gameObject,"SubWeapon").GetComponent<RawImage>();

        m_quickSlots = m_itemSlotPos.GetComponentsInChildren<UIQuickSlot>(true);

        for(int i = 0; i < m_quickSlots.Length; i++)
        {
            m_quickSlots[i].SetTransform();
        }
    }
}
