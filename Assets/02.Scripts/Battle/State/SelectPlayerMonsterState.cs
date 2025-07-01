using UnityEngine.UIElements;

public class SelectPlayerMonsterState : BaseBattleState
{
    public SelectPlayerMonsterState(BattleSystem system) : base(system) {}
    
    public override void Execute()
    {
        // todo 선택한 부분 빛나면서 고르는거 대기 상태
        // todo 버튼 클릭을 받으면
    }

    public void OnMonsterSelected(Monster monster)
    {
        BattleManager.Instance.SelectPlayerMonster(monster);
        battleSystem.ChangeState(new SelectSkillState(battleSystem));
    }
}
