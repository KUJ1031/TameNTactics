using UnityEngine;

public class PlayerMenuState : BaseBattleState
{
    public PlayerMenuState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("플레이어 메뉴 상태로 진입했습니다. 행동을 선택하세요.");
        MonsterSelecter.isClicked = false;
        UIManager.Instance.battleUIManager.DisableHoverSelect();
        UIManager.Instance.battleUIManager.IntoBattleMenuSelect();
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
}
