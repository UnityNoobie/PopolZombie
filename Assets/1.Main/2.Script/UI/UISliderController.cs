using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UISliderController : MonoBehaviour
{
    Slider m_slider;
    TextMeshProUGUI m_valueText;
    RectTransform m_rectTransform;

    public void SetTransform() //������Ʈ �ҷ����ֱ�
    {
        m_slider = GetComponent<Slider>();
        m_valueText = GetComponentInChildren<TextMeshProUGUI>();
        m_rectTransform = GetComponent<RectTransform>();
        m_slider.interactable = false;
    }
    private void Start()
    {
        SetTransform();
    }
    public void SetSliderValue(int MaxValue, int CurrentValue,bool iswiddthControll = true,float widthvalue = 2) //�����̴��� ����������. �ʿ��� ��� iswidthControll ���� MaxValue ����Ͽ� ���̸� ����.
    {
        m_slider.maxValue = MaxValue;
        m_slider.value = CurrentValue;
        if(iswiddthControll)
        m_rectTransform.sizeDelta = new Vector2(MaxValue*2,m_rectTransform.sizeDelta.y);
        m_valueText.text = CurrentValue + " / " + MaxValue;
    }
}
