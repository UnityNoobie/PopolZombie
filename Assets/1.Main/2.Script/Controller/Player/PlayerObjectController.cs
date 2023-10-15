using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour //플레이어의 설치물 관리 
{
    #region Constants and Fields
    PlayerSkillController m_skill;

    TableSkillStat m_skillData = new TableSkillStat();

    List<Barricade> m_barricades = new List<Barricade>();
    List<TowerController> m_turrets = new List<TowerController>();
    int m_maxTurretBuild = 0;
    int m_maxBarricadeBuild = 0;

    #endregion
    #region Methods
    public void MaxBuild() //오브젝트별 설치 가능한 최대 갯수 받아오기.
    {
        GetSkillData();
        m_maxBarricadeBuild = ObjectManager.Instance.GetObjectStat(ObjectType.Barricade).MaxBuild + m_skillData.BarricadeMaxBuild;
        m_maxTurretBuild = ObjectManager.Instance.GetObjectStat(ObjectType.Turret).MaxBuild + m_skillData .TurretMaxBuild;
    }
    public int[] GetObjectBuildData() //Status에서 표시할 최대 건설 가능한 오브젝트 데이터 리턴
    {
        MaxBuild();
        int[] list = new int[4];
        list[0] = m_maxBarricadeBuild;
        list[1] = m_barricades.Count;
        list[2] = m_maxTurretBuild;
        list[3] = m_turrets.Count;
        return list;
    }
    public bool IsCanBuildObject(int id) //최대 설치 수 보다 많은지 적은지 판단하여 불값 리턴. 설치할 수 없을 시 정보 화면에 출력
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
    public void BuildBarricade(Barricade bar) //설치한 오브젝트 리스트에 추가
    {
        m_barricades.Add(bar);
    }
    public void BuildTurret(TowerController tow)//설치한 오브젝트 리스트에 추가
    {
       m_turrets.Add(tow);
    }
    public void DestroyedTurret(TowerController tow) //설치한 오브젝트 리스트에서 제거
    {
        m_turrets.Remove(tow);
    }
    public void DestroyedBarricade(Barricade bar)//설치한 오브젝트 리스트에서 제거
    {
        m_barricades.Remove(bar);
    }
    public void UpdateObjectStat() //설치한 리스트에 있는 오브젝트의 스탯을 최신화해줌.
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
    #endregion
}
