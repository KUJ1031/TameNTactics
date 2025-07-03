using UnityEngine;

public class Paralysis : StatusEffect
{
    private int speedDownEffect;
    private bool isApplied = false;
    private int appliedAmount;

    public Paralysis(int duration) : base(StatusEffectType.Paralysis, duration) {}
    
    public override void OnTurnStart(Monster target)
    {
        if (!isApplied)
        {
            appliedAmount = Mathf.RoundToInt(target.Speed * 0.1f);
            target.SpeedDownEffect(appliedAmount);
            isApplied = true;
        }

        if (duration == 1)
        {
            target.RecoverUpSpeed(appliedAmount);
        }
    }
}
