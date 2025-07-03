using UnityEngine;

public class Burn : StatusEffect
{
    private int damagePerTurn;

    public Burn(int duration) : base(StatusEffectType.Burn, duration){}

    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.MaxHp * 0.1f);
        target.TakeDamage(amount);
    }
}
