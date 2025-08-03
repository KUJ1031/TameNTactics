using System;
using System.Collections.Generic;

public static class UltimateSkillFactory
{
    private static Dictionary<UltimateSkillList, Func<SkillData, ISkillEffect>> UltSkillCreators = new()
    {
        { UltimateSkillList.FireStorm, data => new FireStorm(data)},
        { UltimateSkillList.MiracleTouch , data => new MiracleTouch(data)},
        { UltimateSkillList.BreathOfDeath, data => new BreathOfDeath(data)},
        { UltimateSkillList.GracePulse, data => new GracePulse(data)},
        { UltimateSkillList.PowerSlam, data => new PowerSlam(data)},
        { UltimateSkillList.EnemyTeamAttackAtkDown, data => new EnemyTeamAttackAtkDown(data)},
        { UltimateSkillList.TeamBuffDefUp, data => new TeamBuffDefUp(data)},
        { UltimateSkillList.EnemyTeamAttackPoison, data => new EnemyTeamAttackPoison(data)},
        { UltimateSkillList.EnemyTeamAttackTeamStatUp, data => new EnemyTeamAttackTeamStatUp(data)},
        { UltimateSkillList.SelfCleanseAndShield, data => new SelfCleanseAndShield(data)},
        { UltimateSkillList.EnemyTeamAttackResetUltCost, data => new EnemyTeamAttackResetUltCost(data)},
        { UltimateSkillList.SingleAttackRemoveAllBuffs, data => new SingleAttackRemoveAllBuffs(data)},
        { UltimateSkillList.EnemyTeamAttackHealBlock2Turn, data => new EnemyTeamAttackHealBlock2Turn(data)},
        { UltimateSkillList.SelectAllyCleanseAndUltMax, data => new SelectAllyCleanseAndUltMax(data)},
        { UltimateSkillList.SingleAttackDoubleDamageIfDebuffed, data => new SingleAttackDoubleDamageIfDebuffed(data)},
        { UltimateSkillList.TeamRegenHp3Turn, data => new TeamRegenHp3Turn(data)},
        { UltimateSkillList.EnemyTeamAttackChanceStun, data => new EnemyTeamAttackChanceStun(data)},
        { UltimateSkillList.SingleAttackBonusDamageChance, data => new SingleAttackBonusDamageChance(data)},
        { UltimateSkillList.ReviveAllyFullHp, data => new ReviveAllyFullHp(data)},
        { UltimateSkillList.SingleAttackDoubleDamageWithHpCost30, data => new SingleAttackDoubleDamageWithHpCost30(data)},
        { UltimateSkillList.SingleAllyStatBoost30, data => new SingleAllyStatBoost30(data)},
        { UltimateSkillList.SingleAttackDoubleDamageIfTargetHpOver80, data => new SingleAttackDoubleDamageIfTargetHpOver80(data)},
        { UltimateSkillList.SingleAttackHpScaledDamage, data => new SingleAttackHpScaledDamage(data)},
        { UltimateSkillList.SingleAttackFixedHp40, data => new SingleAttackFixedHp40(data)},
        { UltimateSkillList.SelfTaunt, data => new SelfTaunt(data)}
    };

    public static ISkillEffect GetUltimateSkill(SkillData data)
    {
        if (data == null) return null;

        if (data.skillType == SkillType.UltimateSkill &&
            UltSkillCreators.TryGetValue(data.ultimateSkillList, out var creator))
        {
            return creator(data);
        }
        
        return null;
    }
}
