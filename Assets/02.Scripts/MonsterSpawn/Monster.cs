using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    [Header("몬스터 정보 데이터")]
    public MonsterData monsterData;
    
    [Header("기본 정보")]
    public string monsterName;
    public int monsterID;
    public MonsterType type;
    public Personality personality;

    [field: Header("능력치")]
    [field: SerializeField] public int Level { get; private set; } = 1;

    [field: SerializeField] public int MaxHp { get; private set; }
    [field: SerializeField] public int CurHp { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Defense { get; private set;}
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int CriticalChance { get; private set; }
    [field: SerializeField] public int MaxExp { get; private set; }
    [field: SerializeField] public int CurExp { get; private set; }
    
    [field: Header("배틀 리워드")]
    [field: SerializeField] public int ExpReward { get; private set; }
    [field: SerializeField] public int GoldReward { get; private set; }

    [Header("스킬 정보")]
    public List<SkillData> skills;
     
    private List<StatusEffect> activeStatusEffects = new();
    
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
        monsterID = data.monsterID;
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
    
    public void RecalculateStats()
    {
        int levelMinusOne = Level - 1;

        MaxHp = monsterData.maxHp + 12 * levelMinusOne;
        Attack = monsterData.attack + 3 * levelMinusOne;
        Defense = monsterData.defense + 3 * levelMinusOne;
        Speed = monsterData.speed + 3 * levelMinusOne;
        MaxExp = monsterData.maxExp + 25 * levelMinusOne;
        ExpReward = monsterData.expReward + 25 * levelMinusOne;
        GoldReward = monsterData.goldReward + 30 * levelMinusOne;

        // 만약 curHp가 maxHp보다 크다면 맞춰줌
        if (CurHp > MaxHp)
            CurHp = MaxHp;
    }

    public void Heal(int amount)
    {
        CurHp += amount;
        if (CurHp >= MaxHp) CurHp = MaxHp;
    }

    public void SpeedDownEffect(int amount)
    {
        Speed -= amount;
        if (Speed < 0) Speed = 0;
    }

    public void SpeedUpEffect(int amount)
    {
        Speed += amount;
    }

    public void RecoverUpSpeed(int amount)
    {
        Speed += amount;
    }

    //피해받기
    public void TakeDamage(int damage)
    {
        CurHp -= damage;
        if (CurHp < 0) CurHp = 0;
    }

    //레벨설정
    public void SetLevel(int level)
    {
        Level = level;
        //레벨마다 오르는 능력치 처리
    }

    public void ApplyStatus(StatusEffect effect)
    {
        foreach (var existing in activeStatusEffects)
        {
            if (existing.Name == effect.Name) return;
        }
        
        activeStatusEffects.Add(effect);
    }

    public void OnTurnStart()
    {
        List<StatusEffect> expired = new();

        foreach (var effect in activeStatusEffects)
        {
            effect.OnTurnStart(this);
            effect.duration--;

            if (effect.duration <= 0)
            {
                expired.Add(effect);
            }
        }

        foreach (var effect in expired)
        {
            activeStatusEffects.Remove(effect);
        }
    }
}
