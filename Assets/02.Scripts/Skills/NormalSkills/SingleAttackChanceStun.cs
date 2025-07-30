using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackChanceStun : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackChanceStun(SkillData data)
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
            BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical);

            if (Random.value < 0.2f && caster.Level >= 10)
            {
                yield return new WaitForSeconds(1f);
                target.ApplyStatus(new Stun(2));
            }
        }
    }
}
