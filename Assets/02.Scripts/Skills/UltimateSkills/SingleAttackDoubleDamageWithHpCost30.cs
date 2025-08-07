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
    
    // 단일공격 최대체력의 30% 데미지 입고, 공격 데미지 2배, 25레벨 데미지 1.5배 20% 데미지 입음
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 25 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            int finalDamage = Mathf.RoundToInt(damage * 2);
            
            BattleManager.Instance.DealDamage(target, finalDamage, caster, this.skillData, result.isCritical, result.effectiveness);
            
            int amount = Mathf.RoundToInt(caster.Level >= 25 ? 0.20f : 0.30f);
            int hpCost = Mathf.RoundToInt(target.CurMaxHp * amount);
            
            caster.TakeDamage(hpCost);
        }
    }
}
