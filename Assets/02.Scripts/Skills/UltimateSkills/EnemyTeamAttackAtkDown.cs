using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamAttackAtkDown : ISkillEffect
{
    private SkillData skillData;

    public EnemyTeamAttackAtkDown(SkillData data)
    {
        skillData = data;
    }
    
    // 전체공격 상대팀 공격력 10% 감소, 25레벨 데미지 1.5배 공격력 15% 감소
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 25 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            float decreaseAmount = caster.Level >= 25 ? 0.2f : 0.15f;
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical, result.effectiveness);
            
            int amount = Mathf.RoundToInt(target.CurAttack * decreaseAmount);
            target.PowerDown(amount);
        }
    }
}
