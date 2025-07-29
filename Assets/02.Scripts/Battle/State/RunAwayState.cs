using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunAwayState : BaseBattleState
{
    public RunAwayState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        if (!BattleTutorialManager.Instance.isBattleEscapeTutorialEnded)
        {
            BattleTutorialManager.Instance.RunAwayTry();
            BattleTutorialManager.Instance.EndEscapeTutorial();
            BattleSystem.Instance.ChangeState(new PlayerMenuState(battleSystem));
        }
        else
        {
            if (BattleManager.Instance.TryRunAway())
            {
                // todo 도망 성공 UI 띄우고 배틀 종료
                Debug.Log("도망가기 성공! 이전 씬으로 돌아갑니다.");
                UIManager.Instance.battleUIManager.BattleSelectView.HideSelectPanel();
                SceneManager.LoadScene("MainMapScene");
            }
            else
            {
                Debug.Log("도망가기 실패!");
                BattleDialogueManager.Instance.UseRunFailDialogue();
                BattleManager.Instance.EnemyAttackAfterPlayerTurn();
                BattleSystem.Instance.ChangeState(new PlayerMenuState(battleSystem));
            }
        }
    }
}
