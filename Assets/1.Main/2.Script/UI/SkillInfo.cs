using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SkillInfo : MonoBehaviour
{
    [SerializeField]
    Image m_skillImage;
    [SerializeField]
    TextMeshProUGUI m_skillName;
    [SerializeField]
    TextMeshProUGUI m_skillInfo;
    TableSkillStat m_skill = new TableSkillStat();
    int m_id = 0;


    void SetSkillIfo(int id) //��ų���� ����
    {
        m_skillImage = Utill.GetChildObject(gameObject, "SkillImage").GetComponent<Image>();
        m_skillName = Utill.GetChildObject(gameObject, "SkillName").GetComponent<TextMeshProUGUI>();
        m_skillInfo = Utill.GetChildObject(gameObject, "Info").GetComponent<TextMeshProUGUI>();
        m_skillImage.sprite = ImageLoader.Instance.GetImage(m_skill.GetSkillData(id).Image);
        m_skillName.text = m_skill.GetSkillData(id).SkillName + "\n�ʿ� ����Ʈ : " + m_skill.GetSkillData(id).SkillPoint;
        m_skillInfo.text = m_skill.GetSkillData(id).SkillInfo;
    }
    public void ActiveUI(int id)
    {
        if (m_id.Equals(id) && gameObject.activeSelf) return; //���̵� ���� ���̵�� ���� �̹� �����ִ°�� ���� x
        gameObject.SetActive(true);
        m_id = id;
        SetSkillIfo(m_id);
    }
    public void DeActiveUI()
    {
        gameObject.SetActive(false);
    }


}
