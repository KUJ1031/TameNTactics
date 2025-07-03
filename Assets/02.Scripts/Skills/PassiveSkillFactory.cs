using System;
using System.Collections.Generic;
using UnityEngine;

public static class PassiveSkillFactory
{
    private static Dictionary<PassiveSkillType, Func<IPassiveSkill>> creators = new()
    {
        { PassiveSkillType.AllyTypeBoost, () => new AllyTypeBoost() },
        { PassiveSkillType.SelfHealOnTurnEnd, () => new SelfHealOnTurnEnd() },
        { PassiveSkillType.ReflectDamage, () => new ReflectDamage() },
        { PassiveSkillType.EscapeMaster, () => new EscapeMaster() },
        { PassiveSkillType.LowHpAttackBoost, () => new LowHpAttackBoost() }
    };

    public static IPassiveSkill Get(PassiveSkillType type)
    {
        return creators.TryGetValue(type, out var creator) ? creator() : null;
    }
}
