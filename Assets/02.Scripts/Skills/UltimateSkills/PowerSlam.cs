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
    
    // 100% 스턴 성공
    public void Execute(Monster caster, List<Monster> targets)
    {
        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster);
            target.ApplyStatus(new Stun(2));
        }
    }
}
