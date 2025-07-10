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
    
    // 우리팀 전체 각자 체력의 20% 획복, 궁극기 코스트 1개씩 증가
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);
        
        foreach (var target in targetCopy)
        {
            int amount = Mathf.RoundToInt(target.MaxHp * 0.2f);
            target.Heal(amount);
            target.IncreaseUltimateCost();
        }
        
        yield return new WaitForSeconds(1f);
    }
}
