using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackChanceCrit : ISkillEffect
{
    private SkillData skillData;
    
    public SingleAttackChanceCrit(SkillData data)
    {
        skillData = data;
    }
    
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);

            if (caster.Level >= 10 && !result.isCritical && Random.value < 0.5f)
            {
                result.isCritical = true;
            }

            BattleManager.Instance.DealDamage(target, result.damage, caster, skillData, result.isCritical);
        }
    }
}
