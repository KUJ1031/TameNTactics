using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAllyStatBoost : ISkillEffect
{
    private SkillData skillData;

    public SingleAllyStatBoost(SkillData data)
    {
        skillData = data;
    }
    
    // 우리팀 한명 선택해서 스텟 20% 상승(공격력,방어력,속도,치명타확률), 15레벨 30% 상승
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            int atkAmount = Mathf.RoundToInt(caster.Level >= 15 ? target.CurAttack * 0.3f : target.CurAttack * 0.2f);
            int defAmount = Mathf.RoundToInt(caster.Level >= 15 ? target.CurDefense * 0.3f : target.CurDefense * 0.2f);
            int spdAmount = Mathf.RoundToInt(caster.Level >= 15 ? target.Speed * 0.3f : target.Speed * 0.2f);
            int criChanceAmount = caster.Level >= 15 ? 30 : 20;
            
            target.PowerUp(atkAmount);
            target.BattleDefenseUp(defAmount);
            target.SpeedUpEffect(spdAmount);
            target.BattleCritChanceUp(criChanceAmount);
        }
    }
}
