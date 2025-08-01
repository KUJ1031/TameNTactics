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
    
    // 우리팀 전체 각자 체력의 30% 획복, 궁극기 코스트 1개씩 증가
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);
        
        foreach (var target in targetCopy)
        {
            if (target.CurHp > 0)
            {
                int amount = Mathf.RoundToInt(target.MaxHp * 0.3f);
                target.Heal(amount);
                target.IncreaseUltimateCost();
            }
        }
    }
}
