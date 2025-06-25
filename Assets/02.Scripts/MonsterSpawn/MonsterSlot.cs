using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterSlot : MonoBehaviour
{
    public Image monsterImageUI;
    public TextMeshProUGUI monsterNameText;
    public Button actionButton;
    public TextMeshProUGUI actionButtonText;

    private MonsterData currentData;

    void Start()
    {
        actionButton.onClick.AddListener(OnClick);
    }

    public void SetData(MonsterData data)
    {
        currentData = data;
        monsterImageUI.sprite = data.monsterImage;
        monsterNameText.text = data.monsterName;
        UpdateButtonUI();
    }

    void OnClick()
    {
        EntryManager.Instance.ToggleEntry(currentData);
        UpdateButtonUI();
    }

    void UpdateButtonUI()
    {
        if (EntryManager.Instance.IsInEntry(currentData))
            actionButtonText.text = "UnEquip";
        else
            actionButtonText.text = "Equip";
    }
    void OnEnable()
    {
        EntryManager.Instance.OnEntryChanged += UpdateButtonUI;
    }
    void OnDisable()
    {
        EntryManager.Instance.OnEntryChanged -= UpdateButtonUI;
    }
}
