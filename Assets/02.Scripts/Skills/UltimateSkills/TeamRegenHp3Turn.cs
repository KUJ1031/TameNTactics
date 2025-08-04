using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamRegenHp3Turn : ISkillEffect
{
    private SkillData skillData;
    
    public TeamRegenHp3Turn(SkillData data)
    {
        skillData = data;
    }
    
    // 같은팀 전체 3턴동안 최대체력의 10% 회복, 15레벨 4턴으로 증가
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);
        
        foreach (var target in targetCopy)
        {
            if (target.CurHp > 0)
            {
                int amount = Mathf.RoundToInt(target.CurMaxHp * 0.1f);
                int value = caster.Level >= 15 ? 4 : 3;
                
                target.Heal(amount);
                target.HealDuration(value);
            }
        }
    }
}
