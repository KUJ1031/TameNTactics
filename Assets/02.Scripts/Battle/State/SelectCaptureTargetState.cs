using System.Collections;
using UnityEngine;

public class SelectCaptureTargetState : BaseBattleState
{
    public SelectCaptureTargetState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("포섭하기 상태로 변경");
        UIManager.Instance.battleUIManager.BattleSelectView.HideSelectPanel();
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
        UIManager.Instance.battleUIManager.DisableHoverSelect();
        UIManager.Instance.battleUIManager.BattleSelectView.HideBeHaviorPanel();
        UIManager.Instance.battleUIManager.EmbraceView.ShowGuide("스페이스바를 눌러 포섭을 시도하세요!");

        StartEmbraceMiniGame(selectedMonster);
    }

    private void StartEmbraceMiniGame(Monster targetMonster)
    {
        // UI 초기화
        UIManager.Instance.battleUIManager.EmbraceView.ShowGuide("스페이스바를 눌러 화살표를 멈추세요!");
        MiniGameManager.Instance.StartMiniGame(targetMonster, CheckEmbraceResult);
    }

    private void CheckEmbraceResult(bool isSuccess, Monster targetMonster)
    {
        if (isSuccess)
        {
            Debug.Log("포섭 성공!");
            UIManager.Instance.battleUIManager.DeselectMonster(targetMonster);
            BattleManager.Instance.CaptureSelectedEnemy(targetMonster);
            UIManager.Instance.battleUIManager.RemoveGauge(targetMonster);
            UIManager.Instance.battleUIManager.EmbraceView.ShowSuccessMessage();

            if (BattleManager.Instance.BattleEnemyTeam.Count <= 0)
            {
                BattleSystem.Instance.ChangeState(new EndBattleState(battleSystem));
            }
            else
            {
                BattleManager.Instance.EnemyAttackAfterPlayerTurn();
            }
        }
        else
        {
            Debug.Log("포섭 실패...!");
            UIManager.Instance.battleUIManager.EmbraceView.ShowFailMessage();
            UIManager.Instance.battleUIManager.DeselectMonster(targetMonster);
            BattleManager.Instance.EnemyAttackAfterPlayerTurn();
        }
        battleSystem.StartCoroutine(Delay(2.0f));
        targetMonster = null;
    }

    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    public override void Execute()
    {
        // todo 선택된(방향키나 마우스 올려놓기) 몬스터가 체력이 0이 아니라면
        // 적 몬스터(잡을수있는)를 강조효과 UI 띄우기
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        //if (hit.collider != null)
        //{
        //    if (hit.collider.TryGetComponent<MonsterCharacter>(out var monsterCharacter))
        //    {
        //        if (BattleManager.Instance.BattleEntryTeam.Contains(monsterCharacter.monster))
        //        {
        //            UIManager.Instance.battleUIManager.BattleSelectView.MoveSelectMonster(monsterCharacter.transform);
        //        }
        //    }
        //}
    }

    public void OnCancelSelectCaptureTarget()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }
}
