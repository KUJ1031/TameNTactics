using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("몬스터 기본 정보")]
    public string name;
    public int maxHP;
    public int attackPower;
    public int defense;
    public int speed;
    public int criticalChance;
    public ElementType elementType;
    
    [Header("스킬 설정")]
    public SkillData passiveSkill;
    public SkillData activeSkill;
    public SkillData ultimateSkill;
}

public enum ElementType
{
    Fire,
    Water,
    Grass,
    Ground,
    Steel
}