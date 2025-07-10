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
    
    // 우리팀 한명 100%회복, 상태이상 전체 제거
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        
        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            if (target.CurHp > 0)
            {
                yield return new WaitForSeconds(1f);
                target.Heal(target.MaxHp);
                target.RemoveStatusEffects();
            }
        }
        
        yield break;
    }
}
