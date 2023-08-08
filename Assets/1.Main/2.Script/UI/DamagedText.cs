using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagedText : MonoBehaviour //DamageAbleObjectHUD에서 호출되는 TMP
{
    #region Constants and Fields
    TextMeshProUGUI m_text;
    Color m_color;
    float m_moveTime = 1f;
    float m_startAlpha = 1f;
    float m_startPosy = 0.3f;
    float m_targetPosy = 1f;
    float m_targetAlpha = 0f;
    float m_fadeTime = 0.5f;
    float m_elapsedTime = 0f;
    DamageAbleObjectHUD m_hud;
    #endregion

    #region Coroutine
    IEnumerator Coroutine_DamageText(float value) //코루틴을 통해 자연스럽게 위로 올라가는것, 투명해지는것 구현
    {
        Vector3 startPos = m_hud.transform.position + new Vector3(0f, m_startPosy, 0f); ;
        Vector3 targetPos = startPos + new Vector3(0f, m_targetPosy, 0f);
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
    #endregion

    #region Methods
    void ResetValue() //벨류 초기화
    {
        m_moveTime = 1f;
        m_startAlpha = 1f;
        m_targetAlpha = 0f;
        m_fadeTime = 0.5f;
        m_elapsedTime = 0f;
    }
    public void SetTransform(DamageAbleObjectHUD hud) //좌표지정
    {
        m_hud = hud;
        m_text = GetComponent<TextMeshProUGUI>();
    }
    public void TextValue(float value) // 텍스트의 벨류값 지정. 들어온 값에 따라 다르게 표기
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        ResetValue();
        if(value < -100) //강한 공격 들어왔을 때 따로 표기
        {
            m_color = Color.yellow;
            m_text.color = m_color;
            m_text.fontSize = 0.2f;
        }
        else if(value < 0) //일반공격
        {
            m_color = Color.red;
            m_text.color = m_color;
            m_text.fontSize = 0.15f;
        }
        else //회복
        {
            m_color = Color.green;
            m_text.color = m_color;
            m_text.fontSize = 0.15f;
        }
        StartCoroutine(Coroutine_DamageText(value));
    }
    #endregion
}
