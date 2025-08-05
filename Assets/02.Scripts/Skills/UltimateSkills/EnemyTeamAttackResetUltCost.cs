using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamAttackResetUltCost : ISkillEffect
{
    private SkillData skillData;

    public EnemyTeamAttackResetUltCost(SkillData data)
    {
        skillData = data;
    }
    
    // 전체공격 상대팀 궁극기 코스트 초기화, 15레벨 데미지 1.5배
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            var result = DamageCalculator.CalculateDamage(caster, target, skillData);
            int damage = caster.Level >= 15 ? (Mathf.RoundToInt(result.damage * 1.5f)) : result.damage;
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, result.isCritical, result.effectiveness);
            target.InitializeUltimateCost();
        }
    }
}
