
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
