using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationDamage
{
    public static float NormalDamage(float damage, float defence, float armorpierce,float damageRigist) // 기본공격데미지
    {

        float rigistDefence =  defence - (defence * armorpierce) / 100; //기본적인 방어력 적용. 방어구 관통 효과 적용.
        float attack = damage * (100 /(100 + rigistDefence)); //방어력에 따라 공격력 조정
        attack -= (attack * damageRigist); //최종적인 공격력에서 피해저항 효과 추가
        return attack;
    }
    public static bool CriticalDecision(float criRate) // 크리티컬 확률에 따라 크리티컬 여부 반환
    {
        float result = Random.Range(0f, 100f);
        if (result <= criRate)
        {
            return true;
        }
        return false;
    }
    public static float CriticalDamage(float damage, float criAtk) //크리티컬 데미지 적용
    {
        return damage + (damage * criAtk) / 100f;
    }
}
