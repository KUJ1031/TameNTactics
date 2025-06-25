using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private Image skillSlot1;
    [SerializeField] private Image skillSlot2;
    [SerializeField] private Transform skillPanel;

    public void ShowSkillList(List<SkillData> skills, Vector3 worldPosition)
    {
        // 1번 슬롯
        if (skills.Count >= 1 && skills[0] != null)
        {
            skillSlot1.sprite = skills[0].icon;
        }

        // 2번 슬롯
        if (skills.Count >= 2 && skills[1] != null)
        {
            skillSlot2.sprite = skills[1].icon;
        }

        // 스킬 패널 위치를 몬스터 오른쪽에서 보이게 설정
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        screenPos.x += 50f;
        skillPanel.position = screenPos;

        skillPanel.gameObject.SetActive(true);
    }

    public void HideSkills()
    {
        skillPanel.gameObject.SetActive(false);
    }
}
