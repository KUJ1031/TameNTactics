using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Fire,
    Water,
    Grass,
    Earth,
    Iron,
}

public enum AbilityType
{
    Attack,
    Defense,
    Speed,
    MaxHp,
    CurHp,
    Critical,
}

public enum Skill
{
    PassiveSkill,
    ActiveSkill1,
    UltimateSkill,
}

[System.Serializable]
public class Stat
{
    public AbilityType abilityType;
    public float value;
}

[System.Serializable]
public class SkillData
{
    public Skill skillType;
    public string skillName;
    public float skillPower;
    public int curUltimateCost;
    public bool isAreaAttack;
    public bool isTargetSelf;
    [TextArea]
    public string description;
    public Sprite icon;
    [Header("궁극기 체크")]
    public bool isUltimate;
    public int ultimateCost;
}

[CreateAssetMenu(fileName = "NewMonster", menuName = "New Monster/Create New Monster")]
public class MonsterData : ScriptableObject
{
    [Header("기본 정보")]
    public string monsterName;
    public Sprite monsterImage;
    public MonsterType type;

    [Header("능력치")]
    public int level;
    public float maxHp;
    public float curHp;
    public float attack;
    public float defense;
    public float speed;
    public float critical;

    [Header("추가 스탯")]
    public List<Stat> additionalStats;

    [Header("스킬 정보")]
    public List<SkillData> skills;
}

