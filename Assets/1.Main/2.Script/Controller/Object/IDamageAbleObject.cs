using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAbleObject
{
    public void KillCount(); //óġ ���� ��Ȳ�� ������ �ɷ�ġ �����
    public void SetDamage(float damage,MonsterController mon); //������ ����
}