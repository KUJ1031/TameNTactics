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
    public float maxExp;
    public float curExp;
    
    [Header("배틀 리워드")]
    public float expReward;
    public float goldReward;

    [Header("스킬 정보")]
    public List<SkillData> skills;

    public void LevelUp()
    {
        level++;
        
        maxHp += 12f;
        curHp = maxHp;
        attack += 3f;
        defense += 3f;
        speed += 1f;
        
        
        // switch (personality)
        // {
        //     case Personality.Timid:
        //         attack += 2f;
        //         defense += 3f;
        //         speed += 2f;
        //         break;
        //     
        //     case Personality.Aggressive:
        //         attack += 4f;
        //         defense += 2f;
        //         speed += 1f;
        //         break;
        //     
        //     case Personality.Normal:
        //         attack += 3f;
        //         defense += 3f;
        //         speed += 1f;
        //         break;
        // }
    }
}

