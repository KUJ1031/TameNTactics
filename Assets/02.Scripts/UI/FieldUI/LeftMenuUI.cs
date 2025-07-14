using UnityEngine;
using UnityEngine.UI;

public class LeftMenuUI : MonoBehaviour
{
    [SerializeField] private Button playerInfoButton, inventoryButton, collectionButton, entryMonsterButton, ownedMonsterButton, settingButton, saveButton, exitButton;

    private void Awake()
    {
        playerInfoButton.onClick.AddListener(OnClickPlyerInfoButton);
        inventoryButton.onClick.AddListener(OnClickInventoryButton);
        collectionButton.onClick.AddListener(OnClickCollectionButton);
        entryMonsterButton.onClick.AddListener(OnClickEntryMonsterButton);
        ownedMonsterButton.onClick.AddListener(OnClickOwnedMonsterButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        //closeMenuButton.onClick.AddListener(OnClickCloseMenuButton);
    }

    private void OnClickPlyerInfoButton()
    {
        FieldUIManager.Instance.OpenUI<PlayerInfoUI>();
    }
    private void OnClickInventoryButton()
    {
        FieldUIManager.Instance.OpenUI<InventoryUI>();
    }
    private void OnClickCollectionButton()
    {
        FieldUIManager.Instance.OpenUI<CollectionUI>();
    }
    private void OnClickEntryMonsterButton()
    {
        EntryUIManager.Instance.SetEntryUISlots();
        FieldUIManager.Instance.OpenUI<EntryUI>();
    }
    private void OnClickOwnedMonsterButton()
    {
        OwnedMonsterUIManager.Instance.RefreshOwnedMonsterUI();
        FieldUIManager.Instance.OpenUI<OwnedMonsterUI>();
    }
    private void OnClickSettingButton()
    {
        FieldUIManager.Instance.OpenUI<GameSettingUI>();
    }

    //public void OnClickCloseMenuButton()
    //{
    //    FieldUIManager.Instance.CloseAllUI();
    //    // EntryUI 리셋
    //    var ui = FindObjectOfType<FieldBaseUI>();
    //    if (ui != null) ui.RefreshEntrySlots();
    //}
}
