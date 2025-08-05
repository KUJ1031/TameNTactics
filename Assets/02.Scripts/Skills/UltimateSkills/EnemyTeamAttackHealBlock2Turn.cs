using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamAttackHealBlock2Turn : ISkillEffect
{
    private SkillData skillData;

    public EnemyTeamAttackHealBlock2Turn(SkillData data)
    {
        skillData = data;
    }

    // 전체공격 2턴동안 힐 불가, 25레벨 데미지 1.5배 3턴동안 힐 불가
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 25 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            int amount = Mathf.RoundToInt(caster.Level >= 25 ? 3 : 2);
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical, result.effectiveness);
            target.ApplyStatus(new HealBlock(amount));
        }
    }
}
