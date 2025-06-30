public class PlayerMenuState : BaseBattleState
{
    public PlayerMenuState(BattleSystem system) : base(system) {}
    
    public override void Enter()
    {
        battleSystem.battleManager.StartBattle();
    }

    public override void Execute()
    {
        // todo 입력 대기 버튼, 아래 메서드 호출해서 이동
        // 클릭 혹은 선택
        
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
        // todo 메뉴 UI 숨기기
    }
}
