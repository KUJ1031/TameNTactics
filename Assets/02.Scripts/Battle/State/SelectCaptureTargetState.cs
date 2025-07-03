using System.Collections.Generic;

public class SelectCaptureTargetState : BaseBattleState
{
    public SelectCaptureTargetState(BattleSystem system) : base(system) {}
    
    public override void Execute()
    {
        // todo 선택된(방향키나 마우스 올려놓기) 몬스터가 체력이 0이 아니라면
        // 적 몬스터(잡을수있는)를 강조효과 UI 띄우기
    }

    public void OnSelectCaptureTarget(Monster target)
    {
        List<Monster> enemyTeam = BattleManager.Instance.enemyTeam;
        List<Monster> entryTeam = BattleManager.Instance.BattleEntry;
        var enemyAction = EnemyAIController.DecideAction(enemyTeam, entryTeam);
        
        // todo 미니게임 성공시 잡기
        BattleManager.Instance.CaptureSelectedEnemy(target);
        BattleManager.Instance.ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill, enemyAction.targets);
        
        // todo 잡았다는 UI 혹은 애니메이션 실행
        
        // 상대 몬스터들이 다 죽었을 때
        if (BattleManager.Instance.IsTeamDead(enemyTeam))
        {
            BattleManager.Instance.EndBattle(true);
        }
        
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

    public void OnCancleSelectCaptureTarget()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }
}
