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

    public IEnumerator Execute(Monster caster, List<Monster> targets)
    {
        if (skillData == null || targets == null || targets.Count == 0) yield break;

        caster.ApplyBuff(new Taunt(2));
    }
}
