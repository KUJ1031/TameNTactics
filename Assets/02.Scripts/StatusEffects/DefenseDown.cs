using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDown : StatusEffect
{
    public DefenseDown(int duration) : base(StatusEffectType.DefenseDown, duration) {}

    bool isApplied = false;
    
    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.CurDefense * 0.1f);
        
        if (!isApplied)
        {
            target.BattleDefenseDown(amount);
            isApplied = true;
        }
        
        duration--;
        
        if (duration == 0)
        {
            target.BattleDefenseUp(amount);
            isApplied = false;
        }
    }
}
