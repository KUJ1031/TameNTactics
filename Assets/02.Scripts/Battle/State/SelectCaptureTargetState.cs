using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCaptureTargetState : BaseBattleState
{
    public SelectCaptureTargetState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("포섭하기 상태로 변경");
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

                        if (!PlayerManager.Instance.player.ownedMonsters.Contains(clickedMonster))
                        {
                            selectedMonster = monsterCharacter.monster;
                            selected = true;
                            Debug.Log($"선택된 몬스터 : {selectedMonster.monsterName}");
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

        UIManager.Instance.battleUIManager.EmbraceView.ShowGuide("스페이스바를 눌러 포섭을 시도하세요!");

        StartEmbraceMiniGame(selectedMonster, 50f);
    }

    private void StartEmbraceMiniGame(Monster targetMonster, float successPercent)
    {
        GameObject miniGameObj = Object.Instantiate(UIManager.Instance.battleUIManager.MiniGamePrefab);

        // UI 초기화
        UIManager.Instance.battleUIManager.EmbraceView.ShowGuide("스페이스바를 눌러 화살표를 멈추세요!");
        UIManager.Instance.battleUIManager.EmbraceView.HideMessage();

        // MiniGameManager 가져오기
        MiniGameManager miniGameManager = miniGameObj.GetComponent<MiniGameManager>();

        // MiniGame 시작
        miniGameManager.StartMiniGame(successPercent);

        // 결과 판정 코루틴 실행
        battleSystem.StartCoroutine(CheckEmbraceResult(miniGameManager, targetMonster));
    }

    private IEnumerator CheckEmbraceResult(MiniGameManager miniGameManager, Monster targetMonster)
    {
        RotatePoint rotatePoint = miniGameManager.GetComponentInChildren<RotatePoint>();

        bool finished = false;

        while (!finished)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rotatePoint.SetRotateSpeed(0);

                if (rotatePoint.isInSuccessZone)
                {
                    Debug.Log("포섭 성공!");
                    BattleManager.Instance.CaptureSelectedEnemy(targetMonster);
                    UIManager.Instance.battleUIManager.EmbraceView.ShowSuccessMessage();
                    BattleManager.Instance.EnemyAttackAfterPlayerTurn();

                    if (BattleManager.Instance.IsTeamDead(BattleManager.Instance.BattleEnemyTeam))
                    {
                        BattleSystem.Instance.ChangeState(new EndBattleState(battleSystem));
                    }
                    else
                    {
                        BattleSystem.Instance.ChangeState(new PlayerMenuState(battleSystem));
                    }
                }
                else
                {
                    Debug.Log("포섭 실패...!");
                    UIManager.Instance.battleUIManager.EmbraceView.ShowFailMessage();
                    BattleManager.Instance.EnemyAttackAfterPlayerTurn();
                    BattleSystem.Instance.ChangeState(new PlayerMenuState(battleSystem));
                }

                finished = true;
            }

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        Object.Destroy(miniGameManager.gameObject);
    }

    public override void Execute()
    {
        // todo 선택된(방향키나 마우스 올려놓기) 몬스터가 체력이 0이 아니라면
        // 적 몬스터(잡을수있는)를 강조효과 UI 띄우기
    }

    public override void Exit()
    {
        if (BattleManager.Instance.IsTeamDead(BattleManager.Instance.BattleEnemyTeam))
        {
            BattleManager.Instance.EndBattle(true);
        }
    }

    public void OnCancelSelectCaptureTarget()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }
}
