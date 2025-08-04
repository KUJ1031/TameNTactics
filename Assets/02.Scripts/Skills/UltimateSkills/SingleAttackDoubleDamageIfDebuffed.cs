using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackDoubleDamageIfDebuffed : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackDoubleDamageIfDebuffed(SkillData data)
    {
        skillData = data;
    }
    
    // 단일공격 타겟 몬스터가 상태이상에 걸려있다면 데미지2배, 15레벨 데미지 1.5배 + 본인도 상태이상일때도 데미지 2배
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 15 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            
            if (target.ActiveStatusEffects.Count > 0 || IsDebuffed(caster))
            {
                int finalDamage = damage * 2;
                BattleManager.Instance.DealDamage(target, finalDamage, caster, this.skillData, result.isCritical);
            }
            
            else BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical);
        }
    }

    private bool IsDebuffed(Monster caster)
    {
        if (caster.Level >= 15)
        {
            if (caster.ActiveStatusEffects.Count > 0) return true;
        }
        
        return false;
    }
}
