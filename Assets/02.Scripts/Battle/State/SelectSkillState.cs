using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSkillState : BaseBattleState
{
    public SelectSkillState(BattleSystem system) : base(system) {}

    public override void Enter()
    {
        Debug.Log("스킬 선택 상태로 진입했습니다. 스킬을 선택하세요.");
       
        MonsterData monsterCharacter = BattleManager.Instance.selectedPlayerMonster.monsterData;
        UIManager.Instance.battleUIManager.ShowMonsterSkills(monsterCharacter);

        // todo 스킬UI 보여주기
    }

    public override void Execute()
    {
        // todo 방향키 움직이거나 마우스를 스킬위에 올려놓았을때 활성화(강조) 되는 느낌 UI
        // todo 몬스터 공격자세 애니메이션 활성화
        UIManager.Instance.battleUIManager.HandleMouseClick();
    }

    public void OnSelectedSkill(SkillData skill)
    {
        BattleManager.Instance.SelectSkill(skill);
        Debug.Log($"선택한 스킬: {skill.name}");
        battleSystem.ChangeState(new SelectTargetState(battleSystem));
    }

    public void OnCancelSkill()
    {
        battleSystem.ChangeState(new SelectPlayerMonsterState(battleSystem));
    }

    public override void Exit()
    {
        // todo 아군 몬스터 강조 효과 UI 숨기기
    }
}
