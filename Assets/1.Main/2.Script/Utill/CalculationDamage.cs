using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationDamage
{
    public static float NormalDamage(float damage, float defence, float armorpierce) // 기본공격데미지
    {

        float rigistDefence =  defence - (defence * armorpierce) / 100; //방어구관통
        float attack = damage * (100 /(100 + rigistDefence));
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
