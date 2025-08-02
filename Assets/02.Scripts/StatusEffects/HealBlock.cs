using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBlock : StatusEffect
{
    public HealBlock(int duration) : base(StatusEffectType.HealBlock, duration){}
    
    public override void OnTurnStart(Monster target)
    {
        target.CanBeHealed(false);
        duration--;

        if (duration == 0)
        {
            target.CanBeHealed(true);
        }
    }
}
