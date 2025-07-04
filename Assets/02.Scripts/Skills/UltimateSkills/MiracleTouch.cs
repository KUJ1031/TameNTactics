using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleTouch : ISkillEffect
{
    private SkillData skillData;

    public MiracleTouch(SkillData data)
    {
        skillData = data;
    }
    
    public void Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) return;

        foreach (var target in targets)
        {
            if (target.CurHp > 0)
            {
                target.Heal(target.MaxHp);
                target.RemoveStatusEffects();
            }
        }
    }
}
