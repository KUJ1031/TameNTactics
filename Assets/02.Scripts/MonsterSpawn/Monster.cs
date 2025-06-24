using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;

    public Image monsterImageUI;
    public Text infoText;

    void Start()
    {
        if (monsterData != null)
        {
            ApplyMonsterData();
        }
    }

    void ApplyMonsterData()
    {
        // 이미지 설정
        if (monsterImageUI != null)
            monsterImageUI.sprite = monsterData.monsterImage;

        // 전체 정보 생성
        string info = GenerateMonsterInfo();

        // UI에 표시
        if (infoText != null)
            infoText.text = info;

        // 디버그 출력
        Debug.Log($"[몬스터 정보]\n{info}");
    }

    string GenerateMonsterInfo()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"<b>{monsterData.monsterName}</b>");
        sb.AppendLine($"Type: {monsterData.type}");
        sb.AppendLine($"Level: {monsterData.level}");
        sb.AppendLine($"HP: {monsterData.curHp} / {monsterData.maxHp}");
        sb.AppendLine($"ATK: {monsterData.attack}");
        sb.AppendLine($"DEF: {monsterData.defense}");
        sb.AppendLine($"SPD: {monsterData.speed}");
        sb.AppendLine($"CRT: {monsterData.critical}");

        if (monsterData.additionalStats != null && monsterData.additionalStats.Count > 0)
        {
            sb.AppendLine("추가 스탯:");
            foreach (var stat in monsterData.additionalStats)
            {
                sb.AppendLine($" - {stat.abilityType}: {stat.value}");
            }
        }

        if (monsterData.skills != null && monsterData.skills.Count > 0)
        {
            sb.AppendLine("스킬:");
            foreach (var skill in monsterData.skills)
            {
                sb.AppendLine($" - {skill.skillType}: {skill.skillName}");
                sb.AppendLine($"   {skill.description}");
            }
        }

        return sb.ToString();
    }
}
