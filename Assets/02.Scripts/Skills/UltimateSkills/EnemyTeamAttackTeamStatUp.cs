using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTeamAttackTeamStatUp : ISkillEffect
{
    private SkillData skillData;

    public EnemyTeamAttackTeamStatUp(SkillData data)
    {
        skillData = data;
    }

    // 전체공격 우리팀 전체 공격력, 방어력, 스피드, 치명타확률 10%씩 상승, 25레벨 15%씩 상승
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);
        List<Monster> allyMonsters = BattleManager.Instance.BattleEntryTeam.Where(m => m.CurHp > 0).ToList();

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical, result.effectiveness);
        }

        foreach (var monster in allyMonsters)
        {
            int atkUp = Mathf.RoundToInt(monster.Level >= 25 ? monster.CurAttack * 0.15f : monster.CurAttack * 0.1f);
            int defUp = Mathf.RoundToInt(monster.Level >= 25 ? monster.CurDefense * 0.15f : monster.CurDefense * 0.1f);
            int spdUp = Mathf.RoundToInt(monster.Level >= 25 ? monster.CurSpeed * 0.15f : monster.CurSpeed * 0.1f);
            int criUp = monster.Level >= 15 ? 20 : 10;
            
            monster.PowerUp(atkUp);
            monster.BattleDefenseUp(defUp);
            monster.SpeedUpEffect(spdUp);
            monster.BattleCritChanceUp(criUp);
        }
    }
}
