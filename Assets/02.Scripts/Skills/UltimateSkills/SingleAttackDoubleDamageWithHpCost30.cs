using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackDoubleDamageWithHpCost30 : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackDoubleDamageWithHpCost30(SkillData data)
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
            int amount = Mathf.RoundToInt(result.damage * 2);
            BattleManager.Instance.DealDamage(target, amount, caster, this.skillData, result.isCritical);
            
            int amount2 = Mathf.RoundToInt(target.CurMaxHp * 0.3f);
            caster.TakeDamage(amount2);
        }
    }
}
