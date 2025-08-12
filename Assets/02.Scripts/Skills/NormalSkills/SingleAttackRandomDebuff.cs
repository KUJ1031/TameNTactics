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
            BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical, result.effectiveness);

            if (Random.value < 0.2f && caster.Level >= 10)
            {
                switch (Random.Range(0, 6))
                {
                    case 0:
                        target.ApplyStatus(new Sleep(2));
                        break;
                    case 1:
                        target.ApplyStatus(new Stun(2));
                        break;
                    case 2:
                        target.ApplyStatus(new Burn(2));
                        break;
                    case 3:
                        target.ApplyStatus(new Poison(2));
                        break;
                    case 4:
                        target.ApplyStatus(new Paralysis(2));
                        break;
                    case 5:
                        target.ApplyStatus(new HealBlock(2));
                        break;
                }
            }
        }
    }
}