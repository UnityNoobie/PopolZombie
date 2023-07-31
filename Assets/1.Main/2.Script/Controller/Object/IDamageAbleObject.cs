using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAbleObject
{
    public void KillCount();
    public void SetDamage(float damage,MonsterController mon);
}