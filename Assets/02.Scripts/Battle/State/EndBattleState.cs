using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleState : BaseBattleState
{
    public EndBattleState(BattleSystem battleSystem) : base(battleSystem) { }

    public override void Enter()
    {
        // todo 종료 UI 띄우기
        var alivePlayer = BattleManager.Instance.BattleEntryTeam.Where(m => m.CurHp > 0).ToList();
        if (alivePlayer.Count > 0)
        {
            BattleManager.Instance.BattleReward();
            UIManager.Instance.battleUIManager.BattleEndMessage(true);
        }
        else
        {
            UIManager.Instance.battleUIManager.BattleEndMessage(false);
        }

        battleSystem.StartCoroutine(EndBattleCoroutine());
    }

    private IEnumerator EndBattleCoroutine()
    {
        yield return new WaitForSeconds(2f);

        BattleDialogueManager.Instance.ClearBattleDialogue();
        SceneManager.LoadScene("MainScene");
    }
}
