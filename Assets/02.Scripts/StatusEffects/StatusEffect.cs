public abstract class StatusEffect
{
    public int duration;

    public StatusEffect(int duration)
    {
        this.duration = duration;
    }
    public abstract string Name { get; }
    
    public abstract void OnTurnStart(Monster target);
}