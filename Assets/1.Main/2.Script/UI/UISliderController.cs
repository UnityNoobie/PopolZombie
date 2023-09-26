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
    public void SetSliderValue(int MaxValue, int CurrentValue,bool iswiddthControll = false,float widthvalue = 2f,bool issmallSize = false) //�����̴��� ����������. �ʿ��� ��� iswidthControll ���� MaxValue ����Ͽ� ���̸� ����.
    {
        m_slider.maxValue = MaxValue;
        m_slider.value = CurrentValue;
        float widthValue = MaxValue * widthvalue;
        if(issmallSize && widthValue > 3f)
        {
            widthValue = 3f;
        }
        if (iswiddthControll)
        m_rectTransform.sizeDelta = new Vector2(widthValue,m_rectTransform.sizeDelta.y);
        m_valueText.text = CurrentValue + " / " + MaxValue;
    }
}
