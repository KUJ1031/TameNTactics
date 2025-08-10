using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Button closeMenuButton;

    private Monster monster;
    private Player player;


    //몬스터 디테일 유아이 셋팅
    public void SetMonsterDetailUI(Monster newMonster)
    {
        monster = newMonster;
        player = PlayerManager.Instance.player;

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
        player = PlayerManager.Instance.player;
        monsterImage.sprite = monster.monsterData.monsterImage;
        monsterHPText.text = $"{monster.CurHp}/{monster.MaxHp}";
        monsterHPBar.fillAmount = (float)monster.CurHp / monster.MaxHp;

        monsterLevelText.text = $"Lv.{monster.Level}";
        monsterExpText.text = $"{monster.CurExp}/{monster.MaxExp}";
        monsterExpBar.fillAmount = (float)monster.CurExp / monster.MaxExp;

        monsterNameText.text = monster.monsterName;
        monsterTypeText.text = monster.monsterData.type.ToKorean();
        monsterPersonalityText.text = monster.monsterData.personality.ToKorean();

        //hp는 단순히 몬스터의 현재 체력과 최대 체력을 표시
        monsterHPText.text = $"{monster.CurHp}/{monster.MaxHp}";
        monsterAttackText.text = player.GetTotalEffectBonus(ItemEffectType.attack) > 0 ?
            $"{monster.Attack} <color=red>({PlayerManager.Instance.player.playerEquipment[0].data.itemName} +{PlayerManager.Instance.player.GetTotalEffectBonus(ItemEffectType.attack)})</color>" : $"{monster.Attack}";
        monsterDefenseText.text = player.GetTotalEffectBonus(ItemEffectType.defense) > 0 ?
            $"{monster.Defense} <color=red>({PlayerManager.Instance.player.playerEquipment[0].data.itemName} +{PlayerManager.Instance.player.GetTotalEffectBonus(ItemEffectType.defense)})</color>" : $"{monster.Defense}";
        monsterSpeedText.text = player.GetTotalEffectBonus(ItemEffectType.speed) > 0 ?
            $"{monster.Speed} <color=red>({PlayerManager.Instance.player.playerEquipment[0].data.itemName} +{PlayerManager.Instance.player.GetTotalEffectBonus(ItemEffectType.speed)})</color>" : $"{monster.Speed}";
        monsterCriticalText.text = player.GetTotalEffectBonus(ItemEffectType.criticalChance) > 0 ?
            $"{monster.CriticalChance} <color=red>({PlayerManager.Instance.player.playerEquipment[0].data.itemName} +{PlayerManager.Instance.player.GetTotalEffectBonus(ItemEffectType.criticalChance)})</color>" : $"{monster.CriticalChance}";
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

        iconUI.sprite = skill.icon;
        nameUI.text = skill.skillName;
        infoUI.text = skill.description;

        if (lockObj != null)
            lockObj.SetActive(!isUnLock);
    }

    //버튼관리
    private void SettingButton()
    {
        ToggleEntryButton(player.entryMonsters.Contains(monster));
        backButton.onClick.AddListener(OnClickBackButton);
        addEntryButton.onClick.AddListener(OnClickAddEntryButton);
        removeEntryButton.onClick.AddListener(OnClickRemoveEntryButton);
        closeMenuButton.onClick.AddListener(OnClickCloseMenuButton);
    }
    public void OnClickCloseMenuButton()
    {
        FieldUIManager.Instance.CloseAllUI();
    }

    //엔트리 추가버튼
    private void OnClickAddEntryButton()
    {
        player.TryAddEntryMonster(monster, (_, _) => { ToggleEntryButton(player.entryMonsters.Contains(monster)); });

    }

    //엔트리 제외버튼
    private void OnClickRemoveEntryButton()
    {
        player.RemoveEntryMonster(monster);
        ToggleEntryButton(player.entryMonsters.Contains(monster));
    }

    //뒤로가기버튼
    private void OnClickBackButton()
    {
        OwnedMonsterUIManager.Instance.RefreshOwnedMonsterUI();
        FieldUIManager.Instance.OpenUI<OwnedMonsterUI>();
    }

    //엔트리 추가/제외 버튼토글
    private void ToggleEntryButton(bool isEntry)
    {
        removeEntryButton.gameObject.SetActive(isEntry);
        addEntryButton.gameObject.SetActive(!isEntry);
    }
}
