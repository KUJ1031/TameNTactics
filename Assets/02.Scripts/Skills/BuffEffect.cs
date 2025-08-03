
public enum BuffEffectType
{
    Taunt
}

public abstract class BuffEffect
{
    public BuffEffectType Type { get; private set; }
    public int duration;
    
    public BuffEffect(BuffEffectType type, int duration)
    {
        Type = type;
        this.duration = duration;
    }
    
    
    public abstract void OnTurnStart(Monster target);
}
