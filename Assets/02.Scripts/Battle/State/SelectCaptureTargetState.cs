using System.Collections.Generic;

public class SelectCaptureTargetState : BaseBattleState
{
    public SelectCaptureTargetState(BattleSystem system) : base(system) {}
    
    public override void Execute()
    {
        // todo 선택된(방향키나 마우스 올려놓기) 몬스터가 체력이 0이 아니라면
        // 적 몬스터(잡을수있는)를 강조효과 UI 띄우기
    }

    public void OnCancleSelectCaptureTarget()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }
}
