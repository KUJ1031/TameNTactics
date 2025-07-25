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
    AllyTypeBoost, // 우리팀중 본인과 같은 타입이 2명 이상일 시 발동, 우리팀중 본인과 같은 타입은 모두 공격력 10% 증가
    SelfHealOnTurnEnd, // 턴 끝날때 최대체력의 5% 회복
    LowHpAttackBoost, // 체력 50% 이하일때 공격력 20% 상승
    ReflectDamage, // 받는 데미지의 10% 되돌려줌
    EscapeMaster, // 도망가기 100% 성공
    DefensiveStance, // 받는 피해 10% 감소
    IncreaseMissChance, // 5% 확률로 공격 회피
    StatusEffectImmunity, // 모든 상태이상 무효
    OneHitShield, // 공격을 한번 막아주는 실드(상태이상은 못막음)
    AtkUpOnAllyDeath, // 아군 쓰러질때마다 공격력 20% 상승
    TypeHitRecovery, // 유리한 상성 공격시 데미지의 20% 체력 회복
    EnemyAtkDownTwoTurns, // 게임 시작시 2턴동안 상대 공격력 10% 감소
    UltGaugeChancePerTurn, // 매 턴 끝날때 20% 확률로 궁극기 게이지 1개 증가
    BonusAttack, // 10% 확률로 추가 공격(데미지의 20%)
    PowerBoostPerUlt, // 같은팀(본인 포함) 궁극기를 사용하면 공격력 10% 증가
    HealOnKill, // 적 처치시 최대체력의 20% 체력 회복
    AliveTeamGuard, // 본인이 살아있는 동안 팀 전체 방어력 10% 상승
    CritUpOnCritHit, // 치명타로 맞을시 치명타 확률 10% 상승
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
