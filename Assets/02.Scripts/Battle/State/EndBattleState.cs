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
            var (exp, gold) = BattleManager.Instance.BattleReward();
            Debug.Log($"전투 종료 보상: 경험치 {exp}, 골드 {gold}");
            UIManager.Instance.battleUIManager.BattlePanelWhenWin(exp, gold);
            AudioManager.Instance.PlaySFX("BattleWin");
        }
        else
        {
            UIManager.Instance.battleUIManager.BattlePanelWhenDefeat();
            AudioManager.Instance.PlaySFX("BattleLose");
        }

        if (PlayerManager.Instance.player.playerBattleTutorialCheck)
        {
            battleSystem.StartCoroutine(EndBattleCoroutine());
        }
    }

    public override void Execute()
    {
        if (PlayerManager.Instance.player.playerBattleTutorialCheck)
        {
            battleSystem.StartCoroutine(EndBattleCoroutine());
            Debug.Log("배틀 종료 상태로 진입했습니다. 2초 후 메인 맵으로 이동합니다.");
        }
    }
    private IEnumerator EndBattleCoroutine()
    {
        yield return new WaitForSeconds(2f);

        BattleDialogueManager.Instance.ClearBattleEndPanel();
        SceneManager.LoadScene("MainMapScene");
    }
}
