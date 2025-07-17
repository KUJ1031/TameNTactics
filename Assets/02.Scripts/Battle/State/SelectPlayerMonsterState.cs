using UnityEngine.UIElements;
using UnityEngine;
using System.Collections;

public class SelectPlayerMonsterState : BaseBattleState
{
    public SelectPlayerMonsterState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("플레이어 몬스터 선택 상태로 진입했습니다. 몬스터를 선택하세요.");
        UIManager.Instance.battleUIManager.BattleSelectView.ShowBehaviorPanel("공격할 몬스터를 선택하세요.");
        UIManager.Instance.battleUIManager.EnableHoverSelect(HoverTargetType.PlayerTeam);
    }
    public override void Execute()
    {
        // todo 방향키 혹은 마우스 위에 올려놓을 시 빛나면서 고르는거 대기 상태
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
    }

    public void OnMonsterSelected(Monster monster)
    {
        if (monster.CurHp <= 0 || !monster.canAct) return;
        BattleManager.Instance.SelectPlayerMonster(monster);
        if (BattleManager.Instance.BattleEntryTeam.Contains(monster))
        {
            battleSystem.ChangeState(new SelectSkillState(battleSystem));
        }
        else
        {
            Debug.Log("아군 몬스터만 선택할 수 있습니다.");
        }
    }

    public void OnCancelSelected()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }

    public override void Exit()
    {
        UIManager.Instance.battleUIManager.BattleSelectView.HideBeHaviorPanel();
    }
}
