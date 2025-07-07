using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDetailUI : MonoBehaviour
{
    [Header("기본 정보")]
    [SerializeField] private Image monsterImage;

    [SerializeField] private Image favoriteMark;
    [SerializeField] private TextMeshProUGUI monsterHPText;
    [SerializeField] private Image monsterHPBar;

    [SerializeField] private TextMeshProUGUI monsterLevelText;
    [SerializeField] private TextMeshProUGUI monsterExpText;
    [SerializeField] private Image monsterExpBar;

    [SerializeField] private TextMeshProUGUI monsterNameText;
    [SerializeField] private TextMeshProUGUI monsterTypeText;
    [SerializeField] private TextMeshProUGUI monsterPersonalityText;

    [SerializeField] private TextMeshProUGUI monsterAttackText;
    [SerializeField] private TextMeshProUGUI monsterDefenseText;
    [SerializeField] private TextMeshProUGUI monsterSpeedText;
    [SerializeField] private TextMeshProUGUI monsterCriticalText;

    [SerializeField] private TextMeshProUGUI monsterCaughtDateText;
    [SerializeField] private TextMeshProUGUI monsterCaughtLocationText;
    [SerializeField] private TextMeshProUGUI monsterTimeTogetherText;
    [SerializeField] private TextMeshProUGUI monsterStoryText;

    [Header("스킬 1")]
    [SerializeField] private Image monsterSkill1IconUI;
    [SerializeField] private TextMeshProUGUI monsterSkill1Name;
    [SerializeField] private TextMeshProUGUI monsterSkill1Info;
    //[SerializeField] private GameObject monsterSkill1Lock;

    [Header("스킬 2")]
    [SerializeField] private Image monsterSkill2IconUI;
    [SerializeField] private TextMeshProUGUI monsterSkill2Name;
    [SerializeField] private TextMeshProUGUI monsterSkill2Info;
    [SerializeField] private GameObject monsterSkill2Lock;

    [Header("스킬 3")]
    [SerializeField] private Image monsterSkill3IconUI;
    [SerializeField] private TextMeshProUGUI monsterSkill3Name;
    [SerializeField] private TextMeshProUGUI monsterSkill3Info;
    [SerializeField] private GameObject monsterSkill3Lock;

    private Monster monster;


    public void SetMonsterDetailUI(Monster newMonster)
    {
        monster = newMonster;

        if (monster == null)
        {
            Debug.LogWarning("MonsterDetailUI: monster is null");
            return;
        }

        UpdateMonsterDataUI();
        UpdateMonsterSkillUI();
    }

    private void UpdateMonsterDataUI()
    {
        monsterImage.sprite = monster.monsterData.monsterImage;
        monsterHPText.text = $"{monster.CurHp}/{monster.MaxHp}";
        monsterHPBar.fillAmount = (float)monster.CurHp / monster.MaxHp;

        monsterLevelText.text = $"Lv.{monster.Level}";
        monsterExpText.text = $"{monster.CurExp}/{monster.MaxExp}";
        monsterExpBar.fillAmount = (float)monster.CurExp / monster.MaxExp;

        monsterNameText.text = monster.monsterName;
        monsterTypeText.text = monster.monsterData.type.ToString();
        monsterPersonalityText.text = monster.monsterData.personality.ToString();

        monsterAttackText.text = monster.Attack.ToString();
        monsterDefenseText.text = monster.Defense.ToString();
        monsterSpeedText.text = monster.Speed.ToString();
        monsterCriticalText.text = $"{monster.CriticalChance}%";

        monsterCaughtDateText.text = monster.CaughtDate.ToString();
        monsterCaughtLocationText.text = monster.CaughtLocation;
        monsterTimeTogetherText.text = monster.TimeTogether.ToString();
        monsterStoryText.text = monster.monsterData.description;
    }

    private void UpdateMonsterSkillUI()
    {
        List<SkillData> skills = monster.skills;
        if (skills == null || skills.Count < 3)
        {
            Debug.LogWarning("MonsterDetailUI: Skill list is invalid.");
            return;
        }

        ApplySkillToUI(skills[0], monsterSkill1IconUI, monsterSkill1Name, monsterSkill1Info);
        ApplySkillToUI(skills[1], monsterSkill2IconUI, monsterSkill2Name, monsterSkill2Info);
        ApplySkillToUI(skills[2], monsterSkill3IconUI, monsterSkill3Name, monsterSkill3Info);

        monsterSkill2Lock.SetActive(monster.Level < 5);
        monsterSkill3Lock.SetActive(monster.Level < 20);
    }

    private void ApplySkillToUI(SkillData skill, Image iconUI, TextMeshProUGUI nameText, TextMeshProUGUI infoText)
    {
        iconUI.sprite = skill.icon;
        nameText.text = skill.name;
        infoText.text = skill.description;
    }
}
