using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Fire,
    Water,
    Grass,
    Ground,
    Steel,
}

public enum Personality
{
    Timid,
    Normal,
    Aggressive,
}

public enum MonsterClass
{
    Normal,
    Boss
}

[CreateAssetMenu(fileName = "NewMonster", menuName = "New Monster/Create New Monster")]
public class MonsterData : ScriptableObject
{
    [Header("기본 정보")]
    public MonsterClass monsterClass;
    public string monsterName;
    public int monsterID;
    public Sprite monsterImage;
    public int encounterCount; // 만난 횟수
    public int captureCount; // 잡은 횟수
    public MonsterType type;
    public Personality personality;
    
    [Header("능력치")]
    public int maxHp;
    public int attack;
    public int defense;
    public int speed;
    public int criticalChance;
    public int maxExp;
    public int MaxUltimateCost;
    
    [Header("배틀 리워드")]
    public int expReward;
    public int goldReward;

    [Header("스킬 정보")]
    public List<SkillData> skills;

    [Header("몬스터 정보")]
    public string spawnArea;
    public int spawnTime;
    [TextArea]
    public string description;
}

