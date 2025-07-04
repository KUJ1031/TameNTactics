using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GracePulse : ISkillEffect
{
    private SkillData skillData;
    
    public GracePulse(SkillData data)
    {
        skillData = data;
    }
    
    public void Execute(Monster caster, List<Monster> targets)
    {
        foreach (var target in targets)
        {
            int amount = Mathf.RoundToInt(target.MaxHp * 0.2f);
            target.Heal(amount);
            target.IncreaseUltimateCost();
        }
    }
}
