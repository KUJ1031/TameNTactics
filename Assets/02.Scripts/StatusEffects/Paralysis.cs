using UnityEngine;

public class Paralysis : StatusEffect
{
    private int speedDownEffect;
    private bool isApplied = false;
    private int appliedAmount;

    public Paralysis(int duration) : base(duration) {}
    
    
    public override string Name => "마비";

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
            target.RecoverSpeed(appliedAmount);
        }
    }
}
