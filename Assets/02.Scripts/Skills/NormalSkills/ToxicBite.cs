using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicBite : ISkillEffect
{
    private SkillData skillData;

    public ToxicBite(SkillData data)
    {
        skillData = data;
    }

    // 20% 확률로 2턴동안 독
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);
        
        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster);

            if (Random.value < 0.2f && caster.Level >= 10)
            {
                yield return new WaitForSeconds(1f);
                target.ApplyStatus(new Poison(2));
            }
        }
        
        yield break;
    }
}
