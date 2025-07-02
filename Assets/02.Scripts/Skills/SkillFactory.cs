using System;
using System.Collections.Generic;

public static class SkillFactory
{
    private static Dictionary<string, Func<SkillData, ISkillEffect>> skillEffectCreators = new()
    {
        { "FlareStrike", data => new FlareStrike(data) },
        { "ToxicBite", data => new ToxicBite(data) },
        { "WaterSlash", data => new WaterSlash(data) },
        { "SteelSlash", data => new SteelSlash(data) },
        { "GroundSmash", data => new GroundSmash(data) }
    };

    public static ISkillEffect GetSkillEffect(SkillData data)
    {
        if (data == null || string.IsNullOrEmpty(data.skillId)) return null;

        if (skillEffectCreators.TryGetValue(data.skillId, out var creator))
        {
            return creator(data);
        }
        
        return null;
    }
}
