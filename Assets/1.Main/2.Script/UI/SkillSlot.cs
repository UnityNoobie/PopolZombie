using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour //,IPointerUpHandler, IPointerEnterHandler,IPointerExitHandler
{
    
    TextMeshProUGUI m_text;
    PlayerController m_player;
    public TableSkillStat m_skill = new TableSkillStat();
    Image m_image;
    Button m_Button; 
    TweenAlpha m_TweenAlpha;
    int skillId;
    bool m_isActive = false;
    bool isfirst = true;

    public void SetSkillSlot(int id, PlayerController player, bool isActive,string image)
    {
        m_image = Utill.GetChildObject(gameObject, "ItemImage").GetComponent<Image>();
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        skillId = id;
        m_player = player;
        m_text.text = m_skill.GetSkillData(id).SkillName;
        m_image.sprite = ImageLoader.Instance.GetImage(image);
        
        m_isActive = isActive;
        skillId = id;
        if(isActive) //스킬을 배우고 있다면 알파값 변경
        {
            Color color = m_image.color;
            color.a = .5f;
            m_image.color = color;
        }
    }
    void ActiveSkill(int Id)
    {
        m_player.GetComponent<PlayerSkillController>().ActiveSkill(Id);
    }

}
