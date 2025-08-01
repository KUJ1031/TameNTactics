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

    // 5%확률로 즉사
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            if (Random.value < 0.05f && target.CurHp > 0)
            {
                BattleManager.Instance.DealDamage(target, target.CurHp, caster, this.skillData, false);
            }

            else
            {
                var result = DamageCalculator.CalculateDamage(caster, target, skillData);
                BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical);
            }
        }
    }
}
