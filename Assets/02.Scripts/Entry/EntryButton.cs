using TMPro;
using UnityEngine;

public class EntryButton : MonoBehaviour
{
    [Header("버튼 연결 요소들")]
    public Monster monster; // Monster 인스턴스 참조
    public EntryManager entryManager;
    public TextMeshProUGUI buttonText; // UI 버튼 텍스트

    private void Start()
    {
        UpdateButtonUI();

        if (entryManager != null)
        {
            entryManager.OnEntryChanged += UpdateButtonUI;
        }
    }

    private void OnDestroy()
    {
        if (entryManager != null)
        {
            entryManager.OnEntryChanged -= UpdateButtonUI;
        }
    }

    public void OnClick()
    {
        if (monster == null || entryManager == null) return;

        entryManager.ToggleEntry(monster);
        UpdateButtonUI();
    }

    public void UpdateButtonUI()
    {
        if (monster != null && entryManager.IsInEntry(monster))
            buttonText.text = "UnEquip";
        else
            buttonText.text = "Equip";
    }
}
