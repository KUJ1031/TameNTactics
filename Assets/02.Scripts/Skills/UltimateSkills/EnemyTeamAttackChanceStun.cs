using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamAttackChanceStun : ISkillEffect
{
    private SkillData skillData;

    public EnemyTeamAttackChanceStun(SkillData data)
    {
        skillData = data;
    }
    
    // 전체공격 30% 확률로 2턴동안 스턴, 25레벨 데미지 1.5배 50%확률 스턴
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 25 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            float value = caster.Level >= 25 ? 0.5f : 0.3f;
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical, result.effectiveness);
            
            if (Random.value < value)
            {
                target.ApplyStatus(new Stun(2));
            }
        }
    }
}
