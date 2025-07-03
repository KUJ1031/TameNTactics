using System.Collections.Generic;
using UnityEngine;

public class ToxicBite : ISkillEffect
{
    private SkillData skillData;

    public ToxicBite(SkillData data)
    {
        skillData = data;
    }

    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null) return;
        if (targets == null || targets.Count == 0) return;
        
        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster);

            if (Random.value < 0.7f)
            {
                target.ApplyStatus(new Poison(2));
            }
        }
    }
}
