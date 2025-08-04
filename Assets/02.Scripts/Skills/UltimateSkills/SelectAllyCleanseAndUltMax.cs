using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAllyCleanseAndUltMax : ISkillEffect
{
    private SkillData skillData;

    public SelectAllyCleanseAndUltMax(SkillData data)
    {
        skillData = data;
    }
    
    // 우리팀중 선택한 몬스터 모든 상태이상 제거, 궁극기 최대치로 채움, 15레벨 본인도 상태이상 제거
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);
        
        if (caster.Level >= 15) caster.RemoveStatusEffects();

        foreach (var target in targetCopy)
        {
            target.RemoveStatusEffects();
            target.IncreaseUltimateCostMax();
        }
    }
}
