using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationDamage
{
    // Start is called before the first frame update
    /*
    public static bool AttackDecision(float attackerHit, float defenceDodge) //명중을 했는지 확인하는 클래스 
    {
        if (attackerHit >= 100.0f) return true;
        float total = attackerHit + defenceDodge;
        float hit = Random.Range(0.0f, total);
        if (hit <= attackerHit) return true;
        return false;
    }*/
    public static float NormalDamage(float damage, float defence) // 기본공격데미지
    {
        float attack = damage * (100 /(100 + defence)) ;
        return attack;
    }
    public static bool CriticalDecision(float criRate) // 크리티컬 확률
    {
        float result = Random.Range(0.0f, 100.0f);
        if (result <= criRate)
        {
            return true;
        }
        return false;
    }
    public static float CriticalDamage(float damage, float criAtk) //크리티컬 데미지
    {
        return damage + (damage * criAtk) / 100f;
    }
}
