using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackDoubleDamageIfTargetHpOver80 : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackDoubleDamageIfTargetHpOver80(SkillData data)
    {
        skillData = data;
    }
    
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);

            if (target.CurHp > target.CurMaxHp * 0.8f)
            {
                int amount = Mathf.RoundToInt(result.damage * 2);
                BattleManager.Instance.DealDamage(target, amount, caster, this.skillData, result.isCritical);
            }
            
            else BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical);
        }
    }
}
