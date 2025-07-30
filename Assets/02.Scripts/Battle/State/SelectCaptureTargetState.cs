using System.Collections;
using UnityEngine;

public class SelectCaptureTargetState : BaseBattleState
{
    public SelectCaptureTargetState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("포섭하기 상태로 변경");
        BattleTutorialManager.Instance.InitEnemySelected_Embrace();
        UIManager.Instance.battleUIManager.EmbraceView.HideBehaviorPanel();
        UIManager.Instance.battleUIManager.BattleSelectView.ShowBehaviorPanel("포섭하고 싶은 몬스터를 선택하세요.");
        UIManager.Instance.battleUIManager.EnableHoverSelect(BattleManager.Instance.BattleEnemyTeam);
        battleSystem.StartCoroutine(WaitForMonsterSelection());
    }

    private IEnumerator WaitForMonsterSelection()
    {
        Monster selectedMonster;
        bool selected = false;
        selectedMonster = null;

        while (!selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<MonsterCharacter>(out var monsterCharacter))
                    {
                        Monster clickedMonster = monsterCharacter.monster;

                        if (BattleManager.Instance.BattleEnemyTeam.Contains(clickedMonster))
                        {
                            if (clickedMonster.CurHp <= 0)
                            {
                                Debug.Log("쓰러진 몬스터는 포획할 수 없습니다.");
                            }
                            else
                            {
                                selectedMonster = monsterCharacter.monster;
                                selected = true;
                                Debug.Log($"선택된 몬스터 : {selectedMonster.monsterName}");
                            }
                        }
                        else
                        {
                            Debug.Log("자신이 소유한 몬스터는 포섭할 수 없습니다.");
                        }
                    }
                }
            }
            yield return null;
        }
        UIManager.Instance.battleUIManager.BattleSelectView.HideCancelButton();
        UIManager.Instance.battleUIManager.DisableHoverSelect();
        UIManager.Instance.battleUIManager.BattleSelectView.HideBeHaviorPanel();
        UIManager.Instance.battleUIManager.EmbraceView.ShowGuide("스페이스바를 눌러 포섭을 시도하세요!");

        StartEmbraceMiniGame(selectedMonster);
    }

    private void StartEmbraceMiniGame(Monster targetMonster)
    {
        // UI 초기화
        BattleTutorialManager.Instance.InitMinigame();
        UIManager.Instance.battleUIManager.EmbraceView.ShowGuide("스페이스바를 눌러 화살표를 멈추세요!");
        if (PlayerManager.Instance.player.playerTutorialCheck) MiniGameManager.Instance.StartMiniGame(targetMonster, CheckEmbraceResult);
        else MiniGameManager.Instance.StartMiniGame(targetMonster, CheckEmbraceResult, 0.9f);
    }

    private void CheckEmbraceResult(bool isSuccess, Monster targetMonster)
    {
        if (isSuccess)
        {
            Debug.Log("포섭 성공!");
            BattleTutorialManager.Instance.isBattleEmbraceTutorialEnded = true;
            UIManager.Instance.battleUIManager.DeselectMonster(targetMonster);
            BattleManager.Instance.CaptureSelectedEnemy(targetMonster);
            UIManager.Instance.battleUIManager.RemoveGauge(targetMonster);
            UIManager.Instance.battleUIManager.EmbraceView.ShowSuccessMessage();

            if (BattleTutorialManager.Instance.isBattleEmbraceTutorialEnded)
                BattleTutorialManager.Instance.EndEmbraceTutorial();

            if (BattleManager.Instance.BattleEnemyTeam.Count <= 0)
            {
                BattleSystem.Instance.ChangeState(new EndBattleState(battleSystem));
            }
            else
            {
                battleSystem.StartCoroutine(EnemyAttackAfterDelay(2.0f));
            }
        }
        else
        {
            if (!BattleTutorialManager.Instance.isBattleEmbraceTutorialEnded)
            {
                battleSystem.ChangeState(new PlayerMenuState(battleSystem));
                BattleTutorialManager.Instance.MinigameFailed();
                Debug.Log("포섭 미니게임 실패, 다시 메뉴로 돌아갑니다.");
                return;
            }
                
            Debug.Log("포섭 실패...!");
            UIManager.Instance.battleUIManager.EmbraceView.ShowFailMessage();
            UIManager.Instance.battleUIManager.DeselectMonster(targetMonster);
            battleSystem.StartCoroutine(EnemyAttackAfterDelay(2.0f));
        }
        battleSystem.StartCoroutine(Delay(2.0f));
        targetMonster = null;
    }

    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    private IEnumerator EnemyAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        BattleManager.Instance.EnemyAttackAfterPlayerTurn();
    }

    public void OnCancelSelectCaptureTarget()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }
}
