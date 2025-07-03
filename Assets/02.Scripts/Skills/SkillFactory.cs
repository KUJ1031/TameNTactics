using System;
using System.Collections.Generic;

public static class SkillFactory
{
    private static Dictionary<NormalSkillType, Func<SkillData, ISkillEffect>> normalSkillCreators = new()
    {
        { NormalSkillType.FlareStrike, data => new FlareStrike(data) },
        { NormalSkillType.ToxicBite, data => new ToxicBite(data) },
        { NormalSkillType.WaterSlash, data => new WaterSlash(data) },
        { NormalSkillType.SteelSlash, data => new SteelSlash(data) },
        { NormalSkillType.GroundSmash, data => new GroundSmash(data) }
    };

    public static ISkillEffect GetSkillEffect(SkillData data)
    {
        if (data == null) return null;

        if (data.skillType == SkillType.NormalSkill &&
            normalSkillCreators.TryGetValue(data.normalType, out var creator))
        {
            return creator(data);
        }

        return null;
    }
}
