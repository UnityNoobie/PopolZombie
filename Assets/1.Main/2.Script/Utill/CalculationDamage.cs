using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationDamage
{
    // Start is called before the first frame update
    /*
    public static bool AttackDecision(float attackerHit, float defenceDodge) //������ �ߴ��� Ȯ���ϴ� Ŭ���� 
    {
        if (attackerHit >= 100.0f) return true;
        float total = attackerHit + defenceDodge;
        float hit = Random.Range(0.0f, total);
        if (hit <= attackerHit) return true;
        return false;
    }*/
    public static float NormalDamage(float damage, float defence) // �⺻���ݵ�����
    {
        float attack = damage * (100 /(100 + defence)) ;
        return attack;
    }
    public static bool CriticalDecision(float criRate) // ũ��Ƽ�� Ȯ��
    {
        float result = Random.Range(0.0f, 100.0f);
        if (result <= criRate)
        {
            return true;
        }
        return false;
    }
    public static float CriticalDamage(float damage, float criAtk) //ũ��Ƽ�� ������
    {
        return damage + (damage * criAtk) / 100f;
    }
}
