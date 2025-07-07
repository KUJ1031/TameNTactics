using System.Collections.Generic;
using UnityEngine;

public class FireStorm : ISkillEffect
{
    private SkillData skillData;

    public FireStorm(SkillData data)
    {
        skillData = data;
    }
    
    // 전체 공격, 50% 확률로 2턴동안 화상
    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;

        foreach (var target in targets)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            BattleManager.Instance.DealDamage(target, result.damage, caster);
            
            if (Random.value < 0.5f)
            {
                target.ApplyStatus(new Burn(2));
            }
        }
    }
}
