using UnityEngine;

public class Poison : StatusEffect
{
    private int damagePerTurn;
    
    public Poison(int duration) : base(duration) {}
    
    public override string Name => "중독";

    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.MaxHp * 0.1f);
        target.TakeDamage(amount);
    }
}
