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
            EnemyAttack();
        }
    }

    private void EnemyAttack()
    {
        List<Monster> enemyTeam = BattleManager.Instance.enemyTeam;
        List<Monster> entryTeam = BattleManager.Instance.BattleEntry;
        var enemyAction = EnemyAIController.DecideAction(enemyTeam, entryTeam);
        
        BattleManager.Instance.ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets);
        
        // 우리팀 다 죽었을 때 패배
        if (BattleManager.Instance.IsTeamDead(entryTeam))
        {
            BattleManager.Instance.EndBattle(false);
        }

        else
        {
            battleSystem.ChangeState(new PlayerMenuState(battleSystem));
        }
    }
}
