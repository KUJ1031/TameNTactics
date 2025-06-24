using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("스킬 기본 정보")]
    public string name;
    // 파워가 1.5라면 150%로 작성해주세요
    public string description;
    // 파워는 1.5 이런식으로 적용해주세요
    public float power;
    public SkillType skillType;

    [Header("궁극기 코스트")]
    public int ultimateCost;
}

public enum SkillType
{
    Passive,
    Active,
    Ultimate
}
