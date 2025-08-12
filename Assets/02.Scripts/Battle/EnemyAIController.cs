using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemyAIController
{
    public class EnemyAction
    {
        public Monster actor;
        public SkillData selectedSkill;
        public List<Monster> targets;
    }

    // 조건에 따른 공격을 실행할 몬스터 자동 선택!
    public static EnemyAction DecideAction(List<Monster> actors, List<Monster> targetMonsters)
    {
        if (actors == null || targetMonsters == null) return null;
        
        if (actors.All(enemy => !enemy.debuffCanAct || !enemy.canAct))
        {
            return null;
        }

        // 1. 궁극기 사용 가능한 몬스터가 있다면 우선
        foreach (var enemy in actors)
        {
            if (enemy.CurHp <= 0 || !enemy.debuffCanAct || !enemy.canAct) continue;

            var ultimateSkill = enemy.skills.FirstOrDefault(s => s.skillType == SkillType.UltimateSkill);
            bool maxUltCost = enemy.CurUltimateCost >= enemy.MaxUltimateCost;

            if (ultimateSkill != null && maxUltCost && enemy.Level >= 15)
            {
                if (ultimateSkill.targetScope == TargetScope.PlayerTeam || 
                    ultimateSkill.targetScope == TargetScope.Self)
                {
                    targetMonsters = actors;
                }
                
                var targets = ChooseTargets(ultimateSkill, targetMonsters, actors, enemy);

                if (targets != null && targets.Count > 0)
                {
                    return new EnemyAction
                    {
                        actor = enemy,
                        selectedSkill = ultimateSkill,
                        targets = targets
                    };
                }
            }
        }

        // 2. 상성 유리한 대상이 있다면 NormalSkill 사용
        foreach (var enemy in actors)
        {
            if (enemy.CurHp <= 0 || !enemy.debuffCanAct || !enemy.canAct) continue;

            bool hasAdvantage = targetMonsters.Any(player =>
                player.CurHp > 0 &&
                TypeChart.GetEffectiveness(enemy, player) > 1f);

            if (hasAdvantage)
            {
                var activeSkill = enemy.skills.FirstOrDefault(s => s.skillType == SkillType.NormalSkill);
                if (activeSkill != null)
                {
                    if (activeSkill.targetScope == TargetScope.PlayerTeam || 
                        activeSkill.targetScope == TargetScope.Self)
                    {
                        targetMonsters = actors;
                    }
                    
                    var targets = ChooseTargets(activeSkill, targetMonsters, actors, enemy);

                    if (targets != null && targets.Count > 0)
                    {
                        return new EnemyAction
                        {
                            actor = enemy,
                            selectedSkill = activeSkill,
                            targets = targets
                        };
                    }
                }
            }
        }

        // 3. 조건이 없으면 랜덤 몬스터가 NormalSkill 사용
        var aliveEnemies = actors.Where(m => m.CurHp > 0 || m.debuffCanAct).ToList();
        if (aliveEnemies.Count == 0) return null;

        var randomEnemy = aliveEnemies[Random.Range(0, aliveEnemies.Count)];
        var randomSkill = randomEnemy.skills.FirstOrDefault(s => s.skillType == SkillType.NormalSkill);

        if (randomSkill != null)
        {
            if (randomSkill.targetScope == TargetScope.PlayerTeam || 
                randomSkill.targetScope == TargetScope.Self)
            {
                targetMonsters = actors;
            }
            
            var targets = ChooseTargets(randomSkill, targetMonsters, actors, randomEnemy);

            if (targets != null && targets.Count > 0)
            {
                return new EnemyAction
                {
                    actor = randomEnemy,
                    selectedSkill = randomSkill,
                    targets = targets
                };
            }
        }

        return null;
    }

    // 조건에 맞는 타겟 고르기
    private static Monster ChooseTarget(List<Monster> targetMonsters, Monster attacker)
    {
        // 1. 체력 50% 이하 중 가장 낮은 몬스터
        var lowHp = targetMonsters
            .Where(m => m.CurHp > 0 && m.CurHp / m.CurMaxHp <= 0.5f)
            .OrderBy(m => m.CurHp)
            .ToList();

        if (lowHp.Count > 0) return lowHp[0];

        // 2. 상성 유리한 몬스터
        var effective = targetMonsters
            .Where(m => m.CurHp > 0 && TypeChart.GetEffectiveness(attacker, m) > 1f)
            .OrderBy(m => m.CurHp)
            .ToList();

        if (effective.Count > 0) return effective[0];

        // 3. 랜덤 대상
        var alive = targetMonsters.Where(m => m.CurHp > 0).ToList();
        if (alive.Count > 0) return alive[Random.Range(0, alive.Count)];

        return null;
    }

    // 공격의 형태 고르기
    private static List<Monster> ChooseTargets(
        SkillData skill, List<Monster> targetTeam, List<Monster> actorTeam, Monster actor)
    {
        List<Monster> candidates = new();

        // 후보군 선정: targetScope 기준
        switch (skill.targetScope)
        {
            case TargetScope.Self:
                candidates.Add(actor);
                break;

            case TargetScope.EnemyTeam:
                candidates = targetTeam.Where(m => m.CurHp > 0).ToList();
                break;

            case TargetScope.PlayerTeam:
                candidates = actorTeam.Where(m => m.CurHp > 0).ToList();
                break;

            case TargetScope.All:
                candidates = actorTeam.Concat(targetTeam).Where(m => m.CurHp > 0).ToList();
                break;

            default:
                break;
        }

        if (candidates.Count == 0) return new List<Monster>();

        // targetCount가 0 또는 후보 수보다 크면 전부 선택
        if (skill.targetCount == 0 || skill.targetCount >= candidates.Count)
        {
            return new List<Monster>(candidates);
        }
        else
        {
            List<Monster> selectedTargets = new();

            // targetCount만큼 우선순위 높은 타겟 선택
            var tempCandidates = new List<Monster>(candidates);

            for (int i = 0; i < skill.targetCount; i++)
            {
                var target = ChooseTarget(tempCandidates, actor);
                if (target != null)
                {
                    selectedTargets.Add(target);
                    tempCandidates.Remove(target);
                }
                else
                {
                    break;
                }
            }

            return selectedTargets;
        }
    }
}
