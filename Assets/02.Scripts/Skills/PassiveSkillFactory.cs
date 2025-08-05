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
        { PassiveSkillList.TypeHitRecovery, () => new TypeHitRecovery()},
        { PassiveSkillList.EnemyAtkDown, () => new EnemyAtkDown()},
        { PassiveSkillList.UltGaugeChancePerTurn, () => new UltGaugeChancePerTurn()},
        { PassiveSkillList.BonusAttack, () => new BonusAttack()},
        { PassiveSkillList.PowerBoostPerUlt, () => new PowerBoostPerUlt()},
        { PassiveSkillList.AliveTeamGuard, () => new AliveTeamGuard()},
        { PassiveSkillList.CritUpOnCritHit, () => new CritUpOnCritHit()},
        { PassiveSkillList.InterceptDamage, () => new InterceptDamage()},
        { PassiveSkillList.AtkUpOnDamaged, () => new AtkUpOnDamaged()},
        { PassiveSkillList.HitCritBoost, () => new HitCritBoost()},
        { PassiveSkillList.ReviveOnDeathChance, () => new ReviveOnDeathChance()},
        { PassiveSkillList.CritUpOnTurnEnd, () => new CritUpOnTurnEnd()},
        { PassiveSkillList.PoisonEnemiesOnDeath, () => new PoisonEnemiesOnDeath()},
        { PassiveSkillList.CleanseSelfOnUlt, () => new CleanseSelfOnUlt()}
    };

    public static IPassiveSkill GetPassiveSkill(PassiveSkillList list)
    {
        return creators.TryGetValue(list, out var creator) ? creator() : null;
    }
}
