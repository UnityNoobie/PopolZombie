using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour ,IPointerUpHandler, IPointerEnterHandler,IPointerExitHandler
{

    #region Constants and Fields
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


    string m_skillname;
    public TableSkillStat m_skill = new TableSkillStat();
    #endregion

    #region Methods
    public void SetSkillSlot(int id, PlayerSkillController player, bool isActive,ActiveSkill activeskill,SkillInfo info) //��ų ���� ����
    { 
        m_skillPoint = m_skill.GetSkillData(id).SkillPoint;
        m_image = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>();
        m_text = GetComponentInChildren<TextMeshProUGUI>();                           
        m_active = activeskill;
        m_currentSkillID = id;
        RefreshSkillInfo();
        m_skillInfo = info;
        m_player = player;
        m_image.sprite = ImageLoader.Instance.GetImage(m_skill.GetSkillData(m_currentSkillID).Image);
        m_isActive = isActive;
        m_isChoice = false;
        ActiveSkillAlpha();
    }
    void RefreshSkillInfo() //��ų ���� �ٽ� �����Ȥ�
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

    void ActiveSkillAlpha() // ��ų ��Ƽ�� ���ο� ���� ���İ��� ��������
    {
        Color color = m_image.color;
        if(m_isActive)
        {
            color.a = 1f;
        }
        else
        {
            color.a = 0.5f;
        }
        m_image.color = color;
    }
  
    void LoadNextSkill() //��ų ���� �� ���� ��ų�� �����ϸ� ������
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
            ActiveSkillAlpha();
        }
    }
    public void OnPointerUp(PointerEventData eventData) //Ŭ�� ��
    {
        UGUIManager.Instance.PlayClickSFX();
        if (!m_isChoice) //���� ��Ȳ����
        {
            if (m_isActive)//������ ��ų�� ��� ��� �޼��� ȣ�� �� ������ �ϵ���
            {
                UGUIManager.Instance.SystemMessageSendMessage("�̹� ������ ��ų�Դϴ�.");
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
            m_skillUI.TypeChoiceFinished(); 
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
    public void ActiveSkill() // ��ų Ȱ��ȭ
    {
        if (!m_isActive)
        {
            m_player.GetComponent<PlayerSkillController>().ActiveSkill(m_currentSkillID);
            LoadNextSkill();
        }
    }
    #endregion

}
