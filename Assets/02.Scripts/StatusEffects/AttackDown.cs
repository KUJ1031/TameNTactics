using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttackDown : StatusEffect
{
    public AttackDown(int duration) : base(StatusEffectType.AttackDown, duration){}

    private bool isApplied = false;
    
    // 공격력 낮춤
    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.CurAttack * 0.1f);
        
        if (!isApplied)
        {
            target.PowerDown(amount);
            isApplied = true;
        }
        
        duration--;
        
        if (duration == 0)
        {
            target.PowerUp(amount);
            isApplied = false;
        }
    }
}
