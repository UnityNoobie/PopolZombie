using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagedText : MonoBehaviour
{
    TextMeshProUGUI m_text;
    Color m_color;
    float m_moveTime = 1f;
    float m_startAlpha = 1f;
    float m_targetAlpha = 0f;
    float m_fadeTime = 0.5f;
    float m_elapsedTime = 0f;
    DamageAbleObjectHUD m_hud;
    IEnumerator Coroutine_DamageText(float value)
    {
        Vector3 startPos = m_hud.transform.position + new Vector3(0f, 0.1f, 0f); ;
        Vector3 targetPos = startPos + new Vector3(0f, 1f, 0f);
        m_text.alpha = m_startAlpha;
        m_text.text = value.ToString();
        while (m_elapsedTime < m_moveTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, m_elapsedTime / m_moveTime);
            float t = m_elapsedTime / m_fadeTime;
            m_text.alpha = Mathf.Lerp(m_startAlpha, m_targetAlpha, t);
            m_elapsedTime += Time.deltaTime;
            yield return null;
        }
        m_hud.EnqueDamageText(this);
    }
    void ResetValue()
    {
        m_moveTime = 1f;
        m_startAlpha = 1f;
        m_targetAlpha = 0f;
        m_fadeTime = 0.5f;
        m_elapsedTime = 0f;
    }
    public void SetTransform(DamageAbleObjectHUD hud)
    {
        m_hud = hud;
        m_text = GetComponent<TextMeshProUGUI>();
    }
    public void TextValue(float value)
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        ResetValue();
        if(value > 100) //강한 공격 들어왔을 때 따로 표기
        {
            m_color = Color.yellow;
            m_text.color = m_color;
            m_text.fontSize = 30;
        }
        else if(value < 0)
        {
            m_color = Color.red;
            m_text.color = m_color;
            m_text.fontSize = 20;
        }
        else
        {
            m_color = Color.green;
            m_text.color = m_color;
            m_text.fontSize = 25;
        }
        StartCoroutine(Coroutine_DamageText(value));
    }
}
