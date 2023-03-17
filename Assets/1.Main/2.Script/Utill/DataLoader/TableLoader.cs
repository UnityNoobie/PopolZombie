using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class TableLoader : Singleton<TableLoader>
{
    //  List<List<int>> list; //리스트의 2차원배열
    // Dictionary<string, string> row; 한 행만을 저장가능.
    List<Dictionary<string, string>> m_table = new List<Dictionary<string, string>>(); //이런식으로 딕셔너리의 배열 선언가능!
    public int Length { get { return m_table.Count; } }
    public void Clear()
    {
        m_table.Clear();
    }
    public string GetString(string key, int index)
    {

        return m_table[index][key];

    }
    public byte GetByte(string key, int index)
    {
        return byte.Parse(GetString(key, index));
    }
    public int GetInteger(string key, int index)
    {

        return int.Parse(GetString(key, index)); 
    }
    public float GetFloat(string key, int index)
    {
        return float.Parse(GetString(key, index));
    }
    public bool GetBool(string key, int index)
    {
        return bool.Parse(GetString(key, index));
    }
    public T GetEnum<T>(string key, int index)
    {
        return (T)Enum.Parse(typeof(T), GetString(key, index));
    }
    public byte[] LoadTableData(string tableName)
    {
        var data = Resources.Load<TextAsset>("ExcelDatas/" + tableName);
        return data.bytes;
    }
    public void LoadData(byte[] data)
    {
        m_table.Clear();
        MemoryStream ms = new MemoryStream(data);
        BinaryReader br = new BinaryReader(ms);

        int rowCount = br.ReadInt32();
        int columnCount = br.ReadInt32();
        string textData = br.ReadString();
        var strDatas = textData.Split('\t');
        List<string> keyList = new List<string>();
        int offset = 1;
        for (int i = 0; i < rowCount; i++)
        {
            offset++;
            if (i == 0)
            {
                for (int j = 0; j < columnCount - 1; j++)
                {
                    keyList.Add(strDatas[offset]);
                    offset++;
                }
            }
            else
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>();
                for (int j = 0; j < columnCount - 1; j++)
                {
                    rowData.Add(keyList[j], strDatas[offset]);
                    offset++;
                }
                m_table.Add(rowData);
            }
        }
        br.Close();
        ms.Close();

    }

}
