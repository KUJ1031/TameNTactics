using UnityEngine;

public class PlayerMenuState : BaseBattleState
{
    public PlayerMenuState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        // todo 배틀 기본 화면 띄우기(행동 고르는 메뉴)
        Debug.Log("플레이어 메뉴 상태로 진입했습니다. 행동을 선택하세요.");
        UIManager.Instance.battleUIManager.IntoBattleMenuSelect();
        BattleManager.Instance.StartBattle();
    }

    public override void Execute()
    {
        // todo 메뉴 클릭 혹은 방향키로 고르기
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
