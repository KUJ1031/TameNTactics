using System;
using System.Collections.Generic;
using UnityEngine;

public static class PassiveSkillFactory
{
    private static Dictionary<PassiveSkillList, Func<IPassiveSkill>> creators = new()
    {
        { PassiveSkillList.AllyTypeBoost, () => new AllyTypeBoost() },
        { PassiveSkillList.SelfHealOnTurnEnd, () => new SelfHealOnTurnEnd() },
        { PassiveSkillList.ReflectDamage, () => new ReflectDamage() },
        { PassiveSkillList.EscapeMaster, () => new EscapeMaster() },
        { PassiveSkillList.LowHpAttackBoost, () => new LowHpAttackBoost() }
    };

    public static IPassiveSkill Get(PassiveSkillList list)
    {
        return creators.TryGetValue(list, out var creator) ? creator() : null;
    }
}
