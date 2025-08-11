using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[System.Serializable]
public class Monster
{
    [Header("몬스터 정보 데이터")] public MonsterData monsterData;

    [Header("기본 정보")] public string monsterName;
    public int monsterID;
    public MonsterType type;
    public Personality personality;

    [field: SerializeField] public bool IsFavorite { get; private set; } = false;

    [field: Header("능력치")]
    [field: SerializeField]
    public int Level { get; private set; } = 1;

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
    [field: SerializeField]
    public int ExpReward { get; private set; }

    [field: SerializeField] public int GoldReward { get; private set; }

    [Header("스킬 정보")] public List<SkillData> skills;

    // 배틀 중 변경되는 스텟
    public int CurMaxHp { get; private set; }
    public int CurAttack { get; private set; }
    public int CurDefense { get; private set; }
    public int CurSpeed { get; private set; }
    public int CurCriticalChance { get; private set; }

    public List<StatusEffect> ActiveStatusEffects { get; private set; } = new();
    public List<BuffEffect> ActiveBuffEffects { get; private set; } = new();
    public List<IPassiveSkill> PassiveSkills { get; private set; } = new();

    public bool debuffCanAct { get; private set; } = true;
    public bool canAct { get; private set; } = true;
    private int skipTurnCount = 0;
    private bool isShield;
    private bool canBeHealed = true;
    private int healDuration = 0;
    private bool isImmuneToStatus;

    public Action<Monster> HpChange;
    public Action<Monster> UltimateCostChange;
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
        monsterID += PlayerManager.Instance.player.playerGetMonsterCount;
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
    public (int beforeLevel, int afterLevel) AddExp(int expAmount)
    {
        int beforeLevel = Level;
        CurExp += expAmount;

        while (CurExp >= MaxExp && Level < 30)
        {
            CurExp -= MaxExp;
            Level++;
            RecalculateStats();
            CurHp = MaxHp; // 레벨업 시 체력 회복
        }

        int afterLevel = Level;
        return (beforeLevel, afterLevel);
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
        ExpReward = monsterData.expReward + 20 * levelMinusOne;
        GoldReward = monsterData.goldReward + 100 * levelMinusOne;
    }

    public void MaxHpUp(int amount)
    {
        MaxHp += amount;
    }

    public void MaxHpDown(int amount)
    {
        MaxHp -= amount;
        if (MaxHp < 0) MaxHp = 0;
        if (CurHp > MaxHp) CurHp = MaxHp; // 현재 체력이 최대 체력보다 크면 최대 체력으로 설정
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
        if (CurCriticalChance > 100) CurCriticalChance = 100;
    }

    public void BattleCritChanceUpWithLimit(int amount, int maxLimit)
    {
        CurCriticalChance += amount;

        if (CurCriticalChance > maxLimit)
        {
            CurCriticalChance = maxLimit;
        }
    }

    public void BattleMaxHpUp(int amount)
    {
        CurMaxHp += amount;
    }


    public void Heal(int amount)
    {
        int modifiedAmount;

        if (!canBeHealed)
        {
            modifiedAmount = 0;
        }

        else modifiedAmount = amount;

        CurHp += modifiedAmount;
        if (CurHp >= CurMaxHp) CurHp = CurMaxHp;
        HpChange?.Invoke(this);
    }

