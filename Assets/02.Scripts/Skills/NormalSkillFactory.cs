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
        { NormalSkillList.GroundSmash, data => new GroundSmash(data) }
    };

    public static ISkillEffect GetSkillEffect(SkillData data)
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
