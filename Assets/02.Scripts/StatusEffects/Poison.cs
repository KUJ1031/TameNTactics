using UnityEngine;

public class Poison : StatusEffect
{
    private int damagePerTurn;
    
    public Poison(int duration) : base(StatusEffectType.Poison, duration) {}
    
    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.MaxHp * 0.1f);
        target.TakeDamage(amount);
        duration--;
    }
}
