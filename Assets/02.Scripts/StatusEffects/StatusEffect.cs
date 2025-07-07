public enum StatusEffectType
{
    Burn,
    Paralysis,
    Poison,
    Stun
}

public abstract class StatusEffect
{
    public StatusEffectType Type { get; private set; }
    public int duration;

    // (Burn, 3) 이런식으로 사용됨
    public StatusEffect(StatusEffectType type, int duration)
    {
        Type = type;
        this.duration = duration;
    }
    
    public abstract void OnTurnStart(Monster target);
}