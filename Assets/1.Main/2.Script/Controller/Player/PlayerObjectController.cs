using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour
{
    PlayerSkillController m_skill;
    TowerController m_turret;
    Barricade m_barricade;

    TableSkillStat m_skillData = new TableSkillStat();

    List<Barricade> m_barricades = new List<Barricade>();
    List<TowerController> m_turrets = new List<TowerController>();

  
    int m_maxTurretBuild = 0;
    int m_maxBarricadeBuild = 0;
    
    public bool IsCanBuildObject(int id)
    {
        if(id == 3)
        {
            if (m_maxBarricadeBuild <= m_barricades.Count) //��ġ ���� �� ���� ��ġ�Ǿ� �ִ� ���� ���ٸ�.
            {
                UGUIManager.Instance.SystemMessageSendMessage("�ٸ����̵� �ִ� ��ġ ������ �ʰ��Ͽ����ϴ�. �ִ� ��ġ �� : " + m_maxBarricadeBuild + " ���� ��ġ �� : " + m_barricades);
                return false;
            }
            return true;
        }
        else if(id == 4)
        {
            if (m_maxTurretBuild <= m_turrets.Count)//��ġ ���� �� ���� ��ġ�Ǿ� �ִ� ���� ���ٸ�.
            {
                UGUIManager.Instance.SystemMessageSendMessage("��ž �ִ� ��ġ ������ �ʰ��Ͽ����ϴ�. �ִ� ��ġ �� : " + m_maxTurretBuild + " ���� ��ġ �� : " + m_turrets);
                return false;
            }
            return true;
        }
        else
        {
            UGUIManager.Instance.SystemMessageSendMessage("�ùٸ��� ���� ���� �Ҵ�Ǿ����ϴ�.");
            return false;
        }
    }
    public void BuildBarricade(Barricade bar)
    {
        m_barricades.Add(bar);
    }
    public void BuildTurret(TowerController tow)
    {
       m_turrets.Add(tow);
    }
    public void DestroyedTurret(TowerController tow)
    {
        m_turrets.Remove(tow);
    }
    public void DestroyedBarricade(Barricade bar)
    {
        m_barricades.Remove(bar);
    }
    public void ObjectUpgrade() 
    {
        GetSkillData();
        foreach(var item in m_barricades)
        {
            item.InitStatus(m_skillData, ObjectManager.Instance.GetObjectStat(ObjectType.Barricade));
        }
        foreach (var item in m_turrets)
        {
            item.InitStatus(m_skillData, ObjectManager.Instance.GetObjectStat(ObjectType.Turret));
        }
    }
    void GetSkillData() // �÷��̾��� ��ų ������ �������ֱ�
    {
        m_skillData = m_skill.GetPlayerSkillData();
    }
    private void Start()
    {
        m_skill = GetComponent<PlayerSkillController>();
    }
}
