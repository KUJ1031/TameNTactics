using System.Collections.Generic;
using UnityEngine;

public class FlareStrike : ISkillEffect
{
    private SkillData skillData;

    public FlareStrike(SkillData data)
    {
        skillData = data;
    }
    
    public void Execute(Monster caster, List<Monster> targets)
    {
        foreach (var target in targets)
        {
            int damage = Mathf.RoundToInt(caster.Attack * skillData.skillPower);
            target.TakeDamage(damage);

            if (Random.value < 0.2f)
            {
                target.ApplyStatus(new Burn(2));
            }
        }
    }
}
