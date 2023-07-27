using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : BuildableObject
{
    #region Constants and Fields


    #endregion

    #region Coroutine

    #endregion
    public override void SetDamage(float damage)
    {
        base.SetDamage(damage);
        SoundManager.Instance.PlaySFX("SFX_GunHit", m_audio);
    }
    protected override void Destroyed()
    {
        base.Destroyed();
        StopAllCoroutines();
        Invoke("DestroyGameObject", 1f); // ������Ʈ Ǯ�����ֱ�
    }
    protected override void DestroyGameObject() //Ȱ��ȭ ���� �� Ǯ�� �ٽ� �־��ֱ�
    {
        base.DestroyGameObject();
        ObjectManager.Instance.SetBarricade(this); //Ǯ�� �ֱ�
    }
    public void BuildBarricade(Vector3 buildPos, float barirota, float hudrota,TableSkillStat skill,ObjectStat stat)
    {
        SetTransform();
        InitStatus(skill,stat);
        transform.position = buildPos;
        transform.localEulerAngles = new Vector3(0f, barirota, 0f);
        m_hudPos.transform.localEulerAngles = new Vector3(30f,hudrota, 0f);
        gameObject.SetActive(true); //���ӿ�����Ʈ Ȱ��ȭ
        GameManager.Instance.SetGameObject(gameObject);//���� ������ ������Ʈ ��Ͽ� �߰����ֱ�
    }
}
