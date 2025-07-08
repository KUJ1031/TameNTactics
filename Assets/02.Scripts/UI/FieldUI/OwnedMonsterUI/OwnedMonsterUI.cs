using UnityEngine;
using UnityEngine.UI;

public class OwnedMonsterUI : FieldMenuBaseUI
{
    [SerializeField] private Button detailButton, addEntryButton, releaseButton;
    [SerializeField]private MonsterDetailUI monsterDetailUI;

    private Monster clickMonster;
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

    }
}
