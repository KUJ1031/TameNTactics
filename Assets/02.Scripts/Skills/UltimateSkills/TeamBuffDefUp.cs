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
    
    // 우리팀 방어력 20% 상승, 15레벨 40% 상승
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);
        
        foreach (var target in targetCopy)
        {
            if (target.CurHp > 0)
            {
                int amount = Mathf.RoundToInt(caster.Level >= 15 ? target.CurDefense * 0.4f : target.CurDefense * 0.2f);
                target.BattleDefenseUp(amount);
            }
        }
    }
}
