using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class ScoreData
{
    public string nickName;
    public DateTime time;
    public float gameDuration;
    public int score;
}
public class ScoreUI : MonoBehaviour
{
    #region Constants and Fields   

    TextMeshProUGUI[] m_records;
    Button m_fore;
    Button m_next;
    Button m_exit;

    public string fileName = "record";
    public List<ScoreData> scores = new List<ScoreData>();
    int page = 1;

    #endregion

    #region Methods
    void SetText()
    {
        for(int i = page; i < page + 10; i++)
        {
            m_records[i-1].text = i +"\t"+ scores[i].nickName + "\t\t" + scores[i].time + "\t\t" + scores[i].gameDuration + "\t\t" + scores[i].score+"\t";
        }
    }
    void SaveScore()
    {
        string filePath = Path.Combine("Resources/Score/", fileName);
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            foreach (ScoreData scoreData in scores)
            {
                string line = $"{scoreData.nickName},{scoreData.time},{scoreData.gameDuration},{scoreData.score}";
                sw.WriteLine(line);
            }
        }
    }
    void LoadScores()
    {
        string filePath = Path.Combine("Resources/Score/", fileName);

        if (File.Exists(filePath))
        {
            scores.Clear();
            using (StreamReader sw = new StreamReader(filePath))
            {
                string line;
                while ((line = sw.ReadLine()) != null)
                {
                    string[] data = line.Split(' ');

                    ScoreData scoreData = new ScoreData
                    {
                        nickName = data[0],
                        time = DateTime.Parse(data[1]),
                        gameDuration = float.Parse(data[2]),
                        score = int.Parse(data[3])
                    };
                    scores.Add(scoreData);
                }
            }
        }
    }
    void SortScores()
    {
        scores.Sort((x, y) =>
        {
            int scoreComparison = y.score.CompareTo(x.score);
            if (scoreComparison == 0)
            {
                return x.gameDuration.CompareTo(y.gameDuration);
            }
            return scoreComparison;
        });
    }
    void NextPage()
    {
        page++;
        SetText();
    }
    void BeforePage()
    {
        if (page <= 1) // 페이지가 1 이하면 리턴
            return;
        page--;
        SetText();
    }
    public void AddScore(string nickname, float gameDuration, int score)
    {
        ScoreData newScore = new ScoreData
        {
            nickName = nickname,
            time = DateTime.Now,
            gameDuration = gameDuration,
            score = score
        };
        scores.Add(newScore);
        SortScores();
        SaveScore();
    }
    public void SetTransform()
    {
        m_exit = Utill.GetChildObject(gameObject,"Button_Exit").GetComponent<Button>();
        m_fore = Utill.GetChildObject(gameObject, "Button_Before").GetComponent<Button>();
        m_next = Utill.GetChildObject(gameObject,"Button_Next").GetComponent<Button> ();
        m_records = Utill.GetChildObject(gameObject, "RecordList").GetComponentsInChildren<TextMeshProUGUI>();
        m_exit.onClick.AddListener(DeActiveUI);
        m_fore.onClick.AddListener(BeforePage);
        m_next.onClick.AddListener(NextPage);
    }
    public void ActiveUI()
    {
        SetText();
        gameObject.SetActive(true);
    }
    public void DeActiveUI()
    {
        gameObject.SetActive(false);
    }
    #endregion

}
