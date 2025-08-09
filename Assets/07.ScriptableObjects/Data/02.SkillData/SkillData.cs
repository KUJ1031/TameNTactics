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
    AllyTypeBoost, // 우리팀중 본인과 같은 타입이 2명 이상일 시 발동, 우리팀중 본인과 같은 타입은 모두 공격력 10% 증가, 20레벨 20% 증가
    SelfHealOnTurnEnd, // 턴 끝날때 최대체력의 5% 회복, 20레벨 10% 회복
    LowHpAttackBoost, // 체력 50% 이하일때 공격력 20% 상승, 20레벨 30% 상승
    ReflectDamage, // 받는 데미지의 10% 되돌려줌, 20레벨 15% 증가
    EscapeMaster, // 도망시 100% 도망 가능, 20레벨 게임 시작시 팀 전체 스피드 10% 증가
    DefensiveStance, // 공격받는 데미지 10% 감소, 20레벨 데미지 15% 감소
    IncreaseMissChance, // 5% 확률로 공격 회피, 20레벨 10% 반사 데미지
    StatusEffectImmunity, // 모든 상태이상 무효, 20레벨 배틀 시작시 방어력 10% 상승
    OneHitShield, // 공격을 한번 막아주는 실드(상태이상은 못막음), 20레벨 20% 확률로 공격 받아도 안깨짐
    AtkUpOnAllyDeath, // 아군 쓰러질때마다 공격력 20%씩 상승, 20레벨 스피드 20%씩 상승
    TypeHitRecovery, // 유리한 상성 공격시 데미지의 20% 체력 회복 20레벨 30% 회복
    EnemyAtkDown, // 게임 시작시 상대 공격력 5% 감소 20레벨 상대 방어력 5% 감소
    UltGaugeChancePerTurn, // 매 턴 끝날때 20% 확률로 궁극기 게이지 1개 증가, 20레벨 40% 증가
    BonusAttack, // 10% 확률로 추가 공격(데미지의 20%), 20레벨 20% 확률 데미지의 30%
    PowerBoostPerUlt, // 같은팀(본인 포함) 궁극기를 사용하면 공격력 10% 증가, 20레벨 스피드 10% 증가
    AliveTeamGuard, // 본인이 살아있는 동안 팀 전체 방어력 10% 상승, 20레벨 20% 상승
    CritUpOnCritHit, // 치명타로 맞을시 치명타 확률 30% 상승 (최대 3스택), 20레벨 40% 상승
    AtkUpOnDamaged, // 피격시 공격력 5% 상승(최대 3스택), 20레벨 10% 상승
    HitCritBoost, // 피격시 치명타확률 10% 상승(최대 3스택), 20레벨 20% 상승
    ReviveOnDeathChance, // 쓰러젔을때 50% 확률로 최대체력의 30%로 부활, 20레벨 50%로 부활
    CritUpOnTurnEnd, // 매 턴 끝날때 치명타확률 5% 상승 (최대 60%), 20레벨 최대 100%
    PoisonEnemiesOnDeath, // 쓰러졌을때 상대 몬스터 전체에게 2턴 중독 부여, 20레벨 4턴 부여
    CleanseSelfOnUlt, // 궁극기 사용시 모든 상태이상 제거, 20레벨 공격력 10% 증가
    LowHpDefBoost, // 체력 50% 이하일때 방어력 20% 상승, 20레벨 30% 상승
    HitSpdBoost, // 피격시 스피드 10% 상승, 20레벨 치명타 확률 10% 상승(최대 3스택)
    EnemyTeamDamageTurnEnd // 턴 종료시 적 몬스터 전체 데미지(체력의 10%)
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
    BossDoubleAttack // 단일 더블 데미지 70% 2번 때림(보스용)
}

