public static class SpawnAreaExtensions
{
    public static string ToKorean(this SpawnArea area) => area switch
    {
        SpawnArea.StartForest => "시작의 숲",
        SpawnArea.Castle => "성",
        SpawnArea.Unknown => "알 수 없음",
        _ => area.ToString()
    };
}
