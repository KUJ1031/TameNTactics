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

[CreateAssetMenu(fileName = "NewMonster", menuName = "New Monster/Create New Monster")]
public class MonsterData : ScriptableObject
{
    [Header("기본 정보")]
    public string monsterName;
    public int monsterNumber;//도감번호
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
    public float captureRate;//기본포획확률
    [TextArea]
    public string description;
}

