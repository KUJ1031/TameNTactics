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
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);
        
        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster);
            
            if (caster.Level >= 10)
            {
                yield return new WaitForSeconds(1f);
                int healAmount = Mathf.RoundToInt(result.damage * 0.1f);
                caster.Heal(healAmount);
            }
        }
        
        yield break;
    }
}
