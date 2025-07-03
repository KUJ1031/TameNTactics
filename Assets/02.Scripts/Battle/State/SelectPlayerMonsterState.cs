using UnityEngine.UIElements;
using UnityEngine;

public class SelectPlayerMonsterState : BaseBattleState
{
    public SelectPlayerMonsterState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("플레이어 몬스터 선택 상태로 진입했습니다. 몬스터를 선택하세요.");
    }
    public override void Execute()
    {
        // todo 방향키 혹은 마우스 위에 올려놓을 시 빛나면서 고르는거 대기 상태
        UIManager.Instance.battleUIManager.SelectMonster();
    }

    public void OnMonsterSelected(Monster monster)
    {
        BattleManager.Instance.SelectPlayerMonster(monster);
        UIManager.Instance.battleUIManager.SelectMonster();
        battleSystem.ChangeState(new SelectSkillState(battleSystem));
    }

    public void OnCancelSelected()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }
}
