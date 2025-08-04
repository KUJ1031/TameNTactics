using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTaunt : ISkillEffect
{
    private SkillData skillData;

    public SelfTaunt(SkillData data)
    {
        skillData = data;
    }

    // 도발 2턴 동안 모든 공격 대신 맞음, 15레벨 3턴 도발 실드 추가
    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;
        if (caster.Level >= 15)
        {
            caster.Shield();
        }
        
        int amount = Mathf.RoundToInt(caster.Level >= 15 ? 3 : 2);

        caster.ApplyBuff(new Taunt(amount));

    }
}
