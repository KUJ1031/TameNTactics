using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSelecter : MonoBehaviour, IPointerClickHandler
{
    public int skillIndex; // 스킬 인덱스

    private SkillData skillData;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (BattleSystem.Instance.CurrentState is SelectSkillState state)
        {
            SetMonsterSkills(BattleManager.Instance.selectedPlayerMonster);
            BattleManager.Instance.selectedSkill = skillData;
            state.OnSelectedSkill(skillData);
        }
    }

    public void SetMonsterSkills(Monster monster)
    {
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
}
