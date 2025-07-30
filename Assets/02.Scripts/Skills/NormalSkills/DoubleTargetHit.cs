using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTargetHit : ISkillEffect
{
    private SkillData skillData;

    public DoubleTargetHit(SkillData data)
    {
        skillData = data;
    }
    
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) 
            yield break;
        
        float damageMultiplier = (caster.Level >= 10) ? 1.3f : 1f;
        
        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = Mathf.RoundToInt(result.damage * damageMultiplier);
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical);
        }
    }
}