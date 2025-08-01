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

    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);
        List<Monster> allyMonsters = BattleManager.Instance.BattleEntryTeam.Where(m => m.CurHp > 0).ToList();

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical);
        }

        foreach (var monster in allyMonsters)
        {
            int atkUp = Mathf.RoundToInt(monster.CurAttack * 0.1f);
            int defUp = Mathf.RoundToInt(monster.CurDefense * 0.1f);
            int spdUp = Mathf.RoundToInt(monster.CurSpeed * 0.1f);
            int criUp = 10;
            
            monster.PowerUp(atkUp);
            monster.BattleDefenseUp(defUp);
            monster.SpeedUpEffect(spdUp);
            monster.BattleCritChanceUp(criUp);
        }
    }
}
