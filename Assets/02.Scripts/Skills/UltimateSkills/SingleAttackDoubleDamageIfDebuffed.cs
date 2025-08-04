using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackDoubleDamageIfDebuffed : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackDoubleDamageIfDebuffed(SkillData data)
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
            
            if (target.ActiveStatusEffects.Count > 0)
            {
                int damage = result.damage * 2;
                BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical);
            }
            
            else BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical);
        }
    }
}
