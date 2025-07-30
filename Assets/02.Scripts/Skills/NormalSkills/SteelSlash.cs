using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SteelSlash : ISkillEffect
{
    private SkillData skillData;

    public SteelSlash(SkillData data)
    {
        skillData = data;
    }

    // 본인 스피드의 10% 만큼 스피드값 증가
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster, this.skillData, result.isCritical);
        }

        if (caster.Level >= 10)
        {
            int speedDelta = Mathf.RoundToInt(caster.Speed * 0.1f);
            caster.SpeedUpEffect(speedDelta);
        }

        yield break;
    }
}
