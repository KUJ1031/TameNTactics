using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<MonsterData> playerTeam;
    public List<MonsterData> enemyTeam;

    private MonsterData selectedPlayerMonster;
    private SkillData selectedSkill;
    
    private bool battleEnded = false;

    // 배틀 시작시 초기화
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

    // 플레이어 몬스터 고르기
    public void SelectPlayerMonster(MonsterData selectedMonster)
    {
        if (selectedMonster.curHp <= 0) return;

        selectedPlayerMonster = selectedMonster;
        
        ShowSkillSelectionUI(selectedMonster.skills);
    }

    // 스킬 고르기
    public void SelectSkill(SkillData skill)
    {
        selectedSkill = skill;

        List<MonsterData> possibleTargets = new();

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
        
        ShowTargetSelectionUI(possibleTargets);
    }

    // 타겟 몬스터 선택
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

        var enemyAction = EnemyAIController.DecideAction(enemyTeam, playerTeam);

        bool playerGoesFirst = selectedPlayerMonster.speed >= enemyAction.actor.speed;

        if (playerGoesFirst)
        {
            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            if (IsTeamDead(playerTeam))
            {
                EndBattle(true);
                return;
            }

            ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill,
                enemyAction.targets);
            if (IsTeamDead(enemyTeam))
            {
                EndBattle(true);
                return;
            }
        }

        else
        {
            ExecuteSkill(enemyAction.actor, enemyAction.selectedSkill,
                enemyAction.targets);
            if (IsTeamDead(enemyTeam))
            {
                EndBattle(true);
                return;
            }
            
            ExecuteSkill(selectedPlayerMonster, selectedSkill, selectedTargets);
            if (IsTeamDead(playerTeam))
            {
                EndBattle(true);
                return;
            }
        }
    }

    // 스킬 사용
    private void ExecuteSkill(MonsterData caster, SkillData skill, List<MonsterData> targets)
    {
        if (caster.curHp <= 0 || targets == null || targets.Count == 0) return;

        foreach (var target in targets.Where(t => t.curHp > 0))
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skill);
            target.curHp -= result.damage;
            if (target.curHp < 0) target.curHp = 0;
        }
    }
    
    // 팀 죽었나요?
    private bool IsTeamDead(List<MonsterData> team)
    {
        return team.All(m => m.curHp <= 0);
    }

    // 배틀이 끝났으면 판넬을 띄어주면 될거같아요!
    private void EndBattle(bool playerWin)
    {
        battleEnded = true;
        Debug.Log(playerWin ? "승리 판넬" : "패배 판넬");
    }
    
    // 스킬 목록 보여주세요!
    private void ShowSkillSelectionUI(List<SkillData> skills)
    {
        // 스킬 선택 UI 필요 합니당!
        // 스킬 이름, 설명 등
    }
    
    // 타겟을 나타내줘요!
    private void ShowTargetSelectionUI(List<MonsterData> targets)
    {
        // 타겟 선택 영역 표시 필요해욤!
    }
}
