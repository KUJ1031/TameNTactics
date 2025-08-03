using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : BuffEffect
{
    public Taunt(int duration) : base(BuffEffectType.Taunt, duration){}
    
    public override void OnTurnStart(Monster target)
    {
        if (duration <= 0)
        {
            target.Taunt(false);
            return;
        }
        
        target.Taunt(true);
        duration--;
    }
}
