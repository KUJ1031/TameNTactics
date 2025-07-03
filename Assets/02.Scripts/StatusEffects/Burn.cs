using UnityEngine;

public class Burn : StatusEffect
{
    private int damagePerTurn;

    public Burn(int duration) : base(duration){}

    public override string Name => "화상";

    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.MaxHp * 0.1f);
        target.TakeDamage(amount);
    }
}
