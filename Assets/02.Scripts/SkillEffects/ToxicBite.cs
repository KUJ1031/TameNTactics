using System.Collections.Generic;
using UnityEngine;

public class ToxicBite : ISkillEffect
{
    private SkillData skillData;

    public ToxicBite(SkillData data)
    {
        skillData = data;
    }

    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null) return;
        if (targets == null || targets.Count == 0) return;
        
        foreach (var target in targets)
        {
            int damage = Mathf.RoundToInt(caster.Attack * skillData.skillPower);
            target.TakeDamage(damage);

            if (Random.value < 0.7f)
            {
                target.ApplyStatus(new Poison(2));
            }
        }
    }
}
