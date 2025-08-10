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
    [SerializeField] private Transform skillLockPanel;
    [SerializeField] private Transform activeSkillTooltipPanel;
    [SerializeField] private Transform passiveSkillTooltipPanel;

    [SerializeField] private TextMeshProUGUI activeSkillName;
    [SerializeField] private TextMeshProUGUI activeSkillDescription;
    [SerializeField] private TextMeshProUGUI passiveSkillName;
    [SerializeField] private TextMeshProUGUI passiveSkillDescription;

    public void ShowSkillList(int level, List<SkillData> skills)
    {
        if (skills == null || skills.Count == 0)
        {
            skillPanel.gameObject.SetActive(false);
            return;
        }

        int normalSkillIdx = skills.FindIndex(s => s != null && s.skillType == SkillType.NormalSkill);
        int ultSkillIdx = skills.FindIndex(s => s != null && s.skillType == SkillType.UltimateSkill);

        // 일반 스킬 슬롯
        var normalSlotSelecter = skillSlot1.GetComponent<SkillSelecter>();
        if (normalSkillIdx >= 0 && skills[normalSkillIdx].icon != null)
        {
            skillSlot1.sprite = skills[normalSkillIdx].icon;
            if (normalSlotSelecter != null) normalSlotSelecter.skillIndex = normalSkillIdx;
            skillSlot1.enabled = true;
        }
        else
        {
            skillSlot1.sprite = null;
            if (normalSlotSelecter != null) normalSlotSelecter.skillIndex = -1;
            skillSlot1.enabled = false;
        }

        // 궁극기 스킬 슬롯
        bool ultUnlocked = level >= 15;
        var ultSlotSelecter = skillSlot2.GetComponent<SkillSelecter>();

        if (ultSkillIdx >= 0 && skills[ultSkillIdx].icon != null)
        {
            skillSlot2.sprite = skills[ultSkillIdx].icon;
            if (ultSlotSelecter != null) ultSlotSelecter.skillIndex = ultSkillIdx;
            skillSlot2.enabled = true;
        }
        else
        {
            skillSlot2.sprite = null;
            if (ultSlotSelecter != null) ultSlotSelecter.skillIndex = -1;
            skillSlot2.enabled = false;
        }

        if (skillLockPanel != null)
        {
            skillLockPanel.gameObject.SetActive(!ultUnlocked);
        }

        skillPanel.gameObject.SetActive(true);
    }

    public void ShowActiveSkillTooltip(string name, string desc)
    {
        activeSkillName.text = name;
        activeSkillDescription.text = desc;
        activeSkillTooltipPanel.gameObject.SetActive(true);
    }

    public void HideActiveSkillTooltip()
    {
        activeSkillTooltipPanel.gameObject.SetActive(false);
    }

    public void ShowPassiveSkillTooltip(string name, string desc)
    {
        passiveSkillName.text = name;
        passiveSkillDescription.text = desc;
        passiveSkillTooltipPanel.gameObject.SetActive(true);
    }

    public void HidePassiveSkillTooltip()
    {
        passiveSkillTooltipPanel.gameObject.SetActive(false);
    }
}
