using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDown : StatusEffect
{
    public AttackDown(int duration) : base(StatusEffectType.AttackDown, duration){}

    // 공격력 낮춤
    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.CurAttack * 0.1f);
        target.PowerDown(amount);
    }
}
