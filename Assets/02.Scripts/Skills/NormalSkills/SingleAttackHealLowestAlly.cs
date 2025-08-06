using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleAttackHealLowestAlly : ISkillEffect
{
    private SkillData skillData;
    
    public SingleAttackHealLowestAlly(SkillData data)
    {
        skillData = data;
    }
    
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical, result.effectiveness);

            if (Random.value < 0.5f && caster.Level >= 10)
            {
                List<Monster> monsters = BattleManager.Instance.BattleEntryTeam
                    .Where(m => m.CurHp > 0)
                    .OrderBy(m => m.CurHp)
                    .ToList();

                if (monsters.Count > 0)
                {
                    int amount = Mathf.RoundToInt(monsters[0].CurMaxHp * 0.1f);
                    monsters[0].Heal(amount);
                }
            }
        }
    }
}
