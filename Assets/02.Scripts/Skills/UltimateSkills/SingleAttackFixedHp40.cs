using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackFixedHp40 : ISkillEffect
{
    private SkillData skillData;

    public SingleAttackFixedHp40(SkillData data)
    {
        skillData = data;
    }

    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            int damage = Mathf.RoundToInt(target.MaxHp * 0.4f);
            BattleManager.Instance.DealDamage(target, damage, caster, this.skillData, false);
        }
    }
}
