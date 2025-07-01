using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTargetState : BaseBattleState
{
    public SelectTargetState(BattleSystem system) : base(system) {}
    
    public override void Execute()
    {
        // todo 마우스 혹은 방향키 조정 했을 때 타겟으로 설정 가능한 몬스터 빛나기
        // todo 선택이 됐다면 빛나는거 숨기기
    }
    
    public void OnSelectTargetMonster(Monster monster)
    {
        BattleManager.Instance.SelectTargetMonster(monster);
        // todo 공격 관련 애니메이션이 끝난 후
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
    }

    public override void Exit()
    {
        
    }
}
