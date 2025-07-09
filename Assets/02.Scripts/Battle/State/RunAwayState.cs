using System.Collections.Generic;

public class RunAwayState : BaseBattleState
{
    public RunAwayState(BattleSystem system) : base(system) {}

    public override void Enter()
    {
        if (BattleManager.Instance.TryRunAway())
        {
            // todo 도망 성공 UI 띄우고 배틀 종료
        }

        else
        {
            BattleManager.Instance.EnemyAttackAfterPlayerTurn();
        }
    }
}
