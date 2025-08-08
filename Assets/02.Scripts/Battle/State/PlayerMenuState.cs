using System.Linq;
using UnityEngine;

public class PlayerMenuState : BaseBattleState
{
    public PlayerMenuState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("플레이어 메뉴 상태로 진입했습니다. 행동을 선택하세요.");
        BattleTutorialManager.Instance.InitialBattle();
        MonsterSelecter.isClicked = false;
        UIManager.Instance.battleUIManager.BattleSelectView.HideCancelButton();
        UIManager.Instance.battleUIManager.DisableHoverSelect();
        UIManager.Instance.battleUIManager.IntoBattleMenuSelect();

        if (PlayerManager.Instance.player.playerEliteStartCheck[0] || PlayerManager.Instance.player.playerEliteStartCheck[1] || PlayerManager.Instance.player.playerEliteStartCheck[2])
        {
            UIManager.Instance.battleUIManager.BattleSelectView.HideEmbraceButton();
            UIManager.Instance.battleUIManager.BattleSelectView.HideRunButton();
        }
        OnTurnStart();
    }

    public override void Execute()
    {
        // todo 몬스터 애니메이션 idle 상태(기본 자세)
    }

    public void OnAttackSelected()
    {
        UIManager.Instance.battleUIManager.OnAttackButtonClick();
        battleSystem.ChangeState(new SelectPlayerMonsterState(battleSystem));
    }

    public void OnInventorySelected()
    {
        battleSystem.ChangeState(new InventoryState(battleSystem));
    }

    public void OnCaptureMotionSelected()
    {
        battleSystem.ChangeState(new SelectCaptureMotionState(battleSystem));
    }

    public void OnRunSelected()
    {
        battleSystem.ChangeState(new RunAwayState(battleSystem));
    }

    private void OnTurnStart()
    {
        var allMonsters =
            BattleManager.Instance.BattleEntryTeam.Concat(BattleManager.Instance.BattleEnemyTeam);

        foreach (var monster in allMonsters)
        {
            monster.TriggerOnStartTurnHeal();
        }
    }
}
