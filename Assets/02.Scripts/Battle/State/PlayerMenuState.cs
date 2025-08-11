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

        bool isDeanFight = BattleManager.Instance.enemyTeam.Any(m => m.monsterName == "Dean");
        bool isEisenFight = BattleManager.Instance.enemyTeam.Any(m => m.monsterName == "Eisen");
        bool isDolanFight = BattleManager.Instance.enemyTeam.Any(m => m.monsterName == "Dolan");
        bool isBossFight = BattleManager.Instance.enemyTeam.Any(m => m.monsterName == "Boss");

        if (BattleManager.Instance.BattleEntryTeam.All(m => !m.canAct || !m.debuffCanAct))
        {
            if (PlayerManager.Instance.player.playerBattleTutorialCheck)
            {
                BattleManager.Instance.StartCoroutine(BattleManager.Instance.CompareSpeedAndFight());
                UIManager.Instance.battleUIManager.BattleSelectView.HideSelectPanel();
            }

        }
        bool isOnlyRunDisabled = PlayerManager.Instance.player.playerLastStage == "잊혀진 공간";
        bool isFullyDisabled = isDeanFight || isEisenFight || isDolanFight || isBossFight;

        if (isOnlyRunDisabled)
        {
            UIManager.Instance.battleUIManager.BattleSelectView.InteractableEmbraceButton_true();
            UIManager.Instance.battleUIManager.BattleSelectView.InteractableRunButton_false();
        }
        else if (isFullyDisabled)
        {
            UIManager.Instance.battleUIManager.BattleSelectView.InteractableEmbraceButton_false();
            UIManager.Instance.battleUIManager.BattleSelectView.InteractableRunButton_false();
        }
        else
        {
            Debug.Log("일반 전투에서는 포획 버튼과 도망가기 버튼을 사용할 수 있습니다.");
            UIManager.Instance.battleUIManager.BattleSelectView.InteractableEmbraceButton_true();
            UIManager.Instance.battleUIManager.BattleSelectView.InteractableRunButton_true();
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
