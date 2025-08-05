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
    
    // 단일 공격 상대 몬스터의 체력이 최대체력의 80%가 넘으면 데미지 2배, 15레벨 데미지 1.5배 최대체력 조건 65% 이상
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 15 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            float value = caster.Level >= 15 ? 0.65f : 0.8f;

            if (target.CurHp > target.CurMaxHp * value)
            {
                int amount = Mathf.RoundToInt(damage * 2);
                BattleManager.Instance.DealDamage(target, amount, caster, this.skillData, result.isCritical, result.effectiveness);
            }
            
            else BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical, result.effectiveness);
        }
    }
}
