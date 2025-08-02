using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : StatusEffect
{
    public Sleep(int duration) : base(StatusEffectType.Sleep, duration){}
    
    // 턴이 시작될때 정해진 턴 만큼 행동 불가(어떤것도 할수없음)
    public override void OnTurnStart(Monster target)
    {
        target.ApplyStun(true);
        duration--;

        if (duration == 0)
        {
            target.ApplyStun(false);
        }
    }
}
