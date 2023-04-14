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
    void SetPos() //�������� �����Ǿ��ִ� ��ų���� �θ� ��ġ �޾ƿ��� �� OutOfIndex ��� ���ͼ� ����
    {
        m_contents = Utill.GetChildObject(gameObject, "Content");
    }
    void FindSlots() //��ġ���� �ڽĿ�����Ʈ�� ���Ը�� ��������
    {
        m_SkillSlot = m_contents.GetComponentsInChildren<SkillSlot>();
     //   ArrayList.Sort(m_SkillSlot);
       // m_SkillSlot = m_SkillLvPos[1].GetComponentsInChildren<SkillSlot>();
       // m_SkillSlot = m_SkillLvPos[2].GetComponentsInChildren<SkillSlot>();
       // m_SkillSlot = m_SkillLvPos[1].GetComponentInChildren<SkillSlot>();
        //Debug.Log("������ų ���� : " + m_lowLvSlot.Length + "�߷� : " + m_midLvSlot.Length + "������ : " + m_masterLvSlot);
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
    public void ActiveSkillUI(PlayerController player) //������ ����
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
