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
    //읽기 전용 프로퍼티화
    public int level;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
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

        //디버그로 몬스터의 모든 정보를 전부 출력
        Debug.Log($"[몬스터 생성] 이름: {monsterData.monsterName}, ID: {monsterData.monsterID}, 타입: {monsterData.type}, 성격: {monsterData.personality}");
        Debug.Log($"[몬스터 능력치] 레벨: {monsterData.level}, 최대 HP: {monsterData.maxHp}, 현재 HP: {monsterData.curHp}, 공격력: {monsterData.attack}, 방어력: {monsterData.defense}, 스피드: {monsterData.speed}, 크리티컬 확률: {monsterData.criticalChance}");
        Debug.Log($"[몬스터 배틀 리워드] 경험치: {monsterData.expReward}, 골드: {monsterData.goldReward}");
        Debug.Log($"[몬스터 스킬 정보] 스킬 개수: {monsterData.skills.Count}");
        foreach (var skill in monsterData.skills)
        {
            Debug.Log($" - 스킬 이름: {skill.skillName}, 타입: {skill.skillType}, 설명: {skill.description}");
        }

    }
    public void LoadMonsterBaseStatData()
    {
        int appliedLevel = Level > 0 ? Level : monsterData.level;

        level = appliedLevel;
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
