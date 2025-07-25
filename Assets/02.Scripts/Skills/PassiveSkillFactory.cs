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
        { PassiveSkillList.LowHpAttackBoost, () => new LowHpAttackBoost() },
        { PassiveSkillList.DefensiveStance, () => new DefensiveStance()},
        { PassiveSkillList.IncreaseMissChance, () => new IncreaseMissChance()},
        { PassiveSkillList.StatusEffectImmunity, () => new StatusEffectImmunity()},
        { PassiveSkillList.OneHitShield, () => new OneHitShield()},
        { PassiveSkillList.AtkUpOnAllyDeath , () => new AtkUpOnAllyDeath()},
        { PassiveSkillList.TypeHitRecovery, () => new StatusEffectImmunity()},
        { PassiveSkillList.EnemyAtkDownTwoTurns, () => new EnemyAtkDownTwoTurns()},
        { PassiveSkillList.UltGaugeChancePerTurn, () => new UltGaugeChancePerTurn()},
        { PassiveSkillList.BonusAttack, () => new BonusAttack()}
    };

    public static IPassiveSkill GetPassiveSkill(PassiveSkillList list)
    {
        return creators.TryGetValue(list, out var creator) ? creator() : null;
    }
}
