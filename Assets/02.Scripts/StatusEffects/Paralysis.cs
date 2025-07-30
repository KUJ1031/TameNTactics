using UnityEngine;

public class Paralysis : StatusEffect
{
    private int speedDownEffect;
    private bool isApplied = false;
    private int appliedAmount;

    public Paralysis(int duration) : base(StatusEffectType.Paralysis, duration) {}
    
    // 마비, 턴이 시작될때 정해진 턴 수 만큼 스피드 30%감소
    public override void OnTurnStart(Monster target)
    {
        appliedAmount = Mathf.RoundToInt(target.Speed * 0.3f);
        
        if (!isApplied)
        {
            target.SpeedDownEffect(appliedAmount);
            isApplied = true;
        }
        
        duration--;

        if (duration == 0)
        {
            target.SpeedUpEffect(appliedAmount);
            isApplied = false;
        }
    }
}
