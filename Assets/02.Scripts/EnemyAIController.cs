using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemyAIController
{
    public class EnemyAction
    {
        public MonsterData actor;
        public SkillData selectedSkill;

        public MonsterData singleTarget;
        public List<MonsterData> multiTargets;
    }

    public static EnemyAction DecideEnemyAction(List<MonsterData> enemyMonsters, List<MonsterData> playerMonsters)
    {
        // 1. 궁극기 사용 가능한 몬스터가 있다면 우선
        foreach (var enemy in enemyMonsters)
        {
            if (enemy.curHp <= 0) continue;

            var ultimateSkill = enemy.skills.FirstOrDefault(s =>
                s.skillType == SkillType.UltimateSkill &&
                s.curUltimateCost >= s.ultimateCost);

            if (ultimateSkill != null)
            {
                var targets = ChooseTargets(ultimateSkill, playerMonsters, enemyMonsters, enemy);

                return new EnemyAction
                {
                    actor = enemy,
                    selectedSkill = ultimateSkill,
                    singleTarget = targets.Count == 1 ? targets[0] : null,
                    multiTargets = targets.Count > 1 ? targets : null
                };
            }
        }

        // 2. 상성 유리한 대상이 있다면 ActiveSkill 사용
        foreach (var enemy in enemyMonsters)
        {
            if (enemy.curHp <= 0) continue;

            bool hasAdvantage = playerMonsters.Any(player =>
                player.curHp > 0 &&
                TypeChart.GetEffectiveness(enemy, player) > 1f);

            if (hasAdvantage)
            {
                var activeSkill = GetSkill(enemy, SkillType.ActiveSkill);
                if (activeSkill != null)
                {
                    var targets = ChooseTargets(activeSkill, playerMonsters, enemyMonsters, enemy);

                    return new EnemyAction
                    {
                        actor = enemy,
                        selectedSkill = activeSkill,
                        singleTarget = targets.Count == 1 ? targets[0] : null,
                        multiTargets = targets.Count > 1 ? targets : null
                    };
                }
            }
        }

        // 3. 조건이 없으면 랜덤 몬스터가 ActiveSkill 사용
        var aliveEnemies = enemyMonsters.Where(m => m.curHp > 0).ToList();
        if (aliveEnemies.Count == 0) return null;

        var randomEnemy = aliveEnemies[Random.Range(0, aliveEnemies.Count)];
        var randomSkill = GetSkill(randomEnemy, SkillType.ActiveSkill);

        if (randomSkill != null)
        {
            var targets = ChooseTargets(randomSkill, playerMonsters, enemyMonsters, randomEnemy);

            return new EnemyAction
            {
                actor = randomEnemy,
                selectedSkill = randomSkill,
                singleTarget = targets.Count == 1 ? targets[0] : null,
                multiTargets = targets.Count > 1 ? targets : null
            };
        }

        return null;
    }

    private static SkillData GetSkill(MonsterData monster, SkillType skillType)
    {
        return monster.skills.FirstOrDefault(s => s.skillType == skillType);
    }

    private static MonsterData ChooseTarget(List<MonsterData> targetMonsters, MonsterData attacker)
    {
        // 1. 체력 50% 이하 중 가장 낮은 몬스터
        var lowHp = targetMonsters
            .Where(m => m.curHp > 0 && m.curHp / m.maxHp <= 0.5f)
            .OrderBy(m => m.curHp)
            .ToList();

        if (lowHp.Count > 0) return lowHp[0];

        // 2. 상성 유리하고 HP 낮은 몬스터
        var effective = targetMonsters
            .Where(m => m.curHp > 0 && TypeChart.GetEffectiveness(attacker, m) > 1f)
            .OrderBy(m => m.curHp)
            .ToList();

        if (effective.Count > 0) return effective[0];

        // 3. 랜덤 대상
        var alive = targetMonsters.Where(m => m.curHp > 0).ToList();
        if (alive.Count > 0) return alive[Random.Range(0, alive.Count)];

        return null;
    }

    private static List<MonsterData> ChooseTargets(
        SkillData skill, List<MonsterData> targetTeam, List<MonsterData> actorTeam, MonsterData actor)
    {
        List<MonsterData> result = new List<MonsterData>();

        if (skill.isAreaAttack && skill.isTargetSelf)
        {
            // 공격팀 전체 대상
            result = actorTeam.Where(m => m.curHp > 0).ToList();
        }
        else if (skill.isAreaAttack && !skill.isTargetSelf)
        {
            // 타겟팀 전체 대상
            result = targetTeam.Where(m => m.curHp > 0).ToList();
        }
        else if (!skill.isAreaAttack && skill.isTargetSelf)
        {
            // 공격하는 자기 자신
            result.Add(actor);
        }
        else if (skill.isTargetSingleAlly)
        {
            var possibleActorTeam = actorTeam.Where(m => m.curHp > 0).ToList();

            var target = ChooseTarget(possibleActorTeam, actor);
            
            result.Add(target);
        }

        return result;
    }
}