    public void TriggerOnStartTurnHeal()
    {
        if (healDuration > 0 && canBeHealed)
        {
            int amount = Mathf.RoundToInt(CurMaxHp * 0.1f);
            Heal(amount);
            healDuration--;
        }
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

    //피해받기
    public void TakeDamage(int damage)
    {
        int modifiedDamage;

        if (isShield)
        {
            modifiedDamage = 0;
            isShield = false;
        }

        else modifiedDamage = damage;

        CurHp -= modifiedDamage;
        if (CurHp < 0) CurHp = 0;

        DamagePopup?.Invoke(this, modifiedDamage);
        DamagedAnimation?.Invoke(this);
        HpChange?.Invoke(this);

        if (CurHp <= 0)
        {
            EventBus.OnMonsterDead?.Invoke(this);

            OnAllyDeath(this);
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
        if (isImmuneToStatus)
        {
            return;
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

    public void ApplyBuff(BuffEffect effect)
    {
        foreach (var existing in ActiveBuffEffects)
        {
            if (existing.Type == effect.Type)
            {
                existing.duration += effect.duration;
                return;
            }
        }

        ActiveBuffEffects.Add(effect);
    }

    // 상태이상 정해진 턴 수가 지나면 제거
    public void UpdateStatusEffects()
    {
        List<StatusEffect> expired = new();
        
        if (CurHp <= 0) return;

        if (CurHp > 0)
        {
            if (ActiveStatusEffects.Count == 0) return;
            
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
    }

    public void UpdateBuffEffects()
    {
        List<BuffEffect> expired = new();
        
        if (CurHp <= 0) return;

        if (CurHp > 0)
        {
            if (ActiveBuffEffects.Count == 0) return;
            
            foreach (var effect in ActiveBuffEffects)
            {
                effect.OnTurnStart(this);

                if (effect.duration <= 0)
                {
                    expired.Add(effect);
                }
            }

            foreach (var effect in expired)
            {
                ActiveBuffEffects.Remove(effect);
            }
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

    public IEnumerator TriggerOnAttack(Monster actor, int damage, Monster target, SkillData skill, float effectiveness)
    {
        yield return new WaitForSeconds(1f);
        foreach (var passive in PassiveSkills)
        {
            passive.OnAttack(actor, damage, target, skill, effectiveness);
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
        foreach (var passive in PassiveSkills)
        {
            damage = passive.OnDamaged(this, damage, actor);
        }

        return damage;
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
        UltimateCostChange?.Invoke(this);
    }

    // 궁극기 코스트 1개 증가
    public void IncreaseUltimateCost()
    {
        CurUltimateCost++;
        CurUltimateCost = Mathf.Min(CurUltimateCost, MaxUltimateCost);
        UltimateCostChange?.Invoke(this);
    }

    // 궁극기 코스트 1개 감소
    public void DecreaseUltimateCost()
    {
        CurUltimateCost--;
        CurUltimateCost = Mathf.Clamp(CurUltimateCost, 0, MaxUltimateCost);
        UltimateCostChange?.Invoke(this);
    }

    public void IncreaseUltimateCostMax()
    {
        CurUltimateCost = MaxUltimateCost;
    }

    // 상태이상 제거
    public void RemoveStatusEffects()
    {
        ActiveStatusEffects.Clear();
    }

    // 버프 제거
    public void RemoveBuffEffects()
    {
        ActiveBuffEffects.Clear();
    }

    // 행동불가 상태 적용/해제
    public void ApplyStun(bool isApplied)
    {
        if (isApplied)
        {
            Debug.Log("여기는 들어옴?");
            debuffCanAct = false;
            Debug.Log(debuffCanAct);
        }

        else debuffCanAct = true;
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

    public void EncounterPlus()
    {
        monsterData.encounterCount++;
    }

    public void InitializeBattleStart()
    {
        InitializeUltimateCost();
        InitializePassiveSkills();
        InitializeBattleStats();
        RemoveStatusEffects();
        RemoveBuffEffects();
        InitializeStatus();
    }

    public void Shield()
    {
        isShield = true;
    }

    public void CanBeHealed(bool isHealable)
    {
        if (isHealable) canBeHealed = true;
        else canBeHealed = false;
    }

    public void InitializeStatus()
    {
        ActiveStatusEffects.Clear();
        ActiveBuffEffects.Clear();
        isShield = false;
        canBeHealed = true;
        debuffCanAct = true;
        canAct = true;
        isImmuneToStatus = false;
        healDuration = 0;
        skipTurnCount = 0;
    }

    public void HealDuration(int duration)
    {
        healDuration += duration - 1;
    }

    private void OnAllyDeath(Monster self)
    {
        var team = BattleManager.Instance.BattleEntryTeam.Contains(self)
            ? BattleManager.Instance.BattleEntryTeam
            : BattleManager.Instance.BattleEnemyTeam;

        var enemyTeam = BattleManager.Instance.BattleEntryTeam.Contains(self)
            ? BattleManager.Instance.BattleEnemyTeam
            : BattleManager.Instance.BattleEntryTeam;

        foreach (var monster in team)
        {
            foreach (var passive in monster.PassiveSkills)
            {
                if (passive is AtkUpOnAllyDeath)
                {
                    passive.OnAllyDeath(monster, team);
                }

                if (passive is PoisonEnemiesOnDeath)
                {
                    passive.OnAllyDeath(monster, enemyTeam);
                }

                if (passive is ReviveOnDeathChance)
                {
                    passive.OnAllyDeath(monster, team);
                }
            }
        }
    }

    public void SetImmuneToStatus()
    {
        isImmuneToStatus = true;
    }

    public void ReviveMonster(Monster monster, int amount)
    {
        UIManager.Instance.battleUIManager.ReviveGauge(monster);
        monster.CurHp += amount;
        if (monster.CurHp >= monster.CurMaxHp) monster.CurHp = monster.CurMaxHp;
        HpChange?.Invoke(monster);
        EventBus.OnMonsterRevive?.Invoke(monster);
        InitializeStatus();
    }

    public void ReflectDamage(int damage)
    {
        int modifiedDamage;

        if (isShield)
        {
            modifiedDamage = 0;
            isShield = false;
        }

        else modifiedDamage = damage;

        CurHp -= modifiedDamage;
        if (CurHp < 0) CurHp = 0;

        DamagePopup?.Invoke(this, modifiedDamage);
        HpChange?.Invoke(this);

        if (CurHp <= 0)
        {
            InitializeStatus();
            EventBus.OnMonsterDead?.Invoke(this);

            OnAllyDeath(this);
        }
    }

    public MonsterSaveData ToSaveData()
    {
        var sd = new MonsterSaveData();
        sd.monsterNumber = monsterData != null ? monsterData.monsterNumber : -1;
        sd.monsterID = this.monsterID;
        sd.level = this.Level;
        sd.curHp = this.CurHp;
        sd.curExp = this.CurExp;
        sd.curUltimateCost = this.CurUltimateCost;
        sd.isFavorite = this.IsFavorite;
        sd.caughtDate = this.CaughtDate;
        sd.timeTogether = this.TimeTogether;
        sd.caughtLocation = this.CaughtLocation ?? "";

        // 스킬은 SkillData에 skillNumber가 있다고 가정
        if (skills != null)
            sd.skillIDs = skills.Where(s => s != null).Select(s => s.skillID).ToList();
        else
            sd.skillIDs = new List<int>();

        return sd;
    }

    // DTO -> Monster 재구성 (Monster 내부 static 팩토리)
    public static Monster CreateFromSaveData(MonsterSaveData sd, MonsterDatabase mdb = null, SkillDatabase sdb = null)
    {
        var m = new Monster();

        // 1) MonsterData 찾기: 우선 MonsterDatabase(권장) -> 없으면 Resources fallback
        MonsterData md = null;
        if (mdb != null) md = mdb.GetByNumber(sd.monsterNumber);
        if (md == null)
        {
            // Resources 경로에 MonsterData들이 있다면 검색
            var all = Resources.LoadAll<MonsterData>("Monsters"); // 폴더명 예시
            md = all.FirstOrDefault(x => x.monsterNumber == sd.monsterNumber);
        }

        if (md == null)
        {
            Debug.LogWarning($"[Load] MonsterData not found for number {sd.monsterNumber}. Creating partial Monster.");
        }

        // 2) MonsterData 기반 기본값 채우기 (있으면)
        if (md != null)
        {
            m.monsterData = md;
            m.monsterName = md.monsterName;
            m.type = md.type;
            m.personality = md.personality;
            m.MaxHp = md.maxHp;
            m.Attack = md.attack;
            m.Defense = md.defense;
            m.Speed = md.speed;
            m.CriticalChance = md.criticalChance;
            m.MaxExp = md.maxExp;
            m.MaxUltimateCost = md.MaxUltimateCost;
            m.ExpReward = md.expReward;
            m.GoldReward = md.goldReward;

            // 기본 스킬은 MonsterData의 스킬 복사 (나중에 덮어쓸 수 있음)
            m.skills = md.skills != null ? new List<SkillData>(md.skills) : new List<SkillData>();
        }

        // 3) 저장된 값으로 덮어쓰기
        m.Level = sd.level;
        m.CurHp = sd.curHp;
        m.CurExp = sd.curExp;
        m.CurUltimateCost = sd.curUltimateCost;
        m.monsterID = sd.monsterID;
        m.IsFavorite = sd.isFavorite;
        m.CaughtDate = sd.caughtDate;
        m.TimeTogether = sd.timeTogether;
        m.CaughtLocation = sd.caughtLocation ?? "";

        // 4) 스킬 재연결 (SkillDatabase 권장, 아니면 Resources 탐색)
        if (sd.skillIDs != null && sd.skillIDs.Count > 0)
        {
            var resolvedSkills = new List<SkillData>();
            if (sdb != null)
            {
                foreach (var id in sd.skillIDs)
                {
                    var sk = sdb.GetByNumber(id);
                    if (sk != null) resolvedSkills.Add(sk);
                }
            }
            else
            {
                var allSkills = Resources.LoadAll<SkillData>("Skills");
                foreach (var id in sd.skillIDs)
                {
                    var sk = allSkills.FirstOrDefault(s => s.skillID == id);
                    if (sk != null) resolvedSkills.Add(sk);
                }
            }
            if (resolvedSkills.Count > 0) m.skills = resolvedSkills;
        }

        // 5) 런타임 전용 필드 초기화 (이벤트, 상태 리스트 등)
        m.InitializeRuntimeStateAfterLoad();

        return m;
    }

    // 런타임 필드 초기화 (Monster 내부에 추가)
    private void InitializeRuntimeStateAfterLoad()
    {
        CurMaxHp = MaxHp;
        CurAttack = Attack;
        CurDefense = Defense;
        CurSpeed = Speed;
        CurCriticalChance = CriticalChance;

        ActiveStatusEffects = new List<StatusEffect>();
        ActiveBuffEffects = new List<BuffEffect>();
        PassiveSkills = new List<IPassiveSkill>();

        debuffCanAct = true;
        canAct = true;
        skipTurnCount = 0;
        isShield = false;
        canBeHealed = true;
        healDuration = 0;
        isImmuneToStatus = false;

        // 델리게이트는 null로 초기화, 필요시 로드 후 연결(예: UI 연결)
        HpChange = null;
        UltimateCostChange = null;
        DamagePopup = null;
        DamagedAnimation = null;
    }

}

[System.Serializable]
public class MonsterSaveData
{
    public int monsterNumber;    // MonsterData.monsterNumber (ScriptableObject 식별자)
    public int monsterID;        // 플레이어 내 고유 ID (플레이 중 부여되는 값)
    public int level;
    public int curHp;
    public int curExp;
    public int curUltimateCost;
    public bool isFavorite;
    public float caughtDate;
    public float timeTogether;
    public string caughtLocation;

    // 스킬은 고유 ID 리스트로 저장 (SkillData 에 skillNumber 필요)
    public List<int> skillIDs = new();
}