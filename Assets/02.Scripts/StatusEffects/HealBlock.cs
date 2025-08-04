using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBlock : StatusEffect
{
    public HealBlock(int duration) : base(StatusEffectType.HealBlock, duration){}
    
    public override void OnTurnStart(Monster target)
    {
        if (duration == 0)
        {
            target.CanBeHealed(true);
            return;
        }
        
        target.CanBeHealed(false);
        duration--;
    }
}
