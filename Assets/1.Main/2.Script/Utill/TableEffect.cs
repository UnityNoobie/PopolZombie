/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableEffect : Singleton<TableEffect>
{
    public class Data
    {
        public int Id;
        public string Dummy;
        public string[] Prefab = new string[4];
    }
    public Dictionary<int, Data> m_tableData = new Dictionary<int, Data>();

    public void Load()
    {
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("Effect"));
        m_tableData.Clear();
        Debug.Log(TableLoader.Instance.Length + "���̺� �δ��� �����Դϴ�.");
        for (int i = 0; i < TableLoader.Instance.Length; i++)
        {
            Debug.Log(TableLoader.Instance.Length + "���̺� �δ��� �����Դϴ�.");
            Data data = new Data();
            data.Id = TableLoader.Instance.GetInteger("Id", i);
            data.Dummy = TableLoader.Instance.GetString("Dummy", i);
            Debug.Log(data.Id + "���̵�� ���� :" + data.Dummy + "�׸��� i�� �� " + i);
            for (int j = 0; j < data.Prefab.Length; j++)
            {
                data.Prefab[j] = TableLoader.Instance.GetString("Prefab_" + (j + 1), i);
                Debug.Log(data.Id + "�� " + j + "��°" + data.Prefab[j] + "������");
            }
            m_tableData.Add(data.Id, data);

            TableLoader.Instance.Clear();
        }
    }

}
*/
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TableEffect : Singleton<TableEffect>
{
    public class Data
    {
        public int Id;
        public string Dummy;
        public string[] Prefab = new string[3];

    }
    public Dictionary<int, Data> m_tableData = new Dictionary<int, Data>();

    public void Load()
    {
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("Effect"));
        m_tableData.Clear();
        for (int i = 0; i < 8; i++)
        {
            Data data = new Data();
            data.Id = TableLoader.Instance.GetInteger("Id", i);
            data.Dummy = TableLoader.Instance.GetString("Dummy", i);
            for (int j = 0; j < data.Prefab.Length; j++)
            {
                data.Prefab[j] = TableLoader.Instance.GetString("Prefab_" + (j + 1), i);
            }
            m_tableData.Add(data.Id, data);
        }
        TableLoader.Instance.Clear();
    }

}
