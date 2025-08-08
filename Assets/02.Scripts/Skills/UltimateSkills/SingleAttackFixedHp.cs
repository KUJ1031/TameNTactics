using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackFixedHp : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackFixedHp(SkillData data)
    {
        skillData = data;
    }

    // 단일공격 타겟 최대체력의 40% 고정 데미지, 25레벨 50% 고정 데미지
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            float amount = caster.Level >= 25 ? target.CurMaxHp * 0.5f : target.CurMaxHp * 0.4f;
            int damage = Mathf.RoundToInt(target.CurMaxHp * amount);
            
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, false, 1f);
        }
    }
}
