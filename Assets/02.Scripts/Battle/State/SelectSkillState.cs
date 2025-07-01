using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSkillState : BaseBattleState
{
    public SelectSkillState(BattleSystem system) : base(system) {}

    public override void Enter()
    {
        // todo 스킬UI 보여주기
    }

    public override void Execute()
    {
        
    }

    public void OnSelectedSkill(SkillData skill)
    {
        BattleManager.Instance.SelectSkill(skill);
        battleSystem.ChangeState(new SelectTargetState(battleSystem));
    }

    public override void Exit()
    {
        // todo 아군 몬스터 선택UI 숨기기
    }
}
