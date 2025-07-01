using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTargetState : BaseBattleState
{
    public SelectTargetState(BattleSystem system) : base(system) {}
    
    public override void Execute()
    {
        // todo 마우스 혹은 방향키 조정 했을 때 타겟으로 설정 가능한 몬스터 빛나기
    }
    
    public void OnSelectTargetMonster(Monster monster)
    {
        BattleManager.Instance.SelectTargetMonster(monster);
        // todo 공격 관련 애니메이션
        
        if (!BattleManager.Instance.battleEnded)
        {
            BattleManager.Instance.EndTurn();
            battleSystem.ChangeState(new PlayerMenuState(battleSystem));
        }
    }

    public void OnCancelSelectTarget()
    {
        battleSystem.ChangeState(new SelectSkillState(battleSystem));
    }
    
    public override void Exit()
    {
        // todo 몬스터 강조 효과(빛나기)UI 숨기기
        // todo 스킬 목록 UI 숨기기
    }
}
