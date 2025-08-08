using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackBonusDamageChance : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackBonusDamageChance(SkillData data)
    {
        skillData = data;
    }
    
    // 단일공격 40% 확률로 데미지의 50%만큼 추가 데미지, 25레벨 데미지 1.5배 60% 확률 증가
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 25 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            float value = caster.Level >= 25 ? 0.6f : 0.4f;
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical, result.effectiveness);
            int amount = Mathf.RoundToInt(result.damage * 0.5f);
            
            if (Random.value < value)
            {
                BattleManager.Instance.StartCoroutine(BattleManager.Instance.BonusAttack(target, amount));
            }
        }
    }
}
