using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationDamage
{
    public static float NormalDamage(float damage, float defence, float armorpierce) // �⺻���ݵ�����
    {

        float rigistDefence =  defence - (defence * armorpierce) / 100; //������
        float attack = damage * (100 /(100 + rigistDefence));
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
