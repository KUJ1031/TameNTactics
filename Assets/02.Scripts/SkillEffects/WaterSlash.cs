using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlash : ISkillEffect
{
    private SkillData skillData;

    public WaterSlash(SkillData data)
    {
        skillData = data;
    }

    public void Execute(Monster caster, List<Monster> targets)
    {
        foreach (var target in targets)
        {
            int damage = Mathf.RoundToInt(caster.Attack * skillData.skillPower);
            int healAmount = Mathf.RoundToInt(damage * 0.2f);
            target.TakeDamage(damage);
            caster.Heal(healAmount);
        }
    }
}
