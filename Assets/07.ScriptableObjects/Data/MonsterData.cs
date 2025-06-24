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

    [Header("능력치")]
    public int level;
    public float maxHp;
    public float curHp;
    public float attack;
    public float defense;
    public float speed;
    public float criticalChance;

    [Header("스킬 정보")]
    public List<SkillData> skills;
}

