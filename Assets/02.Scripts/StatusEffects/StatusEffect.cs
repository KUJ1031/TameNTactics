public enum StatusEffectType
{
    Burn,
    Paralysis,
    Poison
}

public abstract class StatusEffect
{
    public StatusEffectType Type { get; private set; }
    public int duration;

    public StatusEffect(StatusEffectType type, int duration)
    {
        Type = type;
        this.duration = duration;
    }
    
    public abstract void OnTurnStart(Monster target);
}