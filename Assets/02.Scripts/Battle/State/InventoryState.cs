public class InventoryState : BaseBattleState
{
    public InventoryState(BattleSystem system) : base(system) {}

    public override void Enter()
    {
        // todo 인벤토리 UI창 띄우기
    }

    public override void Execute()
    {
        // todo 현재 선택이 되려고 대기중인 대상 강조효과(ex: 네모칸으로 강조)
    }

    public void OnSelectedItem()
    {
        // todo 누르면 바로 사용할지 or 누르면 사용하기 UI 떠서 선택을 할지
        // todo 아이템 사용시 적 공격
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }

    public void OnCancleInventory()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }

    public override void Exit()
    {
        // todo 인벤토리 UI 숨기기
    }
    
    
}
