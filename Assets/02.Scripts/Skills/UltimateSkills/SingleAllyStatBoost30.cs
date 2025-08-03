using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAllyStatBoost30 : ISkillEffect
{
    private SkillData skillData;

    public SingleAllyStatBoost30(SkillData data)
    {
        skillData = data;
    }
    
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            int atkAmount = Mathf.RoundToInt(target.CurAttack * 0.3f);
            int defAmount = Mathf.RoundToInt(target.CurDefense * 0.3f);
            int spdAmount = Mathf.RoundToInt(target.Speed * 0.3f);
            int criChanceAmount = 30;
            
            target.PowerUp(atkAmount);
            target.BattleDefenseUp(defAmount);
            target.SpeedUpEffect(spdAmount);
            target.BattleCritChanceUp(30);
        }
    }
}
