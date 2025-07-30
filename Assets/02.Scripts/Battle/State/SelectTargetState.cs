using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectTargetState : BaseBattleState
{
    public SelectTargetState(BattleSystem system) : base(system) { }

    public override void Enter()
    {
        Debug.Log("타겟 선택 상태로 진입했습니다. 공격할 몬스터를 선택하세요.");
        BattleTutorialManager.Instance.InitEnemySelected_Attack();
        ShowPossibleTargets();
        UIManager.Instance.battleUIManager.EnableHoverSelect(BattleManager.Instance.possibleTargets);
        UIManager.Instance.battleUIManager.BattleSelectView.ShowBehaviorPanel("공격할 상대 몬스터를 선택하세요.");

        // todo 타겟 몬스터 강조 효과(빛나기) UI 보여주기
        // todo 스킬 목록 UI 보여주기

        // todo 타겟 몬스터 강조 효과(빛나기) UI 활성화
    }

    public void OnSelectTargetMonster(Monster monster)
    {
        if (!BattleManager.Instance.possibleTargets.Contains(monster))
        {
            Debug.Log("올바른 타겟이 아닙니다.");
            MonsterSelecter.isClicked = false;
            return;
        }
        //UIManager.Instance.battleUIManager.OffSelectMonsterUI();
        BattleManager.Instance.SelectTargetMonster(monster);
        BattleTutorialManager.Instance.EndAttackTutorial();
        UIManager.Instance.battleUIManager.BattleSelectView.HideBeHaviorPanel();
    }

    public void OnCancelSelectTarget()
    {
        battleSystem.ChangeState(new SelectSkillState(battleSystem));
    }

    private void ShowPossibleTargets()
    {
        List<MonsterCharacter> possibleTargets = BattleManager.Instance.CheckPossibleTargets();

        foreach (var target in possibleTargets)
        {
            UIManager.Instance.battleUIManager.ShowPossibleTargets(target);
        }
    }
}
