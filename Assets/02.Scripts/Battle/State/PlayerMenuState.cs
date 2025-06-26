public class PlayerMenuState : BaseBattleState
{
    public PlayerMenuState(BattleSystem system) : base(system) {}

    public override void Enter()
    {
        battleSystem.battleManager.StartBattle();
        // 메뉴 UI 띄우기
    }

    public override void Execute()
    {
        // 입력 대기 및 버튼 선택 처리(여기서 아래 메서드 호출 하는 식)
    }

    public void OnAttackSelected()
    {
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

    public override void Exit()
    {
        // 메뉴 UI 숨기기
    }
}
