using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAbleObject
{
    public void KillCount(); //처치 시의 상황에 적응형 능력치 적용용
    public void SetDamage(float damage,MonsterController mon); //데미지 적용
}