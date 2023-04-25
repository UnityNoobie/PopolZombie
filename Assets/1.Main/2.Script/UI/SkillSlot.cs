using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour ,IPointerUpHandler, IPointerEnterHandler,IPointerExitHandler
{
    
    TextMeshProUGUI m_text;
    PlayerSkillController m_player;
    ActiveSkill m_active;
    SkillInfo m_skillInfo;
    SkillUI m_skillUI;
    Image m_image;
    Button m_Button; 
    int m_skillPoint;
    int m_currentSkillID;
    bool m_isActive = false;
    bool m_isChoice = false; //3�ܰ� ��ų �����Ҷ� ����ϱ��.
    PlayerAbilityType m_type;


    /*
    Queue<int> m_ids = new Queue<int>(); //ť�� ����� ���� �غ���
    Queue<int> m_activedId = new Queue<int>(); // ����� ��ų ���̵� �־����
    Queue<bool> m_isActived = new Queue<bool>();*/ //ť�� �̿��� ������ �õ��Ͽ����� ���� �����ϰ� ������ �������� ���� ���






    string m_skillname;
    public TableSkillStat m_skill = new TableSkillStat();
    public void SetSkillSlot(int id, PlayerSkillController player, bool isActive,ActiveSkill activeskill,SkillInfo info)
    {
        m_skillPoint = m_skill.GetSkillData(id).SkillPoint;
        m_image = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>(); //Start���� Ȱ��ȭ�� 
        m_text = GetComponentInChildren<TextMeshProUGUI>();                            //�����߻��Ͽ� �����������ٶ� ȣ��
        m_active = activeskill;
        m_currentSkillID = id;
        RefreshSkillInfo();
        m_skillInfo = info;
        m_player = player;
        m_image.sprite = ImageLoader.Instance.GetImage(m_skill.GetSkillData(m_currentSkillID).Image);
        m_isActive = isActive;
        m_isChoice = false;
        if (isActive) //��ų�� ���� �ִٸ� ���İ� ����
        {
            ActiveSkillAlpha();
        }
    }
    void RefreshSkillInfo()
    {
        m_skillname = m_skill.GetSkillData(m_currentSkillID).SkillName;
        m_text.text = m_skillname;
    }
    public void ChoiceSkillTypes(PlayerSkillController player,PlayerAbilityType type,string image,string info,SkillUI ui) //3�ܰ� ��ų�� ��� 
    {
        m_isChoice = true;
        m_image = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>(); 
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_type = type;
        m_player = player;
        m_skillUI = ui;
        m_image.sprite = ImageLoader.Instance.GetImage(image);
        m_text.text = info;
    }
    /*
    public void ResetQueue() //ť �ʱ�ȭ
    {
        m_ids.Clear();
        m_isActived.Clear();
    }
    public void EnQueues(int id, bool isactive) //ť �ֱ�
    {
        m_ids.Enqueue(id);
        m_isActived.Enqueue(isactive);
    }
    void DequeueSkills()
    {
        m_currentSkillID = m_ids.Dequeue();
        m_isActived.Dequeue();
        if(m_ids != null)
        {
            m_currentSkillID = m_ids.Peek();
        }
        
    }
    void CheckActiveSkills() // ��ų���� 
    {
        if (m_ids == null) return;
        for(int i = 0; i < m_ids.Count; i++)
        {
            if (m_isActived.Peek()) //���� ��ų�� ��Ƽ�� ���¶�� ��ť���ֱ�
            {
                DequeueSkills();
            }
            else
            {
                break;
            }
        }
        
    }
    public void SetSkillSlots(List<int> id, PlayerSkillController player, List<bool> isActive, string image, ActiveSkill activeskill, SkillInfo info)
    {
        m_image = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>(); //Start���� Ȱ��ȭ�� 
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_ids = id;
        m_isActived = isActive;
        m_player = player;
        m_active = activeskill;
        m_skillInfo = info;
    }*/ //ť�� �̿��� �õ������� �� ���������� ���� ���

    void ActiveSkillAlpha() //���� ��ų�� ���İ��� ��������
    {
        Color color = m_image.color;
        color.a = 1f;
        m_image.color = color;
    }
  
    void LoadNextSkill()
    {
        if (m_skill.GetSkillData(m_currentSkillID).NextID != 0 )
        {
            m_currentSkillID = m_skill.GetSkillData(m_currentSkillID).NextID;
            RefreshSkillInfo();
            m_isActive = false;
        }
        else
        {
            m_isActive = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData) //Ŭ�� ��
    {
        if (!m_isChoice) //���� ��Ȳ����
        {
            if (m_isActive)//������ ��ų�� ��� ��� �޼��� ȣ�� �� ������ �ϵ���
            {
                UIManager.Instance.SystemMessageCantOpen("�̹� ������ ��ų�Դϴ�.");
                return;
            }
            if (m_player.IsCanActive(m_skillPoint, m_skill.GetSkillData(m_currentSkillID).SkillGrade, m_skill.GetSkillData(m_currentSkillID).SkillType)) //���� ������ ����Ʈ�� �ִٸ� ��ų ���� UI�� �ҷ��� ó��
            {
                m_active.ActiveUI(this, m_skillPoint, m_skillname);
            }
        } 
        else // ������ Ư�� ���� ����϶�
        {
            m_player.SetAblityType(m_type);
            m_skillUI.ChoiceFinished();
            
        }
      
    }
    public void OnPointerEnter(PointerEventData eventData) //�ö���� �� ����â �ҷ���
    {
        if (!m_isChoice) //���� ��Ȳ����
            m_skillInfo.ActiveUI(m_currentSkillID);
    }
    public void OnPointerExit(PointerEventData eventData) //������ �� ����â ��
    {
        if (!m_isChoice) //���� ��Ȳ����
            if (eventData.pointerEnter.CompareTag("Slot")) //�±װ� �����ΰͿ��� ����
                m_skillInfo.DeActiveUI();
    }
    public void ActiveSkill()
    {
        if (!m_isActive)
        {
            m_player.GetComponent<PlayerSkillController>().ActiveSkill(m_currentSkillID);
            LoadNextSkill();
        }
    }

}
