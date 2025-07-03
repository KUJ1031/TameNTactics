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
    AllyTypeBoost,     // 배틀 시작 시 같은 속성 공격력 UP
    SelfHealOnTurnEnd, // 턴 종료 시 체력 회복
    LowHpAttackBoost,  // 체력 50% 이하 시 공격력 UP
    ReflectDamage,     // 피격 시 데미지 반사
    EscapeMaster       // 도망 100% 성공
}

public enum NormalSkillType
{
    FlareStrike,
    ToxicBite,
    WaterSlash,
    SteelSlash,
    GroundSmash
}

public enum UltimateSkillType
{
    None
}

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Skill Data")]
public class SkillData : ScriptableObject
{
    public SkillType skillType;
    public PassiveSkillType passiveType;
    public NormalSkillType normalType;
    public UltimateSkillType ultimateType;
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
