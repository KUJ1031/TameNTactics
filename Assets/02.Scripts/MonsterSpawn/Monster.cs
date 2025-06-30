using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

/// <summary>
/// MonsterData를 기반으로 몬스터 정보를 UI에 표시하는 스크립트
/// </summary>
public class Monster : MonoBehaviour
{
    [Header("몬스터 정보 데이터")]
    public MonsterData monsterData;
    
    [Header("기본 정보")]
    public string monsterName;
    public MonsterType type;
    public Personality personality;

    [field: Header("능력치")]
    [field: SerializeField] public int Level { get; set; }

    [field: SerializeField] public int MaxHp { get; private set; }
    [field: SerializeField] public int CurHp { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Defense { get; private set;}
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int CriticalChance { get; private set; }
    [field: SerializeField] public int MaxExp { get; private set; }
    [field: SerializeField] public int CurExp { get; private set; }
    
    [field: Header("배틀 리워드")]
    [field: SerializeField] public int ExpReward { get; private set; }
    [field: SerializeField] public int GoldReward { get; private set; }

    [Header("스킬 정보")]
    public List<SkillData> skills;
    
    [Header("UI 요소")]
    public SpriteRenderer monsterSpriteRenderer; // Image 대신 SpriteRenderer 사용
    public Text infoText;          // 몬스터 상세 정보 출력용
    
    private void Awake()
    {
        ApplyMonsterData();
        LoadMonsterBaseStatData();
    }
    
    public void LoadMonsterBaseStatData()
    {
      //  Level = 1;
        MaxHp = monsterData.maxHp;
        CurHp = 0;
        Attack = monsterData.attack;
        Defense = monsterData.defense;
        Speed = monsterData.speed;
        CriticalChance = monsterData.criticalChance;
        MaxExp = monsterData.maxExp;
        CurExp = 0;
        ExpReward = monsterData.expReward;
        GoldReward = monsterData.goldReward;
        skills = new List<SkillData>(monsterData.skills);
    }

    /// <summary>
    /// 외부에서 이 몬스터의 데이터를 가져갈 수 있도록 제공
    /// </summary>
    public MonsterData GetData()
    {
        return monsterData;
    }

    /// <summary>
    /// UI에 몬스터 이미지 및 정보 텍스트 적용
    /// </summary>
    public void ApplyMonsterData()
    {
        if (monsterSpriteRenderer != null)
            monsterSpriteRenderer.sprite = monsterData.monsterImage;

        string info = GenerateMonsterInfo();

        if (infoText != null)
            infoText.text = info;

        Debug.Log($"[몬스터 정보]\n{info}");
        Debug.Log($"성격 : {monsterData.personality}");
    }

    /// <summary>
    /// MonsterData를 기반으로 텍스트로 된 정보 문자열 생성
    /// </summary>
    string GenerateMonsterInfo()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"<b>{monsterName}</b>");
        sb.AppendLine($"Type: {type}");
        sb.AppendLine($"Level: {Level}");
        sb.AppendLine($"HP: {CurHp} / {MaxHp}");
        sb.AppendLine($"ATK: {Attack}");
        sb.AppendLine($"DEF: {Defense}");
        sb.AppendLine($"SPD: {Speed}");
        sb.AppendLine($"CRT: {CriticalChance}");

        // 스킬 정보 (주석 처리됨)
        if (skills != null && skills.Count > 0)
        {
            sb.AppendLine("스킬:");
            foreach (var skill in skills)
            {
                // 추후 필요 시 주석 해제
                // sb.AppendLine($" - {skill.skillType}: {skill.skillName}");
                // sb.AppendLine($"   {skill.description}");
            }
        }

        return sb.ToString();
    }
    
    public void AddExp(int expAmount)
    {
        CurExp += expAmount;

        while (CurExp >= MaxExp && Level < 30)
        {
            CurExp -= MaxExp;
            Level++;
            RecalculateStats();
            CurHp = MaxHp; // 레벨업 시 체력 회복
        }
    }
    
    public void RecalculateStats()
    {
        int levelMinusOne = Level - 1;

        MaxHp = MaxHp + 12 * levelMinusOne;
        Attack = Attack + 3 * levelMinusOne;
        Defense = Defense + 3 * levelMinusOne;
        Speed = Speed + 3 * levelMinusOne;
        MaxExp = MaxExp + 25 * levelMinusOne;
        ExpReward = ExpReward + 25 * levelMinusOne;
        GoldReward = GoldReward + 30 * levelMinusOne;

        // 만약 curHp가 maxHp보다 크다면 맞춰줌
        if (CurHp > MaxHp)
            CurHp = MaxHp;
    }

    public void TakeDamage(int damage)
    {
        CurHp -= damage;
        if (CurHp < 0) CurHp = 0;
    }

    public void SetLevel(int level)
    {
        Level = level;
    }
}
