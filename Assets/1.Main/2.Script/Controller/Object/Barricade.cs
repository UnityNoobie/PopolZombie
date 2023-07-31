using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : BuildableObject
{
    #region Constants and Fields


    #endregion

    #region Coroutine

    #endregion
    public override void SetDamage(float damage, MonsterController mon)
    {
        base.SetDamage(damage,mon);
        SoundManager.Instance.PlaySFX("SFX_GunHit", m_audio);
    }
    protected override void Destroyed()
    {
        base.Destroyed();
        StopAllCoroutines();
        Invoke("DestroyGameObject", 1f); // 오브젝트 풀링해주기
    }
    protected override void DestroyGameObject() //활성화 종료 후 풀에 다시 넣어주기
    {
        base.DestroyGameObject();
        ObjectManager.Instance.SetBarricade(this); //풀에 넣기
    }
    public void BuildBarricade(Vector3 buildPos, float barirota, float hudrota,TableSkillStat skill,ObjectStat stat)
    {
        SetTransform();
        gameObject.SetActive(true); //게임오브젝트 활성화
        InitStatus(skill,stat);
        transform.position = buildPos;
        transform.localEulerAngles = new Vector3(0f, barirota, 0f);
        m_hudPos.transform.localEulerAngles = new Vector3(30f,hudrota, 0f);
        GameManager.Instance.SetGameObject(gameObject);//공격 가능한 오브젝트 목록에 추가해주기
    }
}
