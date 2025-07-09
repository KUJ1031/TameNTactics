using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private Image skillSlot1;
    [SerializeField] private Image skillSlot2;
    [SerializeField] private Transform skillPanel;

    // 테스트용 코드
    [SerializeField] private TextMeshProUGUI skillName1;
    [SerializeField] private TextMeshProUGUI skillName2;

    public void ShowSkillList(List<SkillData> skills)
    {
        Debug.Log("몬스터가 가지고 있는 스킬을 보여줍니다.");
        for (int i = 0; i < skills.Count; i++)
        {
            Debug.Log($"{i + 1}번째 스킬 타입 : {skills[i].skillType}");
        }

        // 1번 슬롯
        if (skills.Count >= 1 && skills[1] != null)
        {
            skillSlot1.sprite = skills[1].icon;
            if (skills[1].skillType == SkillType.NormalSkill)
            {
                skillName1.text = "N";
            }
            else if (skills[1].skillType == SkillType.UltimateSkill)
            {
                skillName1.text = "U";
            }
            else
            {
                skillName1.text = "P";
            }
        }
        else if (skills[1] == null)
        {
            skillSlot1.sprite = null;
        }

        // 2번 슬롯
        if (skills.Count >= 2 && skills[2] != null)
        {
            skillSlot2.sprite = skills[2].icon;
            if (skills[2].skillType == SkillType.NormalSkill)
            {
                skillName1.text = "N";
            }
            else if (skills[2].skillType == SkillType.UltimateSkill)
            {
                skillName1.text = "U";
            }
            else
            {
                skillName1.text = "P";
            }
        }
        else if (skills[2] == null)
        {
            skillSlot2.sprite = null;
        }

        skillPanel.gameObject.SetActive(true);
    }

    public void HideSkills()
    {
        skillPanel.gameObject.SetActive(false);
    }
}
