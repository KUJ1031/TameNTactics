using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveAllyFullHp : ISkillEffect
{
    private SkillData skillData;

    public ReviveAllyFullHp(SkillData data)
    {
        skillData = data;
    }
    
    // 기절한 우리팀 하나 최대 체력으로 부활, 25레벨 실드 부여
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        var targetCopy = new List<Monster>(targets);

        foreach (var target in targetCopy)
        {
            target.ReviveMonster(target, target.CurMaxHp);
            if (caster.Level >= 25) target.Shield();
        }
    }
}
