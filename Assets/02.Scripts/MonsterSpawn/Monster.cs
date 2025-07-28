using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster
{
    [Header("몬스터 정보 데이터")]
    public MonsterData monsterData;

    [Header("기본 정보")]
    public string monsterName;
    public int monsterID;
    public MonsterType type;
    public Personality personality;

    [field: SerializeField] public bool IsFavorite { get; private set; } = false;

    [field: Header("능력치")]
    [field: SerializeField] public int Level { get; private set; } = 1;

    [field: SerializeField] public int MaxHp { get; private set; }
    [field: SerializeField] public int CurHp { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int CriticalChance { get; private set; }
    [field: SerializeField] public int MaxExp { get; private set; }
    [field: SerializeField] public int CurExp { get; private set; }
    [field: SerializeField] public int MaxUltimateCost { get; private set; }
    [field: SerializeField] public int CurUltimateCost { get; private set; }
    [field: SerializeField] public float CaughtDate { get; private set; }
    [field: SerializeField] public float TimeTogether { get; private set; }
    [field: SerializeField] public string CaughtLocation { get; private set; }


    [field: Header("배틀 리워드")]
    [field: SerializeField] public int ExpReward { get; private set; }
    [field: SerializeField] public int GoldReward { get; private set; }

    [Header("스킬 정보")]
    public List<SkillData> skills;

    // 배틀 중 변경되는 스텟
    public int CurMaxHp { get; private set; }
    public int CurAttack { get; private set; }
    public int CurDefense { get; private set; }
    public int CurSpeed { get; private set; }
    public int CurCriticalChance { get; private set; }

    public List<StatusEffect> ActiveStatusEffects { get; private set; } = new();
    public List<IPassiveSkill> PassiveSkills { get; private set; } = new();

    public bool canAct { get; private set; } = true;
    private int skipTurnCount = 0;

    public Action<Monster> HpChange;
    public Action<Monster> ultimateCostChange;
    public Action<Monster, int> DamagePopup;
    public Action<Monster> DamagedAnimation;

    //몬스터 데이터로 초기화
    public void SetMonster(Monster newMonster)
    {
        if (newMonster == null)
        {
            Debug.LogError("SetMonster: 복사할 sourceMonster가 null입니다.");
            return;
        }
        //몬스터 정보 데이터
        monsterData = newMonster.monsterData;

        //기본 정보
        monsterName = newMonster.monsterName;
        monsterID = newMonster.monsterID;
        type = newMonster.type;
        personality = newMonster.personality;

        //능력치
        Level = newMonster.Level;
        MaxHp = newMonster.MaxHp;
        CurHp = newMonster.CurHp;
        Attack = newMonster.Attack;
        Defense = newMonster.Defense;
        Speed = newMonster.Speed;
        CriticalChance = newMonster.CriticalChance;
        MaxExp = newMonster.MaxExp;
        CurExp = newMonster.CurExp;

        //배틀 리워드
        ExpReward = newMonster.ExpReward;
        GoldReward = newMonster.GoldReward;

        //스킬
        if (newMonster.skills != null)
        {
            skills = new List<SkillData>(newMonster.skills);
        }
        else
        {
            skills = new List<SkillData>();
        }
        //ApplyMonsterData();
    }

    //몬스터데이터로 초기화
    public void SetMonsterData(MonsterData data)
    {
        if (data == null)
        {
            return;
        }

        monsterData = data;

        monsterName = data.monsterName;
        monsterID = PlayerManager.Instance.player.playerGetMonsterCount + 1; //고유 ID는 플레이어가 몬스터를 얻은 횟수로 설정(처음 잡은 몬스터의 ID는 1부터 시작)
        type = data.type;
        personality = data.personality;

        Level = 1; // 기본값

        MaxHp = data.maxHp;
        CurHp = MaxHp;
        Attack = data.attack;
        Defense = data.defense;
        Speed = data.speed;
        CriticalChance = data.criticalChance;
        MaxExp = data.maxExp;
        CurExp = 0;
        MaxUltimateCost = data.MaxUltimateCost;
        CurUltimateCost = 0;

        ExpReward = data.expReward;
        GoldReward = data.goldReward;

        skills = new List<SkillData>(data.skills);
    }

    //경험치 얻기
    public void AddExp(int expAmount)
    {
        CurExp += expAmount;

        while (CurExp >= MaxExp && Level < 30)
        {
            CurExp -= MaxExp;
            Level++;
            RecalculateStats();
            CurHp = MaxHp; // 레벨업 시 체력 회복
        }
    }

    // 배틀 시작 전 사용!
    public void InitializeBattleStats()
    {
        CurMaxHp = MaxHp;
        CurAttack = Attack;
        CurDefense = Defense;
        CurSpeed = Speed;
        CurCriticalChance = CriticalChance;
    }

    // 레벨에따른 스탯 조정
    public void RecalculateStats()
    {
        int levelMinusOne = Level - 1;

        MaxHp = monsterData.maxHp + 12 * levelMinusOne;
        CurHp = MaxHp;
        Attack = monsterData.attack + 3 * levelMinusOne;
        Defense = monsterData.defense + 3 * levelMinusOne;
        Speed = monsterData.speed + 3 * levelMinusOne;
        MaxExp = monsterData.maxExp + 25 * levelMinusOne;
        ExpReward = monsterData.expReward + 25 * levelMinusOne;
        GoldReward = monsterData.goldReward + 30 * levelMinusOne;
    }

    public void PowerUp(int amount)
    {
        CurAttack += amount;
    }

    public void PowerDown(int amount)
    {
        CurAttack -= amount;
        if (CurAttack < 0) CurAttack = 0;
    }

    public void BattleDefenseUp(int amount)
    {
        CurDefense += amount;
    }
    
    public void BattleDefenseDown(int amount)
    {
        CurDefense -= amount;
        if (CurDefense < 0) CurDefense = 0;
    }

    public void AttackUp(int amount)
    {
        Attack += amount;
    }

    public void AttackDown(int amount)
    {
        Attack -= amount;
    }

    public void DefenseUp(int amount)
    {
        Defense += amount;
    }

    public void DefenseDown(int amount)
    {
        Defense -= amount;
    }

    public void SpeedUp(int amount)
    {
        Speed += amount;
        if (CurSpeed < 0) CurSpeed = 0;
    }

    public void SpeedDown(int amount)
    {
        Speed -= amount;
        if (CurSpeed < 0) CurSpeed = 0;
    }

    public void CriticalChanceUp(int amount)
    {
        CriticalChance += amount;
        if (CriticalChance > 100) CriticalChance = 100; // 최대 100%로 제한
    }

    public void CriticalChanceDown(int amount)
    {
        CriticalChance -= amount;
        if (CriticalChance < 0) CriticalChance = 0; // 최소 0%로 제한
    }

    public void BattleCritChanceUp(int amount)
    {
        CurCriticalChance += amount;
        if (CriticalChance > 100) CriticalChance = 100;
    }
    
    public void BattleCritChanceUpWithLimit(int amount, int maxLimit)
    {
        CurCriticalChance += amount;

        if (CurCriticalChance > maxLimit)
        {
            CurCriticalChance = maxLimit;
        }
    }


    public void Heal(int amount)
    {
        CurHp += amount;
        if (CurHp >= CurMaxHp) CurHp = CurMaxHp;
        HpChange?.Invoke(this);
    }

    public void Heal_Potion(int amount)
    {
        CurHp += amount;
        if (CurHp > MaxHp) CurHp = MaxHp;
        HpChange?.Invoke(this);
    }
    public void HealFull()
    {
        CurHp = MaxHp;
        HpChange?.Invoke(this);
    }

    public void SpeedDownEffect(int amount)
    {
        CurSpeed -= amount;
        if (CurSpeed < 0) CurSpeed = 0;
    }

    public void SpeedUpEffect(int amount)
    {
        CurSpeed += amount;
    }

    public void RecoverUpSpeed(int amount)
    {
        CurSpeed += amount;
    }

    //피해받기
    public void TakeDamage(int damage)
    {
        CurHp -= damage;
        if (CurHp < 0) CurHp = 0;

        DamagePopup?.Invoke(this, damage);
        DamagedAnimation?.Invoke(this);

        if (CurHp <= 0)
        {
            foreach (var passive in PassiveSkills)
            {
                if (passive is ReviveOnDeathChance reviveOnDeathChance)
                {
                    reviveOnDeathChance.OnDeath(this);
                }
                else if (passive is PoisonEnemiesOnDeath poisonEnemiesOnDeath)
                {
                    poisonEnemiesOnDeath.OnDeath(BattleManager.Instance.BattleEnemyTeam);
                }
            }
            EventBus.OnMonsterDead?.Invoke(this);
        }
        else
        {
            HpChange?.Invoke(this);
        }
    }

    //레벨설정
    public void SetLevel(int level)
    {
        Level = level;
        //레벨마다 오르는 능력치 처리
    }

    // 상태이상 적용
    public void ApplyStatus(StatusEffect effect)
    {
        foreach (var passive in PassiveSkills)
        {
            if (passive is StatusEffectImmunity immunity && immunity.IsImmuneToStatus)
            {
                return;
            }
        }
        
        foreach (var existing in ActiveStatusEffects)
        {
            if (existing.Type == effect.Type)
            {
                existing.duration += effect.duration;
                return;
            }
        }

        ActiveStatusEffects.Add(effect);
    }

    // 상태이상 정해진 턴 수가 지나면 제거
    public void UpdateStatusEffects()
    {
        List<StatusEffect> expired = new();

        foreach (var effect in ActiveStatusEffects)
        {
            effect.OnTurnStart(this);

            if (effect.duration <= 0)
            {
                expired.Add(effect);
            }
        }

        foreach (var effect in expired)
        {
            ActiveStatusEffects.Remove(effect);
        }
    }

    // 패시브 초기화(레벨5 해금)
    public void InitializePassiveSkills()
    {
        PassiveSkills.Clear();

        foreach (var skill in skills)
        {
            if (skill.skillType == SkillType.PassiveSkill)
            {
                if (Level >= 5)
                {
                    var passive = PassiveSkillFactory.GetPassiveSkill(skill.passiveSkillList);
                    if (passive != null)
                    {
                        PassiveSkills.Add(passive);
                    }
                }
            }
        }
    }
    
    public void TriggerOnAttack(Monster actor, int damage, Monster target, SkillData skill)
    {
        foreach (var passive in PassiveSkills)
        {
            passive.OnAttack(actor, damage, target, skill);
        }
    }

    // 배틀 시작시 패시브 발동
    public void TriggerOnBattleStart(List<Monster> monsters)
    {
        foreach (var passive in PassiveSkills)
        {
            passive.OnBattleStart(this, monsters);
        }
    }

    // 턴 종료시 패시브 발동
    public void TriggerOnTurnEnd()
    {
        foreach (var passive in PassiveSkills)
        {
            passive.OnTurnEnd(this);
        }
    }

    // 데미지 받을 시 패시브 발동
    public int TriggerOnDamaged(int damage, Monster actor)
    {
        int modifiedDamage = damage;

        foreach (var passive in PassiveSkills)
        {
            modifiedDamage = passive.OnDamaged(this, modifiedDamage, actor);
        }

        return modifiedDamage;
    }

    // 도망마스터 패시브 있을 시 100 도망 가능
    public bool TryRunAwayWithPassive(out bool isGuaranteed)
    {
        isGuaranteed = false;
        
        foreach (var passive in PassiveSkills)
        {
            if (passive is EscapeMaster escapeMaster)
            {
                return escapeMaster.TryEscape(this, ref isGuaranteed);
            }
        }
        
        return false;
    }

    // 궁극기 코스트 초기화
    public void InitializeUltimateCost()
    {
        CurUltimateCost = 0;
    }

    // 궁극기 코스트 1개 증가
    public void IncreaseUltimateCost()
    {
        CurUltimateCost++;
        CurUltimateCost = Mathf.Min(CurUltimateCost, MaxUltimateCost);
        ultimateCostChange?.Invoke(this);
    }

    // 궁극기 코스트 1개 감소
    public void DecreaseUltimateCost()
    {
        CurUltimateCost--;
        CurUltimateCost = Mathf.Clamp(CurUltimateCost, 0, MaxUltimateCost);
        ultimateCostChange?.Invoke(this);
    }

    // 상태이상 제거
    public void RemoveStatusEffects()
    {
        ActiveStatusEffects.Clear();
    }

    // 행동불가 상태 적용/해제
    public void ApplyStun(bool isApplied)
    {
        if (isApplied)
        {
            canAct = false;
        }

        else canAct = true;
    }

    //즐겨찾기 변경
    public void ToggleFavorite()
    {
        IsFavorite = !IsFavorite;
    }

    public void SetActionRestriction(int turns)
    {
        skipTurnCount = turns;
        canAct = false;
    }

    public void CheckMonsterAction()
    {
        if (skipTurnCount > 0)
        {
            skipTurnCount--;
        }
        else if (skipTurnCount <= 0)
        {
            canAct = true;
        }
    }

    public void InitializeMonsterAct()
    {
        canAct = true;
        skipTurnCount = 0;
    }

    public void EncounterPlus()
    {
        monsterData.encounterCount++;
    }

    public void InitializeBattleStart()
    {
        InitializeMonsterAct();
        InitializeUltimateCost();
        InitializePassiveSkills();
        InitializeBattleStats();
        RemoveStatusEffects();
    }
}
