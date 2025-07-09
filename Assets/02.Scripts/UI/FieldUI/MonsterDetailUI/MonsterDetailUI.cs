using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDetailUI : FieldMenuBaseUI
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

    [Header("버튼")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button addEntryButton;
    [SerializeField] private Button removeEntryButton;

    private Monster monster;
    private Player player = PlayerManager.Instance.player;

    //몬스터 디테일 유아이 셋팅
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
        SettingButton();
    }

    //몬스터 디테일 몬스터 정보 셋팅
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

    //몬스터 디테일 몬스터 스킬 셋팅

    private void UpdateMonsterSkillUI()
    {
        List<SkillData> skills = monster.skills;

        if (skills == null || skills.Count < 3)
        {
            Debug.LogWarning("MonsterDetailUI: Skill is null");
            return;
        }

        UpdateSkillSlot(skills[0], monsterSkill1IconUI, monsterSkill1Name, monsterSkill1Info, 0, 10, null);
        UpdateSkillSlot(skills[1], monsterSkill2IconUI, monsterSkill2Name, monsterSkill2Info, 5, 20, monsterSkill2Lock);
        UpdateSkillSlot(skills[2], monsterSkill3IconUI, monsterSkill3Name, monsterSkill3Info, 15, 25, monsterSkill3Lock);
    }

    //스킬 칸 셋팅
    private void UpdateSkillSlot(SkillData skill, Image iconUI, TextMeshProUGUI nameUI, TextMeshProUGUI infoUI, int nuLockLevel, int upgradeLevel, GameObject lockObj)
    {
        bool isUnLock = monster.Level >= nuLockLevel;
        bool isUpgrade = monster.Level >= upgradeLevel;

        iconUI.sprite = isUpgrade ? skill.upgradeIcon : skill.icon;
        nameUI.text = skill.name;
        infoUI.text = isUpgrade ? skill.upgradeDescription : skill.description;

        if (lockObj != null)
            lockObj.SetActive(!isUnLock);
    }

    //버튼관리
    private void SettingButton()
    {
        backButton.onClick.AddListener(OnClickBackButton);
        if (player.entryMonsters.Contains(monster))
        {
            removeEntryButton.gameObject.SetActive(true);
            addEntryButton.gameObject.SetActive(false);
        }
        else
        {
            removeEntryButton.gameObject.SetActive(false);
            addEntryButton.gameObject.SetActive(true);
        }
        addEntryButton.onClick.AddListener(() => player.TryAddEntryMonster(monster, _ => { }));
        removeEntryButton.onClick.AddListener(() => player.RemoveEntryMonster(monster));
    }

    private void OnClickBackButton()
    {
        FieldUIManager.Instance.OpenUI<OwnedMonsterUI>();
    }
}
