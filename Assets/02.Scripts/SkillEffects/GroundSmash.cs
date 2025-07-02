using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSmash : ISkillEffect
{
    private SkillData skillData;

    public GroundSmash(SkillData data)
    {
        skillData = data;
    }

    public void Execute(Monster caster, List<Monster> targets)
    {
        foreach (var target in targets)
        {
            int damage = Mathf.RoundToInt(caster.Attack * skillData.skillPower);
            target.TakeDamage(damage);

            if (Random.value < 0.3f)
            {
                target.ApplyStatus(new Paralysis(2));
            }
        }
    }
}
