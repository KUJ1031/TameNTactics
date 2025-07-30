public static class MonsterTypeExtensions
{
    public static string ToKorean(this MonsterType type) => type switch
    {
        MonsterType.Fire => "불",
        MonsterType.Water => "물",
        MonsterType.Grass => "풀",
        MonsterType.Ground => "땅",
        MonsterType.Steel => "철",
        _ => type.ToString()
    };
}
