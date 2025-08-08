using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSelecter : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int skillIndex; // 스킬 인덱스

    private SkillData skillData;
    private Monster caster;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!(BattleSystem.Instance.CurrentState is SelectSkillState)) return;

        SetMonsterSkills(BattleManager.Instance.selectedPlayerMonster);
        if (skillData == null) return;

        if (!IsSkillUsable(caster, skillData))
        {
            Debug.Log("레벨이 낮아서 궁극기 사용 불가!");
            return;
        }

        if (BattleSystem.Instance.CurrentState is SelectSkillState state && !BattleManager.Instance.isAttacking)
        {
            BattleManager.Instance.selectedSkill = skillData;
            state.OnSelectedSkill(skillData);
        }
    }

    private void SetMonsterSkills(Monster monster)
    {
        caster = monster;

        if (caster == null || caster.skills.Count == 0) return;

        if (monster != null && monster.skills.Count > 0)
        {
            //스킬 인덱스에 따른 스킬 설정
            if (skillIndex >= 0 && skillIndex < monster.skills.Count)
            {
                skillData = monster.skills[skillIndex];
            }
            else
            {
                Debug.LogWarning($"Invalid skill index: {skillIndex} for monster: {monster.monsterName}");
                skillData = null; // 유효하지 않은 인덱스일 경우 null로 설정
            }
        }
    }

    private bool IsSkillUsable(Monster monster, SkillData skill)
    {
        if (monster == null || skill == null)
            return false;

        if (skill.skillType == SkillType.UltimateSkill)
        {
            if (monster.Level < 15 && monster.CurUltimateCost < monster.MaxUltimateCost)
            {
                return false;
            }
        }
        
        return true;
    }

    // 스킬 툴팁 표시
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetMonsterSkills(BattleManager.Instance.selectedPlayerMonster);
        if (skillData == null) return;

        UIManager.Instance.battleUIManager.SkillView.ShowActiveSkillTooltip(skillData.skillName, skillData.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.battleUIManager.SkillView.HideActiveSkillTooltip();
    }
}
