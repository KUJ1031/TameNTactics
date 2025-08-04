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

    // 단일공격 5%확률로 즉사, 15레벨 데미지 1.5배 10%확률 즉사
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 15 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            float value = caster.Level >= 15 ? 0.1f : 0.05f;
            
            if (Random.value < value && target.CurHp > 0)
            {
                BattleManager.Instance.DealDamage(target, target.CurHp, caster, this.skillData, false);
            }

            else
            {
                BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical);
            }
        }
    }
}
