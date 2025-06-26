using UnityEngine;

public enum SkillType
{
    PassiveSkill,
    ActiveSkill,
    UltimateSkill,
}

public enum PassiveSkillType
{
    None,
    TeamAttackUp,          // 팀 전체 공격력 5% 증가
    GainSpeedOnHit,        // 피격 시 본인 스피드 +10
    TeamHealEachTurn,      // 매 턴 끝에 팀원 최대체력 10% 회복
    ReduceEnemySpeedOnHit, // 공격 시 상대 스피드 -5
}

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Skill Data")]
public class SkillData : ScriptableObject
{
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
