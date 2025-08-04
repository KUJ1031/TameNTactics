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
    
    // 같은팀 최대체력의 20% 회복, 궁극기 코스트 1개씩 증가, 15레벨 최대체력 40% 회복
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);
        
        foreach (var target in targetCopy)
        {
            if (target.CurHp > 0)
            {
                int amount = Mathf.RoundToInt(caster.Level >= 15 ? target.MaxHp * 0.4f : target.MaxHp * 0.2f);
                target.Heal(amount);
                target.IncreaseUltimateCost();
            }
        }
    }
}