public enum UltimateSkillList
{
    None,
    FireStorm, // 전체공격 50% 확률로 3턴동안 화상, 25레벨 데미지 1.5배 70% 확률 화상
    MiracleTouch, // 같은팀 한명 100% 회복, 모든 상태이상 제거, 25레벨 전체 스텟 10% 상승
    BreathOfDeath, // 단일공격 5%확률로 즉사, 25레벨 데미지 1.5배 10%확률 즉사
    PowerSlam, // 단일공격 100% 2턴동안 스턴, 25레벨 데미지 3턴동안 스턴
    GracePulse, // 같은팀 최대체력의 20% 회복, 궁극기 코스트 1개씩 증가, 25레벨 최대체력 40% 회복
    EnemyTeamAttackAtkDown, // 전체공격 상대팀 공격력 10% 감소, 25레벨 데미지 1.5배 공격력 15% 감소
    TeamBuffDefUp, // 우리팀 방어력 20% 상승, 25레벨 40% 상승
    EnemyTeamAttackPoison, // 전체공격 50% 확률로 3턴동안 중독, 25레벨 데미지 1.5배 70% 확률 3턴 중독
    EnemyTeamAttackTeamStatUp, // 전체공격 우리팀 전체 공격력, 방어력, 스피드, 치명타확률 10%씩 상승, 25레벨 15%씩 상승
    SelfCleanseAndShield, // 자기자신 모든 상태이상 제거, 실드생성(데미지 받아야 사라짐, 1회 방어), 25레벨 도발 2턴 부여
    EnemyTeamAttackResetUltCost, // 전체공격 상대팀 궁극기 코스트 초기화, 25레벨 데미지 1.5배
    SingleAttackRemoveAllBuffs, // 단일공격 상대 스텟버프 초기화, 25레벨 데미지 1.5배 30% 확률로 아무 상태이상 적용
    EnemyTeamAttackHealBlock2Turn, // 전체공격 2턴동안 힐 불가, 25레벨 데미지 1.5배 3턴동안 힐 불가
    SelectAllyCleanseAndUltMax, // 우리팀중 선택한 몬스터 모든 상태이상 제거, 궁극기 최대치로 채움, 25레벨 본인도 상태이상 제거
    SingleAttackDoubleDamageIfDebuffed, // 단일공격 타겟 몬스터가 상태이상에 걸려있다면 데미지2배, 25레벨 본인도 상태이상일때도 2배
    TeamRegenHp3Turn, // 같은팀 전체 3턴동안 최대체력의 10% 회복, 25레벨 4턴으로 증가
    EnemyTeamAttackChanceStun, // 전체공격 30% 확률로 2턴동안 스턴, 25레벨 데미지 1.5배 50%확률 스턴
    SingleAttackBonusDamageChance, // 단일공격 40% 확률로 데미지의 50%만큼 추가 데미지, 25레벨 데미지 1.5배 60% 확률 증가
    ReviveAllyFullHp, // 기절한 우리팀 하나 최대 체력으로 부활, 25레벨 실드 부여
    SingleAttackDoubleDamageWithHpCost30, // 단일공격 최대체력의 35% 데미지 입고, 공격 데미지 2배, 25레벨 데미지 1.5배 25% 데미지 입음
    SingleAllyStatBoost, // 우리팀 한명 선택해서 스텟 20% 상승(공격력,방어력,속도,치명타확률), 25레벨 30% 상승
    SingleAttackDoubleDamageIfTargetHpOver80, // 단일 공격 상대 몬스터의 체력이 최대체력의 80%가 넘으면 데미지 2배, 25레벨 데미지 1.5배 최대체력 조건 65% 이상
    SingleAttackHpScaledDamage, // 단일 공격 타겟 체력에따라 공격력이 달라짐, 25레벨 데미지 1.5배 증가 공격력 달라지는 데미지 배수 올라감
    SingleAttackFixedHp, // 단일공격 타겟 최대체력의 30% 고정 데미지, 25레벨 40% 고정 데미지
    SelfTaunt, // 도발 2턴 동안 모든 공격 대신 맞음, 25레벨 3턴 도발 실드 추가
    BossEnemyTeamDoubleAttack // 전체 공격 더블 데미지(데미지의 70%) 보스용
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
    public bool isTargetingDeadMonster;

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
