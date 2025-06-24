using UnityEngine;

public enum SkillType
{
    PassiveSkill,
    ActiveSkill,
    UltimateSkill,
}

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Skill Data")]
public class SkillData : ScriptableObject
{
    public SkillType skillType;
    public string skillName;
    public float skillPower;
    public int curUltimateCost;
    public int ultimateCost;
    public bool isAreaAttack;
    public bool isTargetSelf;
    [TextArea]
    public string description;
    public Sprite icon;
}
