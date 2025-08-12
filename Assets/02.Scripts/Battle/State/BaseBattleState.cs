public abstract class BaseBattleState
{
    protected BattleSystem battleSystem;
    
    public BaseBattleState(BattleSystem system)
    {
        battleSystem = system;
    }

    public virtual void Enter(){}
    public virtual void Execute(){}
    public virtual void Exit(){}
}
