public static class PersonalityExtensions
{
    public static string ToKorean(this Personality area) => area switch
    {
        Personality.Thorough => "철저한",
        Personality.Devoted => "헌신적인",
        Personality.Decisive => "결단력있는",
        Personality.Bold => "대담한",
        Personality.Cautious => "신중한",
        Personality.Emotional => "감성적인",
        Personality.Energetic => "활동적인",
        Personality.Responsible => "책임감있는",
        Personality.Passionate => "열정적인",
        Personality.Proactive => "적극적인",
        Personality.Cynical => "염세적인",
        Personality.Altruistic => "이타적인",
        Personality.Sociable => "사교적인",
        _ => area.ToString()
    };
}
