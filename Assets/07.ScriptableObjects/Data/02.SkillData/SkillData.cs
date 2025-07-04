using UnityEngine;

public enum SkillType
{
    PassiveSkill,
    NormalSkill,
    UltimateSkill,
}

public enum PassiveSkillList
{
    None,
    AllyTypeBoost,
    SelfHealOnTurnEnd,
    LowHpAttackBoost,
    ReflectDamage,
    EscapeMaster
}

public enum NormalSkillList
{
    None,
    FlareStrike,
    ToxicBite,
    WaterSlash,
    SteelSlash,
    GroundSmash
}

public enum UltimateSkillList
{
    None,
    FireStorm,
    MiracleTouch,
    BreathOfDeath,
    PowerSlam,
    GracePulse
}

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Skill Data")]
public class SkillData : ScriptableObject
{
    public SkillType skillType;
    public PassiveSkillList passiveSkillList;
    public NormalSkillList normalSkillList;
    public UltimateSkillList ultimateSkillList;
    public string skillName;
    public float skillPower;

    public bool isTargetSingleAlly;
    public bool isAreaAttack;
    public bool isTargetSelf;
    
    public Sprite icon;
    public Sprite upgradeIcon;
    [TextArea]
    public string description;
    public string upgradeDescription;
    
}
