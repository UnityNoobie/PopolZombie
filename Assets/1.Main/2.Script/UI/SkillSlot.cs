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
    bool m_isChoice = false; //3단계 스킬 선택할때 사용하기용.
    PlayerAbilityType m_type;


    string m_skillname;
    public TableSkillStat m_skill = new TableSkillStat();
    #endregion

    #region Methods
    public void SetSkillSlot(int id, PlayerSkillController player, bool isActive,ActiveSkill activeskill,SkillInfo info) //스킬 슬롯 지정
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
    void RefreshSkillInfo() //스킬 정보 다시 가져옴ㅁ
    {
        m_skillname = m_skill.GetSkillData(m_currentSkillID).SkillName;
        m_text.text = m_skillname;
    }
    public void ChoiceSkillTypes(PlayerSkillController player,PlayerAbilityType type,string image,string info,SkillUI ui) //3단계 스킬의 경우 
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

    void ActiveSkillAlpha() // 스킬 액티브 여부에 따라 알파값을 변경해줌
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
  
    void LoadNextSkill() //스킬 습득 시 다음 스킬이 존재하면 가져옴
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
    public void OnPointerUp(PointerEventData eventData) //클릭 시
    {
        UGUIManager.Instance.PlayClickSFX();
        if (!m_isChoice) //평상시 상황에서
        {
            if (m_isActive)//습득한 스킬일 경우 경고 메세지 호출 후 무반응 하도록
            {
                UGUIManager.Instance.SystemMessageSendMessage("이미 습득한 스킬입니다.");
                return;
            }
            if (m_player.IsCanActive(m_skillPoint, m_skill.GetSkillData(m_currentSkillID).SkillGrade, m_skill.GetSkillData(m_currentSkillID).SkillType)) //습득 가능한 포인트가 있다면 스킬 습득 UI를 불러와 처리
            {
                m_active.ActiveUI(this, m_skillPoint, m_skillname);
            }
        } 
        else // 슬롯이 특성 선택 모드일때
        {
            m_player.SetAblityType(m_type);
            m_skillUI.TypeChoiceFinished(); 
        }
    }
    public void OnPointerEnter(PointerEventData eventData) //올라왔을 때 정보창 불러움
    {
        if (!m_isChoice) //평상시 상황에서
            m_skillInfo.ActiveUI(m_currentSkillID);
    }
    public void OnPointerExit(PointerEventData eventData) //나갔을 때 정보창 끔
    {
        if (!m_isChoice) //평상시 상황에서
            if (eventData.pointerEnter.CompareTag("Slot")) //태그가 슬롯인것에만 적용
                m_skillInfo.DeActiveUI();
    }
    public void ActiveSkill() // 스킬 활성화
    {
        if (!m_isActive)
        {
            m_player.GetComponent<PlayerSkillController>().ActiveSkill(m_currentSkillID);
            LoadNextSkill();
        }
    }
    #endregion

}
