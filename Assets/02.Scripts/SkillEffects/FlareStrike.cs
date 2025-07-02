using System.Collections.Generic;
using UnityEngine;

public class FlareStrike : ISkillEffect
{
    private SkillData skillData;

    public FlareStrike(SkillData data)
    {
        skillData = data;
    }
    
    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;
        
        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            
            target.TakeDamage(result.damage);

            if (Random.value < 0.2f)
            {
                target.ApplyStatus(new Burn(2));
            }
        }
    }
}
