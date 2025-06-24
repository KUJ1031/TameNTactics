using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<MonsterData> playerTeam;
    public List<MonsterData> enemyTeam;

    private MonsterData selectedPlayerMonster;
    private SkillData selectedSkill;
    
    private bool battleEended = false;

    public void StartBattle()
    {
        foreach (var monster in playerTeam)
        {
            foreach (var skill in monster.skills)
            {
                if (skill.skillType == SkillType.UltimateSkill)
                {
                    skill.curUltimateCost = 0;
                }
            }
        }
        
        foreach (var monster in enemyTeam)
        {
            foreach (var skill in monster.skills)
            {
                if (skill.skillType == SkillType.UltimateSkill)
                {
                    skill.curUltimateCost = 0;
                }
            }
        }
    }

    public void SelectPlayerMonster(MonsterData selectedMonster)
    {
        if (selectedMonster.curHp <= 0) return;

        selectedPlayerMonster = selectedMonster;
        
        ShowSkillSelectionUI(selectedMonster.skills);
    }

    public void SelectSkill(SkillData skill)
    {
        selectedSkill = skill;

        List<MonsterData> possibleTargets;

        if (skill.isTargetSelf && !skill.isAreaAttack)
        {
            possibleTargets = new List<MonsterData> { selectedPlayerMonster };
        }

        else if (skill.isTargetSingleAlly)
        {
            possibleTargets = playerTeam.Where(m => m.curHp > 0).ToList();
        }
        
        else if (skill.isAreaAttack)
        {
            if (skill.isTargetSelf)
            {
                possibleTargets = playerTeam.Where(m => m.curHp > 0).ToList();
            }

            else
            {
                possibleTargets = enemyTeam.Where(m => m.curHp > 0).ToList();
            }
        }

        else if (!skill.isTargetSelf && !skill.isAreaAttack && !skill.isTargetSingleAlly)
        {
            possibleTargets = enemyTeam.Where(m => m.curHp > 0).ToList();
        }
    }

    public void SelectTargetMonster(MonsterData target)
    {
        if (target.curHp <= 0) return;
        
        List<MonsterData> selectedTargets = new();

        if (selectedSkill.isAreaAttack)
        {
            selectedTargets = selectedSkill.isTargetSelf
                ? playerTeam.Where(m => m.curHp > 0).ToList()
                : enemyTeam.Where(m => m.curHp > 0).ToList();
        }

        else
        {
            selectedTargets.Add(target);
        }

        var enemyAction = EnemyAIController.DecideEnemyAction(enemyTeam, playerTeam);

        bool playerGoesFirst = selectedPlayerMonster.speed >= enemyAction.actor.speed;
    }
    
    
    
    private void ShowSkillSelectionUI(List<SkillData> skills)
    {
        // 스킬 선택 UI 필요 합니당!
        // 스킬 이름, 설명 등
    }
    
}
