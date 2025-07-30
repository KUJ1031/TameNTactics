using System;
using System.Collections.Generic;

public static class NormalSkillFactory
{
    private static Dictionary<NormalSkillList, Func<SkillData, ISkillEffect>> normalSkillCreators = new()
    {
        { NormalSkillList.FlareStrike, data => new FlareStrike(data) },
        { NormalSkillList.ToxicBite, data => new ToxicBite(data) },
        { NormalSkillList.WaterSlash, data => new WaterSlash(data) },
        { NormalSkillList.SteelSlash, data => new SteelSlash(data) },
        { NormalSkillList.GroundSmash, data => new GroundSmash(data) },
        { NormalSkillList.DoubleTargetHit, data => new DoubleTargetHit(data)},
        { NormalSkillList.SingleAttackChanceStun, data => new SingleAttackChanceStun(data)},
        { NormalSkillList.SingleAttackChanceSleep, data => new SingleAttackChanceSleep(data)},
        { NormalSkillList.SingleAttackChanceAtkUp, data => new SingleAttackChanceAtkUp(data)},
        { NormalSkillList.SingleAttackChanceDefUp, data => new SingleAttackChanceDefUp(data)},
        { NormalSkillList.SingleAttackChanceCriChanceUp, data => new SingleAttackChanceCriChanceUp(data)},
        { NormalSkillList.SingleAttackChanceHpUp, data => new SingleAttackChanceHpUp(data)},
        { NormalSkillList.SingleAttackRandomDebuff, data => new SingleAttackRandomDebuff(data)},
        { NormalSkillList.SingleAttackTargetAtkDown, data => new SingleAttackTargetAtkDown(data)},
        { NormalSkillList.SingleAttackTargetDefDown, data => new SingleAttackTargetDefDown(data)},
        { NormalSkillList.SingleAttackHealLowestAlly, data => new SingleAttackHealLowestAlly(data)}
    };

    public static ISkillEffect GetNormalSkill(SkillData data)
    {
        if (data == null) return null;

        if (data.skillType == SkillType.NormalSkill &&
            normalSkillCreators.TryGetValue(data.normalSkillList, out var creator))
        {
            return creator(data);
        }

        return null;
    }
}
