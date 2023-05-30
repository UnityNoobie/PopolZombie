using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static SoundManager;
public enum SoundType
{
    BGM,
    SFX,
    MAX
}
public class TableSound
{
    public string Sound { get; set; }
    public string[] soundList { get; set; }
    public SoundType type { get; set; }
    public int maxPlay { get;set; }
    public TableSound() {}
    public TableSound GetSound(string sound)
    {
        TableSound table = new TableSound();
        table = TableSoundInfo.Instance.m_soundDic[sound];
        return table;
    }
}
public class TableSoundInfo : Singleton<TableSoundInfo>
{
    public Dictionary<string, TableSound> m_soundDic = new Dictionary<string, TableSound>();
    public void Load()
    {
        m_soundDic.Clear();
        TableLoader.Instance.LoadData(TableLoader.Instance.LoadTableData("SoundFX"));
        for(int i = 0; i < 37;  i++)
        {
            TableSound data = new TableSound();
            string str = TableLoader.Instance.GetString("Type", i);
            if (str.Equals("BGM"))
                data.type = SoundType.BGM;
            else if(str.Equals("SFX"))
                data.type = SoundType.SFX;
            data.Sound = TableLoader.Instance.GetString("Id", i);
            data.soundList = TableLoader.Instance.GetString("Name",i).Split(',');
            data.maxPlay = TableLoader.Instance.GetInteger("MaxPlay",i);
            m_soundDic.Add(data.Sound, data);
        }
        TableLoader.Instance.Clear();

    }
}
