using UnityEngine.UIElements;
using UnityEngine;
using System.Collections;

public class SelectPlayerMonsterState : BaseBattleState
{
    public SelectPlayerMonsterState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("플레이어 몬스터 선택 상태로 진입했습니다. 몬스터를 선택하세요.");
        battleSystem.StartCoroutine(SelectPlayerMonster());
    }
    public override void Execute()
    {
        // todo 방향키 혹은 마우스 위에 올려놓을 시 빛나면서 고르는거 대기 상태
        // UIManager.Instance.battleUIManager.SelectMonster();
    }

    public void OnMonsterSelected(Monster monster)
    {
        BattleManager.Instance.SelectPlayerMonster(monster);
        //UIManager.Instance.battleUIManager.SelectMonster();
        battleSystem.ChangeState(new SelectSkillState(battleSystem));
    }

    public void OnCancelSelected()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }

    private IEnumerator SelectPlayerMonster()
    {
        bool selected = false;

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

                        if (!PlayerManager.Instance.player.battleEntry.Contains(clickedMonster))
                        {
                            Debug.Log("적 몬스터는 대상으로 선택할 수 없습니다. 아군 몬스터를 선택하세요.");
                        }
                        else
                        {
                            selected = true;
                            UIManager.Instance.battleUIManager.BattleSelectView.ShowSkillPanel();
                            UIManager.Instance.battleUIManager.BattleSelectView.MoveSelectMonster(monsterCharacter.transform);
                        }
                    }
                }
                else yield return null;
            }

            yield return null;
        }
    }
}
