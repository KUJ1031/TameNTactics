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
    public Sprite monsterImage;
    public MonsterType type;
    public Personality personality;
    
    [Header("기본 능력치")]
    public int baseHp;
    public int baseAttack;
    public int baseDefense;
    public int baseSpeed;
    public int baseExp;
    public int baseExpReward;
    public int baseGoldReward;

    [Header("현재 능력치")]
    public int level;
    public int maxHp;
    public int curHp;
    public int attack;
    public int defense;
    public int speed;
    public int criticalChance;
    public int maxExp;
    public int curExp;
    
    [Header("배틀 리워드")]
    public int expReward;
    public int goldReward;

    [Header("스킬 정보")]
    public List<SkillData> skills;

    [Header("몬스터 정보")]
    public string spawnArea;
    public string spawnTime;
    [TextArea]
    public string description;
}

