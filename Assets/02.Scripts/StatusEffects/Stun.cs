using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    public Stun(int duration) : base(StatusEffectType.Stun, duration){}

    private bool isApplied = false;
    
    public override void OnTurnStart(Monster target)
    {
        if (!isApplied)
        {
            target.ApplyStun(true);
            isApplied = true;
        }
        
        duration--;

        if (duration == 0)
        {
            target.ApplyStun(false);
            isApplied = false;
        }
    }
}
