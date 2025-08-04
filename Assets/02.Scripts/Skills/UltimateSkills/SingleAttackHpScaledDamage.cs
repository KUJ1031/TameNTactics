using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackHpScaledDamage : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackHpScaledDamage(SkillData data)
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
            int amount = Mathf.RoundToInt(result.damage * ChanceToDamage(target));
            
            BattleManager.Instance.DealDamage(target, amount, caster, this.skillData, result.isCritical);
        }
    }

    private float ChanceToDamage(Monster target)
    {
        if (target.CurHp >= target.CurMaxHp)
        {
            return 2f;
        }
        else if (target.CurHp >= target.CurMaxHp * 0.7f)
        {
            return 1.5f;
        }
        else if (target.CurHp >= target.CurMaxHp * 0.3f)
        {
            return 1f;
        }
        else
        {
            return 0.5f;
        }
    }
}
