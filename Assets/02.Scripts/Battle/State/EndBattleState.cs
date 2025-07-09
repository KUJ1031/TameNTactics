using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBattleState : BaseBattleState
{
    public EndBattleState(BattleSystem battleSystem) : base(battleSystem) { }

    public override void Enter()
    {
        // todo 종료 UI 띄우기
        BattleManager.Instance.battleEnded = false;
        BattleManager.Instance.BattleReward();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
