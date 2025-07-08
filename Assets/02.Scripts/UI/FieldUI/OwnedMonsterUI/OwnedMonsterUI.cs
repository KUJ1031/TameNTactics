using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OwnedMonsterUI : FieldMenuBaseUI
{
    [SerializeField] private Button detailButton, addEntryButton, releaseButton;
    [SerializeField]private MonsterDetailUI monsterDetailUI;

    [Header("window")]
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject simpleMonsterInfo;

    [Header("MonsterSimpleInfo")]
    [SerializeField] private GameObject monsterFavoriteMark;
    [SerializeField] private TextMeshProUGUI monsterNameText;
    [SerializeField] private TextMeshProUGUI monsterTypeText;
    [SerializeField] private TextMeshProUGUI monsterPersonalityText;
    [SerializeField] private TextMeshProUGUI monsterSkill1Text;
    [SerializeField] private TextMeshProUGUI monsterSkill2Text;
    [SerializeField] private TextMeshProUGUI monsterSkill3Text;



    private void Awake()
    {
        
    }

    public void SetMonsterDetailUIButtons(Monster monster)
    {
        detailButton.onClick.RemoveAllListeners();
        addEntryButton.onClick.RemoveAllListeners();
        releaseButton.onClick.RemoveAllListeners();

        detailButton.onClick.AddListener(() => OnClickDetailButton(monster));
        addEntryButton.onClick.AddListener(() => OnClickAddEntryButton(monster));
        releaseButton.onClick.AddListener(() => OnClickReleaseButton(monster));
    }

    private void OnClickDetailButton(Monster monster)
    {
        monsterDetailUI.SetMonsterDetailUI(monster);
        FieldUIManager.Instance.OpenUI<MonsterDetailUI>();
    }

    private void OnClickAddEntryButton(Monster monster)
    {

    }

    private void OnClickReleaseButton(Monster monster)
    {

    }

    public void SetSimpleMonsterUI(Monster monster)
    {
        monsterFavoriteMark.SetActive(monster.IsFavorite);
        monsterNameText.text = monster.monsterName;
        monsterTypeText.text = monster.type.ToString();
        monsterPersonalityText.text = monster.personality.ToString();
        monsterSkill1Text.text = monster.skills[0].skillName;
        monsterSkill2Text.text = monster.skills[1].skillName;
        monsterSkill3Text.text = monster.skills[2].skillName;
    }
}
