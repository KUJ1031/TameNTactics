using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTargetState : BaseBattleState
{
    public SelectTargetState(BattleSystem system) : base(system) {}
    
    public override void Enter()
    {
        Debug.Log("타겟 선택 상태로 진입했습니다. 공격할 몬스터를 선택하세요.");

        // todo 타겟 몬스터 강조 효과(빛나기) UI 보여주기
        // todo 스킬 목록 UI 보여주기

        // todo 타겟 몬스터 강조 효과(빛나기) UI 활성화
    }

    public override void Execute()
    {
        // todo 마우스 혹은 방향키 조정 했을 때 타겟으로 설정 가능한 몬스터 빛나기
    }
    
    public void OnSelectTargetMonster(Monster monster)
    {
        BattleManager.Instance.SelectTargetMonster(monster);
        battleSystem.ChangeState(new PlayerMenuState(battleSystem));
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
