using System;
using System.Collections.Generic;

public static class UltimateSkillFactory
{
    private static Dictionary<UltimateSkillList, Func<SkillData, ISkillEffect>> UltSkillCreators = new();

    public static ISkillEffect Get(SkillData data)
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
