using System.Collections.Generic;
using UnityEngine;

//타입
public enum MonsterType
{
    Fire,   //불
    Water,  //물
    Grass,  //풀
    Ground, //땅
    Steel,  //철
}

//성격
public enum Personality
{
    /// <summary>
    /// 철저한
    /// </summary>
    Thorough,   //철저한
    /// <summary>
    /// 헌신적인
    /// </summary>
    Devoted,    //헌신적인
    /// <summary>
    /// 결단력있는
    /// </summary>
    Decisive,   //결단력있는
    /// <summary>
    /// 대담한
    /// </summary>
    Bold,       //대담한
    /// <summary>
    /// 신중한
    /// </summary>
    Cautious,   //신중한
    /// <summary>
    /// 감성적인
    /// </summary>
    Emotional,  //감성적인
    /// <summary>
    /// 활동적인
    /// </summary>
    Energetic,  //활동적인
    /// <summary>
    /// 책임감있는
    /// </summary>
    Responsible,//책임감있는
    /// <summary>
    /// 열정적인
    /// </summary>
    Passionate, //열정적인
    /// <summary>
    /// 적극적인
    /// </summary>
    Proactive,  //적극적인
    /// <summary>
    /// 염세적인
    /// </summary>
    Cynical,    //염세적인
    /// <summary>
    /// 이타적인
    /// </summary>
    Altruistic, //이타적인
    /// <summary>
    /// 사교적인
    /// </summary>
    Sociable    //사교적인
}

public enum SpawnArea
{
    StartForest,    //시작의 숲
    Castle,         //성
    Unknown,        //알수없음
}

public enum SpawnTime
{
    Morning,    //아침
    Evening,    //저녁
    Dawn        //새벽
}

[CreateAssetMenu(fileName = "NewMonster", menuName = "New Monster/Create New Monster")]
public class MonsterData : ScriptableObject
{
    [Header("기본 정보")]
    public string monsterName;
    public int monsterNumber;   //도감번호
    public Sprite monsterImage;
    public int encounterCount;  //만난 횟수
    public int captureCount;    //잡은 횟수
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
    public SpawnArea spawnArea;
    public SpawnTime spawnTime;
    public float captureRate;//기본포획확률

    [TextArea]
    public string description;

    [Header("사운드")]
    public AudioData damageSound;
}

