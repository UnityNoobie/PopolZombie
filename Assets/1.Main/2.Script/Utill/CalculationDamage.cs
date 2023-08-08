using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationDamage
{
    public static float NormalDamage(float damage, float defence, float armorpierce,float damageRigist) // �⺻���ݵ�����
    {

        float rigistDefence =  defence - (defence * armorpierce) / 100; //�⺻���� ���� ����. �� ���� ȿ�� ����.
        float attack = damage * (100 /(100 + rigistDefence)); //���¿� ���� ���ݷ� ����
        attack -= (attack * damageRigist); //�������� ���ݷ¿��� �������� ȿ�� �߰�
        return attack;
    }
    public static bool CriticalDecision(float criRate) // ũ��Ƽ�� Ȯ���� ���� ũ��Ƽ�� ���� ��ȯ
    {
        float result = Random.Range(0f, 100f);
        if (result <= criRate)
        {
            return true;
        }
        return false;
    }
    public static float CriticalDamage(float damage, float criAtk) //ũ��Ƽ�� ������ ����
    {
        return damage + (damage * criAtk) / 100f;
    }
}
