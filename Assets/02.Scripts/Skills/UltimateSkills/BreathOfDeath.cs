using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathOfDeath : ISkillEffect
{
    private SkillData skillData;

    public BreathOfDeath(SkillData data)
    {
        skillData = data;
    }

    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;

        foreach (var target in targets)
        {
            if (Random.value < 0.05f && target.CurHp > 0)
            {
                target.TakeDamage(target.CurHp);
            }

            else
            {
                var result = DamageCalculator.CalculateDamage(caster, target, skillData);
                BattleManager.Instance.DealDamage(target, result.damage, caster);
            }
        }
    }
}
