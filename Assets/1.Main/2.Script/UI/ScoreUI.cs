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
    Records[] m_records;
    Button m_fore;
    Button m_next;
    Button m_exit;

    public const string fileName = "Score.json"; //데이터 저장 Json파일
    public List<ScoreData> gameDataList = new List<ScoreData>(); // ScoreData를 임시 저장하는 List
    int page = 0;

    #endregion

    #region Methods
  
    public void SaveGameData(PlayerController player,int gameDuration, int round) // 게임 데이터를 Json형태로 라운드 > 점수 > 게임 시간 순서로 정렬하여 저장
    {
        ScoreData newData = new ScoreData();
        newData.level = player.GetStatus.level;
        newData.nickName = player.GetStatus.KnickName;
        newData.round = round;
        newData.gameDuration = gameDuration;
        newData.score = player.GetScore();

        gameDataList.Add(newData);
        gameDataList = gameDataList.OrderByDescending(data => data.round).ThenByDescending(data => data.score).ThenByDescending(data => Mathf.FloorToInt(data.gameDuration)).ToList();

        string jsonData = JsonConvert.SerializeObject(gameDataList, Formatting.Indented);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, jsonData);
    }
    public void LoadGameData() // 게임 데이터를 불러와 ScoreData List에 넣어주기
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            gameDataList = JsonConvert.DeserializeObject<List<ScoreData>>(jsonData);
        }
    }
    void SetText() //해당 페이지 범위의 점수를 표기해주는 메소드.
    {
        int temp = 0;
        for (int i = page * 10; i < (page * 10) + 10; i++)
        {
            if (i >= gameDataList.Count)
            {
                m_records[temp].NullRecords();
            }
            else
            {
                m_records[temp].SetInfo(i + 1, gameDataList[i].level, gameDataList[i].nickName, gameDataList[i].round, (int)gameDataList[i].gameDuration / 60 + " : " + gameDataList[i].gameDuration % 60, gameDataList[i].score);
            }
            temp++;
        }
    }
    void NextPage() //페이지 이동
    {
        if((gameDataList.Count / 10) > page) //데이터 리스트가 페이지를 넘길 조건이 된다면.
        {
            page++;
            SetText();
        }
    }
    void BeforePage()//페이지 이동
    {
        if (page < 1) // 페이지가 1 미만이면 리턴
            return;
        page--;
        SetText();
    }

    public void SetTransform() //하위 객체 위치지정, 데이터 가져오기
    {
        m_exit = Utill.GetChildObject(gameObject, "Button_Exit").GetComponent<Button>();
        m_fore = Utill.GetChildObject(gameObject, "Button_Before").GetComponent<Button>();
        m_next = Utill.GetChildObject(gameObject, "Button_Next").GetComponent<Button>();
        m_records = Utill.GetChildObject(gameObject,"RecordList").GetComponentsInChildren<Records>();
        for(int i = 0; i < m_records.Length; i++)
        {
            m_records[i].SetTransform();
        }
        m_exit.onClick.AddListener(DeactiveUI);
        m_fore.onClick.AddListener(BeforePage);
        m_next.onClick.AddListener(NextPage);
        LoadGameData();
    }

    public void ActiveUI() // UI를 활성화 하며 데이터 가져와 배치.
    {
        gameObject.SetActive(true);
        LoadGameData();
        SetText();
    }

    public void DeactiveUI() //꺼주기
    {
        gameObject.SetActive(false);
    }

    #endregion
}