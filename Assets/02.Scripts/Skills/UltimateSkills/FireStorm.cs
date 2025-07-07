using System.Collections.Generic;
using UnityEngine;

public class FireStorm : ISkillEffect
{
    private SkillData skillData;

    public FireStorm(SkillData data)
    {
        skillData = data;
    }
    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;

        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster);
            
            if (Random.value < 0.5f)
            {
                target.ApplyStatus(new Burn(2));
            }
        }
    }
}
