using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Records : MonoBehaviour
{
    #region Constants and Fields
    TextMeshProUGUI m_grade;
    TextMeshProUGUI m_level;
    TextMeshProUGUI m_nickName;
    TextMeshProUGUI m_round;
    TextMeshProUGUI m_playTime;
    TextMeshProUGUI m_score;
    #endregion
    #region Methods


    public void SetInfo(int grade, int level,string nickname, int round, string playtime,int score)
    {
        m_grade.text = grade.ToString();
        m_level.text = level.ToString();
        m_nickName.text = nickname;
        m_round.text = round.ToString();
        m_playTime.text = playtime;
        m_score.text = score.ToString();
    }
    public void NullRecords()
    {
        ResetText();
    }
    void ResetText()
    {
        m_grade.text = null;
        m_level.text = null;
        m_nickName.text = null;
        m_round.text = null;
        m_playTime.text = null;
        m_score.text = null;
    }
    public void SetTransform()
    {
        m_grade = Utill.GetChildObject(gameObject, "Grade").GetComponent<TextMeshProUGUI>();
        m_level = Utill.GetChildObject(gameObject, "Level").GetComponent<TextMeshProUGUI>();
        m_nickName = Utill.GetChildObject(gameObject, "NickName").GetComponent<TextMeshProUGUI>();
        m_round = Utill.GetChildObject(gameObject, "Round").GetComponent<TextMeshProUGUI>();
        m_playTime = Utill.GetChildObject(gameObject, "PlayTime").GetComponent<TextMeshProUGUI>();
        m_score = Utill.GetChildObject(gameObject, "Score").GetComponent<TextMeshProUGUI>();
    }
    #endregion

}
