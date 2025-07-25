using UnityEngine;

public class BattleUIButtonHandler : MonoBehaviour
{
    public void OnAttackButtonClick()
    {
        if (BattleSystem.Instance.CurrentState is PlayerMenuState state)
            state.OnAttackSelected();
    }

    public void OnInventoryButtonClick()
    {
        if (BattleSystem.Instance.CurrentState is PlayerMenuState state)
            state.OnInventorySelected();
    }

    public void OnCaptureButtonClick()
    {
        if (BattleSystem.Instance.CurrentState is PlayerMenuState state)
            state.OnCaptureMotionSelected();
    }

    public void OnRunButtonClick()
    {
        if (BattleSystem.Instance.CurrentState is PlayerMenuState state)
            state.OnRunSelected();
    }

    public void OnMonsterSelected(Monster monster)
    {
        if (BattleSystem.Instance.CurrentState is SelectPlayerMonsterState state)
            state.OnMonsterSelected(monster);
    }

    public void OnCancelMonsterSelection()
    {
        if (BattleSystem.Instance.CurrentState is SelectPlayerMonsterState state)
            state.OnCancelSelected();
    }

    public void OnSkillSelected(SkillData skill)
    {
        if (BattleSystem.Instance.CurrentState is SelectSkillState state)
            state.OnSelectedSkill(skill);
    }

    public void OnCancelSkillSelection()
    {
        if (BattleSystem.Instance.CurrentState is SelectSkillState state)
            state.OnCancelSkill();
    }

    // public void OnTargetSelected(Monster target)
    // {
    //     if (BattleSystem.Instance.CurrentState is SelectTargetState state)
    //         state.OnSelectTargetMonster(target);
    // }

    public void OnCancelTargetSelection()
    {
        if (BattleSystem.Instance.CurrentState is SelectTargetState state)
            state.OnCancelSelectTarget();
    }
}
