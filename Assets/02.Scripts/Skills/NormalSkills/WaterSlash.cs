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

    // 최종데미지의 10% 만큼 본인 회복
    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;
        
        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster);

            if (caster.Level >= 10)
            {
                int healAmount = Mathf.RoundToInt(result.damage * 0.1f);
                caster.Heal(healAmount);
            }
        }
    }
}
