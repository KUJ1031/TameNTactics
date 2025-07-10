using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OwnedMonsterUI : FieldMenuBaseUI
{
    [SerializeField] private Button detailButton, addEntryButton, removeEntryButton, releaseButton;
    [SerializeField] private MonsterDetailUI monsterDetailUI;

    [Header("window")]
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject simpleMonsterInfo;

    [Header("MonsterSimpleInfo")]
    [SerializeField] private Image monsterImage;
    [SerializeField] private GameObject monsterFavoriteMark;
    [SerializeField] private TextMeshProUGUI monsterNameText;
    [SerializeField] private TextMeshProUGUI monsterTypeText;
    [SerializeField] private TextMeshProUGUI monsterPersonalityText;
    [SerializeField] private TextMeshProUGUI monsterSkill1Text;
    [SerializeField] private TextMeshProUGUI monsterSkill2Text;
    [SerializeField] private TextMeshProUGUI monsterSkill3Text;

    private OwnedMonsterUIManager ownedUIManager;
    private void Start()
    {
        SetLogoVisibility(true);
        ownedUIManager = OwnedMonsterUIManager.Instance;
    }

    //버튼 세팅
    public void SetMonsterDetailUIButtons(Monster monster)
    {
        detailButton.onClick.RemoveAllListeners();
        addEntryButton.onClick.RemoveAllListeners();
        removeEntryButton.onClick.RemoveAllListeners();
        releaseButton.onClick.RemoveAllListeners();
        monsterFavoriteMark.GetComponent<Button>().onClick.RemoveAllListeners();

        detailButton.onClick.AddListener(() => OnClickDetailButton(monster));
        addEntryButton.onClick.AddListener(() => OnClickAddEntryButton(monster));
        removeEntryButton.onClick.AddListener(() => OnClickRemoveEntryButtonButton(monster));
        releaseButton.onClick.AddListener(() => OnClickReleaseButton(monster));
        monsterFavoriteMark.GetComponent<Button>().onClick.AddListener(() => OnClickFavoriteButton(monster));
    }
    //디테일 버튼
    private void OnClickDetailButton(Monster monster)
    {
        monsterDetailUI.SetMonsterDetailUI(monster);
        FieldUIManager.Instance.OpenUI<MonsterDetailUI>();
    }
    //엔트리에 추가 버튼
    private void OnClickAddEntryButton(Monster monster)
    {
        PlayerManager.Instance.player.TryAddEntryMonster(monster, (swapMonster, newMonster) =>
        {
            ownedUIManager.RefreshSlotFor(swapMonster);
            ownedUIManager.RefreshSlotFor(newMonster);
            ToggleAddEntryButton(newMonster);
        });
    }
    //엔트리에 제외 버튼
    private void OnClickRemoveEntryButtonButton(Monster monster)
    {
        PlayerManager.Instance.player.RemoveEntryMonster(monster);
        ToggleAddEntryButton(monster);
    }
    //방출하기 버튼
    private void OnClickReleaseButton(Monster monster)
    {
        PlayerManager.Instance.player.TryRemoveOwnedMonster(monster, (isOK) =>
        {
            if (isOK)
            {
                SetLogoVisibility(true);
                ownedUIManager.SelectedSlotReset();
                ownedUIManager.RefreshOwnedMonsterUI();
            }
        });
    }
    //즐겨찾기 버튼
    private void OnClickFavoriteButton(Monster monster)
    {
        monster.ToggleFavorite();
        ToggleFavoriteMark(monster.IsFavorite);
        ownedUIManager.RefreshSlotFor(monster);
    }

    //즐겨찾기 색 변경
    private void ToggleFavoriteMark(bool IsFavorite)
    {
        if (IsFavorite) { monsterFavoriteMark.GetComponent<Image>().color = new Color32(255, 212, 0, 255); }
        else { monsterFavoriteMark.GetComponent<Image>().color = new Color32(189, 195, 199, 255); }
    }

    //심플 몬스터 정보 띄우기
    public void SetSimpleMonsterUI(Monster monster)
    {
        SetLogoVisibility(false);

        ToggleFavoriteMark(monster.IsFavorite);

        monsterImage.sprite = monster.monsterData.monsterImage;
        monsterNameText.text = monster.monsterName;
        monsterTypeText.text = monster.type.ToString();
        monsterPersonalityText.text = monster.personality.ToString();
        monsterSkill1Text.text = monster.skills[0].skillName;
        monsterSkill2Text.text = monster.skills[1].skillName;
        monsterSkill3Text.text = monster.skills[2].skillName;

        ToggleAddEntryButton(monster);
    }

    //Logo랑 몬스터 정보 토글
    public void SetLogoVisibility(bool isVisible)
    {
        logo.SetActive(isVisible);
        simpleMonsterInfo.SetActive(!isVisible);
    }

    //엔트리에 추가/제외 버튼토글
    private void ToggleAddEntryButton(Monster monster)
    {
        List<Monster> entry = PlayerManager.Instance.player.entryMonsters;
        bool isEntry = entry.Contains(monster);
        removeEntryButton.gameObject.SetActive(isEntry);
        addEntryButton.gameObject.SetActive(!isEntry);
    }

}
