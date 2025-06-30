using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private Image skillSlot1;
    [SerializeField] private Image skillSlot2;
    [SerializeField] private Transform skillPanel;

    public void ShowSkillList(List<SkillData> skills)
    {
        Debug.Log("몬스터가 가지고 있는 스킬을 보여줍니다.");
        // 1번 슬롯
        if (skills.Count >= 1 && skills[0] != null)
        {
            skillSlot1.sprite = skills[0].icon;
        }
        else if (skills[0] == null)
        {
            skillSlot1.sprite = null;
        }

        // 2번 슬롯
        if (skills.Count >= 2 && skills[1] != null)
        {
            skillSlot2.sprite = skills[1].icon;
        }
        else if (skills[1] == null)
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
