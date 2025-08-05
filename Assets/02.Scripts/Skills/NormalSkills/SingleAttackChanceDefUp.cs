using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackChanceDefUp : ISkillEffect
{
    private SkillData skillData;
    
    public SingleAttackChanceDefUp(SkillData data)
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
                int amount = Mathf.RoundToInt(caster.CurDefense * 0.1f);
                caster.BattleDefenseUp(amount);
            }
        }
    }
}
