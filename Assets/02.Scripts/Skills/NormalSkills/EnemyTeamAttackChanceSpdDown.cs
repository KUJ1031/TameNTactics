using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamAttackChanceSpdDown : ISkillEffect
{
    private SkillData skillData;
    
    public EnemyTeamAttackChanceSpdDown(SkillData data)
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
            int damage = Mathf.RoundToInt(result.damage * 0.4f);
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical);

            if (Random.value < 0.15f && caster.Level >= 10)
            {
                int amount = Mathf.RoundToInt(target.CurSpeed * 0.1f);
                target.SpeedDown(amount);
            }
        }
    }
}
