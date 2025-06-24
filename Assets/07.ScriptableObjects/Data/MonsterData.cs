using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    불,
    물,
    풀,
    땅,
    철,
}

public enum AbilityType
{
    공격력,
    방어력,
    스피드,
    Hp,
    치명타확률,
}

public enum Skill
{
    패시브,
    액티브스킬1,
    액티브스킬2,
    궁극기,
}
public enum Personality
{
    소심함,
    평범함,
    호전적임
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
    public bool isAreaAttack;
    public bool isTargetSelf;
    [TextArea]
    public string description;
    public Sprite icon;
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
    public float attack;
    public float defense;
    public float speed;
    public float critical;

    [Header("추가 스탯")]
    public List<Stat> additionalStats;

    [Header("스킬 정보")]
    public List<SkillData> skills;
}

