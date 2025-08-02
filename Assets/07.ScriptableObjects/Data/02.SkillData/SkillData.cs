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
    EnemyAtkDown, // 게임 시작시 상대 공격력 5% 감소
    UltGaugeChancePerTurn, // 매 턴 끝날때 20% 확률로 궁극기 게이지 1개 증가
    BonusAttack, // 10% 확률로 추가 공격(데미지의 20%)
    PowerBoostPerUlt, // 같은팀(본인 포함) 궁극기를 사용하면 공격력 10% 증가
    HealOnKill, // 적 처치시 최대체력의 20% 체력 회복
    AliveTeamGuard, // 본인이 살아있는 동안 팀 전체 방어력 10% 상승
    CritUpOnCritHit, // 치명타로 맞을시 치명타 확률 30% 상승 (최대 3스택)
    InterceptDamage, // 같은팀의 최대체력의 70% 이상의 데미지를 받을 시 공격을 대신 받음
    AtkUpOnDamaged, // 피격시 공격력 5% 상승(최대 3스택)
    HitCritBoost, // 피격시 치명타확률 15% 상승(최대 3스택)
    ReviveOnDeathChance, // 쓰러젔을때 50% 확률로 최대체력의 30%로 부활
    CritUpOnTurnEnd, // 매 턴 끝날때 치명타확률 5% 상승 (최대 80%)
    PoisonEnemiesOnDeath, // 쓰러졌을때 상대 몬스터 전체에게 3턴 중독 부여
    CleanseSelfOnUlt // 궁극기 사용시 모든 상태이상 제거
}

public enum NormalSkillList
{
    None,
    FlareStrike, // 기본 공격 해금시 화상 데미지
    ToxicBite, // 기본 공격 해금시 독 데미지
    WaterSlash, // 기본 공격 해금시 데미지의 10% 회복
    SteelSlash, // 기본 공격 해금시 현재 스피드의 10% 스피드 상승
    GroundSmash, //  기본 공격 해금시 마비
    DoubleTargetHit, // 선택한 2마리에게 데미지의 50%씩 데미지를 입힘 해금시 데미지 70%로 변경
    SingleAttackChanceStun, // 기본 공격 해금시 일정 확률로 스턴
    SingleAttackChanceSleep, // 기본 공격 해금시 일정 확률로 수면
    SingleAttackChanceAtkUp, // 기본 공격 해금시 일정 확률로 공격력 10% 상승
    SingleAttackChanceDefUp, // 기본 공격 해금시 일정 확률로 방어력 10% 상승
    SingleAttackChanceCriChanceUp, // 기본 공격 해금시 치명타 확률 5% 상승
    SingleAttackChanceHpUp, // 기본 공격 해금시 일정 확률로 최대체력의 10% 상승
    SingleAttackRandomDebuff, // 기본 공격 해금시 일정 확률로 랜덤 상태이상 부여 2턴
    SingleAttackTargetAtkDown, // 기본 공격 해금시 일정 확률로 상대 공격력 10% 감소
    SingleAttackTargetDefDown, // 기본 공격 해금시 일정 확률로 상대 방어력 10% 감소
    SingleAttackHealLowestAlly, // 기본 공격 해금시 일정 확률로 우리팀중 체력이 가장 낮은 몬스터 최대체력의 10% 회복
    SingleAttackChanceCrit, // 기본 공격 해금시 50% 확률로 치명타 적용
    EnemyTeamAttackChanceSpdDown, // 전체 공격 해금시 15% 확률로 적 스피드 10% 감소
    EnemyTeamAttackChanceAtkUp, // 전체 공격 해금시 10% 확률로 공격자 공격력 10% 상승

}

public enum UltimateSkillList
{
    None,
    FireStorm, // 전체공격 50% 확률로 3턴동안 화상
    MiracleTouch, // 같은팀 한명 100% 회복, 모든 상태이상 제거
    BreathOfDeath, // 단일공격 5%확률로 즉사
    PowerSlam, // 단일공격 100% 3턴동안 스턴
    GracePulse, // 같은팀 최대체력의 30% 회복, 궁극기 코스트 1개씩 증가
    EnemyTeamAttackAtkDown, // 전체공격 상대팀 공격력 10% 감소
    TeamBuffDefUp, // 우리팀 방어력 20% 상승
    EnemyTeamAttackPoison, // 전체공격 50% 확률로 3턴동안 중독
    EnemyTeamAttackTeamStatUp, // 전체공격 우리팀 전체 공격력, 방어력, 스피드, 치명타확률 10%씩 상승
    SelfCleanseAndShield, // 자기자신 모든 상태이상 제거, 실드생성(데미지 받아야 사라짐, 1회 방어)
    EnemyTeamAttackResetUltCost, // 전체공격 상대팀 궁극기 코스트 초기화
    SingleAttackRemoveAllBuffs, // 단일공격 상대 스텟버프 초기화
    EnemyTeamAttackHealBlock2Turn, // 전체공격 2턴동안 힐 불가
    SelectAllyCleanseAndUltMax, // 우리팀중 선택한 몬스터 모든 상태이상 제거, 궁극기 최대치로 채움
    SingleAttackDoubleDamageIfDebuffed, // 단일공격 타겟 몬스터가 상태이상에 걸려있다면 데미지2배
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
    
    [Header("사운드")]
    public AudioData damageSound;
}
