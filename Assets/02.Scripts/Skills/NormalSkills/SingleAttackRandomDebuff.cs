using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class SingleAttackRandomDebuff : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackRandomDebuff(SkillData data)
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

            // 랜덤 확률로 상태이상 부여
            if (Random.value < 0.2f && caster.Level >= 10)
            {
                yield return new WaitForSeconds(1f);

                // 랜덤 상태이상 생성
                StatusEffectType[] values = (StatusEffectType[])Enum.GetValues(typeof(StatusEffectType));
                int randomIndex = Random.Range(0, values.Length);
                StatusEffectType randomEffectType = values[randomIndex];

                // 상태이상 생성 (지속시간 2)
                StatusEffect randomEffect = CreateStatusEffect(randomEffectType, 2);

                // 상태이상 적용
                if (randomEffect != null)
                {
                    target.ApplyStatus(randomEffect);
                }
            }
        }
    }

    // 상태이상을 생성하는 메서드
    private StatusEffect CreateStatusEffect(StatusEffectType effectType, int duration)
    {
        return effectType switch
        {
            StatusEffectType.Burn => new Burn(duration),
            StatusEffectType.Paralysis => new Paralysis(duration),
            StatusEffectType.Poison => new Poison(duration),
            StatusEffectType.Stun => new Stun(duration),
            StatusEffectType.Sleep => new Sleep(duration),
            StatusEffectType.AttackDown => new AttackDown(duration),
            StatusEffectType.DefenseDown => new DefenseDown(duration)
        };
    }
}