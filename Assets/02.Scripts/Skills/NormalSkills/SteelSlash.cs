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

    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;
        
        foreach (var target in targets)
        {
            int speedDelta = Mathf.RoundToInt(caster.Speed * 0.1f);
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            
            BattleManager.Instance.DealDamage(target, result.damage, caster);
            caster.SpeedUpEffect(speedDelta);
        }
    }
}
