using UnityEngine.UIElements;

public class SelectPlayerMonsterState : BaseBattleState
{
    public SelectPlayerMonsterState(BattleSystem system) : base(system) {}
    
    public override void Execute()
    {
        // todo 방향키 혹은 마우스 위에 올려놓을 시 빛나면서 고르는거 대기 상태
    }

    public void OnMonsterSelected(Monster monster)
    {
        BattleManager.Instance.SelectPlayerMonster(monster);
        battleSystem.ChangeState(new SelectSkillState(battleSystem));
    }

    public void OnCancelSelected()
    {
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }
}
