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

    public const string fileName = "Score.json"; //������ ���� Json����
    public List<ScoreData> gameDataList = new List<ScoreData>(); // ScoreData�� �ӽ� �����ϴ� List
    int page = 0;

    #endregion

    #region Methods
  
    public void SaveGameData(PlayerController player,int gameDuration, int round) // ���� �����͸� Json���·� ���� > ���� > ���� �ð� ������ �����Ͽ� ����
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
    public void LoadGameData() // ���� �����͸� �ҷ��� ScoreData List�� �־��ֱ�
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            gameDataList = JsonConvert.DeserializeObject<List<ScoreData>>(jsonData);
        }
    }
    void SetText() //�ش� ������ ������ ������ ǥ�����ִ� �޼ҵ�.
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
    void NextPage() //������ �̵�
    {
        if((gameDataList.Count / 10) > page) //������ ����Ʈ�� �������� �ѱ� ������ �ȴٸ�.
        {
            page++;
            SetText();
        }
    }
    void BeforePage()//������ �̵�
    {
        if (page < 1) // �������� 1 �̸��̸� ����
            return;
        page--;
        SetText();
    }

    public void SetTransform() //���� ��ü ��ġ����, ������ ��������
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

    public void ActiveUI() // UI�� Ȱ��ȭ �ϸ� ������ ������ ��ġ.
    {
        gameObject.SetActive(true);
        LoadGameData();
        SetText();
    }

    public void DeactiveUI() //���ֱ�
    {
        gameObject.SetActive(false);
    }

    #endregion
}