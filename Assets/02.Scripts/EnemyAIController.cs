using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemyAIController
{
    public class EnemyAction
    {
        public MonsterData actor;
        public MonsterData target;
        public SkillData selectedSkill;
    }

    public static EnemyAction DecideEnemyAction(List<MonsterData> enemyMonsters,
        List<MonsterData> playerMonsters)
    {
        // 궁극기 코스트가 가득찬 몬스터가 공격
        foreach (var enemy in enemyMonsters)
        {
            if (enemy.curHp <= 0) continue;

            var ultimateSkill = enemy.skills.FirstOrDefault(s => s.isUltimate && s.curUltimateCost >= s.ultimateCost);

            if (ultimateSkill != null)
            {
                return new EnemyAction()
                {
                    actor = enemy,
                    selectedSkill = ultimateSkill,
                    target = ChooseTarget(playerMonsters, enemy)
                };
            }
        }

        // 상성 유리한 대상이 있을 경우 유리한 몬스터가 공격
        foreach (var enemy in enemyMonsters)
        {
            if (enemy.curHp <= 0) continue;

            bool hasAdvantage =
                playerMonsters.Any(player => player.curHp > 0 && TypeChart.GetEffectiveness(enemy, player) > 1f);

            if (hasAdvantage)
            {
                return new EnemyAction()
                {
                    actor = enemy,
                    selectedSkill = GetSkill(enemy, Skill.ActiveSkill1),
                    target = ChooseTarget(playerMonsters, enemy)
                };
            }
        }

        // 설정한 조건이 없다면 랜덤으로 정해진 몬스터가 공격
        var aliveEnemies = enemyMonsters
            .Where(m => m.curHp > 0)
            .ToList();

        if (aliveEnemies.Count == 0) return null;

        var randomEnemy = aliveEnemies[Random.Range(0, aliveEnemies.Count)];
        return new EnemyAction()
        {
            actor = randomEnemy,
            selectedSkill = GetSkill(randomEnemy, Skill.ActiveSkill1),
            target = ChooseTarget(playerMonsters, randomEnemy)
        };
    }

    private static SkillData GetSkill(MonsterData monster, Skill skillType)
    {
        return monster.skills.FirstOrDefault(s => s.skillType == skillType);
    }

    private static MonsterData ChooseTarget(List<MonsterData> targetMonsters, MonsterData attacker)
    {
        // 체력 50% 이하 중 가장 낮은 몬스터 순으로 정리
        var lowHp = targetMonsters
            .Where(m => m.curHp > 0 && m.curHp / m.maxHp <= 0.5f)
            .OrderBy(m => m.curHp)
            .ToList();

        if (lowHp.Count > 0) return lowHp[0];

        // 상성이 유리한 몬스터 중 체력이 가장 낮은 몬스터 순으로 정리
        var effective = targetMonsters
            .Where(m => m.curHp > 0 && TypeChart.GetEffectiveness(attacker, m) > 1f)
            .OrderBy(m => m.curHp)
            .ToList();

        if (effective.Count > 0) return effective[0];

        // 그 외에 조건이 없을시에는 랜덤 대상 선택
        var alive = targetMonsters
            .Where(m => m.curHp > 0)
            .ToList();

        if (alive.Count > 0) return alive[Random.Range(0, alive.Count)];

        return null;
    }
}
