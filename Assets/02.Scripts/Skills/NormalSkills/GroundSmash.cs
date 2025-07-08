using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSmash : ISkillEffect
{
    private SkillData skillData;

    public GroundSmash(SkillData data)
    {
        skillData = data;
    }
    
    // 20% 확률로 마비
    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;
        
        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster);

            if (Random.value < 0.2f & caster.Level >= 10)
            {
                target.ApplyStatus(new Paralysis(2));
            }
        }
    }
}
