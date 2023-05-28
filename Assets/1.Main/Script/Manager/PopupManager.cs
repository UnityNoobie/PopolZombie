using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : SingletonDontDestroy<PopupManager>
{
    [SerializeField]
    GameObject m_popupOkCancelPrefab;
    [SerializeField]
    GameObject m_popupOkPrefab;
    const int StartPopupDepth = 1000;
    const int PopupDepthGap = 10;
    List<GameObject> m_popupList = new List<GameObject>();
    public bool IsOpen { get { return m_popupList.Count > 0; } }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_popupOkCancelPrefab = Resources.Load<GameObject>("Prefab/UI/Popup/Popup_OkCancel");
        m_popupOkPrefab = Resources.Load<GameObject>("Prefab/UI/Popup/Popup_Ok");
    }
    public void PopupOpen_Ok(string title, string body, Action okDel, string okBtnText = "Ok")
    {
        var obj = Instantiate(m_popupOkPrefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        var panels = obj.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = StartPopupDepth + m_popupList.Count * PopupDepthGap + i;
        }
        var popup = obj.GetComponent<Popup_Ok>();
        popup.SetUI(title, body, okDel, okBtnText);
        m_popupList.Add(obj);
    }
    public void PopupOpen_OkCancel(string title, string body, Action okDel, Action cancelDel, string okBtnText = "Ok", string cancelBtnText = "Cancel")
    {
        var obj = Instantiate(m_popupOkCancelPrefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        var panels = obj.GetComponentsInChildren<UIPanel>();
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = StartPopupDepth + m_popupList.Count * PopupDepthGap + i;
        }
        var popup = obj.GetComponent<Popup_OkCancel>();
        popup.SetUI(title, body, okDel, cancelDel, okBtnText, cancelBtnText);
        m_popupList.Add(obj);
    }
    public void PopupClose()
    {
        Destroy(m_popupList[m_popupList.Count - 1]);
        m_popupList.RemoveAt(m_popupList.Count - 1);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (UnityEngine.Random.Range(1, 101) % 2 == 0)
                PopupOpen_OkCancel("공지사항", "안녕하십니까.\r\n현재 팝업테스트 중입니다.", null, null, "확인", "취소");
            else
                PopupOpen_Ok("서비스종료", "데이터 유실로 인해 서비스를 종료합니다. .", null, "결정");
        }
    }
}
