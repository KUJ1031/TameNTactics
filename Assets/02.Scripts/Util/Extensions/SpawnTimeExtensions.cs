public static class SpawnTimeExtensions
{
    public static string ToKorean(this SpawnTime area) => area switch
    {
        SpawnTime.Morning => "아침",
        SpawnTime.Evening => "저녁",
        SpawnTime.Dawn => "새벽",
        _ => area.ToString()
    };
}
