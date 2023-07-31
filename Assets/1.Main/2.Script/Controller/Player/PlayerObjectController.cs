using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour
{
    PlayerSkillController m_skill;
    ObjectStat stat;
    TowerController m_turret;
    Barricade m_barricade;

    TableSkillStat m_skillData = new TableSkillStat();

    List<Barricade> m_barricades = new List<Barricade>();
    List<TowerController> m_turrets = new List<TowerController>();

  
    int m_maxTurretBuild = 0;
    int m_maxBarricadeBuild = 0;
    const int m_maxBuild = 10;
    public void MaxBuild()
    {
        GetSkillData();
        m_maxBarricadeBuild = m_maxBuild + m_skillData.BarricadeMaxBuild;
        m_maxTurretBuild =  m_skillData .TurretMaxBuild;
    }
    public int[] GetObjectBuildData()
    {
        MaxBuild();
        int[] list = new int[4];
        list[0] = m_maxBarricadeBuild;
        list[1] = m_barricades.Count;
        list[2] = m_maxTurretBuild;
        list[3] = m_turrets.Count;
        return list;
    }
    public bool IsCanBuildObject(int id)
    {
        MaxBuild();
        if(id == 2)
        {
            if (m_maxBarricadeBuild <= m_barricades.Count) //설치 가능 수 보다 설치되어 있는 수가 많다면.
            {
                UGUIManager.Instance.SystemMessageSendMessage("바리케이드 최대 설치 개수를 초과하였습니다. 최대 설치 수 : " + m_maxBarricadeBuild + " 현재 설치 수 : " + m_barricades.Count);
                return false;
            }
            return true;
        }
        else if(id == 3)
        {
            if (m_maxTurretBuild <= m_turrets.Count)//설치 가능 수 보다 설치되어 있는 수가 많다면.
            {
                UGUIManager.Instance.SystemMessageSendMessage("포탑 최대 설치 개수를 초과하였습니다. 최대 설치 수 : " + m_maxTurretBuild + " 현재 설치 수 : " + m_turrets.Count);
                return false;
            }
            return true;
        }
        else
        {
            UGUIManager.Instance.SystemMessageSendMessage("올바르지 않은 값이 할당되었습니다.");
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
        foreach (var item in m_barricades)
        {
            item.InitStatus(m_skillData, ObjectManager.Instance.GetObjectStat(ObjectType.Barricade));
        }
        foreach (var item in m_turrets)
        {
            item.InitStatus(m_skillData, ObjectManager.Instance.GetObjectStat(ObjectType.Turret));
        }
    }
    void GetSkillData() // 플레이어의 스킬 데이터 가져와주기
    {
        m_skillData = m_skill.GetPlayerSkillData();
    }
    private void Awake()
    {
        m_skill = GetComponent<PlayerSkillController>();
    }
}
