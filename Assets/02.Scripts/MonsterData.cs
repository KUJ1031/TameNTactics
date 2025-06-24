using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string name;
    public int maxHP;
    public int attackPower;
    public int defense;
    public int speed;
    public int criticalChance;
    public Sprite monsterSprite;
    public ElementType elementType;
    
    public SkillData passiveSkill;
    public SkillData[] activeSkills;
}

public enum ElementType
{
    Fire,
    Water,
    Grass,
    Ground,
    Steel
}