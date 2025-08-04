using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamAttackPoison : ISkillEffect
{
    private SkillData skillData;

    public EnemyTeamAttackPoison(SkillData data)
    {
        skillData = data;
    }
    
    // 전체공격 50% 확률로 3턴동안 중독, 15레벨 데미지 1.5배 70% 확률 3턴 중독
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 15 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            float value = caster.Level >= 15 ? 0.7f : 0.5f;
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical);
            if (Random.value < value)
            {
                target.ApplyStatus(new Poison(3));
            }
        }
    }
}
