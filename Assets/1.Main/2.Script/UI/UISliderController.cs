using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UISliderController : MonoBehaviour
{
    Slider m_slider;
    TextMeshProUGUI m_valueText;
    RectTransform m_rectTransform;
    Vector2 m_baseValue;
    public void SetTransform() //컴포넌트 불러와주기
    {
        m_slider = GetComponent<Slider>();
        m_valueText = GetComponentInChildren<TextMeshProUGUI>();
        m_rectTransform = GetComponent<RectTransform>();
        m_slider.interactable = false;
        m_baseValue = m_rectTransform.sizeDelta;
    }
    private void Start()
    {
        SetTransform();
    }
    public void SetSliderValue(int MaxValue, int CurrentValue,bool iswiddthControll = false,float widthvalue = 2f) //슬라이더의 벨류값조절. 필요한 경우 iswidthControll 통해 MaxValue 비례하여 넓이를 설정.
    {
        m_slider.maxValue = MaxValue;
        m_slider.value = CurrentValue;
        float widthValue = MaxValue * widthvalue;
        if(widthValue > m_baseValue.x * 2) //기본 사이즈의 2배가 넘어간다면 더이상 확장 X
        {
            widthValue = m_baseValue.x * 2;
        }
        if (iswiddthControll)
        m_rectTransform.sizeDelta = new Vector2(widthValue, m_baseValue.y);
        m_valueText.text = CurrentValue + " / " + MaxValue;
    }
}
