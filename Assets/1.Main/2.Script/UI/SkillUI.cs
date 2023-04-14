using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillUI : MonoBehaviour
{
    PlayerSkillController m_PlayerSkills;
    TableSkillStat m_stat = new TableSkillStat();
    PlayerController player;
    Transform m_contents;
    Button[] m_buttons;
    [SerializeField]
    SkillSlot[] m_SkillSlot;
   // SkillSlot m_masterLvSlot;
    SkillType m_Type;
    int[] m_Id;
    List<int> m_list = new List<int>();
    int count;
    List<int> m_ActiveSkillList = new List<int>();
    List<int> m_SkillList = new List<int>();
    void SetPos() //레벨별로 정리되어있는 스킬슬롯 부모 위치 받아오기 는 OutOfIndex 계속 나와서 삭제
    {
        m_contents = Utill.GetChildObject(gameObject, "Content");
    }
    void FindSlots() //위치에서 자식오브젝트의 슬롯목록 가져오기
    {
        m_SkillSlot = m_contents.GetComponentsInChildren<SkillSlot>();
     //   ArrayList.Sort(m_SkillSlot);
       // m_SkillSlot = m_SkillLvPos[1].GetComponentsInChildren<SkillSlot>();
       // m_SkillSlot = m_SkillLvPos[2].GetComponentsInChildren<SkillSlot>();
       // m_SkillSlot = m_SkillLvPos[1].GetComponentInChildren<SkillSlot>();
        //Debug.Log("저렙스킬 갯수 : " + m_lowLvSlot.Length + "중렙 : " + m_midLvSlot.Length + "마스터 : " + m_masterLvSlot);
    }
    void SetSlotList(SkillType type)
    {
        ResetList();
        m_Type = type;
        if(m_Type.Equals(SkillType.Shooter))
        {
            m_SkillList = Skilldata.AgilityList;
        }
        else if(m_Type.Equals(SkillType.Physical))
        {
            m_SkillList = Skilldata.StrengthList;
        }
        else if(m_Type.Equals(SkillType.Utility))
        {
            m_SkillList = Skilldata.UtilityList;
        }
        for (int i = 0; i < m_SkillList.Count; i++)
        {
           m_SkillSlot[i].SetSkillSlot(m_SkillList[i], player, false, m_stat.GetSkillData(m_SkillList[i]).Image);
        }
    }
    void ResetList()
    {
        m_SkillList.Clear();
    }
    public void ActiveSkill()
    {
        gameObject.SetActive(true);
    }
    public void DeActiveSkill()
    {
        gameObject.SetActive(false);
    }
    void GetComponentsSlots(GameObject parent)
    {
        parent.GetComponentsInChildren<SkillSlot>();
    }
    public void ActiveSkillUI(PlayerController player) //켜질때 실행
    {
        m_PlayerSkills = player.GetComponent<PlayerSkillController>();
        m_ActiveSkillList.Clear();
        m_ActiveSkillList = m_PlayerSkills.m_skillList;
    }


    private void Start()
    {
        SetPos();
        FindSlots();
        SetSlotList(SkillType.Shooter);
    }
}
