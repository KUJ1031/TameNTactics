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

public enum TargetScope
{
    None,
    Self,
    All,
    EnemyTeam,
    PlayerTeam
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
    public GameObject skillEffectPrefab;

    public TargetScope targetScope;
    public int targetCount;

    public Sprite icon;
    public Sprite upgradeIcon;
    [TextArea]
    public string description;
    public string upgradeDescription;
}
