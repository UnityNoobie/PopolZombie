using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiterController : MonsterController
{
    Transform Dummy_Projectile;
    ProjectileController m_fire;
    SpiterController m_spit;
    protected override void AnimEvent_SetAttack()
    {
        var dir = m_player.transform.position - Dummy_Projectile.transform.position;
        dir.y = 0;
        var effectName = TableEffect.Instance.m_tableData[5].Prefab[0];
        var effect = EffectPool.Instance.Create(effectName);
        m_fire = effect.GetComponent<ProjectileController>();
        m_fire.SetProjectile(gameObject.GetComponent<SpiterController>(),1f);
        m_fire.transform.position = Dummy_Projectile.transform.position;
        m_fire.transform.forward = dir.normalized;
    }
    protected override void Awake()
    {
        base.Awake(); //부모의 기능을 받아오기위해 base.Awake를 하면 가넝 아니면 override로인해 덮어씌워져서 기능 X\
        Dummy_Projectile = Utill.GetChildObject(gameObject, "Dummy_Projectile");
    }
}
