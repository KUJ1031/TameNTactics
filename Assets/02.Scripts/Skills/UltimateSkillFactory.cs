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
        { UltimateSkillList.PowerSlam, data => new PowerSlam(data)}
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
