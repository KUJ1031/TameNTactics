using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlash : ISkillEffect
{
    private SkillData skillData;

    public WaterSlash(SkillData data)
    {
        skillData = data;
    }

    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;
        
        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int healAmount = Mathf.RoundToInt(result.damage * 0.2f);
            
            BattleManager.Instance.DealDamage(target, result.damage, caster);
            caster.Heal(healAmount);
        }
    }
}
