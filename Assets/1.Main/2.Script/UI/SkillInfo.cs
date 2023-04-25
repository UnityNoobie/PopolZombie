using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SkillInfo : MonoBehaviour
{
    Image m_skillImage;
    TextMeshProUGUI m_skillName;
    TextMeshProUGUI m_skillInfo;
    TableSkillStat m_skill = new TableSkillStat();
    int m_id = 0;


    void SetSkillIfo(int id) //��ų���� ����
    {
        m_skillImage = Utill.GetChildObject(gameObject, "SkillImage").GetComponent<Image>();
        m_skillName = Utill.GetChildObject(gameObject, "SkillName").GetComponent<TextMeshProUGUI>();
        m_skillInfo = Utill.GetChildObject(gameObject, "SkillInfo").GetComponent<TextMeshProUGUI>();
        m_skillImage.sprite = ImageLoader.Instance.GetImage(m_skill.GetSkillData(id).Image);
        m_skillName.text = m_skill.GetSkillData(id).SkillName ;
        m_skillInfo.text = "�ʿ� ����Ʈ : " + m_skill.GetSkillData(id).SkillPoint+ "\n" + m_skill.GetSkillData(id).SkillInfo;
    }
    public void ActiveUI(int id)
    {
        if (m_id.Equals(id) && gameObject.activeSelf) return; //���̵� ���� ���̵�� ���� �̹� �����ִ°�� ���� x
        gameObject.SetActive(true);
     //   if (m_id.Equals(id)) return; //���� ������ ������� ������ �״�� ���
        m_id = id;
        SetSkillIfo(m_id);
        
    }
    public void DeActiveUI()
    {
        gameObject.SetActive(false);
    }


}
