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
        // 1. 궁극기 사용 가능한 몬스터가 있다면 우선
        foreach (var enemy in actors)
        {
            if (enemy.CurHp <= 0) continue;

            var ultimateSkill = enemy.skills.FirstOrDefault(s =>
                s.skillType == SkillType.UltimateSkill &&
                s.curUltimateCost >= s.maxUltimateCost);

            if (ultimateSkill != null)
            {
                var targets = ChooseTargets(ultimateSkill, targetMonsters, actors, enemy);

                return new EnemyAction
                {
                    actor = enemy,
                    selectedSkill = ultimateSkill,
                    targets = targets
                };
            }
        }

        // 2. 상성 유리한 대상이 있다면 ActiveSkill 사용
        foreach (var enemy in actors)
        {
            if (enemy.CurHp <= 0) continue;

            bool hasAdvantage = targetMonsters.Any(player =>
                player.CurHp > 0 &&
                TypeChart.GetEffectiveness(enemy, player) > 1f);

            if (hasAdvantage)
            {
                var activeSkill = enemy.skills.FirstOrDefault(s => s.skillType == SkillType.ActiveSkill);
                if (activeSkill != null)
                {
                    var targets = ChooseTargets(activeSkill, targetMonsters, actors, enemy);

                    return new EnemyAction
                    {
                        actor = enemy,
                        selectedSkill = activeSkill,
                        targets = targets
                    };
                }
            }
        }

        // 3. 조건이 없으면 랜덤 몬스터가 ActiveSkill 사용
        var aliveEnemies = actors.Where(m => m.CurHp > 0).ToList();
        if (aliveEnemies.Count == 0) return null;

        var randomEnemy = aliveEnemies[Random.Range(0, aliveEnemies.Count)];
        var randomSkill = randomEnemy.skills.FirstOrDefault(s => s.skillType == SkillType.ActiveSkill);

        if (randomSkill != null)
        {
            var targets = ChooseTargets(randomSkill, targetMonsters, actors, randomEnemy);

            return new EnemyAction
            {
                actor = randomEnemy,
                selectedSkill = randomSkill,
                targets = targets
            };
        }

        return null;
    }

    // 조건에 맞는 타겟 고르기
    private static Monster ChooseTarget(List<Monster> targetMonsters, Monster attacker)
    {
        // 1. 체력 50% 이하 중 가장 낮은 몬스터
        var lowHp = targetMonsters
            .Where(m => m.CurHp > 0 && (float)m.CurHp / m.MaxHp <= 0.5f)
            .OrderBy(m => m.CurHp)
            .ToList();

        if (lowHp.Count > 0) return lowHp[0];

        // 2. 상성 유리하고 HP 낮은 몬스터
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
        List<Monster> result = new List<Monster>();

        // 공격팀 전체 대상
        if (skill.isAreaAttack && skill.isTargetSelf)
        {
            result = actorTeam.Where(m => m.CurHp > 0).ToList();
        }
        // 타겟팀 전체 대상
        else if (skill.isAreaAttack && !skill.isTargetSelf)
        {
            result = targetTeam.Where(m => m.CurHp > 0).ToList();
        }
        // 공격하는 자기 자신
        else if (!skill.isAreaAttack && skill.isTargetSelf)
        {
            result.Add(actor);
        }
        // 공격팀 중 하나 타겟
        else if (skill.isTargetSingleAlly)
        {
            var possibleActorTeam = actorTeam.Where(m => m.CurHp > 0).ToList();
            var target = ChooseTarget(possibleActorTeam, actor);
            if (target != null) result.Add(target);
        }
        // 타겟팀 중 하나 타겟
        else
        {
            var possibleTargets = targetTeam.Where(m => m.CurHp > 0).ToList();
            var target = ChooseTarget(possibleTargets, actor);
            if (target != null) result.Add(target);
        }

        return result;
    }
}
