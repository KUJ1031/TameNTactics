using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCaptureMotionState : BaseBattleState
{
    public SelectCaptureMotionState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        BattleTutorialManager.Instance.InitTalkingSelected();
        // todo 어떤 모션으로 할건지 UI 띄어주기
        UIManager.Instance.battleUIManager.BattleSelectView.HideSelectPanel();
        UIManager.Instance.battleUIManager.BattleSelectView.ShowCancelButton();
        UIManager.Instance.battleUIManager.ShowBehaviorMenu(PlayerManager.Instance.player, OnCaptureTargetSelected);
    }

    public override void Exit()
    {
        // todo 모션 목록 UI 끄기
        UIManager.Instance.battleUIManager.EmbraceView.HideBehaviorPanel();
    }

    // 잡는걸로 이동
    public void OnCaptureTargetSelected()
    {
        battleSystem.ChangeState(new SelectCaptureTargetState(battleSystem));
    }
}
