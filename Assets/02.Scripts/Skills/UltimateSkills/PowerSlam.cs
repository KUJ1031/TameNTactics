using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSlam : ISkillEffect
{
    private SkillData skillData;

    public PowerSlam(SkillData data)
    {
        skillData = data;
    }

    // 단일공격 100% 2턴동안 스턴, 15레벨 데미지 3턴동안 스턴
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 15 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            int amount = caster.Level >= 15 ? 3 : 2;
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical);
            target.ApplyStatus(new Stun(amount));
        }
    }
}
