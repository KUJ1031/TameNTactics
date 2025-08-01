using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBuffDefUp : ISkillEffect
{
    private SkillData skillData;
    
    public TeamBuffDefUp(SkillData data)
    {
        skillData = data;
    }
    
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);
        
        foreach (var target in targetCopy)
        {
            if (target.CurHp > 0)
            {
                int amount = Mathf.RoundToInt(target.CurDefense * 0.2f);
                target.BattleDefenseUp(amount);
            }
        }
    }
}
