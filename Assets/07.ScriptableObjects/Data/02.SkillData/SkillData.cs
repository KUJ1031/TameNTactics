using UnityEngine;

public enum SkillType
{
    PassiveSkill,
    NormalSkill,
    UltimateSkill,
}

public enum PassiveSkillType
{
    None,
}

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillId;
    public SkillType skillType;
    public PassiveSkillType passiveType;
    public string skillName;
    public float skillPower;
    public int curUltimateCost;
    public int maxUltimateCost;

    public bool isTargetSingleAlly;
    public bool isAreaAttack;
    public bool isTargetSelf;
    [TextArea]
    public string description;
    public Sprite icon;
}
