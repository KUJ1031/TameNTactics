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

    [Header("능력치")]
    public int level;
    public int maxHp;
    public int curHp;
    public int attack;
    public int defense;
    public int speed;
    public int criticalChance;
    public int maxExp;
    public int curExp;
    public int baseExpReward;
    public int baseGoldReward;
    
    [Header("배틀 리워드")]
    public int expReward;
    public int goldReward;

    [Header("스킬 정보")]
    public List<SkillData> skills;
    
    [Header("UI 요소")]
    public SpriteRenderer monsterSpriteRenderer;  // Image 대신 SpriteRenderer 사용
    public Text infoText;          // 몬스터 상세 정보 출력용

    //몬스터 생성자
    public Monster(MonsterData data)
    {
        monsterData = data;
        ApplyMonsterData();
        LoadMonsterBaseStatData();

    }
    public void LoadMonsterBaseStatData()
    {
        level = monsterData.level;
        maxHp = monsterData.maxHp;
        curHp = monsterData.curHp;
        attack = monsterData.attack;
        defense = monsterData.defense;
        speed = monsterData.speed;
        criticalChance = monsterData.criticalChance;
        maxExp = monsterData.maxExp;
        curExp = monsterData.curExp;
        baseExpReward = monsterData.baseExpReward;
        baseGoldReward = monsterData.baseGoldReward;
        expReward = monsterData.expReward;
        goldReward = monsterData.goldReward;
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

        sb.AppendLine($"<b>{monsterData.monsterName}</b>");
        sb.AppendLine($"Type: {monsterData.type}");
        sb.AppendLine($"Level: {level}");
        sb.AppendLine($"HP: {curHp} / {maxHp}");
        sb.AppendLine($"ATK: {attack}");
        sb.AppendLine($"DEF: {defense}");
        sb.AppendLine($"SPD: {speed}");
        sb.AppendLine($"CRT: {criticalChance}");

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
}
