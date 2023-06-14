using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using UnityEngine.Playables;

[System.Serializable]
public class ScoreData
{
    public string nickName;
    public int level;
    public int round;
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

    public const string fileName = "Score.json";
    public List<ScoreData> gameDataList = new List<ScoreData>();
    int page = 0;

    #endregion

    #region Methods

    void SetText()
    {
        for(int i = page*10; i < page * 10 + 10; i++)
        {
            if (i >= gameDataList.Count)
                break;
            m_records[i].text = (i + 1) + "\t" + gameDataList[i].level + "\t" + gameDataList[i].nickName + "\t\t" + gameDataList[i].round + "\t\t" + (int)gameDataList[i].gameDuration/60 +" : "+ gameDataList[i].gameDuration%60 + "\t\t" + gameDataList[i].score + "\t\t";
        }
    }
    public void SaveGameData(PlayerController player,int gameDuration, int round)
    {
        ScoreData newData = new ScoreData();
        newData.level = player.GetStatus.level;
        newData.nickName = player.GetStatus.KnickName;
        newData.round = round;
        newData.gameDuration = gameDuration;
        newData.score = player.GetScore();

        gameDataList.Add(newData);
        gameDataList = gameDataList.OrderByDescending(data => data.round).ThenBy(data => data.score).ToList();

        string jsonData = JsonConvert.SerializeObject(gameDataList, Formatting.Indented);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, jsonData);
    }
    public void LoadGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            gameDataList = JsonConvert.DeserializeObject<List<ScoreData>>(jsonData);
        }
        Debug.Log(filePath);
    }
    void NextPage()
    {
        if((gameDataList.Count / 10) > page) //데이터 리스트가 페이지를 넘길 조건이 된다면.
        {
            page++;
            SetText();
        }
    }

    void BeforePage()
    {
        if (page < 1) // 페이지가 1 미만이면 리턴
            return;
        page--;
        SetText();
    }

    public void SetTransform()
    {
        m_exit = Utill.GetChildObject(gameObject, "Button_Exit").GetComponent<Button>();
        m_fore = Utill.GetChildObject(gameObject, "Button_Before").GetComponent<Button>();
        m_next = Utill.GetChildObject(gameObject, "Button_Next").GetComponent<Button>();
        m_records = Utill.GetChildObject(gameObject, "RecordList").GetComponentsInChildren<TextMeshProUGUI>();
        m_exit.onClick.AddListener(DeactiveUI);
        m_fore.onClick.AddListener(BeforePage);
        m_next.onClick.AddListener(NextPage);
        LoadGameData();
    }

    public void ActiveUI()
    {
        gameObject.SetActive(true);
        LoadGameData();
        SetText();
    }

    public void DeactiveUI()
    {
        gameObject.SetActive(false);
    }

    #endregion
}