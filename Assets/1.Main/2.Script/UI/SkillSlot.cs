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
    bool m_isChoice = false; //3단계 스킬 선택할때 사용하기용.
    PlayerAbilityType m_type;


    /*
    Queue<int> m_ids = new Queue<int>(); //큐를 사용한 도전 해보기
    Queue<int> m_activedId = new Queue<int>(); // 적용된 스킬 아이디를 넣어볼거임
    Queue<bool> m_isActived = new Queue<bool>();*/ //큐를 이용한 도전을 시도하였으나 더욱 복잡하고 가독성 떨어질것 같아 폐기






    string m_skillname;
    public TableSkillStat m_skill = new TableSkillStat();
    public void SetSkillSlot(int id, PlayerSkillController player, bool isActive,ActiveSkill activeskill,SkillInfo info)
    {
        m_skillPoint = m_skill.GetSkillData(id).SkillPoint;
        m_image = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>(); //Start에서 활성화시 
        m_text = GetComponentInChildren<TextMeshProUGUI>();                            //오류발생하여 슬롯지정해줄때 호출
        m_active = activeskill;
        m_currentSkillID = id;
        RefreshSkillInfo();
        m_skillInfo = info;
        m_player = player;
        m_image.sprite = ImageLoader.Instance.GetImage(m_skill.GetSkillData(m_currentSkillID).Image);
        m_isActive = isActive;
        m_isChoice = false;
        if (isActive) //스킬을 배우고 있다면 알파값 변경
        {
            ActiveSkillAlpha();
        }
    }
    void RefreshSkillInfo()
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
    /*
    public void ResetQueue() //큐 초기화
    {
        m_ids.Clear();
        m_isActived.Clear();
    }
    public void EnQueues(int id, bool isactive) //큐 넣기
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
    void CheckActiveSkills() // 스킬들이 
    {
        if (m_ids == null) return;
        for(int i = 0; i < m_ids.Count; i++)
        {
            if (m_isActived.Peek()) //다음 스킬이 액티브 상태라면 디큐해주기
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
        m_image = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>(); //Start에서 활성화시 
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_ids = id;
        m_isActived = isActive;
        m_player = player;
        m_active = activeskill;
        m_skillInfo = info;
    }*/ //큐를 이용한 시도했으나 더 복잡해질것 같아 폐기

    void ActiveSkillAlpha() //익힌 스킬의 알파값을 변경해줌
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
    public void OnPointerUp(PointerEventData eventData) //클릭 시
    {
        if (!m_isChoice) //평상시 상황에서
        {
            if (m_isActive)//습득한 스킬일 경우 경고 메세지 호출 후 무반응 하도록
            {
                UIManager.Instance.SystemMessageCantOpen("이미 습득한 스킬입니다.");
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
            m_skillUI.ChoiceFinished();
            
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
    public void ActiveSkill()
    {
        if (!m_isActive)
        {
            m_player.GetComponent<PlayerSkillController>().ActiveSkill(m_currentSkillID);
            LoadNextSkill();
        }
    }

}
