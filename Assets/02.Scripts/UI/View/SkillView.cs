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

    public void ShowSkillList(List<SkillData> skills)
    {
        // 1번 슬롯
        if (skills.Count >= 2 && skills[1] != null)
        {
            var selectSlot = skillSlot1.GetComponent<SkillSelecter>();
            skillSlot1.sprite = skills[1].icon;
            selectSlot.skillIndex = 1;
        }
        else if (skills[1] == null)
        {
            skillSlot1.sprite = null;
        }

        // 2번 슬롯
        if (skills.Count >= 3 && skills[2] != null)
        {
            var selectSlot = skillSlot2.GetComponent<SkillSelecter>();
            skillSlot2.sprite = skills[2].icon;
            selectSlot.skillIndex = 2;
        }
        else if (skills[2] == null)
        {
            skillSlot2.sprite = null;
        }

        skillPanel.gameObject.SetActive(true);
    }
}
