using UnityEngine;

public class PlayerMenuState : BaseBattleState
{
    public PlayerMenuState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("플레이어 메뉴 상태로 진입했습니다. 행동을 선택하세요.");
        UIManager.Instance.battleUIManager.IntoBattleMenuSelect();
    }

    public override void Execute()
    {
        // todo 몬스터 애니메이션 idle 상태(기본 자세)
        if (BattleManager.Instance.battleEnded)
        {
            battleSystem.ChangeState(new EndBattleState(battleSystem));
        }
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

    public void OnCaptureSelected()
    {
        battleSystem.ChangeState(new SelectCaptureTargetState(battleSystem));
    }

    public void OnRunSelected()
    {
        battleSystem.ChangeState(new RunAwayState(battleSystem));
    }
}
